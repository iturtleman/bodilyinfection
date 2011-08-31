#include "WorldObject.h"
#include "fns.h"


void WorldObject::drawCollisions(vector<Collision*> &vec, const Point2D& pos){
	for(unsigned int i=0; i < vec.size(); i++){
		vec[i]->draw(pos);
	}
}
