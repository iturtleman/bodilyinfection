using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BodilyInfection
{
    class WorldObject
    {
	public WorldObject(int z = 0) {
		ZOrder = z;
        mVisible = true;
        mTransparency = 1;
        mAngle = 0;
	 }

	public virtual abstract void Draw(Microsoft.Xna.Framework.GameTime gameTime);/**< Draws the Object. */
	public void drawCollisions(List<Collision> vec, Vector2 pos) {
        for(int i=0; i < vec.Count; i++){
		    vec[i].Draw(pos);
	    }   
    }/**< Draws Collision data for the Object */



	public void xadd(int num) {mPos.X += num;}/**< Increase x coordiante by a given amount. */
	public void yadd(int num) {mPos.Y += num;}/**< Increase y coordinate by a given amount. */
	public void xset(int x) {mPos.X = x;}/**< Sets the Sprite's x Coordinate. */
	public void yset(int y) {mPos.Y = y;}/**< Sets the Sprite's y coordinate.  */
	public void setPosition(int x, int y) {mPos.X = x; mPos.Y = y;}/**< Sets the Sprite's x an y coordinate. */
	public void setTransparency(float f){ mTransparency = f>1?1:f<0?0:f;}/**< Sets the Sprite's transparency. \param f The sprite's transparancy [0,1] other values will be force set */
	public float getTransparency(){return mTransparency;}/**< Gets the Sprite's transparency. */
	public void setAngle(float a){ mAngle = a; while(a>360)a-=360;}/**< Sets the Sprite's angle in degrees. \param a Angle in degrees */
	public float getAngle(){return mAngle;}/**< Gets the Sprite's angle. */

	protected Vector2 mPos;/**< The current (x,y) position */
	protected bool mVisible;/**< Determine if Object should be visible */
	protected float mTransparency;/**< Transparency! */
	protected float mAngle;/**< Angle of rotation */
	protected int ZOrder;/**< Stacking order. Determines what draws on top. \todo implement. */
}
