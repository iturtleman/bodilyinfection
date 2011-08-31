#include "Background.h"
#include "Game.h"
#include <iostream>
using namespace std;

Background::Background(SDL_Surface *screen, std::string name, std::string filename, bool wrap, Point2D ScreenPosition, SizeD ScreenSize, Point2D bgSamplePos, SizeD bgSize) :
mScreen(screen),
	_name(name),
	wrapping(wrap),
	mScreenSize(ScreenSize)
{
	mPos = ScreenPosition;
	mScreenSize.w = screen->w;
	mScreenSize.h = screen->h;

	source.w = (int)bgSize.w;
	source.h = (int)bgSize.h;
	source.x = (int)bgSamplePos.x;
	source.y = (int)bgSamplePos.y;

	load(filename);

	cout<<"background \""<<_name<<"\" created"<<endl;	
}
void Background::draw(){
	///destination rectangle only position is used and describes where the output is drawn.
	SDL_Rect dest;///<the place where the sample will be drawn (can be reused)
	double x = mPos.x;
	double y = mPos.y;
	const double X = mScreenSize.w;
	const double Y = mScreenSize.h;
	const double W = _imageSize.w;
	const double H = _imageSize.h;
	/*
	if(wrapping){
		while (x < 0) // move it to the left til it's on the screen
			x += W;
		while (y < 0) // move it up til it's on the screen
			y += H;
		while(x > 0)//move back til we are at the spot to the left of the current background
			x -= W;
		int initx = (int)x;
		while(y > 0)
			y -= H;
		for(y; y < Y; y += H)
			for(x = initx; x < X; x += W){
				dest.x = (int)x;
				dest.y = (int)y;
				SDL_BlitSurface(mBackground, NULL, mScreen, &dest);
			}
	}
	else {
		dest.x = (int)mPos.x;
		dest.y = (int)mPos.y;
		SDL_BlitSurface(mBackground, &source, mScreen, &dest);
	}
	//*/

}

SDL_Surface* Background::load(std::string filename){
	/*mBackground = LoadImage("Backgrounds/"+filename);
	cout<<"Loaded file"<<endl;
	cout<<"Setting Size...";
	_imageSize=SizeD(mBackground->w,mBackground->h);///<set the size of the background.
	cout<<"done!"<<endl;
	return mBackground;*/
	return NULL;
}

void Background::setWrap(bool tf){
	wrapping=tf;
}

bool Background::getWrap(){
	return wrapping;
}

void Background::drawCollisions(){
	WorldObject::drawCollisions(collisionData, mPos);
}

