#include "Game.h"
#include "fns.h"
#include "LevelFns.h"

///\file this file should contain implementations for loading resources

void Game::LoadResources(){
	mLevels.push_back(new Level(mScreen, "World", LevelWorldLoad, LevelWorldUpdate));
}

