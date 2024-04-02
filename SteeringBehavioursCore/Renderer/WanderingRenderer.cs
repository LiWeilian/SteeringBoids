using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Renderer
{
    public class WanderingRenderer : RendererSkiaSharp
    {
        public WanderingRenderer(SKCanvas canvas) : base(canvas)
        {
        }
        public override void Render(IField field)
        {
            base.Render(field);

            DrawField();
            DrawWall();
            foreach (var boid in field.Boids)
            {
                DrawTailBoid(boid, _boidColor, field.BoidDisplayBySpeed);
            }
        }
    }
}
