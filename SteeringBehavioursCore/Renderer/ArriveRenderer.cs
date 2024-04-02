using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Renderer
{
    public class ArriveRenderer : RendererSkiaSharp
    {
        public ArriveRenderer(SKCanvas canvas) : base(canvas)
        {
        }
        public override void Render(IField field)
        {
            base.Render(field);

            DrawField();
            DrawWall();
            foreach (var boid in field.Boids)
            {
                if (boid is IEnemy)
                    DrawTailBoid(boid, _enemyColor, field.BoidDisplayBySpeed);
                else
                    DrawTailBoid(boid, _boidColor, field.BoidDisplayBySpeed);
            }
        }
    }
}
