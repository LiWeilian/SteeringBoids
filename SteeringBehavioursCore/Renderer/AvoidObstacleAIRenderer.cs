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
            this.DrawFieldOverview(field);
            foreach (var boid in field.Boids)
            {
                if (boid is AvoidObstacleAIBoid)
                {
                    DrawDetectLine(boid as AvoidObstacleAIBoid);
                }
                DrawTailBoid(boid, _boidColor, field.BoidDisplayBySpeed);
            }
        }

        private void DrawObstacles(AvoidObstacleAIField field)
        {
            float lineWidth = 2f;
            Color color = new Color(247, 175, 49);
            Color[] colors = { new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255), new Color(255, 0, 0), new Color(0, 255, 0), new Color(0, 0, 255) };
            foreach (var obstacle in field.Obstacles)
            {
                for (int i = 0; i < obstacle.Points.Count - 1; i++)
                {
                    DrawLine(new Point(obstacle.Points[i].X, obstacle.Points[i].Y),
                        new Point(obstacle.Points[i + 1].X, obstacle.Points[i + 1].Y),
                        lineWidth,
                        colors[i]);
                }
                DrawLine(new Point(obstacle.Points[obstacle.Points.Count - 1].X, obstacle.Points[obstacle.Points.Count - 1].Y),
                    new Point(obstacle.Points[0].X, obstacle.Points[0].Y),
                        lineWidth,
                        color);
            }
        }

        private void DrawDetectLine(AvoidObstacleAIBoid boid)
        {
            float lineWidth = 1f;
            Color color = new Color(96, 96, 96);
            if (boid.FrontObstacle != null)
            {
                DrawLine(new Point(boid.Position.X, boid.Position.Y),
                            new Point(boid.FrontObstacle.X, boid.FrontObstacle.Y),
                            lineWidth,
                            color);
            }
            if (boid.LeftObstacle != null)
            {
                DrawLine(new Point(boid.Position.X, boid.Position.Y),
                            new Point(boid.LeftObstacle.X, boid.LeftObstacle.Y),
                            lineWidth,
                            color);
            }
            if(boid.RightObstacle != null)
            {
                DrawLine(new Point(boid.Position.X, boid.Position.Y),
                            new Point(boid.RightObstacle.X, boid.RightObstacle.Y),
                            lineWidth,
                            color);
            }
        }

        private void DrawFieldOverview(IField field)
        {
            float start_y_pos = 20f;
            float y_pos = start_y_pos;
            int i = 0;
            foreach (var boid in field.Boids)
            {
                y_pos = start_y_pos + i * 15f;
                DrawText($"Boid {i + 1} Pos_X: {boid.Position.X: 0.###}   Pos_Y: {boid.Position.Y: 0.###}   Speed: {boid.Velocity.GetCurrentSpeed(): 0.####}", 60f, y_pos, _wallColor);
                i++;

                if (boid is AvoidObstacleAIBoid && (boid as AvoidObstacleAIBoid).Intersections != null)
                {
                    foreach (var intersection in (boid as AvoidObstacleAIBoid).Intersections)
                    {
                        FillCircle(new Point(intersection.X, intersection.Y), 2, new Color(255, 255, 0));
                    }
                }
            }
        }
    }
}
