using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Collision
{
    public class Collision_BoundingCircle : CollisionObject
    {
        /// <summary>
        /// Offset of centerPoint from sprite anchor.
        /// </summary>
        public Vector2 centerPointOffset { get; set; }

        /// <summary>
        /// Radius of circle.
        /// </summary>
        public float radius { get; set; }

        /// <summary>
        /// Determines which grid cells the object is in
        /// </summary>
        public override List<Vector2> gridLocations()
        {
            Vector2 bottomLeft = new Vector2(((parentObject.location.X + centerPointOffset.X - radius) / collision.gridCellWidth),
                                             ((parentObject.location.Y + centerPointOffset.Y - radius) / collision.gridCellHeight));
            Vector2 topRight = new Vector2(((parentObject.location.X + centerPointOffset.X + radius) / collision.gridCellWidth),
                                           ((parentObject.location.Y + centerPointOffset.Y + radius) / collision.gridCellHeight));

            List<Vector2> gridLocations = new List<Vector2>();
            for (int i = (int)bottomLeft.X; i <= (int)topRight.X; i++) //cols
            {
                for (int j = (int)bottomLeft.Y; j <= (int)topRight.Y; j++) //rows
                {
                    Vector2 location = new Vector2(i, j);
                }
            }

            return gridLocations;
        }

        /// <summary>
        /// Add this CollisionObject to bucket.
        /// </summary>
        public override void addToBucket()
        {
            Vector2 bottomLeft = new Vector2(((parentObject.location.X + centerPointOffset.X - radius) / collision.gridCellWidth),
                                             ((parentObject.location.Y + centerPointOffset.Y - radius) / collision.gridCellHeight));
            Vector2 topRight = new Vector2(((parentObject.location.X + centerPointOffset.X + radius) / collision.gridCellWidth),
                                           ((parentObject.location.Y + centerPointOffset.Y + radius) / collision.gridCellHeight));

            for (int i = (int)bottomLeft.X; i <= (int)topRight.X; i++) //cols
            {
                for (int j = (int)bottomLeft.Y; j <= (int)topRight.Y; j++) //rows
                {
                    Vector2 location = new Vector2(i, j);
                    if (bucket.ContainsKey(location))
                        bucket[location].Add(this);
                    else
                    {
                        List<CollisionObject> possibleCollisions = new List<CollisionObject>();
                        possibleCollisions.Add(this);
                        bucket.Add(location, possibleCollisions);
                    }
                }
            }
        }

        /// <summary>
        /// Determine if CollisionObject and Collision_BoundingCircle collide
        /// </summary>
        public override bool detectCollision(Collision_BoundingCircle boundingCircle)
        {
            double a2 = Math.Pow(parentObject.location.X + centerPointOffset.X - boundingCircle.parentObject.location.X, 2);
            double b2 = Math.Pow(parentObject.location.Y + centerPointOffset.Y - boundingCircle.parentObject.location.Y, 2);
            return (Math.Pow(radius + boundingCircle.radius,2) <= a2 + b2);
        }

        /// <summary>
        /// Determine if CollisionObject and Collision_BoundingEllipse collide
        /// </summary>
        //public override bool detectCollision(Collision_BoundingEllipse boundingEllipse);

        /// <summary>
        /// Determine if CollisionObject and Collision_AABB collide
        /// </summary>
        //public override bool detectCollision(Collision_AABB AABB);

        /// <summary>
        /// Determine if CollisionObject and Collision_BoundingOBB collide
        /// </summary>
        //public override bool detectCollision(Collision_OBB OBB);
    }
}
