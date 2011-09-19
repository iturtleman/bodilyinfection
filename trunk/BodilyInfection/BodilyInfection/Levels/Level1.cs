using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BodilyInfection.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace BodilyInfection.Levels
{
    internal static class Level1
    {
        #region Timer Variables
        internal static TimeSpan SpawnWaitTime = new TimeSpan(0, 0, 0, 4, 0);
        internal static TimeSpan PreviousSpawn = new TimeSpan(0, 0, 0, 0, 0);
        #endregion Timer Variables

        private static int EnemiesDefeatedWinCondition = 100;   
        
        internal static void Load(GameTime gameTime)
        {
            Level l = This.Game.CurrentLevel;

            /// load background
            l.AddAnimation(new BackgroundAnimation("level1bg.anim"));
            l.Background = new Background("level1bg", new Actor(l.GetAnimation("level1bg.anim")));

            /** load animations */
            l.AddAnimation(new Animation("rbc.anim"));
            l.AddAnimation(new Animation("virusPulse.anim"));
            l.AddAnimation(new Animation("infected.anim"));
            l.AddAnimation(new Animation("antibody.anim"));
            l.AddAnimation(new Animation("shield.anim"));
            l.AddAnimation(new Animation("ship.anim"));
            l.AddAnimation(new Animation("cannon.anim"));
            l.AddAnimation(new Animation("xplosion17.anim"));
            l.AddAnimation(new Animation("BlueExplosion2.anim"));
            l.AddAnimation(new Animation("vulnerable.anim"));
            l.AddAnimation(new Animation("RedExplosion2.anim"));

            /** load music */
            var audioMan = This.Game.AudioManager;
            //bg
            audioMan.AddBackgroundMusic("level1_bg");
            audioMan.PlayBackgroundMusic("level1_bg");
            //ship spawn
            audioMan.AddSoundEffect("ship_spawn");
            //ship explode
            audioMan.AddSoundEffect("ship_explosion");
            //gun
            audioMan.AddSoundEffect("gun1");
            //rbc die
            audioMan.AddSoundEffect("rbc_die");
            //rbc infected
            audioMan.AddSoundEffect("rbc_infect");
            //virus explode
            audioMan.AddSoundEffect("virus_explode");


            /** load sprites */

            // Spawn initial RedBloodCells and Viruses
            LevelFunctions.SpawnEnemies(delegate()
            {
                Actor rbcActor = new Actor(l.GetAnimation("rbc.anim"));
                rbcActor.Animations.Add(l.GetAnimation("infected.anim"));
                rbcActor.Animations.Add(l.GetAnimation("vulnerable.anim"));
                rbcActor.Animations.Add(l.GetAnimation("RedExplosion2.anim"));
                Sprite rbc = new RedBloodCell("rbc", rbcActor);
                return rbc;
            }, 2);
            LevelFunctions.SpawnEnemies(delegate() 
            { 
                Actor virusActor = new Actor(This.Game.CurrentLevel.GetAnimation("virusPulse.anim"));
                virusActor.Animations.Add(l.GetAnimation("BlueExplosion2.anim"));
                return new Virus("virus", virusActor); 
            }, 15);


            // Load ship
            Actor shipActor = new Actor(l.GetAnimation("ship.anim"));
            shipActor.Animations.Add(l.GetAnimation("xplosion17.anim"));
            Sprite ship = new Ship("ship", shipActor);
           
            l.PlayerSpawnPoint = new Vector2(50, 50);
            ship.Pos = l.PlayerSpawnPoint;
            ship.AnimationSpeed = 1;
            Text livesText = new Text("livesText", "Text", "Lives Remaining:");
            livesText.Pos = new Vector2(This.Game.GraphicsDevice.Viewport.X + This.Game.GraphicsDevice.Viewport.Width - livesText.GetAnimation().Width - 50,
                                        This.Game.GraphicsDevice.Viewport.Y);
            livesText.DisplayColor = Color.Red;
            Text lives = new Text("lives", "Text", (ship as Ship).RemainingLives.ToString());
            lives.Pos = new Vector2(This.Game.GraphicsDevice.Viewport.X + This.Game.GraphicsDevice.Viewport.Width - 50,
                                    This.Game.GraphicsDevice.Viewport.Y);
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
                LevelFunctions.SpawnEnemies(delegate() 
                {
                    Actor virusActor = new Actor(This.Game.CurrentLevel.GetAnimation("virusPulse.anim"));
                    virusActor.Animations.Add(This.Game.CurrentLevel.GetAnimation("BlueExplosion2.anim"));
                    return new Virus("virus", virusActor);                 
                }, 5);

                PreviousSpawn = gameTime.TotalGameTime;
            }

            //move the viewport if we need to
            Viewport viewport = This.Game.GraphicsDevice.Viewport;
            Vector2 cameraPos = This.Game.CurrentLevel.Camera.Pos;
            int borderWidth = 100;
            if (player != null)
            {
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
                    cameraPos.Y -= borderWidth -(difference.Y);
                }
                else if (difference.Y > viewport.Y + viewport.Height - borderWidth)
                {
                    cameraPos.Y += borderWidth - (viewport.Height - (difference.Y));
                }
                This.Game.CurrentLevel.Camera.Pos = cameraPos;
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
