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
    public class PathFollowingField : BaseField
    {
        public Position[] PathPoints { get; private set; }
        public PathFollowingField()
        {
            _width = Width;
            _height = Height;

            PathPoints = this.InitializePathPoints();

            Interaction = new BaseInteraction(this);

            Boids = new NormalBoid[20];

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
                new FollowPathBehaviour(this)
            };

            var rnd = new Random();
            for (var i = 0; i < Boids.GetLength(0); i++)
            {
                Boids[i] = new NormalBoid(
                    (float)rnd.NextDouble() * _width,
                    (float)rnd.NextDouble() * _height,
                    (float)(rnd.NextDouble() - .5),
                    (float)(rnd.NextDouble() - .5),
                    (float)(1 + rnd.NextDouble()));

                behaviours.ForEach(
                    behaviour => Boids[i].AddBehaviour(behaviour));
            }
        }

        private Position[] InitializePathPoints()
        {
            List<Position> positions = new List<Position>();
            positions.Add(new Position(200, 200));
            positions.Add(new Position(400, 250));
            positions.Add(new Position(600, 300));
            positions.Add(new Position(800, 200));
            positions.Add(new Position(1000, 400));
            positions.Add(new Position(900, 350));
            positions.Add(new Position(700, 380));
            positions.Add(new Position(400, 200));
            positions.Add(new Position(300, 300));
            positions.Add(new Position(200, 200));

            return positions.ToArray();
        }
    }
}
