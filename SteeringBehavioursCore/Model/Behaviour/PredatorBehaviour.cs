using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Behaviour
{
    internal class PredatorBehaviour : Behaviour
    {
        public PredatorBehaviour(IField field) : base(field)
        {
        }

        public override void Action(IBoid curBoid)
        {
            if (curBoid is IPredator && curBoid is ILife)
            {
                foreach (var boid in Boids)
                {
                    if (boid is IPredator)
                    {
                        continue;
                    }

                    if (boid is ILife && boid is IFood)
                    {
                        if (boid.Position.Distance(curBoid.Position) < 20)
                        {
                            ((ILife)curBoid).Eat((IFood)boid);
                        }
                    }
                }
            }
        }
    }
}
