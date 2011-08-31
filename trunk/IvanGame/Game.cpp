#include "Game.h"
#include "fns.h"
#include <string>
#include <iostream>
#include <GL/gl.h>

const Uint32 Game::waitTime = 1000/60; /* ms */

using namespace std;

void init_GL(int w = 640, int h = 480)
{
	// Set the OpenGL state after creating the context with SDL_SetVideoMode

	glClearColor( 0, 0, 0, 0 );

	glEnable( GL_TEXTURE_2D ); // Need this to display a texture

	glViewport( 0, 0, w, h );

	glMatrixMode( GL_PROJECTION );
	glLoadIdentity();

	glOrtho( 0, w, h, 0, -1, 1 );

	glMatrixMode( GL_MODELVIEW );
	glLoadIdentity();
}


/** Global static pointer used to ensure a single instance of the class. */
Game* Game::m_instance = NULL;

/** This function is called to create an instance of the class.
	Calling the constructor publicly is not allowed.
	The constructor is private and is only called by this Instance function.
*/
Game* Game::game(){
	if(!m_instance)
		m_instance = new Game;
	return m_instance;
}

Game::Game() : mCurrentLevel(0), mScreen(0), ShowCollisions(false), ShowFPS(false), startclock(0), deltaclock(0), currentFPS(0)
{
	int screenwidth = 640;
	int screenheight = 480;

	/** Initialize SDL */
	if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_TIMER) < 0) {
		printf ("Couldn't initialize SDL: %s\n", SDL_GetError ());
		exit (1);
	}
	atexit (SDL_Quit);

	/** Set 800x600 video mode */
	mScreen = SDL_SetVideoMode(screenwidth, screenheight, 32, SDL_OPENGL);
	if (!mScreen) {
		printf ("Couldn't set video mode: %s\n", SDL_GetError ());
		exit (2);
	}

	/** Set the title of our application window handler */
	SDL_WM_SetCaption("Ivan's Game", NULL);

	SDL_ShowCursor(0);
	
	init_GL(screenwidth, screenheight);

	/** Load Resources */
	LoadResources();
	
	// set start time for fps calc
	startclock = SDL_GetTicks();
}

Game::~Game()
{
	for (size_t i = 0; i < mLevels.size(); ++i) {
		delete mLevels[i];
	}
}

Level* Game::getCurrentLevel()
{
	if (mCurrentLevel < (size_t)mLevels.size()) {
		return mLevels[mCurrentLevel];
	} else {
		return NULL;
	}
}

int Game::getLevelIndex()
{
	return mCurrentLevel;
}

void Game::setCurrentLevel(string name){
	for(unsigned int i=0, count = mLevels.size(); i < count; i++){
		if(mLevels[i]->getName() == name){
			mCurrentLevel=i;
			this->mLevels[mCurrentLevel]->load();
			return;
		}
	}
	cout<<"Level "<<name<<" does not exist."<<endl;
}

int Game::getLevelCount()
{
	return mLevels.size();
}

void Game::run()
{
	bool done = false;

	SDL_TimerID timer = SDL_AddTimer(waitTime, timerCallback, NULL);
	if (!timer) {
		std::cerr << "Timer creation failed" << std::endl;
		return;
	}

	setCurrentLevel("World");

	/** Game Loop */
	while (!done) {
		/** This will let us track events */
		SDL_Event event;

		/** Wait for an event to occur */
		if (0 == SDL_WaitEvent(&event)) {
			std::cout<<"waiting"<<std::endl;
			break;
		}

		switch (event.type) {
			/** If the event is a click on the close button in the top
			right corner of the window, we kill the application
			*/
		case SDL_QUIT:
			done = 1;
			break;

		case SDL_USEREVENT:
			getCurrentLevel()->drawScene();
			break;

			/** If our event reports a key being pressed down
			we process it
			*/
		case SDL_KEYDOWN: 
			{
				Uint8 *keys = SDL_GetKeyState(NULL);
				std::cout<<*keys<<std::endl;
				if (keys[SDLK_ESCAPE]) {
					done = 1;
					break;
				}
				if (keys[SDLK_F11]) {
					ShowCollisions = !ShowCollisions;
				}
				if (keys[SDLK_F10]) {
					ShowFPS = !ShowFPS;
				}
			}
		default: /* fallthrough if the key wasn't escape or control*/
			getCurrentLevel()->postEvent(event);
			break;
		}

		getCurrentLevel()->update();
		if(Game::game()->ShowFPS)
			cout<<Game::game()->getFPS()<<endl;

		//obtain the current fps data.
		deltaclock = SDL_GetTicks() - startclock;
		startclock = SDL_GetTicks();
		if ( deltaclock != 0 )
			currentFPS = 1000 / deltaclock;
	}

	SDL_RemoveTimer(timer);
}

Uint32 Game::timerCallback(Uint32 interval, void* data)
{
	SDL_Event event;
	SDL_UserEvent uevent = {
		SDL_USEREVENT, /* type */
		0, /* code */
		0, /* data1 */
		0 }; /* data2 */
		event.user = uevent;

		SDL_PushEvent(&event);

		return interval;
}

