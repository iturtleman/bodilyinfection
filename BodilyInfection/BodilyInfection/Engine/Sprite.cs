﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BodilyInfection
{
    class Sprite : WorldObject
    {
        public Sprite(string name, Actor actor)
        {
            mName = name;
            //mDrawn = false;
            mActor = actor;
            mLastUpdate = new GameTime();
            LoadBehavior = () => { };
            UpdateBehavior = () => { };
            EndBehavior = () => { };
            //add to current level
            This.Game.CurrentLevel.AddSprite(this);

            if (mActor.Animations[mActor.CurrentAnimation].Built)
            {
                if (mActor.Animations[mActor.CurrentAnimation].NumFrames > 1) mAnimating = true;

            }

            Random r = new Random();
            Collision_BoundingCircle b = new Collision_BoundingCircle(r.Next(), new Vector2(0, 0), 10, this);

            foreach (Animation a in mActor.Animations)
            {
                foreach(SpriteFrame frame in a.Frames){
                    frame.CollisionData.Add(b);
                }
            }
        }

        #region Properties
        /// <summary>
        ///     changes to the specified frame of the animation beginning at 0
        /// </summary>
        public int Frame { get; set; }

        /// <summary>
        ///     the sprite's speed
        /// </summary>
        public float AnimationSpeed { get; set; }

        #endregion Properties

        #region Behaviors
        /// <summary>
        /// Sprite's Load Behavior
        /// </summary>
        public Behavior LoadBehavior;

        /// <summary>
        /// Sprite's Update Behavior
        /// </summary>
        public Behavior UpdateBehavior;

        /// <summary>
        /// Sprite's End Behavior
        /// </summary>
        public Behavior EndBehavior;
        #endregion Behaviors

        #region Methods

        internal override List<CollisionObject> GetCollision()
        {
            return GetAnimation().CollisionData;
        }

        /// <summary>
        ///     changes to the specified animation beginning at 0
        /// </summary>
        public SpriteFrame GetAnimation()
        {
            return mActor.Animations[mActor.CurrentAnimation].Frames[mActor.Frame];
        }

        /// <summary>
        /// changes to the specified animation beginning at 0.
        /// </summary>
        /// <param name="animation">The animation to select (begins at 0)</param>
        public void SetAnimation(int animation) { mActor.CurrentAnimation = animation; }

        /// <summary>
        /// Pauses or resumes an animation.
        /// </summary>
        public void ToggleAnim() { mAnimating = !mAnimating; }

        /// <summary>
        /// Causes the animation to play.
        /// </summary>
        public void StartAnim() { mAnimating = true; }

        /// <summary>
        /// Causes the animation to stop.
        /// </summary>
        public void StopAnim() { mAnimating = false; }

        /// <summary>
        ///  Resets the Sprite's animation to the first frame.
        /// </summary>
        public void Rewind() { mActor.Frame = 0; }
        #endregion Methods

        #region Draw
        /// <summary>
        /// Draw the Scene
        /// </summary>
        /// <param name="gameTime">Game time as given by the game class</param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //Frame so we don't have to find it so often
            SpriteFrame frame = GetAnimation();
            if (mAnimating == true)
            {
                //used to update the animation. Occurs once the frame's pause * sprite's speed occurs.
                if (mLastUpdate.TotalGameTime.TotalMilliseconds + frame.Pause * AnimationSpeed < gameTime.TotalGameTime.TotalMilliseconds)
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
                    mLastUpdate = new GameTime(gameTime.TotalGameTime, gameTime.ElapsedGameTime);
                }
            }
            if (mVisible == true)
            {
                SpriteFrame f=mActor.Animations[mActor.CurrentAnimation].Frames[mActor.Frame];
                This.Game.spriteBatch.Draw(f.Image, Pos, new Rectangle((int)f.StartPos.X, (int)f.StartPos.Y, f.Width, f.Height), Color.White, Angle, GetAnimation().AnimationPeg, Scale, SpriteEffects.None, 0);
            }
        }

        #region Collision
        /// <summary>
        /// checks for collision with sprite of a given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sprite CollisionWithSprite(string name)
        {
            //        //get the frame for readability
            //SpriteFrame frame = GetAnimation();
            ////get the first sprite with this name
            //Sprite s= This.Game.getCurrentLevel().findSpriteByName(name);
            //if(s==null)
            //    return null;
            //for(int i=0; i <  frame.CollisionData.Count; i++)
            //    if(frame.CollisionData[i].checkCollisions(s.getCollisionData(), s.Pos + s.GetAnimation().AnimationPeg, Pos + frame.AnimationPeg))//if there is a collision
            //        return s;
            return null;//if there aren't collisions
        }
        /// <summary>
        /// Returns collision data
        /// </summary>
        /// <returns></returns>
        //public vector<Collision> getCollisionData();
        #endregion Collision

        #region Variables

        /// <summary>
        /// Tells whether to animate or not
        /// </summary>
        private bool mAnimating;

        /// <summary>
        /// Tells if the object has been drawn the first time
        /// </summary>
        //private bool mDrawn;

        /// <summary>
        /// Number that indicates when the Sprite'a animation was last updated.
        /// </summary>
        private Microsoft.Xna.Framework.GameTime mLastUpdate;

        /// <summary>
        /// This Sprite's Actor.
        /// </summary>
        protected Actor mActor;

        #endregion Variables

        #endregion Methods
    }
}
