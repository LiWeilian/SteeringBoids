using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Behaviour;
using SteeringBehavioursCore.Model.Boid;

namespace SteeringBehavioursCore.Model.Field
{
    internal class PredatorBoidsField : BaseField
    {
        private int _predator_count = 2;
        private int _herbivore_count = 20;
        private List<HerbivoreBoid> normal_boids = new List<HerbivoreBoid>();
        private List<PredatorBoid> enemy_boids = new List<PredatorBoid>();
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
        public PredatorBoidsField()
        {
            _width = Width;
            _height = Height;

            BoidDisplayBySpeed = true;

            GenerateRandomBoids(_herbivore_count, _predator_count);
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
                new FlockBehaviour(this),
                new AlignBehaviour(this),
                new AvoidBoidsBehaviour(this),
                new AvoidEnemiesBehaviour(this),
                new AvoidWallsBehaviour(this, _width, _height)
            };

            var rnd = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < boids_inc; i++)
            {
                float speed = (float)(1 + rnd.NextDouble());
                HerbivoreBoid new_boid = new HerbivoreBoid(
                    (float)rnd.NextDouble() * _width,
                    (float)rnd.NextDouble() * _height,
                    (float)(rnd.NextDouble() - .5),
                    (float)(rnd.NextDouble() - .5),
                    speed,
                    speed);
                normal_boids.Add(new_boid);

                behaviours.ForEach(behaviour => new_boid.AddBehaviour(behaviour));
            }
        }
        public void IncreaseEnemiesCount(int enemies_inc)
        {
            var behaviours = new List<Behaviour.Behaviour>
            {
                //new FlockBehaviour(this),
                //new AlignBehaviour(this),
                //new AvoidEnemiesBehaviour(this),
                new ChaseBehaviour(this),
                new PredatorBehaviour(this),
                new AvoidWallsBehaviour(this, _width, _height)
            };

            var rnd = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < enemies_inc; i++)
            {
                float speed = (float)(1 + rnd.NextDouble());
                PredatorBoid new_boid = new PredatorBoid(
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

        public void DecreaseBoidsCount(int boids_de)
        {
            int i = boids_de;
            while (i-- > 0 && normal_boids.Count > 0)
            {
                normal_boids.RemoveAt(normal_boids.Count - 1);
            }
        }

        public void DecreaseEnemiesCount(int enemies_de)
        {
            int i = enemies_de;
            while (i-- > 0 && enemy_boids.Count > 0)
            {
                enemy_boids.RemoveAt(enemy_boids.Count - 1);
            }
        }
    }
}
