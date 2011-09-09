using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Collision
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
        /// Parent object of this CollisionObject
        /// </summary>
        public Objects parentObject { get; set; }

        /// <summary>
        /// Reference to bucket in Collision class.
        /// </summary>
        public Dictionary<Vector2, List<CollisionObject>> bucket;

        /// <summary>
        /// Main collision object
        /// </summary>
        public Collision collision;

        /// <summary>
        /// Determines which grid cells the object is in
        /// </summary>
        public abstract List<Vector2> gridLocations();

        /// <summary>
        /// Add this CollisionObject to bucket.
        /// </summary>
        public abstract void addToBucket();

        /// <summary>
        /// Call correct detectCollision function.
        /// </summary>
        public bool detectCollision(CollisionObject collisionObject)
        {
            bool returnValue = false;

            switch (collisionObject.type)
            {
                case 'c':
                    returnValue = detectCollision((Collision_BoundingCircle)collisionObject);
                    break;
                case 'e':
                    //returnValue = detectCollision((Collision_BoundingEllipse)collisionObject);
                    break;
                case 'o':
                    //returnValue = detectCollision((Collision_AABB)collisionObject);
                    break;
                case 'a':
                    //returnValue = detectCollision((Collision_OBB)collisionObject);
                    break;
            }

            return returnValue;
        }

        /// <summary>
        /// Determine if CollisionObject and Collision_BoundingCircle collide
        /// </summary>
        public abstract bool detectCollision(Collision_BoundingCircle boundingCircle);

        /// <summary>
        /// Determine if CollisionObject and Collision_BoundingEllipse collide
        /// </summary>
        //public abstract bool detectCollision(Collision_BoundingEllipse boundingEllipse);

        /// <summary>
        /// Determine if CollisionObject and Collision_AABB collide
        /// </summary>
        //public abstract bool detectCollision(Collision_AABB AABB);

        /// <summary>
        /// Determine if CollisionObject and Collision_BoundingOBB collide
        /// </summary>
        //public abstract bool detectCollision(Collision_OBB OBB);
    }
}
