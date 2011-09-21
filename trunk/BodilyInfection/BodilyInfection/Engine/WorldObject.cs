using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BodilyInfection
{
    abstract public class WorldObject : IComparable<WorldObject>
    {
        

        public WorldObject(int z = 0)
        {
            ZOrder = z;
            mVisible = true;
            mTransparency = 1;
            mAngle = 0;
        }

        #region Methods
        /// <summary>
        /// Draws the obejct
        /// </summary>
        /// <param name="gameTime">The gametime for the drawing frame.</param>
        public abstract void Draw(Microsoft.Xna.Framework.GameTime gameTime);

        /// <summary>
        /// 
        /// </summary>
        public void DoCollisions(GameTime gameTime)
        {
            if (mCollidesWithBackground)
            {
                // do background collisions
            }
            CollisionBehavior(gameTime);
        }
        #endregion Methods

        internal UpdateBehavior CollisionBehavior = (GameTime gameTime) => { };

        #region Properties
        /// <summary>
        ///     gets the sprite's name
        /// </summary>
        public string Name { get { return mName; } }

        /// <summary>
        /// Sets the Sprite's transparency.
        /// </summary>
        /// <param name="f">The sprite's transparancy [0,1] other values will be force set </param>
        public float Transparency
        {
            get { return mTransparency; }
            set { mTransparency = value > 1 ? 1 : value < 0 ? 0 : value; }
        }
        /// <summary>
        /// Angle in degrees.
        /// </summary>
        /// <returns>Angle in degrees</returns>
        public float Angle
        {
            get { return mAngle; }
            set
            {
                float a = value;
                mAngle = a;
                while (a > 360)
                    a -= 360;
            }
        }

        /// <summary>
        /// The current (x,y) position
        /// </summary>
        public Vector2 Pos = new Vector2(0, 0);

        /// <summary>
        /// Stacking order. Determines what draws on top.
        /// </summary>
        public int ZOrder;

        /// <summary>
        /// Sprite's scale for drawing
        /// </summary>
        public Vector2 Scale = new Vector2(1, 1);

        /// <summary>
        /// Determines whether or not the WorldObject is transformed by the camera or not
        /// </summary>
        public bool Static { get; set; }

        #endregion Properties

        #region Variables
        /// <summary>
        /// Sprite's name
        /// </summary>
        protected string mName;

        /// <summary>
        /// Determine if Object should be visible
        /// </summary>
        public bool mVisible;
        /// <summary>
        /// Transparency!
        /// </summary>
        protected float mTransparency;

        /// <summary>
        /// Angle of rotation
        /// </summary>
        protected float mAngle;

        private bool mCollidesWithBackground = true;
        #endregion Variables

        internal abstract List<CollisionObject> GetCollision();

        /// <summary>
        /// Allows sorting
        /// </summary>
        /// <param name="other">value with which to compare</param>
        /// <returns></returns>
        public int CompareTo(WorldObject other)
        {
            return ZOrder.CompareTo(other.ZOrder);
        }
    }
}
