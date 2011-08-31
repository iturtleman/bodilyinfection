#ifndef __COLLISION_H__
#define __COLLISION_H__
#include "fns.h"
#include <vector>
#include <string>

using std::vector;
using std::string;

/**
  This defines Collision boundaries and data for objects/levels

  This class is tasked with the following:
    - being a parent for collisions.
 */

class Collision
{
public:
	Collision():mPos(0.0,0.0),color(0xFF00FF00){ }
	Point2D mPos;/**< The position of the center of the collision data */
	/** 
		Check collision with objects
		\param c the collision data associated with the sprite to check collision with.
		\param cPos the position of the sprite we want to check collision with.
		\param pos the current sprite's position.
		\return Returns true if any of the object's collision datas are colliding  .
	*/
	bool checkCollisions(const vector<Collision*>& c, const Point2D cPos, const Point2D pos); 
	/** 
		Check collision with objects
		\param c a single collision data for an object
		\param cPos the position of the sprite we want to check collision with.
		\param pos the current sprite's position.
		\return Returns true if any of the object's collision datas are colliding  .
	*/
	virtual bool collision(const Collision *c, const Point2D cPos, const Point2D pos) const = 0;
	/// \todo See this http://www.metanetsoftware.com/technique/tutorialA.html
	virtual void draw(const Point2D& pos) = 0;/**< Draws the collision data to the screen */
protected:
	string name;///< Name of this behavior \todo make this actually be useful
	Uint32 color;///< The collision box color
};

/**
  This defines CollisionRectangle boundaries and data for objects/levels

  This class is tasked with the following:
    - Defining a collision rectangle.
	- Checking collisions with other Rectangles
	- Checking collisions with circles
 */

class CollisionRectangle : public Collision
{
public:
	CollisionRectangle():width(1.0),height(1.0){};
	CollisionRectangle(Point2D pt, double w, double h):width(w),height(h){ mPos = pt;}
	double width;/**< Rectangle's width */
	double height;/**< Rectangle's height */
	/** 
		Check collision with objects implements rectangle on rectangle
		\param c a single collision data for an object
		\param cPos the position of the sprite we want to check collision with.
		\param pos the current sprite's position.
		\return Returns true if any of the object's collision datas are colliding  .
	*/
	virtual bool collision(const Collision *c, const Point2D cPos, const Point2D pos) const;
	virtual void draw(const Point2D& pos);/**< Draws the collision data to the screen */
};

/**
  This defines CollisionCircle boundaries and data for objects/levels

  This class is tasked with the following:
    - Defining a collision circle.
	- Checking collisions with Rectangles
	- Checking collisions with other circles
 */

class CollisionCircle : public Collision
{
public:
	CollisionCircle();
	CollisionCircle(Point2D pt, double r) : radius(r){ mPos = pt; }
	double radius; /**< The raidus of the circle */
	/**
		This does collision between Rectangles and Circles
		\param c a single collision data for an object
		\param cPos the position of the sprite we want to check collision with.
		\param pos the current sprite's position.
		\return Returns true if any of the object's collision datas are colliding  .
	*/
	bool collision(const CollisionRectangle* c, const Point2D cPos, const Point2D pos) const;
	/** 
		Check collision with objects implements circle on circle
		\param c a single collision data for an object
		\param cPos the position of the sprite we want to check collision with.
		\param pos the current sprite's position.
		\return Returns true if any of the object's collision datas are colliding  .
	*/
	virtual bool collision(const Collision *c, const Point2D cPos, const Point2D pos) const;
	
	virtual void draw(const Point2D& pos);/**< Draws the collision data to the screen */
};
#endif
