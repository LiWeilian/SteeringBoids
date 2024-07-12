using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Behaviour
{
    internal class DetectObstacleBehaviour : Behaviour
    {
        public DetectObstacleBehaviour(IField field) : base(field)
        {
        }

        private Position NearestIntersection(float x, float y, List<Position> intersections)
        {
            Position np = null;
            float dist = 0;
            foreach (var intersection in intersections)
            {
                if (np == null)
                {
                    np = intersection;
                    dist = (float)Math.Sqrt(Math.Pow(intersection.X - x, 2) + Math.Pow(intersection.Y - y, 2));
                }
                else
                {
                    float dist_temp = (float)Math.Sqrt(Math.Pow(intersection.X - x, 2) + Math.Pow(intersection.Y - y, 2));
                    if (dist_temp < dist)
                    {
                        np = intersection;
                        dist = dist_temp;
                    }
                }
            }
            return np;
        }

        public override void CalcVelocity(IBoid curBoid)
        {
            //更新与障碍物的交点
            if (Field is IObstacleField 
                & curBoid is IObstacleDetector)
            {
                float x = curBoid.Position.X;
                float y = curBoid.Position.Y;
                float angle = curBoid.Velocity.GetAngle();

                IObstacleDetector od_boid = curBoid as IObstacleDetector;
                IObstacleField of_field = Field as IObstacleField;

                float? i_x;
                float? i_y;

                float x_f = (float)(x - Math.Sin(angle / 180 * Math.PI) * 600);
                float y_f = (float)(y + Math.Cos(angle / 180 * Math.PI) * 600);


                od_boid.Intersections = new List<Position>();

                List<Position> i_s;

                i_s = of_field.ObstacleIntersections(x, y, x_f, y_f);
                od_boid.Intersections.AddRange(i_s);

                Position np = NearestIntersection(x, y, i_s);
                if (np != null)
                {
                    od_boid.FrontObstacle = np;
                }
                else
                {
                    od_boid.FrontObstacle = new Position(x_f, y_f);
                }                
                
                float x_l = (float)(x + Math.Cos(angle / 180 * Math.PI) * 600);
                float y_l = (float)(y + Math.Sin(angle / 180 * Math.PI) * 600);

                i_s = of_field.ObstacleIntersections(x, y, x_l, y_l);
                od_boid.Intersections.AddRange(i_s);

                np = NearestIntersection(x, y, i_s);
                if (np != null)
                {
                    od_boid.LeftObstacle = np;
                }
                else
                {
                    od_boid.LeftObstacle = new Position(x_l, y_l);
                }

                
                float x_r = (float)(x - Math.Cos(angle / 180 * Math.PI) * 600);
                float y_r = (float)(y - Math.Sin(angle / 180 * Math.PI) * 600);

                i_s = of_field.ObstacleIntersections(x, y, x_r, y_r);
                od_boid.Intersections.AddRange(i_s);

                np = NearestIntersection(x, y, i_s);
                if (np != null)
                {
                    od_boid.RightObstacle = np;
                }
                else
                {
                    od_boid.RightObstacle = new Position(x_r, y_r);
                }
                
            }
        }
    }
}
