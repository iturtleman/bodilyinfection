#include "Actor.h"
//the beggining
Actor::Actor(Animation *anim):
	mFrame(0),
	mCurrentAnimation(0)
{
	mAnimations.push_back(anim);
}

