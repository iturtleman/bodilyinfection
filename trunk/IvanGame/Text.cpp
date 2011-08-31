#include "Text.h"
#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <iostream>
#include <fstream>
#include <sstream>
#include "fns.h"
using namespace std;

void Text::draw(SDL_Surface *screen, SDL_Surface *img, int x, int y, int w, int h, int x2, int y2)
{
	SDL_Rect dest;
	dest.x = x;
	dest.y = y;
	SDL_Rect src;
	src.x = x2;
	src.y = y2;
	src.w = w;
	src.h = h;
	SDL_BlitSurface(img, &src, screen, &dest);
}

Text* Text::init(std::string iniFile, float r, float g, float b, float a)
{
	Text *tempFont;
	int width;
	string buffer, var, fontFile, datFile;
	unsigned char tmp;
	SDL_Surface *tempSurface;
	iniFile="ArtAssets/Fonts/"+iniFile;
	ifstream fin(iniFile.c_str());

	if(!fin)
	{
		printf("Error opening %s\n\n",iniFile.c_str());
	}

	while(getline(fin,buffer))
	{
		if(buffer[0] != '#' && buffer[0] != '\r' && buffer[0] != '\0' && buffer[0] != '\n' && buffer.length() != 0)
    	{
			stringstream value(buffer);
			value>>width;
			value>>fontFile;
			value>>datFile;
		}
	}
	fin.close();
	tempFont = new Text;
	tempFont->width = width;
	tempFont->data = new unsigned char[width*width*4];
	tempFont->charWidth = width/16;
	FILE *input = fopen(fontFile.c_str(),"r");
	if(input)
	{
		for(int i=0;i<width*width;++i)
		{
			tmp = getc(input);
			tempFont->data[i*4] = (unsigned char)255*(unsigned char)r;
			tempFont->data[i*4+1] = (unsigned char)255*(unsigned char)g;
			tempFont->data[i*4+2] = (unsigned char)255*(unsigned char)b;
			tempFont->data[i*4+3] = (unsigned char)(((float)tmp)*a);
		}
	}
	else
	{
		cout<<"Error loading font: "+fontFile<<endl;
		return 0;
	}
	fclose(input);
	// now let's create a SDL surface for the font
	Uint32 rmask,gmask,bmask,amask;
	#if SDL_BYTEORDER == SDL_BIG_ENDIAN
	rmask = 0xff000000;
	gmask = 0x00ff0000;
	bmask = 0x0000ff00;
	amask = 0x000000ff;
	#else
	rmask = 0x000000ff;
	gmask = 0x0000ff00;
	bmask = 0x00ff0000;
	amask = 0xff000000;
	#endif
	tempFont->font = SDL_CreateRGBSurfaceFrom(tempFont->data, width, width, 32, width*4, rmask, gmask, bmask, amask);
	tempFont->font = SDL_DisplayFormatAlpha(tempSurface);
	SDL_FreeSurface(tempSurface);

	//hold widths of the font
	tempFont->widths = new int[256];

	//read info about width of each char
	input = fopen(datFile.c_str(),"r");
	if(fin)
	{
		for(int i=0; i<256;++i)
		{
			tempFont->widths[i]=getc(input);
		}
	}
	fin.close();
	return tempFont;
}

void Text::drawString(SDL_Surface *screen, Text *font, int x, int y, std::string str, ...)
{
	char string[1024];
	va_list ap;                // Pointer To List Of Arguments
	va_start(ap, str.c_str());         // Parses The String For Variables
	vsprintf(string, str.c_str(), ap); // Converts Symbols To Actual Numbers
	va_end(ap);                // Results Are Stored In Text
	int len = strlen(string);
	int xPos=0;
	for(int i=0;i<len;i++)// Loop through all the chars in the string
	{
		draw(screen, font->font , xPos+x, y, font->widths[string[i]]+2, font->charWidth, (string[i]%16*font->charWidth)+((font->charWidth/2)-(font->widths[string[i]])/2), (((int)string[i]/16)*font->charWidth));
		xPos+=font->widths[string[i]];
	}
}

int Text::stringWidth(Text *font,std::string str,...)
{
  char string[1024];         // Temporary string

  va_list ap;                // Pointer To List Of Arguments
  va_start(ap, str.c_str());         // Parses The String For Variables
  vsprintf(string, str.c_str(), ap); // Converts Symbols To Actual Numbers
  va_end(ap);                // Results Are Stored In Text
  int xPos=0;
  int len=strlen(string);
  for(int i=0;i<len;i++)
  {
    // Add their widths together
    xPos+=font->widths[string[i]];
  }
  return xPos;
}

void Text::DeleteFont(Text *font)
{
  delete [] font->widths;
  delete [] font->data;
  SDL_FreeSurface(font->font);
  delete font;
}

void Text::DeleteFont()
{
  delete [] this->widths;
  delete [] this->data;
  SDL_FreeSurface(this->font);
  delete this;
}
