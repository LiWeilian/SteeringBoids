using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AICore.NeuralNetwork
{
    internal class NeuralLayer
    {
        public string Name { get; private set; }
        public List<Neuron> Neurons { get; set; }
        public double Bias { get; set; }

        public NeuralLayer(string layer_name, int numOfNeurons, int data_dimension)
        {
            Name = string.IsNullOrEmpty(layer_name) ? DateTime.Now.ToString("yyyyMMddHHmmss") : layer_name;
            initialize_neurons(numOfNeurons, data_dimension);
            initialize_bias();
        }

        private void initialize_neurons(int num, int dimension)
        {
            //Random rand = new Random((int)DateTime.Now.Ticks);

            Neurons = new List<Neuron>();
            for (int i = 0; i < num; i++)
            {
                /*
                Neuron neuron = new Neuron();
                neuron.Weights = new double[dimension];
                for (int j = 0; j < dimension; j++)
                {
                    int w_rand = rand.Next() % 200001;
                    neuron.Weights[j] = w_rand / 100000.0 - 1;
                }

                int bias_rand = rand.Next() % 200001;
                neuron.Bias = bias_rand / 100000.0 - 1;
                */
                Neurons.Add(new Neuron(dimension));
            }
        }

        private void initialize_bias()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int bias_rand = rand.Next() % 200001;
            Bias = bias_rand / 100000.0 - 1;
        }
    }
}
