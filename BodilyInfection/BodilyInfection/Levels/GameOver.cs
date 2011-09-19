using System;
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

        internal static void Load(GameTime gameTime)
        {
            levelCompleted = false;
            
            Level l = This.Game.CurrentLevel;
            l.AddAnimation(new BackgroundAnimation("gameover.anim"));
            l.Background = new Background("gameover", new Actor(l.GetAnimation("gameover.anim")));

            /*Text text = new Text("title", "Text", "GAME OVER");
            text.Pos = new Vector2((This.Game.GraphicsDevice.Viewport.Width / 2) - (text.GetAnimation().Width / 2), 150);
            text.DisplayColor = Color.White;

            Text next = new Text("next", "Text", "Press Start or Enter to go back to the title screen.");
            next.Pos = new Vector2((This.Game.GraphicsDevice.Viewport.Width / 2) - (next.GetAnimation().Width / 2),
                text.Pos.Y + text.GetAnimation().Height);
            next.DisplayColor = Color.White;*/
        }

        internal static void Update(GameTime gameTime)
        {
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

        internal static void Unload(GameTime gameTime)
        {
            This.Game.SetCurrentLevel("TitleScreen");
        }

        internal static bool CompletionCondition()
        {
            return levelCompleted;
        }
    }
}
