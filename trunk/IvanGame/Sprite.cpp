#include "Sprite.h"
#include "Game.h"
#include <iostream>
#include <GL/gl.h>

using std::cout;
using std::endl;
Sprite::Sprite(SDL_Surface *screen, std::string name, Actor actor) :
mName(name),
	mDrawn(0),
	mLastUpdate(0),
	mActor(actor),
	mScreen(screen)
{
	//add to current level
	Game::game()->getCurrentLevel()->addSprite((this));

	if(mActor.mAnimations[mActor.mCurrentAnimation] -> mBuilt)
	{
		if (mActor.mAnimations[mActor.mCurrentAnimation]->mNumFrames > 1) mAnimating = 1;
	}
}

void Sprite::draw()
{
	//Frame so we don't have to find it so often
	SpriteFrame* frame = getAnimation();
	if(mAnimating == 1) {
		if(mLastUpdate+frame->pause*mSpeed<SDL_GetTicks()) {
			//obtain current peg 
			Point2D ppos = frame->animationPeg;
			mActor.mFrame++;
			if(mActor.mFrame > mActor.mAnimations[mActor.mCurrentAnimation]->mNumFrames-1)
				mActor.mFrame=0;
			//update frame so we don't need to worry
			frame = &mActor.mAnimations[mActor.mCurrentAnimation]->mFrames[mActor.mFrame];
			//obtain next peg
			Point2D npos = frame->animationPeg;
			//move current position to difference of two
			mPos.add(ppos - npos);
			mLastUpdate = SDL_GetTicks();
		}
	}
	if(mVisible == true){
		glEnable(GL_BLEND);
		glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
		glColor4f(1, 1, 1, mTransparency);
		glLoadIdentity();
		glTranslated(mPos.x,mPos.y,0);//move to position
		glTranslated(frame->animationPeg.x,frame->animationPeg.y,0);//move back to where it was
		glRotatef(mAngle,0.0f,0.0f,1.0f);//rotate shape
		glTranslated(-frame->animationPeg.x,-frame->animationPeg.y,0);//place animation peg on origin
		glBindTexture(GL_TEXTURE_2D, frame->image);
		glBegin( GL_QUADS );
		// Top-left vertex (corner)
		glTexCoord2f( 0, 0 );
		glVertex3d( 0, 0, 0 );

		// Bottom-left vertex (corner)
		glTexCoord2f( 1, 0 );
		glVertex3d( frame->width, 0, 0 );

		// Bottom-right vertex (corner)
		glTexCoord2f( 1, 1 );
		glVertex3d( frame->width, frame->height, 0 );

		// Top-right vertex (corner)
		glTexCoord2f( 0, 1 );
		glVertex3d( 0, frame->height, 0 );
		glEnd();
		glDisable(GL_BLEND);
		glLoadIdentity();
	}

}

vector<Collision*>& Sprite::getCollisionData(){
	return mActor.mAnimations[mActor.mCurrentAnimation]->mFrames[mActor.mFrame].collisionData;
}


void Sprite::drawCollisions(){
	//get the frame for readability
	SpriteFrame frame = mActor.mAnimations[mActor.mCurrentAnimation]->mFrames[mActor.mFrame];
	//center the location
	Point2D pt = mPos + frame.animationPeg;
	WorldObject::drawCollisions(frame.collisionData, pt);
}

Sprite* Sprite::collisionWithSprite(string name){
	//get the frame for readability
	SpriteFrame* frame = getAnimation();
	//get the first sprite with this name
	Sprite* s= Game::game()->getCurrentLevel()->findSpriteByName(name);
	if(s==NULL)
		return NULL;
	for(unsigned int i=0; i <  frame->collisionData.size(); i++)
		if(frame->collisionData[i]->checkCollisions(s->getCollisionData(), s->mPos + s->getAnimation()->animationPeg, mPos + frame->animationPeg))//if there is a collision
			return s;
	return NULL;//if there aren't collisions
}