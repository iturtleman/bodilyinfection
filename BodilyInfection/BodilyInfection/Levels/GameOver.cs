﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BodilyInfection.Engine;

namespace BodilyInfection.Levels
{
    internal static class GameOver
    {
        static bool levelCompleted = false;

        internal static void Load()
        {
            levelCompleted = false;

            Level l = This.Game.CurrentLevel != This.Game.NextLevel && This.Game.NextLevel != null ? This.Game.NextLevel : This.Game.CurrentLevel;
            l.AddAnimation(new BackgroundAnimation("gameover.anim"));
            l.Background = new Background("gameover", new Actor(l.GetAnimation("gameover.anim")));

            GameData.Score = 0;
            GameData.NumberOfLives = GameData.DefaultNumberOfLives;
        }

        internal static void Update()
        {
            GameTime gameTime = This.gameTime;
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

        internal static void Unload()
        {
            This.Game.SetCurrentLevel("TitleScreen");
        }

        internal static bool CompletionCondition()
        {
            return levelCompleted;
        }
    }
}