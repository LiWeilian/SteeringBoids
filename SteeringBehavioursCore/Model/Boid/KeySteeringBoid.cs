using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Boid
{
    public class KeySteeringBoid : IBoid, IKeySteeringBoid
    {
        private const int PositionsToRemember = 25;
        private readonly List<Behaviour.Behaviour> _behaviours;

        public float Size { get; set; }
        public Position Position { get; set; }
        public List<Position> Positions { get; set; } = new List<Position>();
        public float Speed { get; set; }
        public Velocity Velocity { get; set; }

        private float _minSpeed;

        public KeySteeringBoid(float x, float y, float xVel, float yVel, float speed, float minSpeed = 0.1f)
        {
            Position = new Position(x, y);
            Velocity = new Velocity(xVel, yVel);
            Size = 6.0f;
            Speed = speed;
            _minSpeed = minSpeed;
            _behaviours = new List<Behaviour.Behaviour>();
        }

        public void AddBehaviour(Behaviour.Behaviour behaviour)
        {
            _behaviours.Add(behaviour);
        }

        public void Move(float stepSize)
        {
            _behaviours.ForEach(behaviour => behaviour.CalcVelocity(this));
            //fixed speed
            float currentSpeed = Velocity.GetCurrentSpeed();
            if (currentSpeed > Speed)
            {
                Velocity.SetSpeed(Speed);
            }
            else if (currentSpeed < _minSpeed)
            {
                Velocity.SetSpeed(_minSpeed);
            }
            Position.Move(Velocity, stepSize);

            Positions.Add(new Position(Position));
            while (Positions.Count > PositionsToRemember)
                Positions.RemoveAt(0);
        }

        public void SteeringByKey(int key)
        {
            float steeringWeight = 0.3f;
            float currentAngle = Velocity.GetAngle();

            float x, y;
            switch (key)
            {
                case 1:
                    //Keys.Up
                    if (Math.Abs(Velocity.X) <= float.Epsilon)
                    {
                        x = 0f;
                        y = steeringWeight;
                    }
                    else
                    if (Math.Abs(Velocity.Y) <= float.Epsilon)
                    {
                        x = steeringWeight;
                        y = 0f;
                    }
                    else
                    {
                        x = (float)Math.Sqrt(steeringWeight * steeringWeight / (1 + Velocity.Y / Velocity.X))
                            * (Velocity.X / Math.Abs(Velocity.X));
                        y = x * Velocity.Y / Velocity.X;
                    }
                    Velocity += new Position(x, y);
                    break;
                case 2:
                    //Keys.Down
                    if (Math.Abs(Velocity.X) <= float.Epsilon)
                    {
                        x = 0f;
                        y = steeringWeight;
                    }
                    else
                    if (Math.Abs(Velocity.Y) <= float.Epsilon)
                    {
                        x = steeringWeight;
                        y = 0f;
                    }
                    else
                    {
                        x = (float)Math.Sqrt(steeringWeight * steeringWeight / (1 + Velocity.Y / Velocity.X))
                            * (Velocity.X / Math.Abs(Velocity.X));
                        y = x * Velocity.Y / Velocity.X;
                    }
                    Velocity -= new Position(x, y);
                    break;
                case 3:
                    //Keys.Left
                    x = (float)Math.Cos(currentAngle / 180 * Math.PI) * steeringWeight;
                    y = (float)Math.Sin(currentAngle / 180 * Math.PI) * steeringWeight;
                    Velocity += new Position(x, y);
                    break;
                case 4:
                    //Keys.Right
                    x = (float)Math.Cos(currentAngle / 180 * Math.PI) * steeringWeight;
                    y = (float)Math.Sin(currentAngle / 180 * Math.PI) * steeringWeight;
                    Velocity -= new Position(x, y);
                    break;
                default:
                    break;
            }
        }
    }
}
