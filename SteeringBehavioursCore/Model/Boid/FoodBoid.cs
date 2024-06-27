using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Boid
{
    internal class FoodBoid : NormalBoid, IFood, ILife
    {
        public FoodBoid(float x, float y, float xVel, float yVel, float speed, float minSpeed = 0.1F) : base(x, y, xVel, yVel, speed, minSpeed)
        {
            Life = 1f;
            Quantity = 2f;
        }

        public float Quantity { get; set; }

        public float Life { get; private set; }

        public void Consume(float amount)
        {
            Quantity -= amount;
        }

        public void Eat(IFood food)
        {
            //
        }
    }
}
