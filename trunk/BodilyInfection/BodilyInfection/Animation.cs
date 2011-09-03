using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        public Animation(string filename) : this(filename, filename){}

        public Animation(string filename, string name)
        {
            Name = name;
            LoadAnimation(filename);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the animations from a file.
        /// </summary>
        /// <param name="filename"></param>
        private void LoadAnimation(string filename)
        {
            string buffer;
            string file;
            int[] transparency = new int[3];
            int pause;
            filename = string.Format("Content/Sprites/{0}", filename);

            if (!File.Exists(filename))
            {
                Console.WriteLine(string.Format("Error opening \'{0}\'. The file does not exist.", filename));
                return;
            }

            using (StreamReader stream = new StreamReader(filename))
            {
                while (!stream.EndOfStream)
                {
                    buffer = stream.ReadLine();

                    //make sure the first char is not # or whitespace
                    if (buffer.Length > 0 && !buffer.StartsWith("#") && !String.IsNullOrWhiteSpace(buffer))
                    {
                        if (buffer.StartsWith("NumFrames:"))
                        {
                            NumFrames = int.Parse(buffer.Trim().Split().Last());
                            Built = true;
                        }
                        else
                        {
                            string[] frameInfo = buffer.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                            /** File name */
                            file = frameInfo[0];
                            /** Pause time */
                            pause = Int32.Parse(frameInfo[1]);
                            /** transparency #'s R G B */
                            for (int x = 0; x < 3; x++)
                            {
                                int value = Int32.Parse(frameInfo[1 + x]);
                                if (value >= 0 && value <= 255)
                                {
                                    transparency[x] = value;
                                }
                                else
                                {
                                    transparency[x] = 0;
                                    Console.WriteLine("{0}\nInvalid visibility value!", value);
                                }

                                /** AnimationPeg offset*/
                                float pegX = float.Parse(frameInfo[5]);
                                float pegY = float.Parse(frameInfo[6]);

                                /** Collision data */
                                string[] collisiondata = frameInfo[7].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                int index=0;
                                string c = collisiondata[index++];
                                if (c == "(")
                                {
                                    c = collisiondata[index++];
                                    while (c != ")")
                                    {
                                        if (c == "n")
                                        {
                                            c = collisiondata[index++];
                                            continue;
                                        }
                                        else if (c == "c")
                                        {
                                            double xOffset = double.Parse(collisiondata[index++]);
                                            double yOffset = double.Parse(collisiondata[index++]);
                                            double radius = double.Parse(collisiondata[index++]);
                                            //Frames[count].CollisionData.Add(new CollisionCircle(Vector2(xOffset, yOffset), radius));
                                        }
                                        else if (c == "r")
                                        {
                                            double xOffset = double.Parse(collisiondata[index++]);
                                            double yOffset = double.Parse(collisiondata[index++]);
                                            double width = double.Parse(collisiondata[index++]);
                                            double height = double.Parse(collisiondata[index++]);
                                            //Frames[count].CollisionData.Add(new CollisionRectangle(Vector2(xOffset, yOffset), width, height));
                                        }
                                        c = collisiondata[index++];
                                    }
                                }

                                SpriteFrame sf = new SpriteFrame();

                                sf.image =This.Game.Content.Load<Texture2D>(@"Sprites\"+file);
                                
                                /** sets frame delay */
                                sf.Pause = pause;

                                /** Set the animation Peg*/
                                sf.AnimationPeg = new Vector2(pegX + (float)sf.image.Width / 2, pegY + (float)sf.image.Height / 2);

                                Frames.Add(sf);
                            }
                        }
                    }
                }
            }
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
}
