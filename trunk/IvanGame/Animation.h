#ifndef __ANIMATION_H__
#define __ANIMATION_H__
#include "Frame.h"
#include <string>
#include <SDL/SDL.h>
#include <SDL/SDL_image.h>

using std::vector;

/**
  An Animation

  This class is tasked with the following:
    - loading sets of images into an animation set.
*/


class Animation
{
	public:
	Animation(std::string animFile);
	Animation(std::string animFile, string name);
	int loadAnimation(std::string animFile);/**< Loads the Animations from a file in the specified format. */
	SpriteFrame *mFrames;/**< Pointer to the current animation. As an array of SpriteFrames */
	int mBuilt,/**< Using as a bool */
	 mNumFrames,/**< Number of frames in this Animation */
	 mW,/**< Animation's current width */
	 mH;/**< The animation's current Height */
	bool operator==(Animation a){return mName==a.mName;}
	std::string mName;//add name to anim file
};

#endif
