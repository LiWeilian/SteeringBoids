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
        Position FrontLeftObstacle { get; set; }
        Position FrontRightObstacle { get; set; }
        Position RearObstacle { get; set; }
        Position RearLeftObstacle { get; set; }
        Position RearRightObstacle { get; set; }
        List<Position> Intersections { get; set; }
        List<Position> FrontIntersections { get; set; }
        List<Position> FrontLeftIntersections { get; set; }
        List<Position> FrontRightIntersections { get; set; }
        List<Position> LeftIntersections { get; set; }
        List<Position> RightIntersections { get; set; }
        List<Position> RearIntersections { get; set; }
        List<Position> RearLeftIntersections { get; set; }
        List<Position> RearRightIntersections { get; set; }
        float? ObstacleDistance { get; set; }
    }

}
