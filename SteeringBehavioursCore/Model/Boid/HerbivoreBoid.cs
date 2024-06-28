using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Boid
{
    internal class HerbivoreBoid : NormalBoid, IFood, ILife, IHerbivore
    {
        private DateTime _latest_consume_time = DateTime.Now;
        public HerbivoreBoid(float x, float y, float xVel, float yVel, float speed, float minSpeed = 0.1F) : base(x, y, xVel, yVel, speed, minSpeed)
        {
            Life = 1f;
            Quantity = Life;
        }

        public float Quantity { get; set; }

        public float Life { get; private set; }

        public float Consume(float amount)
        {
            float actual_consume = 0;
            if ((DateTime.Now - _latest_consume_time).TotalMilliseconds < 1000)
            {
                return actual_consume;
            }
            //Quantity = 0f;
            //Life = 0f;
            //if (amount >= 0.2)
            //{
            //    Quantity = 0f;
            //    Life = 0f;
            //    return;
            //}

            actual_consume = Math.Min(Quantity, amount);

            Quantity -= actual_consume;
            if (Quantity < 0)
            {
                Quantity = 0;
            }
            Life = Quantity;
            _latest_consume_time = DateTime.Now;

            return actual_consume;
        }

        public void Eat(IFood food)
        {
            //
        }
    }
}
