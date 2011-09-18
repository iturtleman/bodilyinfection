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
        public Collision_BoundingCircle(int _id, Vector2 _centerPointOffset, float _radius)
        {
            centerPointOffset = _centerPointOffset;
            radius = _radius;
            id = _id;
            type = 'c';


            //create collision object's points for drawing
            int numOfPoints = (int)(radius * 2);
            drawPoints = new VertexPositionColor[numOfPoints + 1];
            for (int i = 0; i <= numOfPoints; i++)
            {
                drawPoints[i].Position = new Vector3(centerPointOffset.X + radius * (float)Math.Cos(((double)i / (double)numOfPoints) * (Math.PI * 2))
                                                   , centerPointOffset.Y + radius * (float)Math.Sin(((double)i / (double)numOfPoints) * (Math.PI * 2))
                                                   , 0f);
                drawPoints[i].Color = Color.Red;
            }
        }

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


            //create collision object's points for drawing
            int numOfPoints = (int)(radius * 2);
            drawPoints = new VertexPositionColor[numOfPoints + 1];
            for (int i = 0; i <= numOfPoints; i++)
            {
                drawPoints[i].Position = new Vector3(radius * (float)Math.Cos(((double)i / (double)numOfPoints) * (Math.PI * 2))
                                                   , radius * (float)Math.Sin(((double)i / (double)numOfPoints) * (Math.PI * 2))
                                                   , 0f);
                drawPoints[i].Color = Color.Red;
            }
        }

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
                    gridLocations.Add(location);
                }
            }

            return gridLocations;
        }

        /// <summary>
        /// Add this CollisionObject to bucket.
        /// </summary>
        public override void addToBucket(WorldObject worldObject)
        {
            int bottomLeftX = (int)(worldObject.Pos.X + centerPointOffset.X - radius) / (int)Collision.gridCellWidth;
            int bottomLeftY = (int)(worldObject.Pos.Y + centerPointOffset.Y - radius) / (int)Collision.gridCellHeight;
            int topRightX = (int)(worldObject.Pos.X + centerPointOffset.X + radius) / (int)Collision.gridCellWidth;
            int topRightY = (int)(worldObject.Pos.Y + centerPointOffset.Y + radius) / (int)Collision.gridCellHeight;

            for (int i = bottomLeftX; i <= topRightX; i++) //cols
            {
                for (int j = bottomLeftY; j <= topRightY; j++) //rows
                {
                    Vector2 location = new Vector2(i, j);
                    if (Collision.bucket.ContainsKey(location))
                        Collision.bucket[location].Add(worldObject);
                    else
                    {
                        List<WorldObject> possibleCollisions = new List<WorldObject>();
                        possibleCollisions.Add(worldObject);
                        Collision.bucket.Add(location, possibleCollisions);
                    }
                }
            }
        }

        public override void draw(WorldObject world)
        {
            Collision.basicEffect.World = Matrix.CreateTranslation(new Vector3(world.Pos,0) );

            foreach (EffectPass pass in Collision.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                This.Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, drawPoints, 0, drawPoints.Length-1);
            }
        }
    }
}
