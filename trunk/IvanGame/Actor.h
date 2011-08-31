#ifndef __ACTOR_H__
#define __ACTOR_H__
#include "Animation.h"
#include <SDL/SDL.h>
#include <vector>

/**
  Set of Animations

  This class is tasked with the following:
    - keeping track of the current Animation's state
 */


class Actor
{
	public:
	Actor(Animation *anim);
	int mFrame;/**< The frame # */
	int mCurrentAnimation;/**< Index to the current loaded animation */
	std::vector<Animation*> mAnimations;/**< Vector of pointers to all of this sprite's Animations */

	private:
	

};
#endif
