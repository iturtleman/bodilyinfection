﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BodilyInfection.Engine;
namespace BodilyInfection.Levels
{
    class StageClear
    {
        static readonly TimeSpan RequiredWaitTime = new TimeSpan(0, 0, 0, 0, 0);
        static TimeSpan LevelInitTime = TimeSpan.MinValue;
        private static bool levelCompleted = false;
        private static string nextLevel = null;

        internal static void Load(Level lastLevel)
        {
            for (int x = 0; x < LevelFunctions.LevelProgression.Count - 1; x++)
            {
                if (lastLevel.Name == LevelFunctions.LevelProgression[x])
                {
                    nextLevel = LevelFunctions.LevelProgression[x + 1];
                }
            }
            // 
            if (nextLevel == null)
            {
                if (lastLevel.Name == LevelFunctions.LevelProgression[LevelFunctions.LevelProgression.Count - 1])
                {
                    // Go to "game completed" screen
                    nextLevel = "TitleScreen";
                }
                else
                {
                    nextLevel = "TitleScreen";
                }
            }
            LevelInitTime = TimeSpan.MinValue;
            levelCompleted = false;

            Level l = This.Game.CurrentLevel;
            l.AddAnimation(new BackgroundAnimation("stageclear.anim"));

            l.Background = new Background("stageclear", new Actor(l.GetAnimation("stageclear.anim")));

            /** load music */
            //This.Game.AudioManager.AddBackgroundMusic("title");
            //This.Game.AudioManager.PlayBackgroundMusic("title");
        }

        internal static void Update(GameTime gameTime)
        {
            if (LevelInitTime == TimeSpan.MinValue)
            {
                LevelInitTime = gameTime.TotalGameTime;
            }
            Level l = This.Game.CurrentLevel;

            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            if (currentState.IsConnected)
            {
                if (currentState.Buttons.Start == ButtonState.Pressed)
                {
                    // Go to next
                    // Make awesome sound
                    levelCompleted = true;
                }
            }
            else /* Move with arrow keys */
            {
                KeyboardState keys = Keyboard.GetState();
                if (keys.IsKeyDown(Keys.Enter))
                {
                    // Go to next
                    // Make awesome sound
                    levelCompleted = true;
                }
            }
        }

        internal static bool CompletionCondition()
        {
            return levelCompleted;
        }

        internal static void Unload()
        {
            This.Game.SetCurrentLevel(nextLevel);
            nextLevel = null;
        }
    }
}
