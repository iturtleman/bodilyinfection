using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BodilyInfection.Engine;

namespace BodilyInfection.Levels
{
    internal static class TitleScreen
    {
        static readonly TimeSpan RequiredWaitTime = new TimeSpan(0, 0, 0, 0, 0);
        static TimeSpan LevelInitTime = TimeSpan.MinValue;
        private static bool levelCompleted = false;

        internal static void Load(GameTime gameTime)
        {
            LevelInitTime = TimeSpan.MinValue;
            levelCompleted = false;

            Level l = This.Game.CurrentLevel;
            l.AddAnimation(new BackgroundAnimation("title.anim"));

            l.Background = new Background("title", new Actor(l.GetAnimation("title.anim")));

            /*Text text = new Text("title", "Text", "BodilyInfection Title Screen Placeholder");
            text.Pos = new Vector2((This.Game.GraphicsDevice.Viewport.Width / 2) - (text.GetAnimation().Width / 2), 150);
            text.DisplayColor = Color.Blue;

            Text description = new Text("description", "Text", "Please press Start or Enter to continue.");

            description.Pos = new Vector2((This.Game.GraphicsDevice.Viewport.Width / 2) - (description.GetAnimation().Width / 2),
                text.Pos.Y + text.GetAnimation().Height);
            description.DisplayColor = Color.PowderBlue;

            Text sigh = new Text("!!!", "Text", "Eric, I need your title screen!");
            sigh.Pos = new Vector2((This.Game.GraphicsDevice.Viewport.Width / 2) - (sigh.GetAnimation().Width / 2),
                description.Pos.Y + description.GetAnimation().Height + 5);
            sigh.DisplayColor = Color.Peru;
            
            l.AddSprite(text);*/
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
                if (currentState.Buttons.Start == ButtonState.Pressed )
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

        internal static void Unload(GameTime gameTime)
        {
            This.Game.SetCurrentLevel("World");
        }
    }
}
