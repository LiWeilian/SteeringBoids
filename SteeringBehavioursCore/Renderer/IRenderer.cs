using System;
using SteeringBehavioursCore.Model;
using SteeringBehavioursCore.Model.Field;
using SteeringBehavioursCore.Model.Boid;

namespace SteeringBehavioursCore.Renderer
{
    public interface IRenderer : IDisposable
    {
        void Render(IField field);
    }
}
