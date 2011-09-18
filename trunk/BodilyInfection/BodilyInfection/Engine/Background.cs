using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodilyInfection
{
    class Background : Sprite
    {

        public Background(string name, Actor actor)
            : this(name, actor, 0)
        {
        }

        public Background(string name, Actor actor, int layer)
            :base(name, actor)
        {
            // Allows layering of background objects
            ZOrder = int.MinValue + layer;
        }

        /*internal void Draw()
        {
            
        }

        internal void DrawCollisions()
        {
            
        }*/
    }
}
