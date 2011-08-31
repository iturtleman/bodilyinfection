#include "Collision.h"
#include "Game.h"

//locks the screen for drawing pixels to the screen
void Slock(SDL_Surface *screen)
{   
    if ( SDL_MUSTLOCK(screen) )
    {   
        if ( SDL_LockSurface(screen) < 0 )
        {
            return;
        }
    }
}

void Sulock(SDL_Surface *screen)
{   
    if ( SDL_MUSTLOCK(screen) )
    {
        SDL_UnlockSurface(screen);
    }
}

void DrawPixel(SDL_Surface *screen, int x, int y, Uint32 c)
{
	//keep in bounds
	if(x>0 && y>0 && x < screen->w && y < screen->h){
		Uint32 color = c&0xFFFFFF;
		//Uint32 color1 = SDL_MapRGB(screen->format, c&0xff, c&0xFF00, c&0xFF0000);
		switch (screen->format->BytesPerPixel)
		{
		case 1: // Assuming 8-bpp
			{
				Uint8 *bufp;
				bufp = (Uint8 *)screen->pixels + y*screen->pitch + x;
				*bufp = color;
			}
			break;
		case 2: // Probably 15-bpp or 16-bpp
			{
				Uint16 *bufp;
				bufp = (Uint16 *)screen->pixels + y*screen->pitch/2 + x;
				*bufp = color;
			}
			break;
		case 3: // Slow 24-bpp mode, usually not used
			{
				Uint8 *bufp;
				bufp = (Uint8 *)screen->pixels + y*screen->pitch + x * 3;
				if(SDL_BYTEORDER == SDL_LIL_ENDIAN)
				{
					bufp[0] = color;
					bufp[1] = color >> 8;
					bufp[2] = color >> 16;
				} else {
					bufp[2] = color;
					bufp[1] = color >> 8;
					bufp[0] = color >> 16;
				}
			}
			break;
		case 4: // Probably 32-bpp
			{
				Uint32 *bufp;
				bufp = (Uint32 *)screen->pixels + y*screen->pitch/4 + x;
				*bufp = color;
				break;
			}
		}
	}
}

/**
	This function is tasked with drawing the 8 points on a circle so that only 1/8th need be calculated. 
	\param screen the Screen
	\param wPos This is the world position to draw the points (not & because may want to have an added value pushed in)
	\param pos This is the relative position to draw the points (not & because may want to have an added value pushed in)
	\param color the color to be drawn
*/
void DrawCircle(SDL_Surface* screen, const Point2D wPos, const Point2D pos, Uint32 color){
	DrawPixel(screen, int(wPos.x + pos.x), int(wPos.y + pos.y), color);
	DrawPixel(screen, int(wPos.x - pos.x), int(wPos.y + pos.y), color);
	DrawPixel(screen, int(wPos.x + pos.x), int(wPos.y - pos.y), color);
	DrawPixel(screen, int(wPos.x - pos.x), int(wPos.y - pos.y), color);
	DrawPixel(screen, int(wPos.x + pos.y), int(wPos.y + pos.x), color);
	DrawPixel(screen, int(wPos.x - pos.y), int(wPos.y + pos.x), color);
	DrawPixel(screen, int(wPos.x + pos.y), int(wPos.y - pos.x), color);
	DrawPixel(screen, int(wPos.x - pos.y), int(wPos.y - pos.x), color);
}
/**
	This function is tasked with drawing lines by the midpoint method. 
	\param screen the Screen
	\param start This is the start position to draw the point (not & because may want to have an added value pushed in)
	\param end This is the end position to draw the point (not & because may want to have an added value pushed in)
	\param color the color to be drawn
*/
void DrawLine(SDL_Surface* screen, const Point2D start, Point2D end, Uint32 color){
	Slock(screen);
	//values for calculation and max values
	int x,y;//start as low vals used for algorithm
	int xMax, yMax;
	
	//if the end point is lower swap em
	if(end.y < start.y){
		x = (int)end.x;
		y = (int)end.y;
		xMax = (int)start.x;
		yMax = (int)start.y;
	}
	else {
		x = (int)start.x;
		y = (int)start.y;
		xMax = (int)end.x;
		yMax = (int)end.y;
	}

	//the change in x
	int dX = xMax - x;

	//if divide by 0 draw a vertical line
	if(dX) {
		//change in y
		int dY = y - yMax;
		int sum = 2 * dY + dX;
		DrawPixel(screen, x, y, color);

		while( x < xMax){
			if(sum < 0){
				sum += 2 * dX;
				y++;
			}
			x++;
			sum += 2 * dY;
			DrawPixel(screen, x, y, color);
		}
	}
	else {
		while(y < yMax){
			DrawPixel(screen, x, y, color);
			y++;
		}
	}
	Sulock(screen);
}

