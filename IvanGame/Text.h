#ifndef __TEXT_H__
#define __TEXT_H__
#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <SDL/SDL.h>

/**
  On-screen text

  This class is tasked with the following:
    - displaying text on screen.
 */

class Text
{
	public:
	SDL_Surface *font;
	int width;
	int charWidth;
	int* widths;
	unsigned char* data;
	void draw(SDL_Surface *screen, SDL_Surface *img, int x, int y, int w, int h, int x2, int y2);
	Text* init(std::string imageMap, float r, float g, float b, float a);
	inline Text* initFont(std::string imageMap, float r, float g, float b){ return init(imageMap, r,g,b,1); }
	inline Text* initFont(std::string imageMap){ return init(imageMap, 1,1,1,1); }
	void drawString(SDL_Surface *screen, Text *font, int x, int y, std::string str, ...);
	int stringWidth(Text *font,std::string str,...);
	void DeleteFont();
	void DeleteFont(Text *font);
};
#endif
