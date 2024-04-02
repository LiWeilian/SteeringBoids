using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SteeringBehavioursCore.Model;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Renderer
{
    public class PathFollowingRenderer : RendererSkiaSharp
    {
        public PathFollowingRenderer(SKCanvas canvas) : base(canvas)
        {
        }

        public override void Render(IField field)
        {
            base.Render(field);

            DrawField();
            DrawWall();
            DrawPath((field as PathFollowingField).PathPoints);
            DrawFieldOverview(field);
            foreach (var boid in field.Boids)
            {
                if (boid is IEnemy)
                    DrawTailBoid(boid, _enemyColor, field.BoidDisplayBySpeed);
                else
                    DrawTailBoid(boid, _boidColor, field.BoidDisplayBySpeed);
            }
        }

        private void DrawPath(Position[] pathPoints)
        {
            float lineWidth = 2f;
            Color color = new Color(247, 175, 49);
            for (int i = 0; i < pathPoints.Length - 1; i++)
            {
                DrawLine(new Point(pathPoints[i].X, pathPoints[i].Y),
                    new Point(pathPoints[i + 1].X, pathPoints[i + 1].Y),
                    lineWidth,
                    color);
            }
        }

        private void DrawFieldOverview(IField field)
        {
            DrawText($"Boids Count: {field.Boids.Length}", 60f, 60f, _wallColor);
        }
    }
}
