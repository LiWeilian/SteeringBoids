using SteeringBehavioursCore.Model.Interaction;
using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Model.Boid;
using System;

namespace SteeringBehavioursCore.Model.Behaviour
{
    public class ArriveBehaviour : Behaviour
    {
        private const float Weight = 0.005f;
        public ArriveBehaviour(IField field) : base(field)
        {

        }

        public override void Action(IBoid curBoid)
        {
            if ((Field.Interaction as ArriveInteraction)?.ArrivePoint == null)
            {
                return;
            }
            Position arrivePos = (Field.Interaction as ArriveInteraction).ArrivePoint;
            float distance = curBoid.Position.Distance(arrivePos);
            if (distance > Vision)
            {
                curBoid.Velocity += (arrivePos - curBoid.Position) * Weight;
            } else
            {
                //decelerate to zero
                curBoid.Velocity.SetSpeed(distance/Vision * curBoid.Speed);
            }
        }
    }
}
