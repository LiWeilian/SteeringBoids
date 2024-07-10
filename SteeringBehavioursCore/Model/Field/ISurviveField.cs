using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Field
{
    internal interface ISurviveField
    {
        float CalcSurviveFactor(float x, float y);
    }
}
