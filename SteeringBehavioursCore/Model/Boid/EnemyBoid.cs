using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SteeringBehavioursCore.Model.Boid
{
    public class EnemyBoid : NormalBoid, IEnemy, ILife
    {
        private Timer _timer;
        public EnemyBoid(float x, float y, float xVel, float yVel, float speed, float minSpeed = 0.1f) 
            : base(x, y, xVel, yVel, speed, minSpeed)
        {
            Life = 1f;
            start_life_timer();
        }

        public float Life { get; private set; }

        public void Eat(IFood food)
        {
            if (food == null)
            {
                return;
            }
            float amount = 1 - Life;
            if (amount >= food.Quantity)
            {
                Life += food.Quantity;
                food.Consume(food.Quantity);
            }
            else
            {
                Life += amount;
                food.Consume(amount);
            }
        }

        private void start_life_timer()
        {
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += life_timer_Elapsed;
            _timer.Start();
        }

        private void life_timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Life > 0)
            {
                Life -= 0.01f;
                if (Life < 0f)
                {
                    Life = 0f;
                }
            }
        }
    }
}
