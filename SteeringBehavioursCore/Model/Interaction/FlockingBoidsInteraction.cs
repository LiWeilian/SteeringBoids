using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Interaction
{
    public class FlockingBoidsInteraction : BaseInteraction
    {
        public FlockingBoidsInteraction(IField field) : base(field)
        {
        }
        public override void OnMouseDown(int mouse, float x, float y)
        {
            switch (mouse)
            {
                case 1048576:
                    (_field as FlockingBoidsField).IncreaseBoidsCount(10);
                    (_field as FlockingBoidsField).IncreaseEnemiesCount(1);
                    break;
                case 2097152:
                    (_field as FlockingBoidsField).DecreaseBoidsCount(10);
                    (_field as FlockingBoidsField).DecreaseEnemiesCount(1);
                    break;
                default:
                    break;
            }
        }
    }
}
