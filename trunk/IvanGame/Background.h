#ifndef __BACKGROUND_H__
#define __BACKGROUND_H__
#include "fns.h"
#include "WorldObject.h"
#include "Collision.h"
#include <string>
#include <vector>
#include <iostream>

using std::string;
using std::vector;
/**
  The Background

  This class is tasked with the following:
    - loading the background 
	- drawing the background
 */

class Background : public WorldObject{
	private:
	string _name;///<The background's name
	SizeD _imageSize;///<The actual background image's full size.

	Texture mBackground;///<Background Texture
	SDL_Surface *mScreen;///< The screen to be drawn to.
	SizeD mScreenSize;///<This is the size of the screen to be drawn to. used to get the sections of the background when moving and for screen-at-a-time scrolling.
	SDL_Rect source;///< This is the rectangle that gets taken from the actual background image (h,w) determine the size of the rectangle to sample from the background image. (x,y) determine the position of the rectangle's top left corner on the actual background image.
	bool wrapping;
	vector<Collision*> collisionData;///<The collision data for this Background \todo implement collisions

	
	public:
	Background(	SDL_Surface *screen,
				string name="default", 
				string filename="bg.bmp", 
				bool wrap=true, 
				Point2D ScreenPosition=Point2D(0,0), 
				SizeD screenSize=SizeD(800,600),
				Point2D bgSamplePos=Point2D(0,0), 
				SizeD bgSize=SizeD(800,600)
	);
	void draw();/**< Draw background. */
	string getName(){ return _name; }/** Returns name. */
	void setName(string name){ _name = name; }/** Set Name. */
	SizeD size(){ return _imageSize;}/** returns image size */
	SDL_Surface* load(string name="default");///< Loads a Bacground by name.
	SizeD getPosition(){return mPos;}
	void setWrap(bool tf);
	bool getWrap();
	void drawCollisions();/**< Draws Collision data for the Object (from outside) */

};
#endif
