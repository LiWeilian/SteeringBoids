using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Field
{
    public interface IObstacleField : IField
    {
        List<Obstacle> Obstacles { get; }
    }
}
