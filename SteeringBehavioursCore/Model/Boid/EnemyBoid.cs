using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteeringBehavioursCore.Model.Boid
{
    public class EnemyBoid : NormalBoid, IEnemy
    {
        public EnemyBoid(float x, float y, float xVel, float yVel, float speed, float minSpeed = 0.1f) 
            : base(x, y, xVel, yVel, speed, minSpeed)
        {
        }

        
    }
}
