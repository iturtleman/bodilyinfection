using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace BodilyInfection
{
    /// <summary>
    /// Do anything required for Game-specific code here
    /// to avoid cluttering up the Engine
    /// </summary>
    class BodilyInfection : Game
    {
        public int Score { get; set; }
        public int NumberOfLives { get; set; }
        public readonly int DefaultNumberOfLives = 4;

        public BodilyInfection()
            : base()
        {
            Score = 0;
            NumberOfLives = DefaultNumberOfLives;
        }
    }

    /// <summary>
    /// Add Game-specific level code here to avoid cluttering up the Engine
    /// </summary>
    class BodilyInfectionLevel : Level
    {
        public Vector2 PlayerSpawnPoint = new Vector2(50, 50);

        public BodilyInfectionLevel(string n, LoadBehavior loadBehavior, UpdateBehavior updateBehavior, UnloadBehavior endBehavior, Condition winCondition)
            : base(n, loadBehavior, updateBehavior, endBehavior, winCondition)
        {
        }

        internal override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //move the viewport if we need to
            Sprite player = GetSprite("ship");
            if (player != null)
            {
                Viewport viewport = This.Game.GraphicsDevice.Viewport;
                Vector2 cameraPos = This.Game.CurrentLevel.Camera.Pos;
                int borderWidth = 300;

                Vector2 difference = player.Pos - cameraPos;
                if (difference.X < viewport.X + borderWidth)
                {
                    cameraPos.X -= borderWidth - (difference.X);
                }
                else if (difference.X > viewport.X + viewport.Width - borderWidth)
                {
                    cameraPos.X += borderWidth - (viewport.Width - (difference.X));
                }
                if (difference.Y < viewport.Y + borderWidth)
                {
                    cameraPos.Y -= borderWidth - (difference.Y);
                }
                else if (difference.Y > viewport.Y + viewport.Height - borderWidth)
                {
                    cameraPos.Y += borderWidth - (viewport.Height - (difference.Y));
                }
                This.Game.CurrentLevel.Camera.Pos = cameraPos;
            }
        }
    }
}
