using System.Collections.Generic;
using AICore.GA;
using System;
using System.Linq;


namespace AICore.NeuralNetwork
{
    /// <summary>
    /// 支持多个输出
    /// </summary>
    public class NeuralNetwork
    {
        private List<NeuralLayer> hidden_layers;
        //多个隐藏层
        //private NeuralLayer? hidden_layer1;
        //private NeuralLayer? hidden_layer2;
        private NeuralLayer output_layer;
        private int numOfNeurons = 30;

        //学习率
        double learning_rate = 0.01;

        //使用ga预优化
        bool use_ga = false;

        private double[] inputs_mins;
        private double[] inputs_maxs;
        private double output_min;
        private double output_max;
        private double[] outputs_mins;
        private double[] outputs_maxs;

        private int input_dimension;
        private int output_dimension;

        private string double_format = "0.###";

        private NeuralNetworkOutputMessage output = null;

        #region 归一化数据
        private (double[], double[]) normalize_data(double[,] inputs)
        {
            double[] mins = new double[inputs.GetLength(1)];
            for (int i = 0; i < mins.Length; i++)
            {
                mins[i] = 0.0;
            }
            double[] maxs = new double[inputs.GetLength(1)];
            for (int i = 0; i < maxs.Length; i++)
            {
                maxs[i] = 0.0;
            }
            for (int i = 0; i < inputs.GetLength(0); i++)
            {
                for (int j = 0; j < inputs.GetLength(1); j++)
                {
                    if (inputs[i, j] > maxs[j] || inputs[i, j] < mins[j])
                    {
                        if (inputs[i, j] > maxs[j])
                        {
                            maxs[j] = inputs[i, j];
                        }
                        if (inputs[i, j] < mins[j])
                        {
                            mins[j] = inputs[i, j];
                        }
                    }
                }
            }

            for (int i = 0; i < inputs.GetLength(0); i++)
            {
                for (int j = 0; j < inputs.GetLength(1); j++)
                {
                    inputs[i, j] = (inputs[i, j] - mins[j] + 1) / (maxs[j] - mins[j] + 1);
                }
            }

            return (mins, maxs);
        }

        private (double, double) normalize_array(double[] num)
        {
            double min = 0.0, max = 0.0;
            for (int i = 0; i < num.GetLength(0); i++)
            {
                if (num[i] > max || num[i] < min)
                {
                    if (num[i] > max)
                    {
                        max = num[i];
                    }
                    else
                    {
                        min = num[i];
                    }
                }
            }
            for (int i = 0; i < num.GetLength(0); i++)
            {
                num[i] = (num[i] - min + 1) / (max - min + 1);
            }

            return (min, max);
        }

        private double normalize_single(double num, double min, double max)
        {
            return (num - min + 1) / (max - min + 1);
        }
        #endregion

        //单个隐藏层输出计算
        private double[] compute_hidden(double[] inputs, NeuralLayer hidden_layer)
        {
            List<double> ret = new List<double>();
            for (int i = 0; i < hidden_layer.Neurons.Count; i++)
            {
                double sum = 0.0;
                //隐藏层神经元权重值数量与输入数据维度一致
                for (int j = 0; j < inputs.Length; j++)
                {
                    sum += inputs[j] * hidden_layer.Neurons[i].Weights[j];
                }

                double value = sum + hidden_layer.Neurons[i].Bias;
                value = active(value);
                ret.Add(value);
            }

            return ret.ToArray();
        }

        //输出层计算
        private double compute_output(double[] inputs)
        {
            double output = 0.0;
            for (int i = 0; i < output_layer.Neurons.Count; i++)
            {
                double value = inputs[i] * output_layer.Neurons[i].Weights[0];
                output += value;
            }

            output = (output + output_layer.Bias) / output_layer.Neurons.Count;
            output = active(output);
            return output;
        }

        //输出层计算
        private double[] compute_output_multi(double[] inputs)
        {
            List<double> ret = new List<double>();

            //输出层神经元权重值数量与输出数据维度一致
            for (int i = 0; i < output_dimension; i++)
            {
                double sum = 0.0;
                for (int j = 0; j < output_layer.Neurons.Count; j++)
                {
                    sum += inputs[j] * output_layer.Neurons[j].Weights[i];
                }
                double value = (sum + output_layer.Neurons.Sum(n => n.Bias)) / output_layer.Neurons.Count;
                value = active(value);
                ret.Add(value);
            }

            return ret.ToArray();
        }

