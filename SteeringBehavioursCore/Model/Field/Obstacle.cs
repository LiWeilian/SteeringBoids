﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Field
{
    public class Obstacle
    {
        public List<Position> Points { get; } = new List<Position>();
        public Position Center
        {
            get
            {
                return new Position((Points[0].X + Points[2].X) / 2f, (Points[0].Y + Points[2].Y) / 2);
            }
        }

        public Obstacle()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            Thread.Sleep(10);

            float x = (float)(100 + 800 * rnd.NextDouble());
            float y = (float)(100 + 200 * rnd.NextDouble());

            float width = (float)(50 + 100 * rnd.NextDouble());
            float height = (float)(50 + 100 * rnd.NextDouble());

            Points.Add(new Position(x, y));
            Points.Add(new Position(x, y + height));
            Points.Add(new Position(x + width, y + height));
            Points.Add(new Position(x + width, y));
        }

        public Obstacle(float minX, float minY, float maxX, float maxY)
        {
            Points.Add(new Position(minX, minY));
            Points.Add(new Position(minX, maxY));
            Points.Add(new Position(maxX, maxY));
            Points.Add(new Position(maxX, minY));
        }

        public bool LineDetected(float x, float y, float x2, float y2)
        {
            return Geometry.SegmentIntersectRectangle(Points[0].X,
                Points[0].Y,
                Points[2].X,
                Points[2].Y,
                x,
                y,
                x2,
                y2);
        }

        public bool PointDetected(float x, float y)
        {
            return Geometry.PointIntersectRetangle(Points[0].X,
                Points[0].Y,
                Points[2].X,
                Points[2].Y,
                x, y);
        }

        public bool RectangleDetected(float minX, float minY, float maxX, float maxY)
        {
            return Geometry.RectangleIntersectRectangle(Points[0].X,
                Points[0].Y,
                Points[2].X,
                Points[2].Y,
                minX,
                minY,
                maxX,
                maxY);
        }

        public Position HiddenPosition { get; private set; }

        public void UpdateHiddenPosition(List<Position> antiPositioins)
        {
            if (antiPositioins.Count == 0)
            {
                return;
            }

            Position center = Center;
            Position antiPos = antiPositioins[0];
            HiddenPosition = new Position(center.X * 2 - antiPos.X, center.Y * 2 - antiPos.Y);
            float distanceToCenter = HiddenPosition.Distance(center);
            float extenterRadius = center.Distance(Points[0]);
            if (distanceToCenter > extenterRadius)
            {
                float x = center.X - extenterRadius * (center.X - HiddenPosition.X) / distanceToCenter;
                float y = center.Y - extenterRadius * (center.Y - HiddenPosition.Y) / distanceToCenter;

                HiddenPosition = new Position(x, y);
            }
        }
    }
}
