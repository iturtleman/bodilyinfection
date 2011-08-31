#ifndef __SPRITE_H__
#define __SPRITE_H__
#include <SDL/SDL.h>
#include <string>
#include "fns.h"
#include "Actor.h"
#include "WorldObject.h"

/**
  Characters and other mobile objects displayed on screen with animations.

  This class is tasked with the following:
    - loading and controlling an Animation 
 */

class Sprite : public WorldObject
{
	public:
	Sprite(SDL_Surface *screen, std::string name, Actor actor);
	void setAnimation(int animation){ mActor.mCurrentAnimation = animation; }/**< changes to the specified animation beginning at 0. */
	SpriteFrame* getAnimation(){ return &mActor.mAnimations[mActor.mCurrentAnimation]->mFrames[mActor.mFrame]; }/**< returns active animation. (actually the current frame within the animation) \todo see if this is slower maybe undo */
	void setFrame(int frame) { mActor.mFrame = frame; }/**< cahnges to the specified frame of the animation beginning at 0. */
	int getFrame() { return mActor.mFrame; }/**< returns active frame. */
	void setSpeed(float speed) {mSpeed = speed;}/**< sets the Sprite's speed. */
	float getSpeed() {return mSpeed;}/**< returns a Sprite's current speed. */
	std::string getName(){ return mName;}/**< returns the Sprite's name */
	void toggleAnim(){ mAnimating = !mAnimating;}/**< Pauses or resumes an animation. */
	void startAnim() {mAnimating = 1;}/**< Causes the animation to play. */
	void stopAnim() {mAnimating = 0;}/**< Causes the animation to stop. */
	void rewind() {mActor.mFrame = 0;}/**< Resets the Sprite's animation to the first frame. */
	void draw();/**< draws Sprite. */
	void drawCollisions();/**< Draws Collision data for the Object (from outside) */
	Sprite* collisionWithSprite(string name);/**< checks for collision with sprite of a given name. */
	vector<Collision*>& getCollisionData();/**< Returns collision data */
	
	
private:
	std::string mName;/**< Sprite's name */
	bool mAnimating;/**< Tells whether to animate or not */
	bool mDrawn;/**< Tells if the object has been drawn the first time */
	float mSpeed;/**< Movement speed of the Sprite. */
	long mLastUpdate;/**< Number that indicates when the Sprite has last updated. Overflows in about 24 days so no worries. */
	Actor mActor;/**< This Sprite's Actor. */
	SDL_Surface *mScreen;/**< Screen to be drawn to. */
	Behavior mBehavior;/**< Sprite's Behavior */
};
#endif
