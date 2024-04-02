using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Behaviour;

namespace SteeringBehavioursCore.Model.Boid
{
    public interface IBoid
    {
        float Size { get; set; }
        Position Position { get; set; }
        List<Position> Positions { get; set; }
        float Speed { get; set; }
        Velocity Velocity { get; set; }

        void AddBehaviour(Behaviour.Behaviour behaviour);
        void Move(float stepSize);
    }
}
