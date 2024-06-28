using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using SteeringBehavioursCore.Model.Behaviour;

namespace SteeringBehavioursCore.Model.Boid
{
    internal class PredatorBoid : NormalBoid, IEnemy, ILife, IPredator
    {
        private System.Timers.Timer _timer;
        public PredatorBoid(float x, float y, float xVel, float yVel, float speed, float minSpeed = 0.1F) : base(x, y, xVel, yVel, speed, minSpeed)
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
                Life += food.Consume(food.Quantity);
            }
            else
            {
                Life += food.Consume(Math.Max(amount, 0.123f));
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

        public override void Move(float stepSize)
        {
            _behaviours.ForEach(behaviour => behaviour.CalcVelocity(this));
            //fixed speed
            float currentSpeed = Velocity.GetCurrentSpeed();
            if (currentSpeed > 10f)
            {
                Random rnd = new Random();
                Velocity.SetSpeed((float)(10 * rnd.NextDouble()));
                //Velocity.SetSpeed(0.789f);
            }
            else if (currentSpeed < 1f)
            {
                Velocity.SetSpeed(1f);
            }
            Position.Move(Velocity, stepSize);

            Positions.Add(new Position(Position));
            while (Positions.Count > PositionsToRemember)
                Positions.RemoveAt(0);
        }
    }
}
