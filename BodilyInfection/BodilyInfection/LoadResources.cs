using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodilyInfection
{
    internal partial class Game : Microsoft.Xna.Framework.Game
    {
        void LoadResources(){
            mLevels.Add(new Level("World", LevelFunctions.LevelWorldLoad, LevelFunctions.LevelWorldUpdate));
        }
    }
}
