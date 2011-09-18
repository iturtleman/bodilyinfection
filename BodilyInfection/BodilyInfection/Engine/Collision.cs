﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace BodilyInfection
{
    public static class Collision
    {
        public static Dictionary<Vector2, List<WorldObject>> bucket;
        private static List<VertexPositionColor[]> gridPoints = new List<VertexPositionColor[]>();
        public static int gridCellHeight { get; set; }
        public static int gridCellWidth { get; set; }

        //Tuple value defs: 1=Key's Collision Object;  2=WorldObject that collided with Key;  3=Value 2's Key that Collided
        public static Dictionary<WorldObject, List<Tuple<CollisionObject, WorldObject, CollisionObject>>> collisionData;
        public static bool ShowCollisionData { get; set; }
        public static BasicEffect basicEffect = new BasicEffect(This.Game.GraphicsDevice);

        public static void createGrid(int bottomLeftX, int bottomLeftY, int topRightX, int topRightY)
        {
            gridPoints.Clear();

            for (float i = bottomLeftX; i <= topRightX; i += gridCellWidth) //vertical
            {
                VertexPositionColor[] line1 = new VertexPositionColor[2];
                line1[0].Position = new Vector3(i, bottomLeftY, 0f);
                line1[0].Color = Color.Gray;
                line1[1].Position = new Vector3(i, topRightY, 0f);
                line1[1].Color = Color.Gray;

                gridPoints.Add(line1);
            }

            for (float i = bottomLeftY; i <= topRightY; i += gridCellHeight) //horizontal
            {
                VertexPositionColor[] line2 = new VertexPositionColor[2];
                line2[0].Position = new Vector3(bottomLeftX, i, 0f);
                line2[0].Color = Color.Gray;
                line2[1].Position = new Vector3(topRightX, i, 0f);
                line2[1].Color = Color.Gray;

                gridPoints.Add(line2);
            }
        }

        public static void fillBucket()
        {
            bucket = new Dictionary<Vector2, List<WorldObject>>();

            foreach (WorldObject worldObject in This.Game.CurrentLevel.mSprites)
            {
                foreach (CollisionObject collisionObject in worldObject.GetCollision())
                {
                    collisionObject.addToBucket(worldObject);
                }
            }
        }

        public static void detectCollisions()
        {
            collisionData = new Dictionary<WorldObject, List<Tuple<CollisionObject, WorldObject, CollisionObject>>>();

            KeyValuePair<Vector2, List<WorldObject>> bucketElem;
            while (bucket.Count > 0)
            {
                bucketElem = bucket.First();
                bucket.Remove(bucketElem.Key);
                List<WorldObject> list = bucketElem.Value;

                if (list.Count <= 1)
                    continue;

                while (list.Count > 1)
                {
                    WorldObject front = list.First();
                    list.RemoveAt(0);
                    for (int k = 0; k < list.Count; k++)
                    {
                        List<Tuple<CollisionObject, CollisionObject>> detectedCollisions = detectCollision(front, list[k]);
                        if (detectedCollisions.Count != 0)
                        {
                            if (!collisionData.ContainsKey(front))
                            {
                                List<Tuple<CollisionObject, WorldObject, CollisionObject>> collisions = new List<Tuple<CollisionObject, WorldObject, CollisionObject>>();
                                collisionData.Add(front, collisions);
                            }
                            if (!collisionData.ContainsKey(list[k]))
                            {
                                List<Tuple<CollisionObject, WorldObject, CollisionObject>> collisions = new List<Tuple<CollisionObject, WorldObject, CollisionObject>>();
                                collisionData.Add(list[k], collisions);
                            }

                            foreach (Tuple<CollisionObject, CollisionObject> tuple in detectedCollisions)
                            {
                                collisionData[front].Add(new Tuple<CollisionObject, WorldObject, CollisionObject>(tuple.Item1, list[k], tuple.Item2));
                                collisionData[list[k]].Add(new Tuple<CollisionObject, WorldObject, CollisionObject>(tuple.Item2, front, tuple.Item1));
                            }
                        }
                    }
                }
            }

            #region new
            /*
            List<Thread> threads = new List<Thread>();
            List<List<Collision>> threadResults = new List<List<Collision>>();
            Object thislock = new Object();
            List<Circle>[] values = bucket.Values.ToArray();
            for (int i = 0; i < threadMax; i++)
            {
                threadResults.Add(new List<Collision>());
                int index = i;
                List<Circle>[] valuesPortion = values.Skip(i * values.Length / threadMax).Take(values.Length / threadMax).ToArray();
                Thread t = new Thread(new ThreadStart(delegate()
                    {
                        for(int j=0; j<valuesPortion.Length; j++)
                        {
                            List<Circle> list = valuesPortion[j];

                            if (list.Count <= 1)
                                continue;

                            while (list.Count > 1)
                            {
                                Circle front = list.First();
                                list.RemoveAt(0);
                                for (int k = 0; k < list.Count; k++)
                                {
                                    if (distance(front, list[k]) <= front.radius + list[k].radius)
                                    {
                                        Collision collision = new Collision();
                                        collision.objectOne = front;
                                        collision.objectTwo = list[k];
                                        threadResults[index].Add(collision);
                                    }
                                }
                            }
                        }
                    }));
                threads.Add(t);
                t.Start();
            }
            for (int i = 0; i < threadMax; i++)
                threads[i].Join();

            for (int i = 0; i < threadMax; i++)
                collisions.AddRange(threadResults[i]);
            */
            #endregion
        }

        public static void Draw()
        {
            if (ShowCollisionData)
            {
                //basicEffect.World = Matrix.Identity;
                float height = This.Game.GraphicsDevice.Viewport.Height;
                float width = This.Game.GraphicsDevice.Viewport.Width;
                basicEffect.View = Matrix.CreateLookAt(new Vector3(This.Game.GraphicsDevice.Viewport.X + width / 2, This.Game.GraphicsDevice.Viewport.Y + height / 2, -10),
                                                       new Vector3(This.Game.GraphicsDevice.Viewport.X + width / 2, This.Game.GraphicsDevice.Viewport.Y + height / 2, 0), new Vector3(0, -1, 0));
                basicEffect.Projection = Matrix.CreateOrthographic(This.Game.GraphicsDevice.Viewport.Width, This.Game.GraphicsDevice.Viewport.Height, 1, 20);
                basicEffect.VertexColorEnabled = true;


                drawGraph();

                foreach (WorldObject world in This.Game.CurrentLevel.mSprites)
                {
                    foreach (CollisionObject collisionObject in world.GetCollision())
                    {
                        collisionObject.draw(world);
                    }
                }
            }
        }

        private static void drawGraph()
        {
            Collision.basicEffect.World = Matrix.Identity;

            foreach (EffectPass pass in Collision.basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                for (int i = 0; i < gridPoints.Count; i++)
                    This.Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, gridPoints[i], 0, 1);
            }
        }

        public static void update()
        {
            fillBucket();
            detectCollisions();
        }

        public static float distanceSquared(float x1, float y1, float x2, float y2)
        {
            float a = x1 - x2;
            float b = y1 - y2;
            return a * a + b * b;
        }

        public static List<Tuple<CollisionObject, CollisionObject>> detectCollision(WorldObject w1, WorldObject w2)
        {
            List<Tuple<CollisionObject, CollisionObject>> output = new List<Tuple<CollisionObject, CollisionObject>>();

            List<CollisionObject> w1CollisionObj = w1.GetCollision();
            List<CollisionObject> w2CollisionObj = w2.GetCollision();

            foreach (CollisionObject cw1 in w1CollisionObj)
                foreach (CollisionObject cw2 in w2CollisionObj)
                {
                    if (cw1.detectCollision(w1, cw2, w2))
                    {
                        output.Add(new Tuple<CollisionObject, CollisionObject>(cw1, cw2));
                    }
                }

            return output;
        }

        /// <summary>
        /// Determine if BoundingCircle and BoundingCircle collide
        /// </summary>
        public static bool detectCollision(WorldObject w1, Collision_BoundingCircle c1, WorldObject w2, Collision_BoundingCircle c2)
        {
            float ds = Collision.distanceSquared((w1.Pos.X + c1.centerPointOffset.X), (w1.Pos.Y + c1.centerPointOffset.Y),
                                                 (w2.Pos.X + c2.centerPointOffset.X), (w2.Pos.Y + c2.centerPointOffset.Y));
            float r = c1.radius + c2.radius;
            return (ds <= r * r);
        }

        /// <summary>
        /// Determine if AABB and BoundingCircle collide
        /// </summary>
        public static bool detectCollision(WorldObject w1, Collision_AABB a1, WorldObject w2, Collision_BoundingCircle c1)
        {
            Vector2 centerPoint = c1.centerPointOffset + w2.Pos;
            Vector2 topLeftPoint = a1.topLeftPointOffset + w1.Pos;
            Vector2 bottomRightPoint = a1.bottomRightPointOffset + w1.Pos;

            int regionCode = 0;

            if (centerPoint.X < topLeftPoint.X)
                regionCode += 1; // 0001
            if (centerPoint.X > bottomRightPoint.X)
                regionCode += 2; // 0010
            if (centerPoint.Y > topLeftPoint.Y)
                regionCode += 4; // 0100
            if (centerPoint.Y < bottomRightPoint.Y)
                regionCode += 8;

            float radius = c1.radius;
            switch (regionCode)
            {
                case 0: //0000
                    return true;
                case 1: //0001
                    if (Math.Abs(topLeftPoint.X - centerPoint.X) <= radius)
                        return true;
                    break;
                case 2: //0010
                    if (Math.Abs(centerPoint.X - bottomRightPoint.X) <= radius)
                        return true;
                    break;
                case 4: //0100
                    if (Math.Abs(centerPoint.Y - topLeftPoint.Y) <= radius)
                        return true;
                    break;
                case 8: //1000
                    if (Math.Abs(bottomRightPoint.Y - centerPoint.Y) <= radius)
                        return true;
                    break;
                case 5: //0101
                    if (Collision.distanceSquared(centerPoint.X, centerPoint.Y, topLeftPoint.X, topLeftPoint.Y) <= radius * radius)
                        return true;
                    break;
                case 6: //0110
                    if (Collision.distanceSquared(centerPoint.X, centerPoint.Y, bottomRightPoint.X, topLeftPoint.Y) <= radius * radius)
                        return true;
                    break;
                case 9: //1001
                    if (Collision.distanceSquared(centerPoint.X, centerPoint.Y, topLeftPoint.X, bottomRightPoint.Y) <= radius * radius)
                        return true;
                    break;
                case 10: //1010
                    if (Collision.distanceSquared(centerPoint.X, centerPoint.Y, bottomRightPoint.X, bottomRightPoint.Y) <= radius * radius)
                        return true;
                    break;
            }


            return false;
        }

        /// <summary>
        /// Determine if OBB and BoundingCircle collide
        /// </summary>
        public static bool detectCollision(WorldObject w1, Collision_OBB o1, WorldObject w2, Collision_BoundingCircle c1)
        {
            Vector2 c1Center = c1.centerPointOffset + w2.Pos;
            Vector2 o1Anchor = new Vector2(w1.Pos.X, w1.Pos.Y);

            Vector2 drawPoint0 = new Vector2(o1.drawPoints[0].Position.X + o1Anchor.X, o1.drawPoints[0].Position.Y + o1Anchor.Y);
            Vector2 drawPoint1 = new Vector2(o1.drawPoints[1].Position.X + o1Anchor.X, o1.drawPoints[1].Position.Y + o1Anchor.Y);
            Vector2 drawPoint2 = new Vector2(o1.drawPoints[2].Position.X + o1Anchor.X, o1.drawPoints[2].Position.Y + o1Anchor.Y);

            Vector2 C = drawPoint1 + Vector2.Dot(c1Center - drawPoint1, Vector2.Normalize(drawPoint1 - drawPoint0)) * Vector2.Normalize(drawPoint1 - drawPoint0);
            Vector2 D = drawPoint1 + Vector2.Dot(c1Center - drawPoint1, Vector2.Normalize(drawPoint1 - drawPoint2)) * Vector2.Normalize(drawPoint1 - drawPoint2);

            if (((distanceSquared(C.X, C.Y, drawPoint1.X, drawPoint1.Y) <= c1.radius * c1.radius) || (distanceSquared(C.X, C.Y, drawPoint0.X, drawPoint0.Y) <= c1.radius * c1.radius)) &&
                ((distanceSquared(D.X, D.Y, drawPoint1.X, drawPoint1.Y) <= c1.radius * c1.radius) || (distanceSquared(D.X, D.Y, drawPoint2.X, drawPoint2.Y) <= c1.radius * c1.radius)))
                return true;


            return false;
        }
    }
}
