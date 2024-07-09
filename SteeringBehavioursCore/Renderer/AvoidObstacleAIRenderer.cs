using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Renderer
{
    public class AvoidObstacleAIRenderer : RendererSkiaSharp
    {
        public AvoidObstacleAIRenderer(SKCanvas canvas) : base(canvas)
        {
        }

        public override void Render(IField field)
        {
            base.Render(field);

            DrawField();
            DrawWall();
            this.DrawObstacles(field as AvoidObstacleAIField);
            foreach (var boid in field.Boids)
            {
                DrawTailBoid(boid, _boidColor, field.BoidDisplayBySpeed);
            }
        }

        private void DrawObstacles(AvoidObstacleAIField field)
        {
            float lineWidth = 2f;
            Color color = new Color(247, 175, 49);
            foreach (var obstacle in field.Obstacles)
            {
                for (int i = 0; i < obstacle.Points.Count - 1; i++)
                {
                    DrawLine(new Point(obstacle.Points[i].X, obstacle.Points[i].Y),
                        new Point(obstacle.Points[i + 1].X, obstacle.Points[i + 1].Y),
                        lineWidth,
                        color);
                }
                DrawLine(new Point(obstacle.Points[obstacle.Points.Count - 1].X, obstacle.Points[obstacle.Points.Count - 1].Y),
                    new Point(obstacle.Points[0].X, obstacle.Points[0].Y),
                        lineWidth,
                        color);
            }
        }
    }
}