        //计算前向输出梯度误差
        private double pro_output(double predict, double real)
        {
            return derivative(predict) * (real - predict);
        }

        private double sigmod(double num)
        {
            return 1 / (1 + Math.Exp(-num));
        }

        private double sigmod_derivative(double num)
        {
            return num * (1 - num);
        }

        private double relu(double num)
        {
            return Math.Max(0, num);
        }

        private double relu_derivative(double num)
        {
            return num > 0 ? 1 : 0;
        }

        private double active(double num)
        {
            return sigmod(num);
            //return relu(num);
        }

        private double derivative(double num)
        {
            return sigmod_derivative(num);
            //return relu_derivative(num);
        }

        //bp算法
        private void bp(double[] inputs, double[] real)
        {
            //隐藏层
            List<double[]> hidden_layer_ouputs = new List<double[]>();
            hidden_layer_ouputs.Add(inputs);
            double[] inputvalues = inputs;
            foreach (var layer in hidden_layers)
            {
                double[] hl_output = compute_hidden(inputvalues, layer);
                hidden_layer_ouputs.Add(hl_output);
                inputvalues = hl_output;
            }
            //最后隐藏层的输出
            double[] last_hidden_layer_output = hidden_layer_ouputs.LastOrDefault();

            //输出层
            double[] output_final = compute_output_multi(last_hidden_layer_output);

            for (int i_out = 0; i_out < output_final.Length; i_out++)
            {
                //计算前向输出误差
                double error_output = pro_output(output_final[i_out], real[i_out]);

                //更新输出层权重和偏置
                for (int i = 0; i < numOfNeurons; i++)
                {
                    output_layer.Neurons[i].Weights[i_out] = output_layer.Neurons[i].Weights[i_out]
                        + learning_rate * error_output * last_hidden_layer_output[i];
                    output_layer.Neurons[i].Bias += learning_rate * error_output;
                }
                output_layer.Bias = output_layer.Bias + learning_rate * error_output;

                //更新隐藏层权重和偏置
                //最后的隐藏层
                NeuralLayer last_hidden_layer = hidden_layers.LastOrDefault();
                double[] error_last_hl = new double[numOfNeurons];
                for (int i = 0; i < numOfNeurons; i++)
                {
                    error_last_hl[i] = derivative(last_hidden_layer_output[i]) * (error_output * output_layer.Neurons[i].Weights[0]);
                }

                for (int i = 0; i < numOfNeurons; i++)
                {
                    last_hidden_layer.Neurons[i].Update(hidden_layer_ouputs[hidden_layer_ouputs.Count - 2], learning_rate, error_last_hl[i]);
                }

                //其他隐藏层
                double[] next_hl_error = error_last_hl;
                for (int i = hidden_layers.Count - 2; i >= 0; i--)
                {
                    NeuralLayer current_layer = hidden_layers[i];
                    NeuralLayer next_layer = hidden_layers[i + 1];
                    double[] current_layer_output = hidden_layer_ouputs[i + 1];
                    double[] pre_layer_output = hidden_layer_ouputs[i];
                    double[] current_hl_error = new double[numOfNeurons];
                    for (int j = 0; j < next_hl_error.Length; j++)
                    {
                        for (int k = 0; k < numOfNeurons; k++)
                        {
                            current_hl_error[k] += derivative(current_layer_output[k]) * next_hl_error[j] * next_layer.Neurons[j].Weights[k];
                        }
                    }

                    for (int j = 0; j < numOfNeurons; j++)
                    {
                        current_layer.Neurons[j].Update(pre_layer_output, learning_rate, current_hl_error[j]);
                    }
                    next_hl_error = current_hl_error;
                }
            }
        }

