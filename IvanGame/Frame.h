#ifndef __FRAME_H__
#define __FRAME_H__
#include "Collision.h"
#include <string>
#include <SDL/SDL.h>
#include <SDL/SDL_image.h>
#include <vector>
#include "fns.h"

struct SpriteFrame{
	Texture image;/**< Image Texture */
	int pause;/**< Tells the amount of time to pause between this frame and the next. */
	int width;/**< Base width of the frame's image. \todo make this and animation's match or at least sync or delete*/
	int height;/**< Base height of the frame's image. \todo make this and animation's match or at least sync or delete*/
	vector<Point2D> hotSpots;/**< Hot spots that can be used for locating objects on the sprite default is tagged to center of the sprite \todo implement default*/
	Point2D animationPeg;/**< The offeset from position to place the image. Defaults to (0,0) */
	vector<Collision*> collisionData;/**< The collision data for this sprite */
};

#endif