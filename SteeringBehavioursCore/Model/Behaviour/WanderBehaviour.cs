using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Behaviour
{
    public class WanderBehaviour : Behaviour
    {
        private const float Weight = 0.005f;
        public WanderBehaviour(IField field) : base(field)
        {
        }

        public override void CalcVelocity(IBoid curBoid)
        {
            Position nextPos = this.GetRandomCirclePositionInTheFrontOfBoid(curBoid);
            curBoid.Velocity += (nextPos - curBoid.Position) * Weight;
        }

        private Position GetRandomCirclePositionInTheFrontOfBoid(IBoid boid)
        {
            float x = boid.Position.X;
            float y = boid.Position.Y;
            float size = boid.Size;
            float angle = boid.Velocity.GetAngle();
            float speed = boid.Velocity.GetCurrentSpeed();
            float centerX = (float)(x - size * Math.Sin(angle / 180 * Math.PI) * 4 * speed);
            float centerY = (float)(y + size * Math.Cos(angle / 180 * Math.PI) * 4 * speed);

            Random rnd = new Random();
            float theta = (float)(rnd.NextDouble() * 2 * Math.PI);
            float posX = (float)(centerX + size * Math.Cos(theta) * 10 * boid.Velocity.X);
            float posY = (float)(centerY + size * Math.Sin(theta) * 10 * boid.Velocity.Y);

            return new Position(posX, posY);
        }
    }
}
