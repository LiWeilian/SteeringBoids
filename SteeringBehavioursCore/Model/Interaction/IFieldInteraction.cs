using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Interaction
{
    public interface IFieldInteraction
    {
        void OnMouseDown(int mouse, float x, float y);
    }
}
