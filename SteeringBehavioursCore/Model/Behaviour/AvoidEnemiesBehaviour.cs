using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Model.Boid;

namespace SteeringBehavioursCore.Model.Behaviour
{
    internal class AvoidEnemiesBehaviour : Behaviour
    {
        private const float Weight = 0.005f;

        public AvoidEnemiesBehaviour(IField field) : base(field)
        {
        }

        public override void Action(IBoid curBoid)
        {
            foreach (var boid in Boids)
                if ((boid is IEnemy) &&
                    boid.Position.Distance(curBoid.Position) < Vision)
                    curBoid.Velocity -=
                        (boid.Position - curBoid.Position) * Weight;
        }
    }
}
