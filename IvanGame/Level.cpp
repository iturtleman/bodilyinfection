#include "Level.h"
#include "Game.h"
#include "fns.h"
#include <GL/gl.h>

Level::Level(SDL_Surface* screen) : mScreen(screen)
{
	mLoadBehavior = DoNothing;
	mUpdateBehavior = DoNothing;
}

Level::Level(SDL_Surface* screen, string n, Behavior loadBehave, Behavior updateBehave) : mScreen(screen), mName(n), mBackground(NULL)
{
	/// Set behavior
	mLoadBehavior = loadBehave;
	mUpdateBehavior = updateBehave;
}

Level::~Level()
{
	for (size_t i = 0; i < mSprites.size(); ++i) {
		delete mSprites[i];
	}
}

void Level::DrawIMG(SDL_Surface *img, int x, int y)
{
    SDL_Rect dest;
    dest.x = x;
    dest.y = y;
    SDL_BlitSurface(img, NULL, mScreen, &dest);
}


void Level::DrawIMG(SDL_Surface *img, int x, int y, int w, int h, int x2, int y2)
{
    SDL_Rect dest;
    dest.x = x;
    dest.y = y;
    SDL_Rect src;
    src.x = x2;
    src.y = y2;
    src.w = w;
    src.h = h;
    SDL_BlitSurface(img, &src, mScreen, &dest);
}


void Level::drawScene()
{
	Uint32 color;

    /** Create a black mBackgroundground using the mScreen pixel format (32 bpp) */
	glClear(GL_COLOR_BUFFER_BIT);
	
	/** Draw BG */
	if(mBackground!=NULL)
		mBackground->draw();
	
	/** Draw Sprites*/
	DrawSprites();
    
	/** Draw Boundary Data */
	DrawCollisions();

	/** Flip the working image buffer with the mScreen buffer */
	SDL_GL_SwapBuffers();
}

void Level::DrawSprites()
{
	for (size_t i=0; i<mSprites.size(); ++i) {
		mSprites[i]->draw();
	}
}

void Level::DrawCollisions()
{
	if(Game::game()->ShowCollisions){
		mBackground->drawCollisions();
		for (size_t i=0; i<mSprites.size(); ++i) {
			mSprites[i]->drawCollisions();
		}
	}
}

void Level::LoadBG(std::string name)
{
	mBackground->load(name);
}

void Level::postEvent(SDL_Event event)
{
	switch (event.type) {
	case SDL_KEYDOWN: {
		
	}
	}
}

/** Will handle all movement and dynamic triggers. */
void Level::update(){
	mUpdateBehavior();
}

void Level::load(){
	mLoadBehavior();
}


void Level::addSprite(Sprite* sp){
	mSprites.push_back(sp);
}

Sprite* Level::getSprite(string name){
	for(int i=0, count=mSprites.size(); i < count; i++){
		if(mSprites[i]->getName()==name){
			return mSprites[i];
		}
	}
	return NULL;
}

void Level::removeSprite(Sprite* sp){
	for(vector<Sprite*>::iterator ptr = mSprites.begin(); ptr != mSprites.end(); ptr++){
		if(*ptr == sp){
			mSprites.erase(ptr);
			return;
		}
	}
}

void Level::addAnimation(Animation anim){
	mAnims.push_back(anim);
}

Animation* Level::getAnimation(string name){
	for(int i=0, count=mAnims.size(); i < count; i++){
		if(mAnims[i].mName==name){
			return &mAnims[i];
		}
	}
	return NULL;
}

void Level::removeAnimation(Animation anim){
	for(vector<Animation>::iterator ptr = mAnims.begin(); ptr != mAnims.end(); ptr++){
		if(*ptr == anim){
			mAnims.erase(ptr);
			return;
		}
	}
}

void Level::addActor(string name, Actor actor){
	mActors.insert(make_pair(name, actor));
}

void Level::removeActor(string name){
	mActors.erase(name);
}

Sprite* Level::findSpriteByName(string name){
	/// \todo make this return a list of all sprites with the same name (or make it specifiable)
	for(unsigned int i=0; i < mSprites.size(); i++){
		if(mSprites[i]->getName()==name)//find the sprite with the same name
			return mSprites[i];
	}
	return NULL;//if a sprite wasn't found return null
}
