using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Collision
{
    public class CollisionHierarchy
    {
        /// <summary>
        /// The Collision that should occur if the currentLevel collision is successful.
        /// </summary>
        public List<CollisionObject> branches;

        /// <summary>
        /// The Collision at this Level.
        /// </summary>
        public CollisionObject node;
    }
}
