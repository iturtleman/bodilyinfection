using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodilyInfection
{
    internal partial class Game : Microsoft.Xna.Framework.Game
    {
        void LoadResources(){
            mLevels.Add(new Level("TitleScreen", Levels.TitleScreen.Load, Levels.TitleScreen.Update, Levels.TitleScreen.Unload, Levels.TitleScreen.CompletionCondition));
            mLevels.Add(new Level("GameOver", Levels.GameOver.Load, Levels.GameOver.Update, Levels.GameOver.Unload, Levels.GameOver.CompletionCondition));
            mLevels.Add(new Level("World", Levels.Level1.Load, Levels.Level1.Update, Levels.Level1.Unload, Levels.Level1.CompletionCondition));
        }
    }
}
