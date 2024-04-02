using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model
{
    public class Geometry
    {
        public static bool SegmentIntersectRectangle(
            float rectangleMinX,
            float rectangleMinY,
            float rectangleMaxX,
            float rectangleMaxY,
            float p1X,
            float p1Y,
            float p2X,
            float p2Y)
        {
            // Find min and max X for the segment
            float minX = p1X;
            float maxX = p2X;

            if (p1X > p2X)
            {
                minX = p2X;
                maxX = p1X;
            }

            // Find the intersection of the segment's and rectangle's x-projections
            if (maxX > rectangleMaxX)
            {
                maxX = rectangleMaxX;
            }

            if (minX < rectangleMinX)
            {
                minX = rectangleMinX;
            }

            if (minX > maxX) // If their projections do not intersect return false
            {
                return false;
            }

            // Find corresponding min and max Y for min and max X we found before
            float minY = p1Y;
            float maxY = p2Y;

            float dx = p2X - p1X;

            if (Math.Abs(dx) > 0.0000001)
            {
                float a = (p2Y - p1Y) / dx;
                float b = p1Y - a * p1X;
                minY = a * minX + b;
                maxY = a * maxX + b;
            }

            if (minY > maxY)
            {
                float tmp = maxY;
                maxY = minY;
                minY = tmp;
            }

            // Find the intersection of the segment's and rectangle's y-projections
            if (maxY > rectangleMaxY)
            {
                maxY = rectangleMaxY;
            }

            if (minY < rectangleMinY)
            {
                minY = rectangleMinY;
            }

            if (minY > maxY) // If Y-projections do not intersect return false
            {
                return false;
            }

            return true;
        }

        public static bool PointIntersectRetangle(
            float rectangleMinX,
            float rectangleMinY,
            float rectangleMaxX,
            float rectangleMaxY,
            float pX,
            float pY)
        {
            return pX >= rectangleMinX && pX <= rectangleMaxX
                && pY >= rectangleMinY && pY <= rectangleMaxY;
        }

        public static bool RectangleIntersectRectangle(
            float rect1MinX,
            float rect1MinY,
            float rect1MaxX,
            float rect1MaxY,
            float rect2MinX,
            float rect2MinY,
            float rect2MaxX,
            float rect2MaxY)
        {
            return PointIntersectRetangle(rect1MinX, rect1MinY, rect1MaxX, rect1MaxY, rect2MinX, rect2MinY)
                || PointIntersectRetangle(rect1MinX, rect1MinY, rect1MaxX, rect1MaxY, rect2MinX, rect2MaxY)
                || PointIntersectRetangle(rect1MinX, rect1MinY, rect1MaxX, rect1MaxY, rect2MaxX, rect2MinY)
                || PointIntersectRetangle(rect1MinX, rect1MinY, rect1MaxX, rect1MaxY, rect2MaxX, rect2MaxY)
                || PointIntersectRetangle(rect2MinX, rect2MinY, rect2MaxX, rect2MaxY, rect1MinX, rect1MinY)
                || PointIntersectRetangle(rect2MinX, rect2MinY, rect2MaxX, rect2MaxY, rect1MinX, rect1MaxY)
                || PointIntersectRetangle(rect2MinX, rect2MinY, rect2MaxX, rect2MaxY, rect1MaxX, rect1MinY)
                || PointIntersectRetangle(rect2MinX, rect2MinY, rect2MaxX, rect2MaxY, rect1MaxX, rect1MaxY);
        }
    }
}
