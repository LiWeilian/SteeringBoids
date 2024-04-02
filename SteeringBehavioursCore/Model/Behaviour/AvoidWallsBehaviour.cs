using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Model.Boid;
using System;

namespace SteeringBehavioursCore.Model.Behaviour
{
    internal class AvoidWallsBehaviour : Behaviour
    {
        public static float WallPad { get { return 50; } }
        private const float Pad = 50;
        private const float Turn = 0.3f;
        private const float Weight = 1.5f;
        private readonly float _height;
        private readonly float _width;

        public AvoidWallsBehaviour(IField field, float width, float height)
            : base(field)
        {
            _width = width;
            _height = height;
        }

        public override void CalcVelocity(IBoid curBoid)
        {
            var resultVelocity = new Velocity();

            float x = curBoid.Position.X;
            float y = curBoid.Position.Y;


            #region 碰撞时逐渐减速，然后反方向逐渐加速，但是方向调整后前端已脱离墙体，导致不能充分加速
            if (x < Pad)
                resultVelocity.X += Turn;
            if (y < Pad)
                resultVelocity.Y += Turn;
            if (x > _width - Pad)
                resultVelocity.X -= Turn;
            if (y > _height - Pad)
                resultVelocity.Y -= Turn;

            curBoid.Velocity += resultVelocity * Weight;
            #endregion

            #region 直接反弹
            //float size = curBoid.Size;
            //float angle = curBoid.Velocity.GetAngle();
            //float speed = curBoid.Velocity.GetCurrentSpeed();

            ////Arrow Position
            //x = (float)(x - size * Math.Sin(angle / 180 * Math.PI) * 2 * speed);
            //y = (float)(y + size * Math.Cos(angle / 180 * Math.PI) * 2 * speed);
            //if (x < Pad || x > _width - Pad)
            //    curBoid.Velocity.X *= -1;
            //if (y < Pad || y > _height - Pad)
            //    curBoid.Velocity.Y *= -1;
            #endregion
        }
    }
}
