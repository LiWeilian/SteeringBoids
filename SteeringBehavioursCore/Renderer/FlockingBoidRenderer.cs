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
    public class FlockingBoidRenderer : RendererSkiaSharp
    {
        public FlockingBoidRenderer(SKCanvas canvas) : base(canvas)
        {
        }

        public override void Render(IField field)
        {
            base.Render(field);

            DrawField();
            DrawWall();
            this.DrawFieldOverview(field);
            foreach (var boid in field.Boids)
            {
                if (boid is ILife && ((ILife)boid).Life <= 0)
                {
                    continue;
                }
                if (boid is IEnemy)
                    DrawTailBoid(boid, _enemyColor, field.BoidDisplayBySpeed);
                else
                    DrawTailBoid(boid, _boidColor, field.BoidDisplayBySpeed);
            }
        }

        private void DrawFieldOverview(IField field)
        {
            int boids_count = 0;
            int enemies_count = 0;
            foreach (var boid in field.Boids)
            {
                if (boid is ILife && ((ILife)boid).Life <= 0)
                {
                    continue;
                }

                if (boid is IEnemy)
                {
                    enemies_count++;
                } else
                {
                    boids_count++;
                }
            }
            DrawText($"Boids Count: {boids_count}", 60f, 60f, _wallColor);
            DrawText($"Enemies Count: {enemies_count}", 60f, 75f, _wallColor);
        }
    }
}
