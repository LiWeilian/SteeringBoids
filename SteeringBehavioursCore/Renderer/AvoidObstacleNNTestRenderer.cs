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
    public class AvoidObstacleNNTestRenderer : RendererSkiaSharp
    {
        public AvoidObstacleNNTestRenderer(SKCanvas canvas) : base(canvas)
        {
        }
        public override void Render(IField field)
        {
            base.Render(field);

            DrawField();
            DrawWall();
            this.DrawObstacles(field as AvoidObstacleNNTestField);
            this.DrawFieldOverview(field);
            foreach (var boid in field.Boids)
            {
                if (boid is AvoidObstacleNNTestBoid
                    && (boid as AvoidObstacleNNTestBoid).Life > 0f)
                {
                    DrawDetectedLine(boid as AvoidObstacleNNTestBoid);
                    DrawDetectedObstacles(boid as IObstacleDetector);
                    DrawTailBoid(boid, _boidColor, field.BoidDisplayBySpeed);
                }
            }
        }

        private void DrawObstacles(AvoidObstacleNNTestField field)
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

        private void DrawFieldOverview(IField field)
        {
            float start_y_pos = 20f;
            float y_pos = start_y_pos;
            int i = 0;
            foreach (var boid in field.Boids)
            {
                y_pos = start_y_pos + i * 15f;
                DrawText($"Boid {i + 1} Pos_X: {boid.Position.X: 0.000}   Pos_Y: {boid.Position.Y: 0.000}  Angle: {boid.Velocity.GetAngle(): 0.000}  Speed: {boid.Velocity.GetCurrentSpeed(): 0.000}  Life: {(boid as ILife)?.Life: 0.000}", 60f, y_pos, _wallColor);
                i++;
            }
        }
        private void DrawDetectedLine(AvoidObstacleNNTestBoid boid)
        {
            float lineWidth = 1f;
            Color color = new Color(128, 128, 128);
            if (boid.FrontObstacle != null)
            {
                DrawLine(new Point(boid.Position.X, boid.Position.Y),
                            new Point(boid.FrontObstacle.X, boid.FrontObstacle.Y),
                            lineWidth,
                            color);
            }

            if (boid.FrontLeftObstacle != null)
            {
                DrawLine(new Point(boid.Position.X, boid.Position.Y),
                            new Point(boid.FrontLeftObstacle.X, boid.FrontLeftObstacle.Y),
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

            if (boid.FrontRightObstacle != null)
            {
                DrawLine(new Point(boid.Position.X, boid.Position.Y),
                            new Point(boid.FrontRightObstacle.X, boid.FrontRightObstacle.Y),
                            lineWidth,
                            color);
            }
            if (boid.RightObstacle != null)
            {
                DrawLine(new Point(boid.Position.X, boid.Position.Y),
                            new Point(boid.RightObstacle.X, boid.RightObstacle.Y),
                            lineWidth,
                            color);
            }
            if (boid.RearObstacle != null)
            {
                DrawLine(new Point(boid.Position.X, boid.Position.Y),
                            new Point(boid.RearObstacle.X, boid.RearObstacle.Y),
                            lineWidth,
                            color);
            }

            if (boid.RearLeftObstacle != null)
            {
                DrawLine(new Point(boid.Position.X, boid.Position.Y),
                            new Point(boid.RearLeftObstacle.X, boid.RearLeftObstacle.Y),
                            lineWidth,
                            color);
            }

            if (boid.RearRightObstacle != null)
            {
                DrawLine(new Point(boid.Position.X, boid.Position.Y),
                            new Point(boid.RearRightObstacle.X, boid.RearRightObstacle.Y),
                            lineWidth,
                            color);
            }
        }

        private void DrawDetectedObstacles(IObstacleDetector boid)
        {
            if (boid.FrontIntersections != null && boid.FrontIntersections.Count > 0)
            {
                FillCircle(new Point(boid.FrontIntersections[0].X, boid.FrontIntersections[0].Y), 2, new Color(255, 255, 0));
            }
            if (boid.FrontLeftIntersections != null && boid.FrontLeftIntersections.Count > 0)
            {
                FillCircle(new Point(boid.FrontLeftIntersections[0].X, boid.FrontLeftIntersections[0].Y), 2, new Color(255, 255, 0));
            }
            if (boid.FrontRightIntersections != null && boid.FrontRightIntersections.Count > 0)
            {
                FillCircle(new Point(boid.FrontRightIntersections[0].X, boid.FrontRightIntersections[0].Y), 2, new Color(255, 255, 0));
            }
            if (boid.LeftIntersections != null && boid.LeftIntersections.Count > 0)
            {
                FillCircle(new Point(boid.LeftIntersections[0].X, boid.LeftIntersections[0].Y), 2, new Color(255, 255, 0));
            }
            if (boid.RightIntersections != null && boid.RightIntersections.Count > 0)
            {
                FillCircle(new Point(boid.RightIntersections[0].X, boid.RightIntersections[0].Y), 2, new Color(255, 255, 0));
            }
            if (boid.RearIntersections != null && boid.RearIntersections.Count > 0)
            {
                FillCircle(new Point(boid.RearIntersections[0].X, boid.RearIntersections[0].Y), 2, new Color(255, 255, 0));
            }
            if (boid.RearLeftIntersections != null && boid.RearLeftIntersections.Count > 0)
            {
                FillCircle(new Point(boid.RearLeftIntersections[0].X, boid.RearLeftIntersections[0].Y), 2, new Color(255, 255, 0));
            }
            if (boid.RearRightIntersections != null && boid.RearRightIntersections.Count > 0)
            {
                FillCircle(new Point(boid.RearRightIntersections[0].X, boid.RearRightIntersections[0].Y), 2, new Color(255, 255, 0));
            }
        }
    }
}
