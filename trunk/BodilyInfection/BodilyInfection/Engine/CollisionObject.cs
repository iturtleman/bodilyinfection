using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BodilyInfection
{
    public abstract class CollisionObject
    {
        /// <summary>
        /// Id that will be used to determine which CollisionObject was touched.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// The type of CollisionObject (ex: c = circle, a = AABB, o = OBB, e = Ellipse)
        /// </summary>
        public char type { get; set; }

        /// <summary>
        /// Previous bucket locations
        /// </summary>
        public List<Vector2> bucketLocations = new List<Vector2>();

        public Vector2 previousPos = new Vector2();

        /// <summary>
        /// Determines which grid cells the object is in
        /// </summary>
        public abstract List<Vector2> gridLocations(WorldObject worldObject);

        /// <summary>
        /// Add this CollisionObject to bucket.
        /// </summary>
        public abstract void addToBucket(WorldObject worldObject);

        /// <summary>
        /// Holds the points that make up the linestrip for drawing
        /// </summary>
        public VertexPositionColor[] drawPoints;

        public abstract void draw(WorldObject world, Matrix transformation);

        public override int GetHashCode(){
            return id;
        }
    }
}
