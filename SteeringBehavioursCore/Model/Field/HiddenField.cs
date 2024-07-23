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
    public class HiddenField : BaseField, IObstacleField
    {
        public List<Obstacle> Obstacles { get; }
        private const int _obstacleCount = 5;

        public override IBoid[] Boids
        {
            get
            {
                List<IBoid> boids = new List<IBoid>();
                enemy_boids.ForEach(eb => boids.Add(eb));
                normal_boids.ForEach(nb => boids.Add(nb));

                return boids.ToArray();
            }
        }

        private List<NormalBoid> normal_boids = new List<NormalBoid>();
        private List<EnemyBoid> enemy_boids = new List<EnemyBoid>();

        public HiddenField()
        {
            _width = Width;
            _height = Height;

            BoidDisplayBySpeed = false;

            Interaction = new BaseInteraction(this);

            Obstacles = new List<Obstacle>();
            GenerateObstacles();

            GenerateRandomBoids(5, 1);
        }

        private void GenerateObstacles()
        {
            Random rnd = new Random();
            for (int i = 0; i < _obstacleCount; i++)
            {
                Obstacles.Add(new Obstacle());
            }
        }

        private void GenerateRandomBoids(int boidsCount, int enemyCount)
        {
            this.IncreaseEnemiesCount(enemyCount);
            this.IncreaseBoidsCount(boidsCount);
        }

        public void IncreaseBoidsCount(int boids_inc)
        {
            var behaviours = new List<Behaviour.Behaviour>
            {
                //new FlockBehaviour(this),
                //new AlignBehaviour(this),
                //new AvoidBoidsBehaviour(this),
                new AvoidEnemiesBehaviour(this),
                new AvoidObstacleBehaviour(this),
                new HiddenBehaviour(this),
                new AvoidWallsBehaviour(this, _width, _height)
            };

            var rnd = new Random();

            for (int i = 0; i < boids_inc; i++)
            {
                NormalBoid new_boid = new NormalBoid(
                    (float)rnd.NextDouble() * _width,
                    (float)rnd.NextDouble() * _height,
                    (float)(rnd.NextDouble() - .5),
                    (float)(rnd.NextDouble() - .5),
                    (float)(1 + rnd.NextDouble()),
                    0f);
                normal_boids.Add(new_boid);

                behaviours.ForEach(behaviour => new_boid.AddBehaviour(behaviour));
            }
        }
        public void IncreaseEnemiesCount(int enemies_inc)
        {
            var behaviours = new List<Behaviour.Behaviour>
            {
                new FlockBehaviour(this),
                new AlignBehaviour(this),
                new AvoidBoidsBehaviour(this),
                new AvoidEnemiesBehaviour(this),
                new AvoidObstacleBehaviour(this),
                new HiddenBehaviour(this),
                new WanderBehaviour(this),
                new AvoidWallsBehaviour(this, _width, _height)
            };

            var rnd = new Random();

            for (int i = 0; i < enemies_inc; i++)
            {
                float speed = (float)(1.5 + rnd.NextDouble());
                EnemyBoid new_boid = new EnemyBoid(
                    (float)rnd.NextDouble() * _width,
                    (float)rnd.NextDouble() * _height,
                    (float)(rnd.NextDouble() - .5),
                    (float)(rnd.NextDouble() - .5),
                    speed,
                    speed);
                enemy_boids.Add(new_boid);

                behaviours.ForEach(behaviour => new_boid.AddBehaviour(behaviour));
            }
        }

        public (float?, float?) NearestObstacleIntersection(float x1, float y1, float x2, float y2)
        {
            throw new NotImplementedException();
        }

        public List<Position> ObstacleIntersections(float x1, float y1, float x2, float y2)
        {
            throw new NotImplementedException();
        }

        public bool PointIntersects(float x, float y)
        {
            throw new NotImplementedException();
        }
    }
}
