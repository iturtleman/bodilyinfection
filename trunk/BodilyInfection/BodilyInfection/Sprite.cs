using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BodilyInfection
{
    class Sprite : WorldObject
    {
        public Sprite(string name, Actor actor)
        {
            mName = name;
            mActor = actor;
        }

        #region Properties
        /// <summary>
        ///     changes to the specified frame of the animation beginning at 0
        /// </summary>
        public int Frame { get; set; }

        /// <summary>
        ///     the sprite's speed
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        ///     gets the sprite's name
        /// </summary>
        public string Name { get { return mName; } }

        /// <summary>
        ///     changes to the specified animation beginning at 0
        /// </summary>
        public SpriteFrame GetAnimation()
        {
            return mActor.Animations[mActor.CurrentAnimation].Frames[mActor.Frame];
        }

        #endregion Properties


        #region Methods
        public void SetAnimation(int animation) { mActor.CurrentAnimation = animation; }/**< changes to the specified animation beginning at 0. */
        public void ToggleAnim() { mAnimating = !mAnimating; }/**< Pauses or resumes an animation. */
        public void StartAnim() { mAnimating = true; }/**< Causes the animation to play. */
        public void StopAnim() { mAnimating = false; }/**< Causes the animation to stop. */
        public void Rewind() { mActor.Frame = 0; }/**< Resets the Sprite's animation to the first frame. */
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //Frame so we don't have to find it so often
            SpriteFrame frame = GetAnimation();
            if (mAnimating == true)
            {
                if (mLastUpdate.ElapsedGameTime.Ticks + frame.Pause * mSpeed < gameTime.ElapsedGameTime.Ticks)
                {
                    //obtain current peg 
                    Vector2 ppos = frame.AnimationPeg;
                    mActor.Frame++;
                    if (mActor.Frame > mActor.Animations[mActor.CurrentAnimation].NumFrames - 1)
                        mActor.Frame = 0;
                    //update frame so we don't need to worry
                    frame = mActor.Animations[mActor.CurrentAnimation].Frames[mActor.Frame];
                    //obtain next peg
                    Vector2 npos = frame.AnimationPeg;
                    //move current position to difference of two
                    Pos += (ppos - npos);
                    mLastUpdate = gameTime;
                }
            }
            if (mVisible == true) { }
        }/**< draws Sprite. */

        ///Draws Collision data for the Object (from outside)
        public void DrawCollisions()
        {
            //get the frame for readability
            SpriteFrame frame = mActor.Animations[mActor.CurrentAnimation].Frames[mActor.Frame];
            //center the location
            Vector2 pt = Pos + frame.AnimationPeg;
            drawCollisions(frame.CollisionData, pt);
        }
        public Sprite CollisionWithSprite(string name)
        {
            //        //get the frame for readability
            //SpriteFrame frame = GetAnimation();
            ////get the first sprite with this name
            //Sprite s= Game::game().getCurrentLevel().findSpriteByName(name);
            //if(s==null)
            //    return null;
            //for(int i=0; i <  frame.CollisionData.Count; i++)
            //    if(frame.CollisionData[i].checkCollisions(s.getCollisionData(), s.Pos + s.GetAnimation().AnimationPeg, Pos + frame.AnimationPeg))//if there is a collision
            //        return s;
            return null;//if there aren't collisions
        }/**< checks for collision with sprite of a given name. */
        // public vector<Collision> getCollisionData();/**< Returns collision data */
        #endregion Methods

        private string mName;/**< Sprite's name */
        private bool mAnimating;/**< Tells whether to animate or not */
        private bool mDrawn;/**< Tells if the object has been drawn the first time */
        private float mSpeed;/**< Movement speed of the Sprite. */
        private Microsoft.Xna.Framework.GameTime mLastUpdate;/**< Number that indicates when the Sprite has last updated. Overflows in about 24 days so no worries. */
        private Actor mActor;/**< This Sprite's Actor. */
        private Behavior mBehavior;/**< Sprite's Behavior */

    }
}
