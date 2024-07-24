using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Behaviour
{
    internal class AvoidObstacleNNTrainBehaviour : Behaviour
    {
        public AvoidObstacleNNTrainBehaviour(IField field) : base(field)
        {
        }

        public override void Action(IBoid curBoid)
        {
            if (!(curBoid is AvoidObstacleNNTrainBoid))
            {
                return;
            }

            if (!(Field is AvoidObstacleNNTrainField))
            {
                return;
            }
            AvoidObstacleNNTrainField field = Field as AvoidObstacleNNTrainField;
            AvoidObstacleNNTrainBoid boid = curBoid as AvoidObstacleNNTrainBoid;

            float x = boid.Position.X;
            float y = boid.Position.Y;

            //碰撞障碍物或墙体生命值直接置零
            if (field.PointIntersects(x, y))
            {
                boid.Eat(boid);
                return;
            }

            //还生存的记录存活时间，符合条件的加入训练集
            var survive_time = (DateTime.Now - boid.BornTime).TotalSeconds;
            if (survive_time > 3)
            {
                AddToTrainSet(boid.DetectorRecorder, survive_time);
            }
        }
        private object obj_lock = new object();
        private void AddToTrainSet(ObstacleDetectorRecorder odr, double survive_time)
        {
            lock (obj_lock)
            {
                string dir = $"{AppDomain.CurrentDomain.BaseDirectory}\\NNTrainData";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (!Directory.Exists(dir))
                {
                    return;
                }
                string train_data_file = $"{dir}\\train_data.csv";
                string data_row = $"{odr.FrontDist},{odr.FrontLeftDist},{odr.FrontRightDist},{odr.LeftDist},{odr.RightDist},{odr.RearLeftDist},{odr.RearRightDist},{odr.RearDist},{odr.Velocity.X},{odr.Velocity.Y}\r\n";
                File.AppendAllText(train_data_file, data_row);
            }
        }
    }
}
