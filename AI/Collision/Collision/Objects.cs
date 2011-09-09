using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Collision
{
    public class Objects
    {
        public List<CollisionHierarchy> collision = new List<CollisionHierarchy>();
        public Vector2 location;
        public VertexPositionColor[] points;
        public float velocityX;
        public float velocityY;
    }
}
