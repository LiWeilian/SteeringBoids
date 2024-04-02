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
    public class AvoidObstacleField : BaseField, IObstacleField
    {
        public List<Obstacle> Obstacles { get; }
        private const int _boidsCount = 50;
        private const int _obstacleCount = 5;
        public AvoidObstacleField()
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
                Boids[i] = new NormalBoid(
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
    }

    
}
