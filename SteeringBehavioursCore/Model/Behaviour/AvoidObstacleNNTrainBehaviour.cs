using System;
using System.Collections.Generic;
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
            float x = curBoid.Position.X;
            float y = curBoid.Position.Y;

            //碰撞障碍物或墙体生命值直接置零


            //还生存的记录存活时间，符合条件的加入训练集
        }
    }
}
