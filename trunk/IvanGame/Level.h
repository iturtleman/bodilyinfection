#ifndef __LEVEL_H__
#define __LEVEL_H__
#include "Sprite.h"
#include "Background.h"
#include <vector>
#include <map>


#include <string>

using std::vector;
using std::string;

/**
  This is a stand alone Level that will have varying jobs

  This class is tasked with the following:
    - drawing the scence and all objects in order
	- loading the background
	- keeping track of animations
	- moving objects
	- handling events
 */

class Level
{
public:
	Level(SDL_Surface* screen);
	/**
		Level constructor
		\param screen the Screen to be drawn to
		\param name of the level
		\param loadBehavior the Loading Behavior for this level (Run once).
		\param updateBehavior the Updating Behavior for this level (Run every loop).
	*/
	Level(SDL_Surface* screen, string name, Behavior loadBehave = DoNothing, Behavior updateBehave = DoNothing);
	~Level();
	void drawScene();/**< Draws everything that is set to draw on the screen. */
	void LoadBG(string name);/**< Loads the Background. \param name Name of the background to load */
	virtual void postEvent(SDL_Event event);/**< Passes along SDL events */
	void update();/**< Loop that runs every game frame that calculates movement, placement, etc.  as well as obtains key strokes (limited by keyboard hardware)*/
	void load();/**< Runs load action*/
	void addSprite(Sprite* sp);/**< add a Sprite to the list of sprites \param sp Sprite to add */
	Sprite* getSprite(string name);/**< gets the first sprite with the given name \param name Name of the sprite \return Pointer to the requested Sprite */
	void removeSprite(Sprite* sp);/**< remove the Sprite sp from the list of sprites \param ps Sprite to remove */
	void addAnimation(Animation anim);/**< add a Actor to the list of sprites \param anim The actor to add */
	Animation* getAnimation(string name);/**< get a Actor* to the list of sprites \param name Name of the actor \return Pointer to the actor we requested or null */
	void removeAnimation(Animation anim);/**< remove an Actor to the list of sprites  \param anim The actor to add */
	void addActor(string name, Actor actor);/**< add a Actor to the list of sprites \param name Name of the actor \param actor The actor to add */
	void removeActor(string name);/**< remove the Actor sp from the list of sprites \param name Name of the actor to remove */
	string getName(){ return mName; }/**< returns the current level's name \return Level's name */
	void setBackground(Background* b){mBackground=b;}/**< sets the current level's background \param b Background */
	Background* getBackground(){return mBackground;}/**< gets the current level's name \return Level's Background */
	SDL_Surface* getScreen(){return mScreen;}/**< gets the current level's name \return Level's SDL_Surface */
	
	Sprite* findSpriteByName(string name);/**< returns the first Sprite with a given name \param name Name of the sprite to return \return Pointer to the requested sprite */
	
protected://allow inheritance
	string mName;
	Background* mBackground;/**< Pointer to the current Background. */
	SDL_Surface *mScreen;/**< Pointer to the screen. */
	vector<Sprite*> mSprites;/**< Vector of all sprites on the level. \todo Maybe make into map? This should be a list of World objects once implemented. \todo add accessor or move to public \todo make a map by name (for collisions) \todo make a list ordered by Zorder (for drawing*/
	map<string,Actor> mActors;/**< This level's actors. */
	vector<Animation> mAnims;/**< This level's Animations */
	void DrawIMG();/**< Draws an image to the screen. (Not used) */
	void DrawIMG(SDL_Surface *img, int x, int y);/**< Draws the specified image to the screen at location (x,y) */
	void DrawIMG(SDL_Surface *img, int x, int y, int w, int h, int x2, int y2);/**< Draws an image at the specified location (x,y) that is blitted with width w and height h from the point (x2,y2) of the given image */
	void DrawSprites();/**< Draws all Sprites. */
	void DrawCollisions();/**< Draws all Collision data */
	Behavior mLoadBehavior;/**< will be used to define the Load action of the level */
	Behavior mUpdateBehavior;/**< will be used to define the update action of the level */
};
#endif
