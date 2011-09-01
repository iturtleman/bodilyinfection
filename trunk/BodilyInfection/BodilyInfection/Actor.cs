using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodilyInfection
{
    class Actor
    {
        #region Properties
        int frame { get; set; }
        int currentAnimation { get; set; }
        #endregion

        #region Variables
        List<Animation> animations = new List<Animation>();
        #endregion

        #region Constructor
        Actor(Animation anim)
        {
            animations.Add(anim);
        }
        #endregion
    }
}
