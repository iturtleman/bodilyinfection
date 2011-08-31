#ifndef __GAME_H__
#define __GAME_H__

#include "Level.h"
#include "Sprite.h"
#include <vector>
#include <string>

/**
  High-level controller for the game

  This class is tasked with the following:
    - creating the SDL screen
    - loading levels from appropriate locations
    - switching levels as appropriate
  NOTE: If the screen needs to be accessed, you can use Game::game()->Screen();
 */
class Game
{
public:
	/** This Makes an instance of Game if there is not one and then returns it \return The level.*/
	static Game* game();

	/** Decides whether or not to draw bounding boxes*/
	bool ShowCollisions;

	/** Decides whether or not to output FPS*/
	bool ShowFPS;

	/**
	  Gets the current level being played.
	  \return current level
	 */
	Level* getCurrentLevel();

	/**
	  Sets the current level to play.
	  \param name Name of the level to load
	 */
	void setCurrentLevel(string name);

	/**
	  Gets the index of the current level being played.
	  \return index of the current level
	 */
	int getLevelIndex();

	/**
	  Gets the total count of levels.
	  \return number of levels
	 */
	int getLevelCount();

	/** Makes the game loop begin and load objects. */
	void run();

	/** Returns the main screen to allow for drawing */
	SDL_Surface* Screen(){ return mScreen; }

	/** Retrns the Current FPS of the game */
	Uint32 getFPS() { return currentFPS;}

	/** The current Sprite selected by the game */
	static Sprite* CurrentSprite;

protected:
	Game();
	~Game();
	/** This is the pointer to the Static Game object insuring only one */
	static Game* m_instance;

	/** Loads the game's resources. */
	void LoadResources();

private:
	static const Uint32 waitTime;

	unsigned int mCurrentLevel;/**< Current Level index. */
	std::vector<Level*> mLevels;/**< Vector of pointers to all of the levels. \todo Make into map? */
	SDL_Surface *mScreen;/**< The surface to draw to. */
	map<string,Behavior*> mBehaviors;/**< This level's Behaviors */

	static Uint32 timerCallback(Uint32 interval, void* data);
	
	//fps data calc
	Uint32 startclock;
	Uint32 deltaclock;
	Uint32 currentFPS;

};

#endif
