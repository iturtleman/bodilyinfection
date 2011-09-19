﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BodilyInfection
{
    public class Collision_AABB : CollisionObject
    {
        /// <summary>
        /// Initializes a Bounding Circle.
        /// </summary>
        public Collision_AABB(int _id, Vector2 _topLeftPointOffset, Vector2 _bottomRightPointOffset)
        {
            topLeftPointOffset = _topLeftPointOffset;
            bottomRightPointOffset = _bottomRightPointOffset;
            id = _id;
            type = 'a';


            //create collision object's points for drawing
            drawPoints = new VertexPositionColor[5];
            drawPoints[0].Position = new Vector3(topLeftPointOffset.X, topLeftPointOffset.Y, 0f);
            drawPoints[0].Color = Color.Red;
            drawPoints[1].Position = new Vector3(topLeftPointOffset.X, bottomRightPointOffset.Y, 0f);
            drawPoints[1].Color = Color.Red;
            drawPoints[2].Position = new Vector3(bottomRightPointOffset.X, bottomRightPointOffset.Y, 0f);
            drawPoints[2].Color = Color.Red;
            drawPoints[3].Position = new Vector3(bottomRightPointOffset.X, topLeftPointOffset.Y, 0f);
            drawPoints[3].Color = Color.Red;
            drawPoints[4].Position = new Vector3(topLeftPointOffset.X, topLeftPointOffset.Y, 0f);
            drawPoints[4].Color = Color.Red;
        }

        /// <summary>
        /// Initializes a Bounding Circle.
        /// </summary>
        public Collision_AABB(int _id, Vector2 _topLeftPointOffset, Vector2 _bottomRightPointOffset, WorldObject _parentObject)
        {
            topLeftPointOffset = _topLeftPointOffset;
            bottomRightPointOffset = _bottomRightPointOffset;
            id = _id;
            parentObject = _parentObject;
            type = 'a';


            //create collision object's points for drawing
            drawPoints = new VertexPositionColor[5];
            drawPoints[0].Position = new Vector3(topLeftPointOffset.X, topLeftPointOffset.Y, 0f);
            drawPoints[0].Color = Color.Red;
            drawPoints[1].Position = new Vector3(topLeftPointOffset.X, bottomRightPointOffset.Y, 0f);
            drawPoints[1].Color = Color.Red;
            drawPoints[2].Position = new Vector3(bottomRightPointOffset.X, bottomRightPointOffset.Y, 0f);
            drawPoints[2].Color = Color.Red;
            drawPoints[3].Position = new Vector3(bottomRightPointOffset.X, topLeftPointOffset.Y, 0f);
            drawPoints[3].Color = Color.Red;
            drawPoints[4].Position = new Vector3(topLeftPointOffset.X, topLeftPointOffset.Y, 0f);
            drawPoints[4].Color = Color.Red;
        }

        /// <summary>
        /// Offset of topLeftPoint from sprite anchor.
        /// </summary>
        public Vector2 topLeftPointOffset { get; set; }

        /// <summary>
        /// Offset of bottomLeftPoint from sprite anchor.
        /// </summary>
        public Vector2 bottomRightPointOffset { get; set; }

        /// <summary>
        /// Determines which grid cells the object is in
        /// </summary>
        public override List<Vector2> gridLocations()
        {
            int bottomLeftX = (int)(parentObject.Pos.X + topLeftPointOffset.X) / (int)Collision.gridCellWidth;
            int bottomLeftY = (int)(parentObject.Pos.Y + bottomRightPointOffset.Y) / (int)Collision.gridCellHeight;
            int topRightX = (int)(parentObject.Pos.X + bottomRightPointOffset.X) / (int)Collision.gridCellWidth;
            int topRightY = (int)(parentObject.Pos.Y + topLeftPointOffset.Y) / (int)Collision.gridCellHeight;

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
            int bottomLeftX = (int)(parentObject.Pos.X + topLeftPointOffset.X) / (int)Collision.gridCellWidth;
            int bottomLeftY = (int)(parentObject.Pos.Y + bottomRightPointOffset.Y) / (int)Collision.gridCellHeight;
            int topRightX = (int)(parentObject.Pos.X + bottomRightPointOffset.X) / (int)Collision.gridCellWidth;
            int topRightY = (int)(parentObject.Pos.Y + topLeftPointOffset.Y) / (int)Collision.gridCellHeight;

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

        public override void draw(WorldObject world, Matrix transformation)
        {
            Collision.basicEffect.World = Matrix.CreateTranslation(new Vector3(world.Pos, 0)) * transformation;

            foreach (EffectPass pass in Collision.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                

                This.Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, drawPoints, 0, 4);
            }
        }
    }
}