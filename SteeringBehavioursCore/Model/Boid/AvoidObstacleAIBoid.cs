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
        public Position FrontLeftObstacle { get; set; }
        public Position FrontRightObstacle { get; set; }
        public Position RearObstacle { get; set; }
        public Position RearLeftObstacle { get; set; }
        public Position RearRightObstacle { get; set; }
        public List<Position> Intersections { get; set; }
        public List<Position> FrontIntersections { get; set; }
        public List<Position> FrontLeftIntersections { get; set; }
        public List<Position> FrontRightIntersections { get; set; }
        public List<Position> LeftIntersections { get; set; }
        public List<Position> RightIntersections { get; set; }
        public List<Position> RearIntersections { get; set; }
        public List<Position> RearLeftIntersections { get; set; }
        public List<Position> RearRightIntersections { get; set; }
        public float? ObstacleDistance
        {
            get
            {
                return _obstacle_dist;
            }
            set
            {
                _obstacle_dist = value;

                if (_obstacle_dist != null)
                {
                    float factor = _obstacle_dist.Value - 100f;
                    if (factor < 0f)
                    {
                        factor /= 500f;

                        if (factor < -0.2f)
                        {
                            factor = -0.2f;
                        }
                    } else
                    {
                        factor /= 1000f;
                    }
                    SurviveFactor = factor;
                }
            }
        }
        private float? _obstacle_dist;
        #endregion

        public AvoidObstacleAIBoid(float x, float y, float xVel, float yVel, float speed, float minSpeed = 0.1F) : base(x, y, xVel, yVel, speed, minSpeed)
        {
            SurviveFactor = 0f;
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
                //float currentSpeed = Velocity.GetCurrentSpeed();
                //Life -= 0.005f * currentSpeed;
                Life += SurviveFactor;
                if (Life < 0f)
                {
                    Life = 0f;
                }
            }
        }



        public override void Move(float stepSize)
        {
            _behaviours.ForEach(behaviour => behaviour.Action(this));
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
