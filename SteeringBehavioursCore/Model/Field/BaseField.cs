using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Interaction;

namespace SteeringBehavioursCore.Model.Field
{
    public abstract class BaseField : IField
    {
        public static float Width { get { return 1200f; } }
        public static float Height { get { return 600f; } }
        public virtual IBoid[] Boids { get; protected set; }
        public bool BoidDisplayBySpeed { get; protected set; } = true;

        public virtual IFieldInteraction Interaction { get; protected set; }

        protected float _width, _height;

        public virtual void Advance(float stepSize = 1)
        {
            Parallel.ForEach(Boids, boid => boid.Move(stepSize));
        }

        public virtual void SetFieldSize(float width, float height)
        {
            if (width <= 0 || height <= 0)
                throw new Exception(
                    "Wrong size of field");
            (_width, _height) = (width, height);
        }
    }
}
