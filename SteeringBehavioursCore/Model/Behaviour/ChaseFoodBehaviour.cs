using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Behaviour
{
    internal class ChaseFoodBehaviour : Behaviour
    {
        public ChaseFoodBehaviour(IField field) : base(field)
        {
        }

        public override void CalcVelocity(IBoid curBoid)
        {
            
        }
    }
}
