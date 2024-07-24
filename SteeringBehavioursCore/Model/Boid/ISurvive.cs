using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Boid
{
    internal interface ISurvive
    {
        float SurviveFactor { get; set; }
        DateTime BornTime { get; }
    }
}
