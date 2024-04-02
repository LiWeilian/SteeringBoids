using SteeringBehavioursCore.Model;
using SteeringBehavioursCore.Renderer;
using SteeringBehavioursCore.Model.Field;

namespace SteeringBehavioursCore.Controller
{
    public class FlockingBoidsController
    {
        private const int BoidsCount = 50;
        private const int EnemyCount = 5;
        public IField Field { get; private set; }
        public IRenderer Renderer { get; private set; }

        public void CreateField()
        {
            Field = new FlockingBoidsField(
                BoidsCount,
                EnemyCount
            );
        }

        public void CreateRenderer(IRenderer renderer)
        {
            Renderer = renderer;
        }
    }
}
