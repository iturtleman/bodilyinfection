using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodilyInfection
{
    internal partial class Game : Microsoft.Xna.Framework.Game
    {
        void LoadResources(){
            mLevels.Add(new Level("stageclear", Levels.StageClear.Load, Levels.StageClear.Update, Levels.StageClear.Unload, Levels.StageClear.CompletionCondition));
            mLevels.Add(new Level("TitleScreen", Levels.TitleScreen.Load, Levels.TitleScreen.Update, Levels.TitleScreen.Unload, Levels.TitleScreen.CompletionCondition));
            mLevels.Add(new Level("GameOver", Levels.GameOver.Load, Levels.GameOver.Update, Levels.GameOver.Unload, Levels.GameOver.CompletionCondition));
            mLevels.Add(new Level("World", Levels.Level1.Load, Levels.Level1.Update, Levels.Level1.Unload, Levels.Level1.CompletionCondition));
            mLevels.Add(new Level("level2", Levels.Level2.Load, Levels.Level2.Update, Levels.Level2.Unload, Levels.Level2.CompletionCondition));
        }
    }
}
