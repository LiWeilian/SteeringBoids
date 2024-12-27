using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Renderer
{
    public class MapEditorRenderer : RendererSkiaSharp
    {
        public MapEditorRenderer(SKCanvas canvas) : base(canvas)
        {

        }

        public override void Render(IField field)
        {
            base.Render(field);

            DrawField();
            DrawWall();

            if (field is MapEditorField)
            {
                DrawBasePaths(field as MapEditorField);
                DrawBasePathPoints(field as MapEditorField);

                DrawFieldObstacles(field as MapEditorField);
                DrawFieldPathLines(field as MapEditorField);
            }            
        }

        private void DrawBasePaths(MapEditorField field)
        {
            float lineWidth = 1f;
            Color color = new Color(128, 128, 128);
            foreach (var path in field.BasePaths)
            {
                DrawLine(new Point(path.FromPosition.Position.X, path.FromPosition.Position.Y),
                        new Point(path.ToPosition.Position.X, path.ToPosition.Position.Y),
                        lineWidth,
                        color);
            }
        }

        private void DrawBasePathPoints(MapEditorField field)
        {
            foreach (var pathPoint in field.BasePathPoints)
            {
                FillCircle(new Point(pathPoint.Position.X, pathPoint.Position.Y), 2, new Color(128, 128, 128));
            }
        }

        private void DrawFieldPathLines(MapEditorField field)
        {
            if (field.PathLines == null)
            {
                return;
            }


            float lineWidth = 2f;
            Color lineColor = new Color(255, 128, 0);

            float pointSize = 4f;
            Color pointColor = new Color(255, 128, 0);

            foreach (var line in field.PathLines)
            {
                foreach (var path in line.Paths)
                {
                    DrawLine(new Point(path.FromPosition.Position.X, path.FromPosition.Position.Y),
                        new Point(path.ToPosition.Position.X, path.ToPosition.Position.Y),
                        lineWidth,
                        lineColor);


                    FillCircle(new Point(path.FromPosition.Position.X, path.FromPosition.Position.Y),
                        pointSize,
                        pointColor);
                    FillCircle(new Point(path.ToPosition.Position.X, path.ToPosition.Position.Y),
                        pointSize,
                        pointColor);
                }
            }
        }

        private void DrawFieldObstacles(MapEditorField field)
        {
            if (field.Obstacles == null)
            {
                return;
            }
            float lineWidth = 2f;
            Color lineColor = new Color(255, 128, 0);

            float pointSize = 4f;
            Color pointColor = new Color(255, 128, 0);

            foreach(var obstacle in field.Obstacles)
            {

                for (int i = 0; i < obstacle.Points.Count - 1; i++)
                {
                    DrawLine(new Point(obstacle.Points[i].X, obstacle.Points[i].Y),
                        new Point(obstacle.Points[i + 1].X, obstacle.Points[i + 1].Y),
                        lineWidth,
                        lineColor);
                }
                DrawLine(new Point(obstacle.Points[obstacle.Points.Count - 1].X, obstacle.Points[obstacle.Points.Count - 1].Y),
                    new Point(obstacle.Points[0].X, obstacle.Points[0].Y),
                        lineWidth,
                        lineColor);
            }
        }
    }
}
