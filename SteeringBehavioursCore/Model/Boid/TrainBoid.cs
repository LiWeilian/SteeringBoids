using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Boid
{
    internal class TrainBoid : IBoid
    {
        private const int PositionsToRemember = 25;
        private readonly List<Behaviour.Behaviour> _behaviours;

        public float Size { get; set; }
        public Position Position { get; set; }
        public List<Position> Positions { get; set; } = new List<Position>();
        public float Speed { get; set; }
        public Velocity Velocity { get; set; }

        private float _minSpeed;

        public TrainBoid(float x, float y, float xVel, float yVel, float speed, float minSpeed = 0.1f)
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
            _behaviours.ForEach(behaviour => behaviour.Action(this));
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
    }
}