void CollisionRectangle::draw(const Point2D& pos){
	Point2D nPos = pos + mPos;
	//top
	Point2D startPos = nPos + Point2D(-width/2, -height/2);
	Point2D endPos = nPos + Point2D(width/2, -height/2);
	DrawLine(Game::game()->Screen(), startPos, endPos, color);
	//bottom
	startPos = nPos + Point2D(-width/2, height/2);
	endPos = nPos + Point2D(width/2, height/2);
	DrawLine(Game::game()->Screen(), startPos, endPos, color);
	//left
	startPos = nPos + Point2D(-width/2, -height/2);
	endPos = nPos + Point2D(-width/2, height/2);
	DrawLine(Game::game()->Screen(), startPos, endPos, color);
	//right
	startPos = nPos + Point2D(width/2, -height/2);
	endPos = nPos + Point2D(width/2, height/2);
	DrawLine(Game::game()->Screen(), startPos, endPos, color);
}

void CollisionCircle::draw(const Point2D& pos){
	Slock(Game::game()->Screen());
	int x=0,y=(int)radius;///<The relative x,y pos
	int midpt = 1-(int)radius;///<the midpt of the circle
	//draw pts
	DrawCircle(Game::game()->Screen(), pos + mPos, Point2D(x,y), color);
	//calculate other points
	while(x<y){
		x++;
		if(midpt<0)
			midpt += 2 * x + 1;
		else{
			y--;
			midpt += 2 * (x-y) + 1;
		}
		//draw pts
		DrawCircle(Game::game()->Screen(), pos + mPos, Point2D(x,y), color);
	}
	Sulock(Game::game()->Screen());
}

bool Collision::checkCollisions(const vector<Collision*>& c, const Point2D cPos, const Point2D pos){
	for(unsigned int i=0; i < c.size(); i++){
		if(collision(c[i], cPos, pos))//if any are true return true then and there
			return true;
	}
	return false;//if none were found then return false.
}

bool CollisionRectangle::collision(const Collision *c, const Point2D cPos, const Point2D pos) const {
	if(const CollisionRectangle* rec = dynamic_cast<const CollisionRectangle*>(c)){
		///check rect vs rect really just axis aligned box check (simpler)
		double r1Left = -width/2 + mPos.x + pos.x;
		double r1Right = width/2 + mPos.x + pos.x;
		double r1Top = -height/2 + mPos.y + pos.y;
		double r1Bottom = height/2 + mPos.y + pos.y;

		double r2Left = -rec->width/2 + rec->mPos.x + cPos.x;
		double r2Right = rec->width/2 + rec->mPos.x + cPos.x;
		double r2Top = -rec->height/2 + rec->mPos.y + cPos.y;
		double r2Bottom = rec->height/2 + rec->mPos.y + cPos.y;

		bool outsideX = r1Right < r2Left || r1Left > r2Right;
		bool outsideY = r1Bottom < r2Top || r1Top > r2Bottom;
		return !(outsideY || outsideX);
	}
	else if(dynamic_cast<const CollisionCircle*>(c)){
		return c->collision(this, cPos, pos);
	}
	//if something breaks bad
	return false;
}

bool CollisionCircle::collision(const Collision *c, const Point2D cPos, const Point2D pos) const {
	if (const CollisionRectangle* r = dynamic_cast<const CollisionRectangle*>(c)) {
		collision(r, cPos, pos);///call the circle rect fn
	}
	else if(const CollisionCircle* col = dynamic_cast<const CollisionCircle*>(c)){
		///check circle vs circle
		if(((col->mPos + cPos) - (mPos + pos)).length() <= (col->radius + radius))
			return true;
		else
			return false;
	}

	return false;
}

bool CollisionCircle::collision(const CollisionRectangle* c, const Point2D cPos, const Point2D pos) const {
	///check rect vs circle \todo implement
	Point2D rectCent = cPos + c->mPos;//center of rect
	Point2D tl = rectCent + Point2D(-c->width/2,-c->height/2);//top left of rect
	Point2D bl = rectCent + Point2D(-c->width/2, c->height/2);//bottom left of rect
	Point2D tr = rectCent + Point2D( c->width/2,-c->height/2);//top right of rect
	Point2D br = rectCent + Point2D( c->width/2, c->height/2);//bottom right of rect
	//Are any of the points inside the circle?
	if( ((tl.x - mPos.x)*(tl.x - mPos.x) + (tl.y - mPos.y)*(tl.y - mPos.y) - radius*radius) <= 0 || 
		((tr.x - mPos.x)*(tr.x - mPos.x) + (tr.y - mPos.y)*(tr.y - mPos.y) - radius*radius) <= 0 ||
		((bl.x - mPos.x)*(bl.x - mPos.x) + (bl.y - mPos.y)*(bl.y - mPos.y) - radius*radius) <= 0 ||
		((br.x - mPos.x)*(br.x - mPos.x) + (br.y - mPos.y)*(br.y - mPos.y) - radius*radius) <= 0 )
		return true;
	//Is the circle inside the rectangle? (considers bounds)
	else if( !(
		mPos.x + radius < tl.x || 
		mPos.x - radius > tr.x || 
		mPos.y + radius < tl.y || 
		mPos.y - radius > bl.y ))
		return true;
	//It's not in there 
	return false;
}