        public void train(double[,] inputs, double[,] outputs, int epoches)
        {
            (inputs_mins, inputs_maxs) = normalize_data(inputs);
            for (int i = 0; i < inputs_mins.Length; i++)
            {
                output?.Invoke($"输入集[{i}]归一化的最大值和最小值分别为：{inputs_maxs[i].ToString(double_format)}，{inputs_mins[i].ToString(double_format)}", NNOutputMessageType.TrainInput);
            }
            (outputs_mins, outputs_maxs) = normalize_data(outputs);
            for (int i = 0; i < outputs_mins.Length; i++)
            {
                output?.Invoke($"输出集[{i}]归一化的最大值和最小值分别为：{outputs_maxs[i].ToString(double_format)}，{outputs_mins[i].ToString(double_format)}", NNOutputMessageType.TrainInput);
            }
            output?.Invoke($"迭代次数[{epoches}]，隐藏层数量[{hidden_layers.Count}]，输出层数量[1]，每层神经元数量[{numOfNeurons}]，学习率[{learning_rate}]", NNOutputMessageType.TrainInput);

            DisplayParams();

            for (int j = 0; j < epoches; j++)
            {
                for (int i = 0; i < inputs.GetLength(0); i++)
                {
                    double[] input_data_row = new double[input_dimension];
                    for (int k = 0; k < input_dimension; k++)
                    {
                        input_data_row[k] = inputs[i, k];
                    }
                    double[] output_data_row = new double[output_dimension];
                    for(int k = 0;k < output_dimension; k++)
                    {
                        output_data_row[k] = outputs[i, k];
                    }
                    bp(input_data_row, output_data_row);
                }
            }

            DisplayParams();
        }

        private void DisplayParams()
        {
            output?.Invoke("模型参数：", NNOutputMessageType.TrainInfo);
            for (int i = 0; i < hidden_layers.Count; i++)
            {
                output?.Invoke($"隐藏层[{i + 1}, {hidden_layers[i].Name}]：", NNOutputMessageType.TrainInfo);
                for (int j = 0; j < hidden_layers[i].Neurons.Count; j++)
                {
                    output?.Invoke($"神经元[{j + 1}]，权重：[{string.Join(", ", hidden_layers[i].Neurons[j].Weights)}]，偏置：[{hidden_layers[i].Neurons[j].Bias}]", NNOutputMessageType.TrainInfo);
                }
            }

            output?.Invoke($"输出层[{output_layer.Name}]：", NNOutputMessageType.TrainInfo);
            for (int i = 0; i < output_layer.Neurons.Count; i++)
            {
                output?.Invoke($"神经元[{i + 1}]，权重：[{string.Join(", ", output_layer.Neurons[i].Weights)}]，偏置：[{output_layer.Neurons[i].Bias}]", NNOutputMessageType.TrainInfo);
            }
            output?.Invoke("");
        }

        public double[] test(double[,] inputs, double[,] outputs)
        {
            int test_len = inputs.GetLength(0);
            double[,] MSE = new double[test_len, output_dimension];
            for (int i = 0; i < test_len; i++)
            {
                double[] input_row = new double[input_dimension];
                for (int j = 0; j < input_dimension; j++)
                {
                    input_row[j] = inputs[i, j];
                }

                string input_row_str = string.Join("，", input_row);

                double[] input_row_norm = new double[input_dimension];
                for (int j = 0; j < input_dimension; j++)
                {
                    input_row_norm[j] = normalize_single(input_row[j], inputs_mins[j], inputs_maxs[j]);
                }

                List<double> true_value_list = new List<double>();
                for (int j = 0; j < output_dimension; j++)
                {
                    double true_value = normalize_single(outputs[i,j], outputs_mins[j], outputs_maxs[j]);
                    true_value_list.Add(true_value);
                }
                //计算
                double[] input_values = input_row_norm;
                for (int j = 0; j < hidden_layers.Count; j++)
                {
                    input_values = compute_hidden(input_values, hidden_layers[j]);
                }
                double[] predict_values = compute_output_multi(input_values);
                //从归一化还原
                for (int j = 0; j < output_dimension; j++)
                {
                    predict_values[j] = (predict_values[j] * (outputs_maxs[j] - outputs_mins[j] + 1)) - 1 + outputs_mins[j];
                }

                string predict_row_str = string.Join("，", predict_values);

                double[] true_row = new double[output_dimension];
                for (int j = 0; j < output_dimension; j++)
                {
                    true_row[j] = outputs[i, j];
                }

                string true_row_str = string.Join("，", true_row);

                //求均方差
                for (int j = 0; j < output_dimension; j++)
                {
                    MSE[i, j] = (predict_values[j] - true_row[j]) * (predict_values[j] - true_row[j]);
                }
                output?.Invoke($"输入值：{input_row_str}，预测值：{predict_row_str}   真实值：{true_row_str}", NNOutputMessageType.TestOutput);
            }

            double[] mean_square_error = new double[output_dimension];
            string[] mse_strs = new string[output_dimension];
            for (int i = 0; i < output_dimension; i++)
            {
                for (int j = 0; j < test_len; j++)
                {
                    mean_square_error[i] += MSE[j, i];
                }
                mean_square_error[i] = mean_square_error[i] / test_len;
                mse_strs[i] = mean_square_error[i].ToString("0.##########");
            }

            output?.Invoke($"均方误差为：[{string.Join(", ", mse_strs)}]", NNOutputMessageType.TestOutput);
            output?.Invoke("以上为测试集\n", NNOutputMessageType.TestInfo);

            return mean_square_error;
        }
        
