using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AICore.NeuralNetwork
{
    public class Neuron
    {
        public double[] Weights { get; set; }
        public double Bias { get; set; }

        public Neuron(int num)
        {
            initialzie_weightis(num);
            initialzie_bias();
        }

        private void initialzie_weightis(int num)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            Weights = new double[num];
            for (int j = 0; j < num; j++)
            {
                int w_rand = rand.Next() % 200001;
                Weights[j] = w_rand / 100000.0 - 1;
            }
        }

        private void initialzie_bias()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            int bias_rand = rand.Next() % 200001;
            Bias = bias_rand / 100000.0 - 1;
        }

        public void update_weights(double[] data, double learning_rate, double error)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Weights[i] = Weights[i] + learning_rate * error * data[i];
            }
        }

        public void update_bias(double learning_rate, double error)
        {
            Bias += learning_rate * error;
        }

        public void Update(double[] data, double learning_rate, double error)
        {
            for (int i = 0; i < data.Length; i++)
            {
                Weights[i] = Weights[i] + learning_rate * error * data[i];
            }
            Bias += learning_rate * error;
        }
    }
}
