#include <stdio.h>
#include <stdlib.h>
#include <string>
#include <SDL/SDL.h>
#include <iostream>
#include <sstream>
#include <vector>
#include "fns.h"
#include "Game.h"

using namespace std;

template<class T> string to_string(const T& t)
{
    ostringstream os;
    os << t;
    return os.str();
}

//-------------------------------------------------------------------//
// Function : main()    - Params : argc, argv
// Main program function
// This function calls the init() function then loops on draw function 
// until an escape condition is reached
int main (int argc, char *argv[])
{
	Game::game()->run();

    return 0;
}