        public void train_test(double[,] train_inputs, double[,] train_outputs, int train_epoches,
            double[,] test_inputs, double[,] test_outputs)
        {
            //
            int train_len = train_inputs.GetLength(0);
            int train_test_len = 10;//train_len / 200;
            double[,] train_test_inputs = new double[train_test_len, input_dimension];
            double[,] train_test_outputs = new double[train_test_len, output_dimension];
            for (int k = 0; k < train_test_len; k++)
            {
                for (int m = 0; m < input_dimension; m++)
                {
                    train_test_inputs[k, m] = train_inputs[k, m];
                }
                for (int m = 0; m < output_dimension; m++)
                {
                    train_test_outputs[k, m] = train_outputs[k, m];
                }
            }

            //
            (inputs_mins, inputs_maxs) = normalize_data(train_inputs);
            for (int i = 0; i < inputs_mins.Length; i++)
            {
                output?.Invoke($"输入集[{i}]归一化的最大值和最小值分别为：{inputs_maxs[i].ToString(double_format)}，{inputs_mins[i].ToString(double_format)}", NNOutputMessageType.TrainInput);
            }
            (outputs_mins, outputs_maxs) = normalize_data(train_outputs);
            for (int i = 0; i < outputs_mins.Length; i++)
            {
                output?.Invoke($"输出集[{i}]归一化的最大值和最小值分别为：{outputs_maxs[i].ToString(double_format)}，{outputs_mins[i].ToString(double_format)}", NNOutputMessageType.TrainInput);
            }

            output?.Invoke($"迭代次数[{train_epoches}]，隐藏层数量[{hidden_layers.Count}]，输出层数量[1]，每层神经元数量[{numOfNeurons}]，学习率[{learning_rate}]", NNOutputMessageType.TrainInfo);

            DisplayParams();            

            for (int j = 0; j < train_epoches; j++)
            {
                for (int i = 0; i < train_inputs.GetLength(0); i++)
                {
                    double[] input_data_row = new double[input_dimension];
                    for (int k = 0; k < input_dimension; k++)
                    {
                        input_data_row[k] = train_inputs[i, k];
                    }
                    double[] output_data_row = new double[output_dimension];
                    for (int k = 0; k < output_dimension; k++)
                    {
                        output_data_row[k] = train_outputs[i, k];
                    }
                    bp(input_data_row, output_data_row);
                }

                if ((j + 1) % 1000 == 0)
                {
                    test(train_test_inputs, train_test_outputs);
                    output?.Invoke($"以上是使用训练集进行测试\n", NNOutputMessageType.TestInfo);

                    test(test_inputs, test_outputs);
                    output?.Invoke($"以上是为第{j + 1}次迭代进行测试\n", NNOutputMessageType.TestInfo);
                }
            }

            DisplayParams();
        }

