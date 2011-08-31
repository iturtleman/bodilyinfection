#ifndef __FNS_H__
#define __FNS_H__
#include <SDL/SDL.h>
#include <string>
/// \todo remove this this is just to make testing easy w/o gdb
#include <iostream>
using namespace std;

///Behaviors use functions that take nothing and return nothing
typedef void (*Behavior) ();
///As the name denotes, this just simply does nothing
static void DoNothing(){}
///Texture name
typedef unsigned int Texture;
///deg/rad
const float deg2rad = (float)(3.14159/180);

SDL_Surface* LoadImage( std::string filename );/**< Loads supported images. */

class SizeD;

/** 2D position or vector placed here because of general access*/
class Point2D
{
	public:
	friend class SizeD;
	Point2D ():x(0),y(0){}
	Point2D (double X, double Y):x(X),y(Y){}
	Point2D (const Point2D& copy)///< Copy object 
	{
		x = copy.x; 
		y = copy.y;
	}
	Point2D (const SizeD& copy);///< Copy object

	/** \todo add nomal vectors  */
	double x;/**< x position */
	double y;/**< y Position */
	double length();/**< The length of the Point (from origin)
	\return length of the vector/point
	*/
	Point2D unitVec();/**< The unit vector of the Point (from origin)
	\return length of the vector/point
	*/
	Point2D add(Point2D pt);/**< Adds the value of the point to this \return This after modificaiton*/
	Point2D sub(Point2D pt);/**< Subtracts the value of the point from this \return This after modification*/
	Point2D mult(double d);/**< Multiplies the values of this by i \return This after modificaiton*/
	Point2D div(double d);/**< Divides the values of this by i \return This after modificaiton*/
	Point2D operator+ (const Point2D& pt) const { return Point2D(x + pt.x, y + pt.y); }
	Point2D operator- (const Point2D& pt) const { return Point2D(x - pt.x, y - pt.y); }
	Point2D operator+= (Point2D& pt){  return add(pt); }
	Point2D operator-= (Point2D& pt){ return sub(pt); }
};

/** Just for more logical names for sizes of objects*/
class SizeD
{
	public:
	friend class Pont2D;
	SizeD ():w(0),h(0){}
	SizeD (double width, double height):w(width),h(height){}
	SizeD (const SizeD& copy)
	{
		w = copy.w;
		h = copy.h;
	}
	SizeD (const Point2D& copy)
	{
		w = copy.x;
		h = copy.y;
	}
	double w;/**< x position */
	double h;/**< y Position */
};
//template<class T> string to_string(const T& t);
#endif
