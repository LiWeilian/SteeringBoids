using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Model.Boid;

namespace SteeringBehavioursCore.Model.Behaviour
{
    internal class AvoidBoidsBehaviour : Behaviour
    {
        private const float Weight = 0.005f;

        public AvoidBoidsBehaviour(IField field) : base(field)
        {
        }

        public override void CalcVelocity(IBoid curBoid)
        {
            foreach (var boid in Boids)
            {
                var closeness =
                    Distance - boid.Position.Distance(curBoid.Position);
                if (closeness > 0)
                    curBoid.Velocity -= (boid.Position - curBoid.Position) *
                                        Weight * closeness;
            }
        }
    }
}