        public double[,] predict(double[,] inputs)
        {
            int test_len = inputs.GetLength(0);
            double[,] outputs = new double[test_len, output_dimension];
            double[] MSE = new double[test_len];
            for (int i = 0; i < test_len; i++)
            {
                double[] input_row = new double[input_dimension];
                for (int j = 0; j < input_dimension; j++)
                {
                    input_row[j] = inputs[i, j];
                }

                string input_row_str = string.Join("，", input_row);

                double[] input_row_norm = new double[input_dimension];
                for (int j = 0; j < input_dimension; j++)
                {
                    input_row_norm[j] = normalize_single(input_row[j], inputs_mins[j], inputs_maxs[j]);
                }

                //计算
                double[] input_values = input_row_norm;
                for (int j = 0; j < hidden_layers.Count; j++)
                {
                    input_values = compute_hidden(input_values, hidden_layers[j]);
                }

                double[] predict_values = compute_output_multi(input_values);
                //从归一化还原
                for (int j = 0; j < output_dimension; j++)
                {
                    predict_values[j] = (predict_values[j] * (outputs_maxs[j] - outputs_mins[j] + 1)) - 1 + outputs_mins[j];
                    outputs[i, j] = predict_values[j];
                }

                string predict_row_str = string.Join("，", predict_values);

                output?.Invoke($"输入值：{input_row_str}，预测值：{predict_row_str}", NNOutputMessageType.PreditcOutput);
            }
            output?.Invoke("以上为预测集", NNOutputMessageType.PredictInfo);
            return outputs;
        }

        public void Export(string name)
        {

        }

        //隐藏层和神经元过多会导致过拟合。
        public NeuralNetwork(int input_dimension,
            int output_dimension,
            int hidden_layer_count,
            int num_of_neurons,
            double learning_rate,
            bool use_ga = false,
            NeuralNetworkOutputMessage output = null)
        {
            this.input_dimension = input_dimension;
            this.output_dimension = output_dimension;
            this.numOfNeurons = num_of_neurons;
            this.learning_rate = learning_rate;
            this.use_ga = use_ga;
            this.output = output;

            initialize_hidden_layers(input_dimension, hidden_layer_count);
            initilize_output_layer(output_dimension);
        }

        private void initialize_hidden_layers(int input_dimension, int hidden_layer_count)
        {

            hidden_layers = new List<NeuralLayer>();
            //默认有一个隐藏层
            NeuralLayer layer = new NeuralLayer("HL001", numOfNeurons, input_dimension);
            hidden_layers.Add(layer);
            for (int i = 1; i < hidden_layer_count; i++)
            {
                layer = new NeuralLayer($"HL{(i + 1).ToString().PadLeft(3, '0')}", numOfNeurons, numOfNeurons);
                hidden_layers.Add(layer);
            }

        }

        private void initilize_output_layer(int output_dimension)
        {
            output_layer = new NeuralLayer("OL", numOfNeurons, output_dimension);
        }

        #region 导入导出模型参数
        public NeuralNetwork(string file)
        {
            LoadParams(file);
        }
        public void ExportParams(string file)
        {
            NeuralNetworkParams nnp = new NeuralNetworkParams()
            {
                hidden_layers = this.hidden_layers,
                output_layer = this.output_layer,
                numOfNeurons = this.numOfNeurons,
                learning_rate = this.learning_rate,
                use_ga = this.use_ga,
                inputs_mins = this.inputs_mins,
                inputs_maxs = this.inputs_maxs,
                outputs_mins = this.outputs_mins,
                outputs_maxs = this.outputs_maxs,
                input_dimension = this.input_dimension,
                output_dimension = this.output_dimension,
                double_format = this.double_format
            };
            nnp.Save(file);
        }

        public void LoadParams(string file)
        {
            NeuralNetworkParams nnp = new NeuralNetworkParams();
            nnp.Load(file);

            hidden_layers = nnp.hidden_layers;
            output_layer = nnp.output_layer;
            numOfNeurons = nnp.numOfNeurons;
            learning_rate = nnp.learning_rate;
            use_ga = nnp.use_ga;
            inputs_mins = nnp.inputs_mins;
            inputs_maxs = nnp.inputs_maxs;
            outputs_mins = nnp.outputs_mins;
            outputs_maxs = nnp.outputs_maxs;
            input_dimension = nnp.input_dimension;
            output_dimension = nnp.output_dimension;
            double_format = nnp.double_format;
        }
        #endregion
    }

    public delegate void NeuralNetworkOutputMessage(string msg, NNOutputMessageType type = NNOutputMessageType.Normal);
    public enum NNOutputMessageType
    {
        Normal = 0,
        TrainInput = 1,
        TrainOutput = 2,
        TrainInfo = 3,
        TrainWarning = 4,
        TrainError = 5,
        TestInput = 6,
        TestOutput = 7,
        TestInfo = 8,
        TestWarning = 9,
        TestError = 10,
        PredictInput = 11,
        PreditcOutput = 12,
        PredictInfo = 13,
        PredictWarning = 14,
        PredictError = 15,
    }
}
