using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Behaviour
{
    public class HiddenBehaviour : Behaviour
    {
        private const float Weight = 0.005f;
        public HiddenBehaviour(IField field) : base(field)
        {
        }

        public override void Action(IBoid curBoid)
        {
            IObstacleField field = Field as IObstacleField;
            if (field == null)
            {
                return;
            }

            if (curBoid is IEnemy)
            {
                foreach (var obstacle in field.Obstacles)
                {
                    obstacle.UpdateHiddenPosition(new List<Position> { curBoid.Position });
                }
            } else
            {
                Position closenessPos = null;
                float closenessDist = float.MaxValue;
                foreach (var obstacle in field.Obstacles)
                {
                    if (obstacle.HiddenPosition == null)
                    {
                        continue;
                    }

                    float distance = curBoid.Position.Distance(obstacle.HiddenPosition);
                    if (closenessPos == null || distance < closenessDist)
                    {
                        closenessPos = obstacle.HiddenPosition;
                        closenessDist = distance;
                    }
                }
                if (closenessPos != null)
                {
                    if (closenessDist > 20)
                    {
                        //accelerate to max speed
                        curBoid.Velocity += (closenessPos - curBoid.Position) * Weight;
                    }
                    else
                    {
                        //decelerate to zero
                        curBoid.Velocity.SetSpeed(closenessDist / Vision * curBoid.Speed);
                    }
                }
            }
        }
    }
}
