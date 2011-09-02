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
	    public SpriteFrame GetAnimation(){ 
            get{
                return mActor.Animations[mActor.CurrentAnimation].mFrames[mActor.mFrame];
            } 

        }

        #endregion Properties

        
      #region Methods
        public void SetAnimation(int animation){ mActor.CurrentAnimation = animation; }/**< changes to the specified animation beginning at 0. */  
	    public void ToggleAnim(){ mAnimating = !mAnimating;}/**< Pauses or resumes an animation. */
	    public void StartAnim() {mAnimating = true;}/**< Causes the animation to play. */
	    public void StopAnim() {mAnimating = false;}/**< Causes the animation to stop. */
	    public void Rewind() {mActor.mFrame = 0;}/**< Resets the Sprite's animation to the first frame. */
        public void Draw()
        {
            //Frame so we don't have to find it so often
            SpriteFrame frame = GetAnimation();
            if (mAnimating == true)
            {
                if (mLastUpdate + frame.pause * mSpeed < SDL_GetTicks())
                {
                    //obtain current peg 
                    Vector2 ppos = frame.animationPeg;
                    mActor.mFrame++;
                    if (mActor.mFrame > mActor.mAnimations[mActor.mCurrentAnimation].mNumFrames - 1)
                        mActor.mFrame = 0;
                    //update frame so we don't need to worry
                    frame = mActor.mAnimations[mActor.mCurrentAnimation].mFrames[mActor.mFrame];
                    //obtain next peg
                    Vector2 npos = frame.animationPeg;
                    //move current position to difference of two
                    mPos.add(ppos - npos);
                    mLastUpdate = SDL_GetTicks();
                }
            }
            if (mVisible == true) { }
        }/**< draws Sprite. */

	    public void DrawCollisions();/**< Draws Collision data for the Object (from outside) */
	    public Sprite CollisionWithSprite(string name);/**< checks for collision with sprite of a given name. */
	    // public vector<Collision> getCollisionData();/**< Returns collision data */
      #endregion Methods 
   
	    private string mName;/**< Sprite's name */
	    private bool mAnimating;/**< Tells whether to animate or not */
	    private bool mDrawn;/**< Tells if the object has been drawn the first time */
	    private float mSpeed;/**< Movement speed of the Sprite. */
	    private long mLastUpdate;/**< Number that indicates when the Sprite has last updated. Overflows in about 24 days so no worries. */
	    private Actor mActor;/**< This Sprite's Actor. */
	    private Behavior mBehavior;/**< Sprite's Behavior */

    }
}
