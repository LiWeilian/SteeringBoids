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
    public class SteeringByKeysField : BaseField
    {
        private const int _boidsCount = 1;
        public SteeringByKeysField()
        {
            _width = Width;
            _height = Height;

            Interaction = new BaseInteraction(this);

            Boids = new KeySteeringBoid[_boidsCount];

            GenerateRandomBoids();
        }

        private void GenerateRandomBoids()
        {
            var behaviours = new List<Behaviour.Behaviour>
            {
                new FlockBehaviour(this),
                new AlignBehaviour(this),
                new AvoidBoidsBehaviour(this),
                new AvoidWallsBehaviour(this, _width, _height)
            };

            var rnd = new Random();
            for (var i = 0; i < Boids.GetLength(0); i++)
            {
                float speed = (float)(2 + rnd.NextDouble());
                Boids[i] = new KeySteeringBoid(
                    0.5f * _width,
                    0.5f * _height,
                    (float)(rnd.NextDouble() - .5),
                    (float)(rnd.NextDouble() - .5),
                    speed,
                    0.1f);

                behaviours.ForEach(
                    behaviour => Boids[i].AddBehaviour(behaviour));
            }
        }
    }
}
