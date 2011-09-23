using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodilyInfection
{
    internal partial class Game : Microsoft.Xna.Framework.Game
    {


        void LoadResources(){
            mLevels.Add(new BodilyInfectionLevel("stageclear", Levels.StageClear.Load, Levels.StageClear.Update, Levels.StageClear.Unload, Levels.StageClear.CompletionCondition));
            mLevels.Add(new BodilyInfectionLevel("TitleScreen", Levels.TitleScreen.Load, Levels.TitleScreen.Update, Levels.TitleScreen.Unload, Levels.TitleScreen.CompletionCondition));
            mLevels.Add(new BodilyInfectionLevel("GameOver", Levels.GameOver.Load, Levels.GameOver.Update, Levels.GameOver.Unload, Levels.GameOver.CompletionCondition));
            mLevels.Add(new BodilyInfectionLevel("Stomach", Levels.Stomach.Load, Levels.Stomach.Update, LevelFunctions.ToStageClear, Levels.Stomach.CompletionCondition));
            mLevels.Add(new BodilyInfectionLevel("Lungs", Levels.Lungs.Load, Levels.Lungs.Update, LevelFunctions.ToStageClear, Levels.Lungs.CompletionCondition));
            mLevels.Add(new BodilyInfectionLevel("Credits", Levels.Credits.Load, Levels.Credits.Update, Levels.Credits.Unload, Levels.Credits.CompletionCondition));
        }
    }
}
