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

        public static bool PointIntersectRetangle2(
            float rectangleMinX,
            float rectangleMinY,
            float rectangleMaxX,
            float rectangleMaxY,
            float pX,
            float pY)
        {
            float tolerance = 0.001f;
            return pX >= rectangleMinX - tolerance && pX <= rectangleMaxX + tolerance
                && pY >= rectangleMinY - tolerance && pY <= rectangleMaxY + tolerance;
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

        public static (float?, float?) FindIntersection(float x1, float y1,
            float x2, float y2,
            float x3, float y3,
            float x4, float y4)
        {
            float x = ((x2 - x1) * (x3 - x4) * (y3 - y1) - x3 * (x2 - x1) * (y3 - y4) + x1 * (y2 - y1) * (x3 - x4)) / ((y2 - y1) * (x3 - x4) - (x2 - x1) * (y3 - y4));
            float y = ((y2 - y1) * (y3 - y4) * (x3 - x1) - y3 * (y2 - y1) * (x3 - x4) + y1 * (x2 - x1) * (y3 - y4)) / ((x2 - x1) * (y3 - y4) - (y2 - y1) * (x3 - x4));
            return (x, y);
        }
        public static (float?, float?) FindIntersection2(float x1, float y1, 
            float x2, float y2,
            float x3, float y3, 
            float x4, float y4)
        {
            // 计算直线的斜率和截距
            float a1 = y2 - y1;
            float b1 = x1 - x2;
            float c1 = a1 * x1 + b1 * y1;

            float a2 = y4 - y3;
            float b2 = x3 - x4;
            float c2 = a2 * x3 + b2 * y3;

            float determinant = a1 * b2 - a2 * b1;

            if (determinant == 0)
            {
                // 平行或重合
                return (null, null);
            }
            else
            {
                float x = (b2 * c1 - b1 * c2) / determinant;
                float y = (a1 * c2 - a2 * c1) / determinant;
                return (x, y);
            }
        }
    }
}
