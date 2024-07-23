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
        //(float?, float?) NearestObstacleIntersection(float x1, float y1, float x2, float y2);
        List<Position> ObstacleIntersections(float x1, float y1, float x2, float y2);
        bool PointIntersects(float x, float y);
    }
}
