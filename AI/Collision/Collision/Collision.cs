using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Collision
{
    public class Collision
    {
        private Dictionary<Vector2, List<CollisionObject>> bucket;
        private List<VertexPositionColor[]> gridPoints = new List<VertexPositionColor[]>();
        public int gridCellHeight { get; set; }
        public int gridCellWidth { get; set; }
        public List<Objects> objects;

        public bool drawGrid { get; set; }

        public void createGrid(int bottomLeftX, int bottomLeftY, int topRightX, int topRightY)
        {
            for (float i = bottomLeftX; i <= topRightX; i += gridCellWidth) //vertical
            {
                VertexPositionColor[] line1 = new VertexPositionColor[2];
                line1[0].Position = new Vector3(i, bottomLeftY, 0f);
                line1[0].Color = Color.DarkBlue;
                line1[1].Position = new Vector3(i, topRightY, 0f);
                line1[1].Color = Color.DarkBlue;

                gridPoints.Add(line1);
            }

            for (float i = bottomLeftY; i <= topRightY; i += gridCellHeight) //horizontal
            {
                VertexPositionColor[] line2 = new VertexPositionColor[2];
                line2[0].Position = new Vector3(bottomLeftX, i, 0f);
                line2[0].Color = Color.DarkBlue;
                line2[1].Position = new Vector3(topRightX, i, 0f);
                line2[1].Color = Color.DarkBlue;

                gridPoints.Add(line2);
            }
        }

        public void fillBucket()
        {
            bucket = new Dictionary<Vector2, List<CollisionObject>>();

            foreach (Objects c in objects)
            {
                foreach (CollisionHierarchy collisionHierarchy in c.collision)
                {
                    collisionHierarchy.node.addToBucket();
                }

            }
        }

        public List<CollisionPair> detectCollisions()
        {
            List<CollisionPair> collisions = new List<CollisionPair>();

            KeyValuePair<Vector2, List<CollisionObject>> bucketElem;
            while (bucket.Count > 0)
            {
                bucketElem = bucket.First();
                bucket.Remove(bucketElem.Key);
                List<CollisionObject> list = bucketElem.Value;

                if (list.Count <= 1)
                    continue;

                while (list.Count > 1)
                {
                    CollisionObject front = list.First();
                    list.RemoveAt(0);
                    for (int k = 0; k < list.Count; k++)
                    {
                        if ( front.detectCollision(list[k]) )
                        {
                            CollisionPair collisionPair = new CollisionPair();
                            collisionPair.objectOne = front;
                            collisionPair.objectTwo = list[k];
                            collisions.Add(collisionPair);
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

            return collisions;
        }

        public void draw(GraphicsDevice device)
        {
            if (drawGrid)
                for (int i = 0; i < gridPoints.Count; i++)
                    device.DrawUserPrimitives(PrimitiveType.LineList, gridPoints[i], 0, 1);
        }
    }


        public class CollisionPair
    {
        public CollisionObject objectOne;
        public CollisionObject objectTwo;
    }
}
