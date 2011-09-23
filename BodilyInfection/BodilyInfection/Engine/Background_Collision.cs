using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodilyInfection
{
    class Background_Collision : WorldObject
    {
        public Background_Collision(CollisionObject col)
        {
            Col = col;
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        internal override List<CollisionObject> GetCollision()
        {
            return new List<CollisionObject>() { Col };
        }
    }
}
