using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;

namespace BodilyInfection
{

    /// <summary>
    /// This class loads sets of images into
    /// an animation set.
    /// </summary>
    class Animation
    {
        #region Properties
        public int Built { get; set; }
        public int NumFrames { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; }
        #endregion

        #region Variables
        private string spritePath = "ArtAssets/Sprites/";
        public List<SpriteFrame> Frames = new List<SpriteFrame>();
        #endregion

        #region Constructors
        public Animation(string filename) :
            this(filename, filename)
        {
        }

        public Animation(string filename, string name)
        {
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
            filename = spritePath + filename;

            using (StreamReader stream = new StreamReader(filename))
            {
                int count = 0;
                while (!stream.EndOfStream)
                {
                    buffer = stream.ReadLine();

                    // ...
                    Built = 1;

                    if (buffer.Length > 0 &&
                        !new char[] { 
                            '#', '\r', '\0', '\n' 
                        }.Contains(buffer[0]) &&
                        !buffer.StartsWith("NumFrames:"))
                    {
                        string[] frameInfo = buffer.Split('\t');
                        file = frameInfo[0];
                        pause = Int32.Parse(frameInfo[1]);
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
                            }
                            float pegX = 0;
                            float pegY = 0;
                            float.TryParse(frameInfo[5], out pegX);
                            float.TryParse(frameInfo[6], out pegY);

                            /** sets frame delay and makes sure height and width are correct */
                            Frames[count].Pause = pause;
                            Width = Frames[count].Width;
                            Height = Frames[count].Height;

                            /** Set the animation Peg*/
                            Frames[count].AnimationPeg = new Vector2(pegX + (float)Width / 2, pegY + (float)Height / 2);

                            count++;
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
