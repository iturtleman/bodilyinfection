#include "fns.h"
#include <SDL/SDL_image.h>
#include <math.h>

#ifndef RELEASE
#include <iostream>
using std::cout;
using std::endl;
#endif

SDL_Surface* LoadImage( std::string filename )
{
	SDL_Surface* loaded_image = NULL;
	SDL_Surface* compatible_image = NULL;

	filename="ArtAssets/"+filename;
	if(filename.c_str() == NULL) /**< check to see if a filename was provided. */
	{
		printf("No Filename %s\n\n",filename.c_str());
		return NULL;/**< if not exit the function */
	}


	/** Load the image using our new IMG_Load function from sdl-Image1.2 */
	loaded_image = IMG_Load( filename.c_str() );
	
	if(!loaded_image) /**< check to see if it loaded properly */
	{
		printf("Error opening %s\n\n",filename.c_str());
		return NULL;
	}

	
	/** the image loaded fine so we can now convert it to the current display depth */
	compatible_image = SDL_DisplayFormat( loaded_image );

	/** Destroy the old copy */
	SDL_FreeSurface( loaded_image );

	/** return a pointer to the newly created display compatible image */
	return compatible_image;
}

Point2D::Point2D (const SizeD& copy){
	x = copy.w; 
	y = copy.h;
}

double Point2D::length(){
	return sqrt(x*x + y*y);
}

Point2D Point2D::unitVec(){
	double len = length();///< \todo optimize
	return Point2D((x/len), (y/len));
}

Point2D Point2D::add(Point2D pt){
	x += pt.x;
	y += pt.y;
	return *this;
}
Point2D Point2D::sub(Point2D pt){
	x -= pt.x;
	y -= pt.y;
	return *this;
}
Point2D Point2D::mult(double d){
	x *= d;
	y *= d;
	return *this;
}
Point2D Point2D::div(double d){
	x /= d;
	y /= d;
	return *this;
}


/*
template<class T> string to_string(const T& t)
{
    ostringstream os;
    os << t;
    return os.str();
}//*/
