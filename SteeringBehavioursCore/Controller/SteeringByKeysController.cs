using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteeringBehavioursCore.Model.Boid;
using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Renderer;

namespace SteeringBehavioursCore.Controller
{
    public class SteeringByKeysController
    {
        public IField Field { get; private set; }
        public IRenderer Renderer { get; private set; }

        public void CreateField()
        {
            Field = new SteeringByKeysField();
        }

        public void CreateRenderer(IRenderer renderer)
        {
            Renderer = renderer;
        }

        public void KeySteering(int key)
        {
            foreach (var boid in Field.Boids)
            {
                if (boid is IKeySteeringBoid)
                {
                    (boid as IKeySteeringBoid).SteeringByKey(key);
                }
            }
        }
    }
}
