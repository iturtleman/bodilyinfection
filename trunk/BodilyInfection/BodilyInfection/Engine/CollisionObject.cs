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
        /// Parent object of this CollisionObject
        /// </summary>
        public WorldObject parentObject { get; set; }

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
            bool returnValue = true;

            char[] types_cArray = new char[2];
            types_cArray[0] = type;
            types_cArray[1] = collisionObject.type;
            string types = new string(types_cArray) ;

            switch (types) //[0] = this.type; [1] = collisionObject.type
            {
                case "cc":
                    returnValue = Collision.detectCollision((Collision_BoundingCircle)this, (Collision_BoundingCircle)collisionObject);
                    break;
                case "aa":
                    //returnValue = Collision.detectCollision((Collision_AABB)this, (Collision_AABB)collisionObject);
                    break;
                case "ee":
                    //returnValue = Collision.detectCollision((Collision_BoundingEllipse)this, (Collision_BoundingEllipse)collisionObject);
                    break;
                case "oo":
                    //returnValue = Collision.detectCollision((Collision_OBB)this, (Collision_OBB)collisionObject);
                    break;
                case "ca":
                    returnValue = Collision.detectCollision((Collision_AABB)collisionObject, (Collision_BoundingCircle)this);
                    break;
                case "ac":
                    returnValue = Collision.detectCollision((Collision_AABB)this, (Collision_BoundingCircle)collisionObject);
                    break;
                case "co":
                    //returnValue = Collision.detectCollision((Collision_BoundingCircle)this, (Collision_OBB)collisionObject);
                    break;
                case "oc":
                    //returnValue = Collision.detectCollision((Collision_OBB)this, (Collision_BoundingCircle)collisionObject);
                    break;
                case "ce":
                    //returnValue = Collision.detectCollision((Collision_BoundingCircle)this, (Collision_BoundingEllipse)collisionObject);
                    break;
                case "ec":
                    //returnValue = Collision.detectCollision((Collision_BoundingEllipse)this, (Collision_BoundingCircle)collisionObject);
                    break;
                case "ao":
                    //returnValue = Collision.detectCollision((Collision_AABB)this, (Collision_OBB)collisionObject);
                    break;
                case "oa":
                    //returnValue = Collision.detectCollision((Collision_OBB)this, (Collision_AABB)collisionObject);
                    break;
                case "ae":
                    //returnValue = Collision.detectCollision((Collision_AABB)this, (Collision_BoundingEllipse)collisionObject);
                    break;
                case "ea":
                    //returnValue = Collision.detectCollision((Collision_BoundingEllipse)this, (Collision_AABB)collisionObject);
                    break;
                case "eo":
                    //returnValue = Collision.detectCollision((Collision_BoundingEllipse)this, (Collision_OBB)collisionObject);
                    break;
                case "oe":
                    //returnValue = Collision.detectCollision((Collision_OBB)this, (Collision_BoundingEllipse)collisionObject);
                    break;
            }

            return returnValue;
        }

        public abstract void draw();
    }
}
