using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Behaviour
{
    public class FollowPathBehaviour : Behaviour
    {
        private PathFollowingField _field;

        private const float Weight = 0.005f;

        private List<BoidFollowPosition> boidPositions;
        public FollowPathBehaviour(IField field) : base(field)
        {
            _field = field as PathFollowingField;
        }

        public override void Action(IBoid curBoid)
        {
            if (boidPositions == null)
            {
                this.InitBoidNextPositions();
            }

            Position nextPos = this.FindBoidNextPosition(curBoid);

            //already in path
            if (nextPos != null)
            {
                if (nextPos.Distance(curBoid.Position) < 3f)
                {
                    nextPos = this.FindNextPosition(nextPos);
                    this.UpdateBoidNextPosition(curBoid, nextPos);
                }
                curBoid.Velocity += (nextPos - curBoid.Position) * Weight;
                return;
            }

            //find the closeness path point
            Position closenessPos = this.FindClosenessPosition(curBoid.Position);

            //arrive or follow the path
            if (closenessPos.Distance(curBoid.Position) >= 3f)
            {
                nextPos = closenessPos;
            } else
            {
                nextPos = this.FindNextPosition(closenessPos);
            }
            this.UpdateBoidNextPosition(curBoid, nextPos);
            curBoid.Velocity += (nextPos - curBoid.Position) * Weight;
        }

        private Position FindClosenessPosition(Position curPos)
        {
            Position closenessPos = _field.PathPoints[0];
            float minDistance = curPos.Distance(closenessPos);
            for (int i = 1; i < _field.PathPoints.Length; i++)
            {
                float curDistance = curPos.Distance(_field.PathPoints[i]);
                if (curDistance < minDistance)
                {
                    closenessPos = _field.PathPoints[i];
                    minDistance = curDistance;
                }
            }

            return closenessPos;
        }

        private Position FindNextPosition(Position curPos)
        {
            for (int i = 0; i < _field.PathPoints.Length; i++)
            {
                if (curPos == _field.PathPoints[i])
                {
                    if (i == _field.PathPoints.Length - 1)
                    {
                        return _field.PathPoints[1];
                    } else
                    {
                        return _field.PathPoints[i + 1];
                    }
                }
            }

            return null;
        }

        private Position FindBoidNextPosition(IBoid boid)
        {
            return boidPositions.FirstOrDefault(b => b.Boid == boid)?.NextPosition;
        }

        private void InitBoidNextPositions()
        {
            boidPositions = new List<BoidFollowPosition>();
            foreach (var boid in _field.Boids)
            {
                boidPositions.Add(new BoidFollowPosition() { Boid = boid, NextPosition = null });
            }
        }

        private void UpdateBoidNextPosition(IBoid boid, Position nextPos)
        {
            BoidFollowPosition boidPos = boidPositions.FirstOrDefault(b => b.Boid == boid);
            boidPos.NextPosition = nextPos;
        }
    }

    class BoidFollowPosition
    {
        public IBoid Boid { get; set; }
        public Position NextPosition { get; set; }
    }
}
