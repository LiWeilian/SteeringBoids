using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Behaviour
{
    internal class ChaseBehaviour : Behaviour
    {
        private const float Weight = 0.005f;
        private const float distance = 1500f;
        public ChaseBehaviour(IField field) : base(field)
        {
        }

        public override void CalcVelocity(IBoid curBoid)
        {
            if (curBoid is IPredator)
            {
                IBoid closeness_boid = null;
                float closeness_distance = -1f;
                foreach (var boid in Boids)
                {
                    if (boid is IPredator)
                    {
                        continue;
                    }
                    if (boid is IFood)
                    {
                        if (closeness_distance < 0f)
                        {
                            closeness_distance = boid.Position.Distance(curBoid.Position);
                            closeness_boid = boid;
                        }
                        else
                        {
                            if (boid.Position.Distance(curBoid.Position) < closeness_distance)
                            {
                                closeness_distance = boid.Position.Distance(curBoid.Position);
                                closeness_boid = boid;
                            }
                        }
                        //var closeness =
                        //    distance - boid.Position.Distance(curBoid.Position);
                        //var speed_weight = 10f / distance;
                        //if (speed_weight >= 1f)
                        //{
                        //    speed_weight = 1f;
                        //}
                        //if (closeness > 0)
                        //    curBoid.Velocity += (boid.Position - curBoid.Position) *
                        //                        Weight * speed_weight;
                    }
                }

                //var speed_weight = 10f / closeness_distance;
                //curBoid.Velocity += (closeness_boid.Position - curBoid.Position) *
                //                        Weight * (1 + speed_weight);

                float distance = curBoid.Position.Distance(closeness_boid.Position);
                if (distance > Vision)
                {
                    curBoid.Velocity += (closeness_boid.Position - curBoid.Position) * Weight;
                }
                else
                {
                    curBoid.Velocity.SetSpeed( Vision / distance * curBoid.Speed);
                }

            }
            
        }
    }
}
