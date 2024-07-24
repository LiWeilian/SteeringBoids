using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AICore.NeuralNetwork
{
    internal class NeuralNetworkParams
    {
        public List<NeuralLayer> hidden_layers { get; set; }
        public NeuralLayer output_layer { get; set; }
        public int numOfNeurons { get; set; }

        public double learning_rate { get; set; }

        public bool use_ga { get; set; }

        public double[] inputs_mins { get; set; }
        public double[] inputs_maxs { get; set; }
        public double[] outputs_mins { get; set; }
        public double[] outputs_maxs { get; set; }

        public int input_dimension { get; set; }
        public int output_dimension { get; set; }

        public string double_format { get; set; }

        public void Save(string params_file)
        {
            string params_obj_str = JsonConvert.SerializeObject(this);
            File.WriteAllText(params_file, params_obj_str);
        }

        public void Load(string params_file)
        {
            string params_obj_str = File.ReadAllText(params_file);
            NeuralNetworkParams params_obj = JsonConvert.DeserializeObject<NeuralNetworkParams>(params_obj_str);

            this.hidden_layers = params_obj.hidden_layers;
            this.output_layer = params_obj.output_layer;
            this.numOfNeurons = params_obj.numOfNeurons;
            this.learning_rate = params_obj.learning_rate;
            this.use_ga = params_obj.use_ga;
            this.inputs_mins = params_obj.inputs_mins;
            this.inputs_maxs = params_obj.inputs_maxs;
            this.outputs_mins = params_obj.outputs_mins;
            this.outputs_maxs = params_obj.outputs_maxs;
            this.input_dimension = params_obj.input_dimension;
            this.output_dimension = params_obj.output_dimension;
            this.double_format = params_obj.double_format;
        }
    }
}
