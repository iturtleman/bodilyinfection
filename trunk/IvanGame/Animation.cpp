#include <string>
#include <stdio.h>
#include <stdlib.h>
#include <SDL/SDL.h>
#include <SDL/SDL_image.h>
#include <vector>
#include <fstream>
#include <sstream>
#include <iostream>
#include "Animation.h"
#include "fns.h"
#include <GL/gl.h>
using namespace std;

Animation::Animation(std::string animFile):mName(animFile)
{
	loadAnimation(animFile);
}

Animation::Animation(std::string animFile, string name):mName(name)
{
	loadAnimation(animFile);
}

int Animation::loadAnimation(std::string animFile)
{
	//variables
	string buffer,file;
	int transparency[3];
	int pause;
	animFile="ArtAssets/Sprites/"+animFile;
	//file
	ifstream fin(animFile.c_str());

	if(!fin)
	{
		printf("Error opening %s\n\n",animFile.c_str());
		return -1;
	}

	int count=0;
	while(getline(fin,buffer))
	{
		if(buffer[0] != '#' && buffer[0] != '\r' && buffer[0] != '\0' && buffer[0] != '\n' && buffer.length() != 0)
    	{
			stringstream value(buffer);
			string temp;
			value>>temp;
			if(temp=="NumFrames:")
			{
				value>>mNumFrames;
				mFrames = new SpriteFrame[mNumFrames];
				mBuilt = 1;
			}
			else
			{
				/** File name */
				file=temp;
				/** Pause time */
				value>>pause;
				/** transparency #'s R G B */
				for(int i=0; i<3;++i)
				{
					int num;
					value>>num;
					if(num >= 0 && num<=255)
						transparency[i] = num;///< \todo check to see if this is being stupid.
					else
					{
						transparency[i] = 0;
						cout<<"Invalid value!"<<endl;
					}
				}
				/** AnimationPeg */
				double x=0.0, y=0.0;
				value>>x;
				value>>y;

				
				/** Collision data */
				char c;
				value>>c;
				if(c == '('){
					value>>c;
					while(c != ')'){
						if(c == 'n'){
							value>>c;
							continue;
						}
						else if(c == 'c')
						{
							double xOffset = 0.0, yOffset = 0.0;
							value>>xOffset;
							value>>yOffset;
							double radius;
							value>>radius;
							mFrames[count].collisionData.push_back(new CollisionCircle(Point2D(xOffset, yOffset), radius));
						}
						else if(c == 'r')
						{
							double xOffset = 0.0, yOffset = 0.0;
							value>>xOffset;
							value>>yOffset;
							double width = 0.0 , height = 0.0;
							value>>width;
							value>>height;
							mFrames[count].collisionData.push_back(new CollisionRectangle(Point2D(xOffset, yOffset), width, height));
						}
						value>>c;
					}
				}

				/** Load image for Frame */
				SDL_Surface *temp=NULL;
				if((temp = LoadImage("Sprites/"+file)) == NULL) return -1;//Load image for Frame
	        	
				/** Set transparent color */
				SDL_SetColorKey(temp, SDL_SRCCOLORKEY, SDL_MapRGB(temp->format, transparency[0], transparency[1], transparency[2]));
				
				/** Loads image into animation */
				SDL_Surface* surface = SDL_DisplayFormatAlpha(temp);
				SDL_FreeSurface(temp);
				mFrames[count].width=surface->w;
				mFrames[count].height=surface->h;

				// Check that the image’s width is a power of 2
				if ( (surface->w & (surface->w - 1)) != 0 ) {
					cout<<"warning: image.bmp’s width is not a power of 2"<<endl;
				}

				// Also check if the height is a power of 2
				if ( (surface->h & (surface->h - 1)) != 0 ) {
					cout<<"warning: image.bmp’s height is not a power of 2"<<endl;
				}

				//get number of channels in the SDL surface
				GLint nofcolors=surface->format->BytesPerPixel;

				GLenum texture_format=NULL;

				//contains an alpha channel
				if(surface->format->Rmask==0x000000ff)
					texture_format=GL_RGBA;
				else
					texture_format=GL_BGRA;
				if(!(nofcolors==4 || nofcolors==3))
				{
					cout<<"warning: the image is not truecolor...this will break "<<endl;
				}

				// allocate a texture name
				glGenTextures( 1, &mFrames[count].image );
				
				// select our current texture
				glBindTexture( GL_TEXTURE_2D, mFrames[count].image );
				
				// select modulate to mix texture with color for shading
				glTexEnvf( GL_TEXTURE_ENV, GL_TEXTURE_ENV_MODE, GL_MODULATE );

				// when texture area is small, bilinear filter the closest mipmap
				glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_LINEAR);
				// when texture area is large, bilinear filter the original
				glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR );

				// the texture wraps over at the edges (repeat)
				glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP );
				glTexParameterf( GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP );

				glTexImage2D( GL_TEXTURE_2D, 0, nofcolors, surface->w, surface->h, 0,
					texture_format, GL_UNSIGNED_BYTE, surface->pixels );

				/** sets frame delay and makes sure height and width are correct */
				mFrames[count].pause = pause;
				
				/** Set the animation Peg*/
				mFrames[count].animationPeg = Point2D(x + double(mFrames[count].width)/2, y + double(mFrames[count].height)/2);

				count++;
			}
    	}
  	}
  fin.close();
  return 0;
}
