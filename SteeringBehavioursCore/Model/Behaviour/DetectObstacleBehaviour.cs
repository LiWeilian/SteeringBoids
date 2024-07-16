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

        private Position NearestIntersection(float x, float y, List<Position> intersections, out float? dist)
        {
            Position np = null;
            dist = null;
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

                float? dist_nearest = null;
                float? dist_temp = null;

                //正前方
                float x_f = (float)(x - Math.Sin(angle / 180 * Math.PI) * 600);
                float y_f = (float)(y + Math.Cos(angle / 180 * Math.PI) * 600);

                List<Position> i_s = of_field.ObstacleIntersections(x, y, x_f, y_f);
                od_boid.FrontIntersections = new List<Position>();

                Position np = NearestIntersection(x, y, i_s, out dist_temp);
                if (np != null)
                {
                    od_boid.FrontIntersections.Add(np);
                    od_boid.FrontObstacle = np;
                    if (dist_nearest == null || dist_nearest.Value > dist_temp.Value)
                    {
                        dist_nearest = dist_temp;
                    }
                }
                else
                {
                    od_boid.FrontObstacle = new Position(x_f, y_f);
                }

                //左前方
                float x_fl = (float)(x - Math.Sin((angle - 45) / 180 * Math.PI) * 600);
                float y_fl = (float)(y + Math.Cos((angle - 45) / 180 * Math.PI) * 600);

                i_s = of_field.ObstacleIntersections(x, y, x_fl, y_fl);
                od_boid.FrontLeftIntersections = new List<Position>();

                np = NearestIntersection(x, y, i_s, out dist_temp);
                if (np != null)
                {
                    od_boid.FrontLeftIntersections.Add(np);
                    od_boid.FrontLeftObstacle = np;
                    if (dist_nearest == null || dist_nearest.Value > dist_temp.Value)
                    {
                        dist_nearest = dist_temp;
                    }
                }
                else
                {
                    od_boid.FrontLeftObstacle = new Position(x_fl, y_fl);
                }

                //左方
                float x_l = (float)(x + Math.Cos(angle / 180 * Math.PI) * 600);
                float y_l = (float)(y + Math.Sin(angle / 180 * Math.PI) * 600);

                i_s = of_field.ObstacleIntersections(x, y, x_l, y_l);
                od_boid.LeftIntersections = new List<Position>();

                np = NearestIntersection(x, y, i_s, out dist_temp);
                if (np != null)
                {
                    od_boid.LeftIntersections.Add(np);
                    od_boid.LeftObstacle = np;
                    if (dist_nearest == null || dist_nearest.Value > dist_temp.Value)
                    {
                        dist_nearest = dist_temp;
                    }
                }
                else
                {
                    od_boid.LeftObstacle = new Position(x_l, y_l);
                }

                //右前方
                float x_fr = (float)(x - Math.Sin((angle + 45) / 180 * Math.PI) * 600);
                float y_fr = (float)(y + Math.Cos((angle + 45) / 180 * Math.PI) * 600);

                i_s = of_field.ObstacleIntersections(x, y, x_fr, y_fr);
                od_boid.FrontRightIntersections = new List<Position>();

                np = NearestIntersection(x, y, i_s, out dist_temp);
                if (np != null)
                {
                    od_boid.FrontRightIntersections.Add(np);
                    od_boid.FrontRightObstacle = np;
                    if (dist_nearest == null || dist_nearest.Value > dist_temp.Value)
                    {
                        dist_nearest = dist_temp;
                    }
                }
                else
                {
                    od_boid.FrontRightObstacle = new Position(x_fr, y_fr);
                }

                //右方
                float x_r = (float)(x - Math.Cos(angle / 180 * Math.PI) * 600);
                float y_r = (float)(y - Math.Sin(angle / 180 * Math.PI) * 600);

                i_s = of_field.ObstacleIntersections(x, y, x_r, y_r);
                od_boid.RightIntersections = new List<Position>();

                np = NearestIntersection(x, y, i_s, out dist_temp);
                if (np != null)
                {
                    od_boid.RightIntersections.Add(np);
                    od_boid.RightObstacle = np;
                    if (dist_nearest == null || dist_nearest.Value > dist_temp.Value)
                    {
                        dist_nearest = dist_temp;
                    }
                }
                else
                {
                    od_boid.RightObstacle = new Position(x_r, y_r);
                }

                //正后方
                float x_rear = (float)(x - Math.Sin((angle + 180) / 180 * Math.PI) * 600);
                float y_rear = (float)(y + Math.Cos((angle + 180) / 180 * Math.PI) * 600);

                i_s = of_field.ObstacleIntersections(x, y, x_rear, y_rear);
                od_boid.RearIntersections = new List<Position>();

                np = NearestIntersection(x, y, i_s, out dist_temp);
                if (np != null)
                {
                    od_boid.RearIntersections.Add(np);
                    od_boid.RearObstacle = np;
                    if (dist_nearest == null || dist_nearest.Value > dist_temp.Value)
                    {
                        dist_nearest = dist_temp;
                    }
                }
                else
                {
                    od_boid.RearObstacle = new Position(x_rear, y_rear);
                }

                //左后方
                float x_rl = (float)(x - Math.Sin((angle - 135) / 180 * Math.PI) * 600);
                float y_rl = (float)(y + Math.Cos((angle - 135) / 180 * Math.PI) * 600);

                i_s = of_field.ObstacleIntersections(x, y, x_rl, y_rl);
                od_boid.RearLeftIntersections = new List<Position>();

                np = NearestIntersection(x, y, i_s, out dist_temp);
                if (np != null)
                {
                    od_boid.RearLeftIntersections.Add(np);
                    od_boid.RearLeftObstacle = np;
                    if (dist_nearest == null || dist_nearest.Value > dist_temp.Value)
                    {
                        dist_nearest = dist_temp;
                    }
                }
                else
                {
                    od_boid.RearLeftObstacle = new Position(x_rl, y_rl);
                }

                //右前方
                float x_rr = (float)(x - Math.Sin((angle + 135) / 180 * Math.PI) * 600);
                float y_rr = (float)(y + Math.Cos((angle + 135) / 180 * Math.PI) * 600);

                i_s = of_field.ObstacleIntersections(x, y, x_rr, y_rr);
                od_boid.RearRightIntersections = new List<Position>();

                np = NearestIntersection(x, y, i_s, out dist_temp);
                if (np != null)
                {
                    od_boid.RearRightIntersections.Add(np);
                    od_boid.RearRightObstacle = np;
                    if (dist_nearest == null || dist_nearest.Value > dist_temp.Value)
                    {
                        dist_nearest = dist_temp;
                    }
                }
                else
                {
                    od_boid.RearRightObstacle = new Position(x_rr, y_rr);
                }

                od_boid.ObstacleDistance = dist_nearest;
            }
        }
    }
}
