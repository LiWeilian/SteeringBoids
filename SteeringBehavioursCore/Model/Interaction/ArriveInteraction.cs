using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Model.Interaction
{
    public class ArriveInteraction : BaseInteraction
    {
        public Position ArrivePoint { get; set; }
        public ArriveInteraction(IField field) : base(field)
        {

        }
        public override void OnMouseDown(int mouse, float x, float y)
        {
            switch (mouse)
            {
                case 1048576:
                    //left button
                    ArrivePoint = new Position(x, y);
                    break;
                case 2097152:
                    //right button
                    ArrivePoint = null;
                    break;
                default:
                    break;
            }
        }
    }
}
