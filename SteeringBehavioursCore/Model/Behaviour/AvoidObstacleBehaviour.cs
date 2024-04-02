using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Behaviour
{
    public class AvoidObstacleBehaviour : Behaviour
    {
        private const float Weight = 0.005f;
        public AvoidObstacleBehaviour(IField field) : base(field)
        {
        }

        public override void CalcVelocity(IBoid curBoid)
        {
            IObstacleField field = Field as IObstacleField;
            if (field == null)
            {
                return;
            }

            float x = curBoid.Position.X;
            float y = curBoid.Position.Y;
            float size = curBoid.Size;
            float angle = curBoid.Velocity.GetAngle();
            float speed = curBoid.Velocity.GetCurrentSpeed();

            //Arrow Position
            x = (float)(x - size * Math.Sin(angle / 180 * Math.PI) * 10 * speed);
            y = (float)(y + size * Math.Cos(angle / 180 * Math.PI) * 10 * speed);

            foreach (var obstacle in field.Obstacles)
            {
                if (DetectObstacle(x, y, curBoid.Position.X, curBoid.Position.Y, obstacle))
                {
                    curBoid.Velocity -= (obstacle.Center - curBoid.Position) * Weight;
                }
            }
        }

        private bool DetectObstacle(float x, float y, float x2, float y2, Obstacle obstacle)
        {
            return obstacle.LineDetected(x, y, x2, y2); ;
        }
    }
}
