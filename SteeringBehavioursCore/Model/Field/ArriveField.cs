using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SteeringBehavioursCore.Model.Behaviour;
using SteeringBehavioursCore.Model.Interaction;
using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Model.Boid;

namespace SteeringBehavioursCore.Model.Field
{
    public class ArriveField : BaseField
    {
        private const int boidsCount = 5;
        public ArriveField()
        {
            _width = Width;
            _height = Height;

            BoidDisplayBySpeed = false;

            Interaction = new ArriveInteraction(this);

            Boids = new NormalBoid[boidsCount];

            GenerateRandomBoids();
        }

        private void GenerateRandomBoids()
        {
            var behaviours = new List<Behaviour.Behaviour>
            {
                //new FlockBehaviour(this),
                //new AlignBehaviour(this),
                //new AvoidBoidsBehaviour(this),
                new AvoidWallsBehaviour(this, _width, _height),
                new ArriveBehaviour(this)
            };

            var rnd = new Random();
            for (var i = 0; i < Boids.GetLength(0); i++)
            {
                Boids[i] = new NormalBoid(
                    (float)rnd.NextDouble() * _width,
                    (float)rnd.NextDouble() * _height,
                    (float)(rnd.NextDouble() - .5),
                    (float)(rnd.NextDouble() - .5),
                    (float)(1 + rnd.NextDouble()),
                    0f);

                behaviours.ForEach(
                    behaviour => Boids[i].AddBehaviour(behaviour));
            }
        }
    }
}
