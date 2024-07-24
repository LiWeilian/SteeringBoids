using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Boid
{
    internal class AvoidObstacleNNTestBoid : NormalBoid, ISurvive, ILife, IObstacleDetector, IFood
    {
        private System.Timers.Timer _timer;
        public float SurviveFactor { get; set; }
        public float Life { get; private set; }

        public float Quantity { get; set; }

        public DateTime BornTime { get; private set; }
        public DateTime? VelocityUpdateTime { get; set; }

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
                    }
                    else
                    {
                        factor /= 1000f;
                    }
                    SurviveFactor = factor;
                }
            }
        }

        private float? _obstacle_dist;
        #endregion

        #region NN训练参数
        public ObstacleDetectorRecorder DetectorRecorder { get; private set; }
        public bool UpdateDetectorRecorder()
        {
            DetectorRecorder = new ObstacleDetectorRecorder();
            DetectorRecorder.FrontDist = CalcDistance(FrontObstacle);
            DetectorRecorder.FrontLeftDist = CalcDistance(FrontLeftObstacle);
            DetectorRecorder.FrontRightDist = CalcDistance(FrontRightObstacle);
            DetectorRecorder.LeftDist = CalcDistance(LeftObstacle);
            DetectorRecorder.RightDist = CalcDistance(RightObstacle);
            DetectorRecorder.RearLeftDist = CalcDistance(RearLeftObstacle);
            DetectorRecorder.RearRightDist = CalcDistance(RearRightObstacle);
            DetectorRecorder.RearDist = CalcDistance(RearObstacle);
            DetectorRecorder.Velocity = new Velocity(this.Velocity.X, this.Velocity.Y);

            return true;
        }
        private float CalcDistance(Position pos)
        {
            return (float)Math.Sqrt(Math.Pow((pos.X - this.Position.X), 2) + Math.Pow((pos.Y - this.Position.Y), 2));
        }
        #endregion

        public AvoidObstacleNNTestBoid(float x, float y, float xVel, float yVel, float speed, float minSpeed = 0.1F) : base(x, y, xVel, yVel, speed, minSpeed)
        {
            SurviveFactor = 0f;
            BornTime = DateTime.Now;
            Life = 1f;
            Quantity = Life;
        }

        public void Eat(IFood food)
        {
            if (food is AvoidObstacleNNTrainBoid)
            {
                Consume((food as AvoidObstacleNNTrainBoid).Life);
            }
        }

        public override void Move(float stepSize)
        {
            if (Life == 0)
            {
                //生命值为0不再活动
                return;
            }
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

        public float Consume(float amount)
        {
            float a = Life;
            Life -= amount;
            if (Life < 0f)
            {
                Life = 0f;
            }
            Quantity = Life;

            return a - Life;
        }
    }
}
