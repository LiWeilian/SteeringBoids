using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Interaction
{
    public class BaseInteraction : IFieldInteraction
    {
        protected IField _field = null;
        public BaseInteraction(IField field)
        {
            this._field = field;
        }
        public virtual void OnMouseDown(int mouse, float x, float y)
        {

        }
    }
}
