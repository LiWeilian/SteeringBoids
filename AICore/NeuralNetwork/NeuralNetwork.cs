using System.Drawing;
using System.Collections.Generic;
using AICore.GA;
using System;
using System.Linq;


namespace AICore.NeuralNetwork
{
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

        private int input_dimension;
        private int output_dimension;

        private string double_format = "0.###";

        #region 归一化数据
        private (double[], double[]) normalize_inputs(double[,] inputs)
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
        private void bp(double[] inputs, double real)
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
            double output_final = compute_output(last_hidden_layer_output);
            //计算前向输出误差
            double error_output = pro_output(output_final, real);

            //更新输出层权重和偏置
            for (int i = 0; i < numOfNeurons; i++)
            {
                output_layer.Neurons[i].Weights[0] = output_layer.Neurons[i].Weights[0]
                    + learning_rate * error_output * last_hidden_layer_output[i];
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

        public void train(double[,] inputs, double[] outputs, int epoches)
        {
            (inputs_mins, inputs_maxs) = normalize_inputs(inputs);
            //for (int i = 0; i < inputs_mins.Length; i++)
            //{
            //    Console.WriteLine($"输入集[{i}]归一化的最大值和最小值分别为：{inputs_maxs[i].ToString(double_format)}，{inputs_mins[i].ToString(double_format)}", Color.LightGreen);
            //}
            (output_min, output_max) = normalize_array(outputs);
            //Console.WriteLine($"输出集归一化的最大值和最小值为：{output_max.ToString(double_format)}，{output_min.ToString(double_format)}\r\n", Color.LightGreen);

            //Console.WriteLine($"迭代次数[{epoches}]，隐藏层数量[{hidden_layers.Count}]，输出层数量[1]，每层神经元数量[{numOfNeurons}]，学习率[{learning_rate}]", Color.LightGreen);

            DisplayParams();

            for (int j = 0; j < epoches; j++)
            {
                for (int i = 0; i < inputs.GetLength(0); i++)
                {
                    double[] data = new double[input_dimension];
                    for (int k = 0; k < input_dimension; k++)
                    {
                        data[k] = inputs[i, k];
                    }
                    bp(data, outputs[i]);
                }
            }

            DisplayParams();
        }

        private void DisplayParams()
        {
            /*
            Console.WriteLine();
            Console.WriteLine("模型参数：", Color.LightGreen);
            for (int i = 0; i < hidden_layers.Count; i++)
            {
                Console.WriteLine($"隐藏层[{i + 1}, {hidden_layers[i].Name}]：", Color.Orange);
                for (int j = 0; j < hidden_layers[i].Neurons.Count; j++)
                {
                    Console.WriteLine($"神经元[{j+1}]，权重：[{string.Join(", ", hidden_layers[i].Neurons[j].Weights)}]，偏置：[{hidden_layers[i].Neurons[j].Bias}]", Color.LightBlue);
                }
            }

            Console.WriteLine($"输出层[{output_layer.Name}]：", Color.Orange);
            for(int i = 0; i < output_layer.Neurons.Count; i++)
            {
                Console.WriteLine($"神经元[{i + 1}]，权重：[{string.Join(", ", output_layer.Neurons[i].Weights)}]，偏置：[{output_layer.Neurons[i].Bias}]", Color.LightBlue);
            }
            Console.WriteLine();
            */
        }

        public double test(double[,] inputs, double[] outputs)
        {
            int test_len = inputs.GetLength(0);
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
                double true_value = normalize_single(outputs[i], output_min, output_max);
                //计算
                double[] input_values = input_row_norm;
                for (int j = 0; j < hidden_layers.Count; j++)
                {
                    input_values = compute_hidden(input_values, hidden_layers[j]);
                }
                double predict_value = compute_output(input_values);
                //求均方差
                MSE[i] = (predict_value - true_value) * (predict_value - true_value);
                //从归一化还原
                predict_value = (predict_value * (output_max - output_min + 1)) - 1 + output_min;
                true_value = outputs[i];

                //Console.WriteLine($"输入值：{input_row_str}，预测值：{predict_value.ToString(double_format)}   真实值：{true_value.ToString(double_format)}", Color.SkyBlue);
            }

            double mean_square_error = 0;
            for (int i = 0; i < test_len; i++)
            {
                mean_square_error += MSE[i];
            }

            mean_square_error = mean_square_error / test_len;

            //Console.WriteLine($"均方误差为：{mean_square_error.ToString("0.##########")}", Color.LightGreen);
            //Console.WriteLine("以上为测试集\n", Color.Orange);

            return mean_square_error;
        }

