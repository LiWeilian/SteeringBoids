using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SteeringBehavioursCore.Model.Boid
{
    internal class AvoidObstacleAIBoid : NormalBoid, ISurvive, ILife, IObstacleDetector
    {
        private System.Timers.Timer _timer;
        public float SurviveFactor { get; set; }
        public float Life { get; private set; }

        #region 与障碍物交点
        public Position FrontObstacle { get; set; }
        public Position LeftObstacle { get; set; }
        public Position RightObstacle { get; set; }
        public List<Position> Intersections { get; set; }
        #endregion

        public AvoidObstacleAIBoid(float x, float y, float xVel, float yVel, float speed, float minSpeed = 0.1F) : base(x, y, xVel, yVel, speed, minSpeed)
        {
            SurviveFactor = 1f;
            Life = 1f;
            start_life_timer();
        }

        public void Eat(IFood food)
        {
            //
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
                float currentSpeed = Velocity.GetCurrentSpeed();
                Life -= 0.005f * currentSpeed;
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
