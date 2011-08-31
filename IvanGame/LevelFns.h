#ifndef __LEVELFNS_H__
#define __LEVELFNS_H__
#include "Game.h"
#include "Level.h"

///\file this file should contain all Behaviors for Levels

void LevelWorldLoad(){
	Level* l = Game::game()->getCurrentLevel();

	SDL_Surface* screen = l->getScreen();

	/// load background
	l->setBackground(new Background(screen, "default", "bg.bmp"));//mBackground = LoadImage("Backgrounds/bg.bmp");

	/** load animations */
	l->addAnimation(Animation("viking.anim"));
	l->addAnimation(Animation("sun.anim"));

	/** load sprites */
	Sprite* vikings1 = new Sprite(screen, "viking1", l->getAnimation("viking.anim"));
	vikings1->setPosition(0,0);
	vikings1->setSpeed(1);

	Sprite* vikings2 = new Sprite(screen, "viking2", l->getAnimation("viking.anim"));
	vikings2->setPosition(350,300);
	vikings2->setSpeed(1.5);

	Sprite* sun = new Sprite(screen, "sun", l->getAnimation("sun.anim"));
	sun->setPosition(480,50);
	sun->setSpeed(1);
}

void LevelWorldUpdate(){
	Level* LevelWorld = Game::game()->getCurrentLevel();

	Sprite* player = LevelWorld->getSprite("viking1");

	Background* bg = LevelWorld->getBackground();

	Uint8 *keys = SDL_GetKeyState(NULL);
	if ( keys[SDLK_LEFT] ) { player->xadd(-1); }
	if ( keys[SDLK_RIGHT] ) { player->xadd(1); }
	if ( keys[SDLK_UP] ) { player->yadd(-1); }
	if ( keys[SDLK_DOWN] ) { player->yadd(1); }
	if ( keys[SDLK_a] ) { bg->xadd(-1); }
	if ( keys[SDLK_d] ) { bg->xadd(1); }
	if ( keys[SDLK_w] ) { bg->yadd(-1); }
	if ( keys[SDLK_s] ) { bg->yadd(1); }
}

#endif