        public double ga_test(double[,] inputs, double[] outputs)
        {
            int test_len = inputs.GetLength(0);
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
                double true_value = normalize_single(outputs[i], output_min, output_max);
                //计算
                double[] input_values = input_row_norm;
                for (int j = 0; j < hidden_layers.Count; j++)
                {
                    input_values = compute_hidden(input_values, hidden_layers[j]);
                }
                double predict_value = compute_output(input_values);
                //求均方差
                MSE[i] = (predict_value - true_value) * (predict_value - true_value);
                //从归一化还原
                predict_value = (predict_value * (output_max - output_min + 1)) - 1 + output_min;
                true_value = outputs[i];

                //Console.WriteLine($"输入值：{input_row_str}，预测值：{predict_value.ToString(double_format)}   真实值：{true_value.ToString(double_format)}", Color.SkyBlue);
            }

            double mean_square_error = 0;
            for (int i = 0; i < test_len; i++)
            {
                mean_square_error += MSE[i];
            }

            mean_square_error = mean_square_error / test_len;

            //Console.WriteLine($"均方误差为：{mean_square_error.ToString("0.##########")}", Color.LightGreen);
            //Console.WriteLine("以上为测试集\n", Color.Orange);

            return mean_square_error;
        }

        public void train_test(double[,] train_inputs, double[] train_outputs, int train_epoches,
            double[,] test_inputs, double[] test_outputs)
        {
            //
            int train_len = train_inputs.GetLength(0);
            int train_test_len = 10;//train_len / 200;
            double[,] train_test_inputs = new double[train_test_len, input_dimension];
            double[] train_test_outputs = new double[train_test_len];
            for (int k = 0; k < train_test_len; k++)
            {
                for (int m = 0; m < input_dimension; m++)
                {
                    train_test_inputs[k, m] = train_inputs[k, m];
                }
                train_test_outputs[k] = train_outputs[k];
            }

            //
            (inputs_mins, inputs_maxs) = normalize_inputs(train_inputs);
            //for (int i = 0; i < inputs_mins.Length; i++)
            //{
            //    Console.WriteLine($"输入集[{i}]归一化的最大值和最小值分别为：{inputs_maxs[i].ToString(double_format)}，{inputs_mins[i].ToString(double_format)}", Color.LightGreen);
            //}
            (output_min, output_max) = normalize_array(train_outputs);
            //Console.WriteLine($"输出集归一化的最大值和最小值为：{output_max.ToString(double_format)}，{output_min.ToString(double_format)}\r\n", Color.LightGreen);

            //Console.WriteLine($"迭代次数[{train_epoches}]，隐藏层数量[{hidden_layers.Count}]，输出层数量[1]，每层神经元数量[{numOfNeurons}]，学习率[{learning_rate}]", Color.LightGreen);

            if (use_ga)
            {
                //ga算法预优化参数，没什么用，均方误差很难小于0.0001，效果不及bp
                //Console.WriteLine("开始使用GA算法预优化参数...", Color.Orange);
                ga(test_inputs, test_outputs);
                //Console.WriteLine("优化完毕", Color.Orange);
                //
            }

            DisplayParams();

            

            for (int j = 0; j < train_epoches; j++)
            {
                for (int i = 0; i < train_inputs.GetLength(0); i++)
                {
                    double[] data = new double[input_dimension];
                    for (int k = 0; k < input_dimension; k++)
                    {
                        data[k] = train_inputs[i, k];
                    }
                    bp(data, train_outputs[i]);
                }

                if ((j + 1) % 1000 == 0)
                {
                    test(train_test_inputs, train_test_outputs);
                    //Console.WriteLine($"以上是使用训练集进行测试\n", Color.Orange);

                    test(test_inputs, test_outputs);
                    //Console.WriteLine($"以上是为第{j + 1}次迭代进行测试\n", Color.Orange);
                }
            }

            DisplayParams();
        }

        public void predict(double[,] inputs)
        {
            int test_len = inputs.GetLength(0);
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
                double predict_value = compute_output(input_values);
                //从归一化还原
                predict_value = (predict_value * (output_max - output_min + 1)) - 1 + output_min;

                //Console.WriteLine($"输入值：{input_row_str}，预测值：{predict_value.ToString(double_format)}", Color.SkyBlue);
            }
            //Console.WriteLine("以上为预测集", Color.Orange);
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
            bool use_ga = false)
        {
            this.input_dimension = input_dimension;
            this.output_dimension = output_dimension;
            this.numOfNeurons = num_of_neurons;
            this.learning_rate = learning_rate;
            this.use_ga = use_ga;

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

        //使用遗传算法优化权重和偏置
        private void ga(double[,] test_inputs, double[] test_outputs)
        {
            double[] values = get_params();
            double mutation_ratio = 0.3;
            double max = 50;
            double min = -50;
            Chromosome init_chromosome = new Chromosome(values.ToList(), mutation_ratio, max, min);
            List<Chromosome> chromosomes = new List<Chromosome>();
            chromosomes.Add(init_chromosome);

            int popSize = 10;
            float xoverRatio = 0.5f;

            for (int i = 1; i < popSize; i++)
            {
                Chromosome copy = init_chromosome.GetCopy();
                copy.ChromosomeMutate();
                chromosomes.Add(copy);
            }

            Population pop = new Population(DateTime.Now.ToString("yyyyMMddHHmmss"),
                        xoverRatio, popSize, chromosomes, mutation_ratio);

            //int evoloveGens = 10000;
            List<Chromosome> historyChromosomes = new List<Chromosome>();

            int calc_count = 0;
            while (true)
            {
                pop.Generation++;
                //Console.WriteLine($"Generation: {pop.Generation}");
                if (pop.Generation > 1)
                {
                    pop.PopulationCrossover();
                    pop.ChromosomeMutate();
                }

                foreach (var chromosome in pop.Chromosomes)
                {
                    chromosome.Fitness = calc_fitness(chromosome, test_inputs, test_outputs);
                }

                //Sort chromosome in population
                pop.ChromosomeSort();

                //update selected ratio of chromosome
                pop.UpdateChromosomeSelectedRatio();

                //save population to history list
                foreach (var chrom in pop.Chromosomes)
                {
                    historyChromosomes.Add(chrom.GetCopy());
                }

                //FitnessForSum.OutputBest(pop.Chromosomes.FirstOrDefault(), $"Generation: {pop.Generation} Best ");

                double best_fitness = calc_fitness(pop.Chromosomes.FirstOrDefault(), test_inputs, test_outputs);
                //Console.WriteLine($"第[{pop.Generation}]代最佳均方误差：{1 / best_fitness}", Color.LightBlue);
                calc_count++;
                if ((1 / best_fitness <= 0.0001) || calc_count >= 10000)
                {
                    break;
                }
            }

            Population best_pop = new Population(DateTime.Now.ToString("yyyyMMddHHmmss"),
                        xoverRatio, popSize, historyChromosomes, mutation_ratio);

            double fitness = calc_fitness(best_pop.Chromosomes.FirstOrDefault(), test_inputs, test_outputs);
            //Console.WriteLine($"共迭代运算[{pop.Generation}]次，最终最佳均方误差：{1 / fitness}", Color.LightBlue);

        }

        private double[] get_params()
        {
            List<double> values = new List<double>();

            for (int i = 0; i < hidden_layers.Count; i++)
            {
                for (int j = 0; j < hidden_layers[i].Neurons.Count; j++)
                {
                    for (int k = 0; k < hidden_layers[i].Neurons[j].Weights.Length; k++)
                    {
                        values.Add(hidden_layers[i].Neurons[j].Weights[k]);
                    }
                    values.Add(hidden_layers[i].Neurons[j].Bias);
                }
            }

            for (int j = 0; j < output_layer.Neurons.Count; j++)
            {
                for (int k = 0; k < output_layer.Neurons[j].Weights.Length; k++)
                {
                    values.Add(output_layer.Neurons[j].Weights[k]);
                }
                values.Add(output_layer.Neurons[j].Bias);
            }

            return values.ToArray();
        }

        private void set_params(double[] values)
        {
            int index = 0;

            for (int i = 0; i < hidden_layers.Count; i++)
            {
                for (int j = 0; j < hidden_layers[i].Neurons.Count; j++)
                {
                    for (int k = 0; k < hidden_layers[i].Neurons[j].Weights.Length; k++)
                    {
                        hidden_layers[i].Neurons[j].Weights[k] = values[index++];
                    }
                    hidden_layers[i].Neurons[j].Bias = values[index++];
                }
            }

            for (int j = 0; j < output_layer.Neurons.Count; j++)
            {
                for (int k = 0; k < output_layer.Neurons[j].Weights.Length; k++)
                {
                    output_layer.Neurons[j].Weights[k] = values[index++];
                }
                output_layer.Neurons[j].Bias = values[index++];
            }

        }

        private double calc_fitness(Chromosome chromosome, double[,] test_inputs, double[] test_outputs)
        {
            set_params(chromosome.Values.ToArray());

            double mse = ga_test(test_inputs, test_outputs);

            return 1 / mse;
        }
    }

    public delegate void NeuralNetworkOutputMessage(string msg, NNOutputMessageType type);
    public enum NNOutputMessageType
    {

    }
}
