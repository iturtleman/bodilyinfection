using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BodilyInfection.Engine;

namespace BodilyInfection.Levels
{
    internal static class Level1
    {
        #region Timer Variables
        internal static TimeSpan SpawnWaitTime = new TimeSpan(0, 0, 0, 4, 0);
        internal static TimeSpan PreviousSpawn = new TimeSpan(0, 0, 0, 0, 0);
        #endregion Timer Variables

        private static int EnemiesDefeatedWinCondition = 30;

        internal static void Load(GameTime gameTime)
        {
            Level l = This.Game.CurrentLevel;

            /// load background
            l.Background = new Background("default", "bg.bmp");

            /** load animations */
            l.AddAnimation(new Animation("rbc.anim"));
            l.AddAnimation(new Animation("virusPulse.anim"));
            l.AddAnimation(new Animation("infectedrbc.anim"));
            l.AddAnimation(new Animation("antibody.anim"));
            l.AddAnimation(new Animation("shield.anim"));
            l.AddAnimation(new Animation("ship.anim"));

            /** load sprites */

            // Spawn initial RedBloodCells and Viruses
            LevelFunctions.SpawnEnemies(delegate()
            {
                Actor rbcActor = new Actor(l.GetAnimation("rbc.anim"));
                rbcActor.Animations.Add(l.GetAnimation("infectedrbc.anim"));
                Sprite rbc = new RedBloodCell("rbc", rbcActor);
                return rbc;
            }, 2);
            LevelFunctions.SpawnEnemies(delegate() { return new Virus("virus", new Actor(This.Game.CurrentLevel.GetAnimation("virusPulse.anim"))); }, 15);

            // Load ship
            Actor shipActor = new Actor(l.GetAnimation("ship.anim"));
            shipActor.Animations.Add(l.GetAnimation("shield.anim"));
            Sprite ship = new Ship("ship", shipActor);

            ship.Pos = new Vector2(50, 50);
            ship.AnimationSpeed = 1;

            Text livesText = new Text("livesText", "Text", "Lives Remaining:");
            livesText.Pos = new Vector2(This.Game.GraphicsDevice.Viewport.Width - livesText.GetAnimation().Width - 50, 0);
            livesText.DisplayColor = Color.Red;
            Text lives = new Text("lives", "Text", (ship as Ship).RemainingLives.ToString());
            lives.Pos = new Vector2(This.Game.GraphicsDevice.Viewport.Width - 50, 0);
            lives.DisplayColor = Color.Red;
        }

        internal static void Update(GameTime gameTime)
        {
            Level LevelWorld = This.Game.CurrentLevel;

            Sprite player = LevelWorld.GetSprite("ship");
            Sprite lives = LevelWorld.GetSprite("lives");
            if (lives != null && player != null)
            {
                (lives as Text).Content = (player as Ship).RemainingLives.ToString();
            }

            Background bg = LevelWorld.Background;
            if (gameTime.TotalGameTime >= SpawnWaitTime + PreviousSpawn)
            {
                LevelFunctions.SpawnEnemies(delegate() { return new Virus("virus2", new Actor(This.Game.CurrentLevel.GetAnimation("virusPulse.anim"))); }, 5);
                PreviousSpawn = gameTime.TotalGameTime;
            }
        }

        internal static void Unload(GameTime gameTime)
        {
            This.Game.CurrentLevel.mSprites.Clear();
            This.Game.SetCurrentLevel("TitleScreen");
        }

        internal static bool CompletionCondition()
        {
            return This.Game.CurrentLevel.EnemiesDefeated > EnemiesDefeatedWinCondition;
        }
    }
}
