using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BodilyInfection
{
    class WorldObject
    {
        public WorldObject(int z = 0)
        {
            ZOrder = z;
            mVisible = true;
            mTransparency = 1;
            mAngle = 0;
        }
        /// <summary>
        /// Draws the obejct
        /// </summary>
        /// <param name="gameTime">The gametime for the drawing frame.</param>
        public virtual abstract void Draw(Microsoft.Xna.Framework.GameTime gameTime);
        /// <summary>
        /// Draws Collision data for the Object
        /// </summary>
        /// <param name="vec">List of Collision data for the given object</param>
        /// <param name="pos">Position where to draw the collision from</param>
        public void drawCollisions(List<Collision> vec, Vector2 pos)
        {
            for (int i = 0; i < vec.Count; i++)
            {
                vec[i].Draw(pos);
            }
        }
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
        /// Determine if Object should be visible
        /// </summary>
        protected bool mVisible;
        /// <summary>
        /// Transparency!
        /// </summary>
        protected float mTransparency;
        /// <summary>
        /// Angle of rotation
        /// </summary>
        protected float mAngle;
        /// <summary>
        /// Stacking order. Determines what draws on top.
        /// </summary>
        protected int ZOrder;
    }
}
