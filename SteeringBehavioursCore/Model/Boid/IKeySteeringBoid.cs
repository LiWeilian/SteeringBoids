using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Boid
{
    public interface IKeySteeringBoid
    {
        void SteeringByKey(int key);
    }
}
