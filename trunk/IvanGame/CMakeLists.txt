cmake_minimum_required(VERSION 2.6)
project(game)

find_package(SDL REQUIRED)
find_package(SDL_image REQUIRED)

add_executable(game Animation.cpp fns.cpp Game.cpp Level.cpp main.cpp WorldObject.cpp Sprite.cpp Text.cpp Actor.cpp Background.cpp Collision.cpp LoadResources.cpp)

target_link_libraries(game ${SDL_LIBRARY} ${SDLIMAGE_LIBRARY})
