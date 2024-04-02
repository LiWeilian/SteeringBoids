using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Model.Boid;

namespace SteeringBehavioursCore.Model.Behaviour
{
    internal class AlignBehaviour : Behaviour
    {
        private const float Weight = 0.05f;

        public AlignBehaviour(IField field) : base(field)
        {
        }

        public override void CalcVelocity(IBoid curBoid)
        {
            var neighborCount = 0;
            var resultVelocity = new Velocity();
            foreach (var boid in Boids)
                if (boid.Position.Distance(curBoid.Position) < Vision)
                {
                    resultVelocity += boid.Velocity;
                    neighborCount += 1;
                }

            resultVelocity /= neighborCount;
            curBoid.Velocity -= (curBoid.Velocity - resultVelocity) * Weight;
        }
    }
}
