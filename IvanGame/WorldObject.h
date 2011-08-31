#ifndef WORLDOBJECT
#define WORLDOBJECT
#include "fns.h"
#include "Collision.h"

class WorldObject
{
public:
	WorldObject(int z = 0) :
		ZOrder(z),
		mVisible(true),
		mTransparency(1),
		mAngle(0)
	  {}
	virtual void draw() = 0;/**< Draws the Object. */
	void drawCollisions(vector<Collision*>& vec, const Point2D& pos);/**< Draws Collision data for the Object */
	void xadd(int num) {mPos.x += num;}/**< Increase x coordiante by a given amount. */
	void yadd(int num) {mPos.y += num;}/**< Increase y coordinate by a given amount. */
	void xset(int x) {mPos.x = x;}/**< Sets the Sprite's x Coordinate. */
	void yset(int y) {mPos.y = y;}/**< Sets the Sprite's y coordinate.  */
	void setPosition(int x, int y) {mPos.x = x; mPos.y = y;}/**< Sets the Sprite's x an y coordinate. */
	void setTransparency(float f){ mTransparency = f>1?1:f<0?0:f;}/**< Sets the Sprite's transparency. \param f The sprite's transparancy [0,1] other values will be force set */
	float getTransparency(){return mTransparency;}/**< Gets the Sprite's transparency. */
	void setAngle(float a){ mAngle = a; while(a>360)a-=360;}/**< Sets the Sprite's angle in degrees. \param a Angle in degrees */
	float getAngle(){return mAngle;}/**< Gets the Sprite's angle. */

protected:
	Point2D mPos;/**< The current (x,y) position */
	bool mVisible;/**< Determine if Object should be visible */
	float mTransparency;/**< Transparency! */
	float mAngle;/**< Angle of rotation */
	int ZOrder;/**< Stacking order. Determines what draws on top. \todo implement. */
};
#endif
