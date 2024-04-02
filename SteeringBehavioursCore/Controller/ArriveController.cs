using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model;
using SteeringBehavioursCore.Renderer;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Controller
{
    public class ArriveController
    {
        public IField Field { get; private set; }
        public IRenderer Renderer { get; private set; }

        public void CreateField()
        {
            Field = new ArriveField();
        }

        public void CreateRenderer(IRenderer renderer)
        {
            Renderer = renderer;
        }
    }
}
