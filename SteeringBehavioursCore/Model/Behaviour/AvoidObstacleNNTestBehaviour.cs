using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;
using AICore.NeuralNetwork;
using SteeringBehavioursCore.Controller;

namespace SteeringBehavioursCore.Model.Behaviour
{
    internal class AvoidObstacleNNTestBehaviour : Behaviour
    {
        private NeuralNetwork _network;
        public AvoidObstacleNNTestBehaviour(IField field) : base(field)
        {
            InitNeuralNetwork();
        }

        private void InitNeuralNetwork()
        {
            string params_file = $"{AppDomain.CurrentDomain.BaseDirectory}\\NNTrainData\\NNParams_AvoidObstacle.txt";
            _network = new NeuralNetwork(params_file);
        }

        public override void Action(IBoid curBoid)
        {
            AvoidObstacleNNTestBoid boid = curBoid as AvoidObstacleNNTestBoid;
            if (boid == null || boid.DetectorRecorder == null)
            {
                return;
            }

            if (boid.VelocityUpdateTime != null
                && (DateTime.Now - boid.VelocityUpdateTime).Value.TotalSeconds < 0.2)
            {
                return;
            }

            double[,] inputs = new double[1,8];
            inputs[0, 0] = boid.DetectorRecorder.FrontDist;
            inputs[0, 1] = boid.DetectorRecorder.FrontLeftDist;
            inputs[0, 2] = boid.DetectorRecorder.FrontRightDist;
            inputs[0, 3] = boid.DetectorRecorder.LeftDist;
            inputs[0, 4] = boid.DetectorRecorder.RightDist;
            inputs[0, 5] = boid.DetectorRecorder.RearLeftDist;
            inputs[0, 6] = boid.DetectorRecorder.RearRightDist;
            inputs[0, 7] = boid.DetectorRecorder.RearDist;

            double[,] outputs = _network.predict(inputs);
            boid.Velocity.X = (float)outputs[0, 0];
            boid.Velocity.Y = (float)outputs[0, 1];

            boid.VelocityUpdateTime = DateTime.Now;
        }
    }
}
