using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BodilyInfection
{
    public class Collision_BoundingCircle : CollisionObject
    {
        /// <summary>
        /// Initializes a Bounding Circle.
        /// </summary>
        public Collision_BoundingCircle(int _id, Vector2 _centerPointOffset, float _radius, WorldObject _parentObject)
        {
            centerPointOffset = _centerPointOffset;
            radius = _radius;
            id = _id;
            parentObject = _parentObject;
            type = 'c';
        }

        /// <summary>
        /// Offset of centerPoint from sprite anchor.
        /// </summary>
        public Vector2 centerPointOffset { get; set; }

        /// <summary>
        /// Return Center Point of Circle (centerPointOffset + ParentObject.location)
        /// </summary>
        public Vector2 calcCenterPoint
        {
            get
            {
                return centerPointOffset + parentObject.Pos;
            }
        }

        /// <summary>
        /// Radius of circle.
        /// </summary>
        public float radius { get; set; }

        /// <summary>
        /// Determines which grid cells the object is in
        /// </summary>
        public override List<Vector2> gridLocations()
        {
            int bottomLeftX = (int)(parentObject.Pos.X + centerPointOffset.X - radius) / (int)Collision.gridCellWidth;
            int bottomLeftY = (int)(parentObject.Pos.Y + centerPointOffset.Y - radius) / (int)Collision.gridCellHeight;
            int topRightX = (int)(parentObject.Pos.X + centerPointOffset.X + radius) / (int)Collision.gridCellWidth;
            int topRightY = (int)(parentObject.Pos.Y + centerPointOffset.Y + radius) / (int)Collision.gridCellHeight;

            List<Vector2> gridLocations = new List<Vector2>();
            for (int i = bottomLeftX; i <= topRightX; i++) //cols
            {
                for (int j = bottomLeftY; j <= topRightY; j++) //rows
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
            int bottomLeftX = (int)(parentObject.Pos.X + centerPointOffset.X - radius) / (int)Collision.gridCellWidth;
            int bottomLeftY = (int)(parentObject.Pos.Y + centerPointOffset.Y - radius) / (int)Collision.gridCellHeight;
            int topRightX = (int)(parentObject.Pos.X + centerPointOffset.X + radius) / (int)Collision.gridCellWidth;
            int topRightY = (int)(parentObject.Pos.Y + centerPointOffset.Y + radius) / (int)Collision.gridCellHeight;

            for (int i = bottomLeftX; i <= topRightX; i++) //cols
            {
                for (int j = bottomLeftY; j <= topRightY; j++) //rows
                {
                    Vector2 location = new Vector2(i, j);
                    if (Collision.bucket.ContainsKey(location))
                        Collision.bucket[location].Add(this);
                    else
                    {
                        List<CollisionObject> possibleCollisions = new List<CollisionObject>();
                        possibleCollisions.Add(this);
                        Collision.bucket.Add(location, possibleCollisions);
                    }
                }
            }
        }

        public override void draw()
        {
            int numOfPoints = (int)(radius * 2);
            VertexPositionColor[] drawPoints = new VertexPositionColor[numOfPoints + 1];

            for(int i=0; i<=numOfPoints; i++)
            {
                drawPoints[i].Position = new Vector3(parentObject.Pos.X + radius * (float)Math.Cos(((double)i / (double)numOfPoints) * (Math.PI * 2))
                                                   , parentObject.Pos.Y + radius * (float)Math.Sin(((double)i / (double)numOfPoints) * (Math.PI * 2))
                                                   , 0f);
                drawPoints[i].Color = Color.Red;
            }

            This.Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, drawPoints, 0, numOfPoints);
        }
    }
}
