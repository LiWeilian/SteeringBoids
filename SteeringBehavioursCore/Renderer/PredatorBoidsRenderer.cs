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
    public class PredatorBoidsRenderer : RendererSkiaSharp
    {
        public PredatorBoidsRenderer(SKCanvas canvas) : base(canvas)
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
            List<PredatorBoid> predators = new List<PredatorBoid>();
            List<HerbivoreBoid> herbivores = new List<HerbivoreBoid>();
            foreach (var boid in field.Boids)
            {
                if (boid is IPredator)
                {
                    predators.Add((PredatorBoid)boid);
                }
                else
                {
                    herbivores.Add((HerbivoreBoid)boid);
                }
                if (boid is ILife && ((ILife)boid).Life <= 0)
                {
                    continue;
                }

                if (boid is IPredator)
                {
                    enemies_count++;
                }
                else
                {
                    boids_count++;
                }
            }
            DrawText($"Herbivores Count: {boids_count}", 60f, 60f, _wallColor);
            DrawText($"Predators Count: {enemies_count}", 60f, 75f, _wallColor);
            float start_y_pos = 100f;
            float y_pos = start_y_pos;
            for (int i = 0; i < predators.Count; i++)
            {
                y_pos = start_y_pos + i * 15f;
                DrawText($"Predator {i + 1} Life: {predators[i].Life: 0.##}   Speed: {predators[i].Velocity.GetCurrentSpeed(): 0.####}", 60f, y_pos, _wallColor);
            }
            start_y_pos = y_pos + 25f;
            for (int i = 0; i < herbivores.Count; i++)
            {
                y_pos = start_y_pos + i * 15f;
                DrawText($"Herbivore {i + 1} Life: {herbivores[i].Life: 0.##}", 60f, y_pos, _wallColor);
            }
        }
    }
}
