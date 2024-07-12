using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Boid
{
    internal interface IObstacleDetector
    {
        Position FrontObstacle { get; set; }
        Position LeftObstacle { get; set; }
        Position RightObstacle { get; set; }

        List<Position> Intersections { get; set; }
    }

}
