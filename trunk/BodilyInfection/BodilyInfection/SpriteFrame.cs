using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BodilyInfection
{
    class SpriteFrame
    {
        #region Properties
        /// <summary>
        /// Image Texture
        /// </summary>
        public Texture2D image { get; set; }

        /// <summary>
        /// Amount of time to pause between
        /// this frame and next.
        /// </summary>
        public long Pause { get; set; }
        
        /// <summary>
        /// The offeset from position to place
        /// the image. Defaults to (0,0)
        /// </summary>
        public Vector2 AnimationPeg { get; set; }
        #endregion

        #region Variables
        /// <summary>
        ///  Hot spots that can be used for
        ///  locating objects on the sprite
        ///  default is tagged to center
        ///  of the sprite
        /// </summary>
        public List<Vector2> HotSpots = new List<Vector2>();

        /// <summary>
        /// The collision data for this sprite.
        /// </summary>
        public List<Collision> CollisionData = new List<Collision>();
        #endregion
    }
}
