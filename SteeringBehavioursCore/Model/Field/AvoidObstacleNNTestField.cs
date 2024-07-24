using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Behaviour;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Interaction;

namespace SteeringBehavioursCore.Model.Field
{
    internal class AvoidObstacleNNTestField : BaseField, IObstacleField
    {
        public List<Obstacle> Obstacles { get; }

        private const int _boidsCount = 3;
        private const int _obstacleCount = 3;

        public AvoidObstacleNNTestField()
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
            Obstacles.Add(new Obstacle(200, 150, 300, 250));
            Obstacles.Add(new Obstacle(400, 100, 525, 250));
            Obstacles.Add(new Obstacle(650, 135, 950, 350));
            Obstacles.Add(new Obstacle(450, 300, 550, 400));
            Obstacles.Add(new Obstacle(300, 375, 400, 500));
            Obstacles.Add(new Obstacle(750, 400, 850, 500));
        }

        private void GenerateRandomBoids()
        {
            var behaviours = new List<Behaviour.Behaviour>
            {
                new DetectObstacleBehaviour(this),
                new AvoidObstacleNNTestBehaviour(this),
                new FlockBehaviour(this),
                new AlignBehaviour(this),
                new AvoidBoidsBehaviour(this),
                //new AvoidObstacleBehaviour(this),
                //new AvoidWallsBehaviour(this, _width, _height)
            };

            var rnd = new Random();
            for (var i = 0; i < Boids.GetLength(0); i++)
            {
                float speed = (float)(1 + rnd.NextDouble());
                Boids[i] = new AvoidObstacleNNTestBoid(
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
        /*
        public (float?, float?) NearestObstacleIntersection(float x1, float y1, float x2, float y2)
        {
            float? nx = null;
            float? ny = null;
            float? dist = null;
            List<Obstacle> obstacles_temp = new List<Obstacle>();
            obstacles_temp.AddRange(Obstacles);
            obstacles_temp.Add(new Obstacle(50, 50, 1150, 550));
            foreach (var obstacle in obstacles_temp)
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
                        }
                        else
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
        */
        public List<Position> ObstacleIntersections(float x1, float y1, float x2, float y2)
        {
            List<Position> intersections = new List<Position>();
            List<Obstacle> obstacles_temp = new List<Obstacle>();
            obstacles_temp.AddRange(Obstacles);
            obstacles_temp.Add(new Obstacle(0, 50, 50, 550));
            obstacles_temp.Add(new Obstacle(50, 0, 1150, 50));
            obstacles_temp.Add(new Obstacle(1150, 50, 1200, 550));
            obstacles_temp.Add(new Obstacle(50, 550, 1150, 600));
            foreach (var obstacle in obstacles_temp)
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

        public bool PointIntersects(float x, float y)
        {
            List<Obstacle> obstacles_temp = new List<Obstacle>();
            obstacles_temp.AddRange(Obstacles);
            obstacles_temp.Add(new Obstacle(0, 50, 50, 550));
            obstacles_temp.Add(new Obstacle(50, 0, 1150, 50));
            obstacles_temp.Add(new Obstacle(1150, 50, 1200, 550));
            obstacles_temp.Add(new Obstacle(50, 550, 1150, 600));
            foreach (var obstacle in obstacles_temp)
            {
                if (obstacle.PointDetected(x, y))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
