using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;

namespace BodilyInfection
{

    /// <summary>
    /// This class loads sets of images into
    /// an animation set.
    /// </summary>
    class Animation
    {
        #region Properties
        public bool Built { get; set; }
        public int NumFrames { get; set; }
        public string Name { get; set; }
        #endregion

        #region Variables
        public List<SpriteFrame> Frames = new List<SpriteFrame>();
        #endregion

        #region Constructors
        public Animation()
        {
            NumFrames = 0;
            Built = false;
        }

        public Animation(string filename) : this(filename, filename) { }

        public Animation(string filename, string name)
        {
            Name = name;
            LoadAnimation(filename);
            Built = true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the animations from a file.
        /// </summary>
        /// <param name="filename"></param>
        private void LoadAnimation(string filename)
        {
            filename = string.Format("Content/Sprites/{0}", filename);

            if (!File.Exists(filename))
            {
                Console.WriteLine(string.Format("Error opening \'{0}\'. The file does not exist.", filename));
                return;
            }
            XDocument doc = XDocument.Load(filename);
            int count = 0;
            foreach (var frame in doc.Descendants("Frame"))
            {
                SpriteFrame sf = new SpriteFrame();

                string[] sp = frame.Attribute("TLPos").Value.Split(',');
                sf.StartPos = new Vector2(float.Parse(sp[0]), float.Parse(sp[1]));

                ///image
                string file = frame.Attribute("SpriteSheet").Value;
                sf.Image = This.Game.Content.Load<Texture2D>(@"Sprites\" + file);

                /** sets frame delay */
                sf.Pause = int.Parse(frame.Attribute("FrameDelay").Value);

                //Image's width and height
                sf.Width = int.Parse(frame.Attribute("Width").Value);
                sf.Height = int.Parse(frame.Attribute("Height").Value);

                var point = frame.Attribute("AnimationPeg").Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                float pegX = float.Parse(point.First());
                float pegY = float.Parse(point.Last());

                /** Set the animation Peg*/
                sf.AnimationPeg = new Vector2(pegX + (float)sf.Width / 2, pegY + (float)sf.Height / 2);

                int idCount = 0;
                foreach (var collision in frame.Descendants("Collision"))
                {
                    if (collision.Attribute("Type").Value == "Circle")
                    {
                        string[] pt = collision.Attribute("Pos").Value.Split(',');
                        sf.CollisionData.Add(new Collision_BoundingCircle(
                            idCount++,
                            new Vector2(float.Parse(pt[0]), float.Parse(pt[1])),
                            float.Parse(collision.Attribute("Radius").Value)));
                    }
                    else if (collision.Attribute("Type").Value == "Rectangle")
                    {
                        string[] tl = collision.Attribute("TLPos").Value.Split(',');
                        float tlx = float.Parse(tl[0]);
                        float tly = float.Parse(tl[1]);
                        string[] br = collision.Attribute("BRPos").Value.Split(',');
                        float brx = float.Parse(tl[0]);
                        float bry = float.Parse(tl[1]);
                        sf.CollisionData.Add( new Collision_AABB(
                            idCount++,
                            new Vector2(tlx, tly),
                            new Vector2(brx, bry)
                            ));
                    }
                }


                Frames.Add(sf);
                count++;
            }
            NumFrames = count;
        }

        #endregion

        #region Compare
        public static bool operator ==(Animation x, Animation y)
        {
            return x.Name == y.Name;
        }

        public static bool operator !=(Animation x, Animation y)
        {
            return x.Name != y.Name;
        }

        /// <summary>
        /// Check equality by object
        /// </summary>
        /// <param name="obj">The object to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //has to be an animation
            if (obj as Animation == null)
                return false;
            return this == obj as Animation;
        }

        public override int GetHashCode()
        {
            //\todo Implement
            return base.GetHashCode();
        }

        /// <summary>
        /// Return the hash code for this string.
        /// </summary>
        //public override int GetHashCode(object obj)
        //{
        //    // Stores the result.
        //    int result = 0;

        //    // Don't compute hash code on null object.
        //    if (obj == null)
        //    {
        //        return 0;
        //    }

        //    //if it's not an animation
        //    if (obj as Animation == null)
        //        return 0;

        //    string n = (obj as Animation).Name;

        //    // Get length.
        //    int length = n.Length;

        //    // Return default code for zero-length strings [valid, nothing to hash with].
        //    if (length > 0)
        //    {
        //        // Compute hash for strings with length greater than 1
        //        char let1 = n[0];          // First char of string we use
        //        char let2 = n[length - 1]; // Final char

        //        // Compute hash code from two characters
        //        int part1 = let1 + length;
        //        result = (_multiplier * part1) + let2 + length;
        //    }
        //    return result;
        //}

        /// <summary>
        /// Has a good distribution.
        /// </summary>
        const int _multiplier = 89;

        #endregion Compare
    }

    class DummyAnimation : Animation
    {
        public DummyAnimation(string name)
        {
            Name = name;
            Built = true;
            Frames.Add(new SpriteFrame());
        }
    }
}
