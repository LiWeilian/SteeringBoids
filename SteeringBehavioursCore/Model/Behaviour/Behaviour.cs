using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Model.Boid;

namespace SteeringBehavioursCore.Model.Behaviour
{
    public abstract class Behaviour
    {
        public const float Distance = 20;
        public const float Vision = 100;
        public IBoid[] Boids { get { return Field.Boids; } }
        public IField Field { get; private set; }

        protected Behaviour(IField field)
        {
            Field = field;
        }

        public abstract void CalcVelocity(IBoid curBoid);
    }
}
