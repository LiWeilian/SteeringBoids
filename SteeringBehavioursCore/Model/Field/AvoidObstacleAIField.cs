using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model;
using SteeringBehavioursCore.Model.Behaviour;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Interaction;

namespace SteeringBehavioursCore.Model.Field
{
    public class AvoidObstacleAIField : BaseField, IObstacleField, ISurviveField
    {
        public List<Obstacle> Obstacles { get; }

        private const int _boidsCount = 1;
        private const int _obstacleCount = 20;
        public AvoidObstacleAIField()
        {
            _width = Width;
            _height = Height;

            BoidDisplayBySpeed = false;

            Interaction = new BaseInteraction(this);

            Obstacles = new List<Obstacle>();
            GenerateObstacles();

            Boids = new NormalBoid[_boidsCount];
            GenerateRandomBoids();
        }

        private void GenerateObstacles()
        {
            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < _obstacleCount; i++)
            {
                float minX;
                float minY;
                float maxX;
                float maxY;
                while (true)
                {
                    Thread.Sleep(10);

                    minX = (float)(100 + 800 * rnd.NextDouble());
                    minY = (float)(100 + 200 * rnd.NextDouble());

                    float width = (float)(50 + 100 * rnd.NextDouble());
                    float height = (float)(50 + 100 * rnd.NextDouble());

                    maxX = minX + width;
                    maxY = minY + height;

                    bool detected = false;
                    foreach (Obstacle obstacle in Obstacles)
                    {
                        detected = obstacle.RectangleDetected(minX, minY, maxX, maxY);
                        if (detected)
                        {
                            break;
                        }
                    }
                    if (!detected)
                    {
                        break;
                    }
                }
                Obstacles.Add(new Obstacle(minX, minY, maxX, maxY));
            }
        }

        private void GenerateRandomBoids()
        {
            var behaviours = new List<Behaviour.Behaviour>
            {
                new DetectObstacleBehaviour(this),
                new FlockBehaviour(this),
                new AlignBehaviour(this),
                new AvoidBoidsBehaviour(this),
                new AvoidObstacleBehaviour(this),
                new AvoidWallsBehaviour(this, _width, _height)
            };

            var rnd = new Random();
            for (var i = 0; i < Boids.GetLength(0); i++)
            {
                float speed = (float)(1 + rnd.NextDouble());
                Boids[i] = new AvoidObstacleAIBoid(
                    (float)rnd.NextDouble() * _width,
                    (float)rnd.NextDouble() * _height,
                    (float)(rnd.NextDouble() - .5),
                    (float)(rnd.NextDouble() - .5),
                    speed,
                    speed);

                behaviours.ForEach(
                    behaviour => Boids[i].AddBehaviour(behaviour));
            }
        }

        public float CalcSurviveFactor(float x, float y)
        {
            return 1f;
        }

        public (float?, float?) NearestObstacleIntersection(float x1, float y1, float x2, float y2)
        {
            float? nx = null;
            float? ny = null;
            float? dist = null;
            foreach (var obstacle in Obstacles)
            {
                List<float?> xs = new List<float?>();
                List<float?> ys = new List<float?>();
                (xs, ys) = obstacle.LineIntersectPoint(x1, y1, x2, y2);
                if (xs != null && ys != null)
                {
                    for (int i = 0; i < xs.Count; i++)
                    {
                        if (nx == null)
                        {
                            nx = xs[i];
                            ny = ys[i];
                            dist = (float)Math.Sqrt(Math.Pow((nx - x1).Value, 2) + Math.Pow((ny - y1).Value, 2));
                        } else
                        {
                            float? x = xs[i];
                            float? y = ys[i];
                            float dist_temp = (float)Math.Sqrt(Math.Pow((x - x1).Value, 2) + Math.Pow((y - y1).Value, 2));

                            if (dist_temp < dist)
                            {
                                nx = x;
                                ny = y;
                                dist = dist_temp;
                            }
                        }
                    }
                }
            }

            return (nx, ny);
        }

        public List<Position> ObstacleIntersections(float x1, float y1, float x2, float y2)
        {
            List<Position> intersections = new List<Position>();
            foreach (var obstacle in Obstacles)
            {
                List<float?> xs = new List<float?>();
                List<float?> ys = new List<float?>();
                (xs, ys) = obstacle.LineIntersectPoint(x1, y1, x2, y2);
                if (xs != null && ys != null)
                {
                    for (int i = 0; i < xs.Count; i++)
                    {
                        intersections.Add(new Position(xs[i].Value, ys[i].Value));
                    }
                }
            }

            return intersections;
        }
    }    
}
