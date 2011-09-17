using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BodilyInfection
{
    internal static class LevelFunctions
    {
        #region Timer Variables

        internal static TimeSpan SpawnWaitTime = new TimeSpan(0, 0, 0, 4, 0);
        internal static TimeSpan NextRespawn = new TimeSpan(0, 0, 0, 0, 0);
        internal static TimeSpan PreviousSpawn = new TimeSpan(0, 0, 0, 0, 0);

        #endregion Timer Variables

        internal static void DoNothing(GameTime gameTime) { }

        /// <summary>
        /// Returns left thumb state for given player
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Vector2 GetLeftThumbState(PlayerIndex p)
        {
            Vector2 state = new Vector2(GamePad.GetState(p).ThumbSticks.Left.X, -GamePad.GetState(p).ThumbSticks.Left.Y);
            return state;
        }


        /// <summary>
        /// Spawns Viruses at random locations on the screen.
        /// </summary>
        /// <param name="gameTime"></param>
        public static void SpawnEnemiesRecurring(GameTime gameTime, Level l, int numEnemies) 
        {



            if (gameTime.TotalGameTime >= SpawnWaitTime + PreviousSpawn ) 
            {
                Random rand = new Random();

                for (int i = 0; i < numEnemies; i++)
                {    
                    Sprite virus2 = new Virus("virus2", new Actor(l.GetAnimation("virusPulse.anim")));
                    virus2.Pos = new Vector2(rand.Next(0, This.Game.GraphicsDevice.Viewport.Width), rand.Next(0, This.Game.GraphicsDevice.Viewport.Height));
                    virus2.AnimationSpeed = 1;
                }

                PreviousSpawn = gameTime.TotalGameTime;
                NextRespawn = gameTime.TotalGameTime + SpawnWaitTime;
            }

            
        }

        /// <summary>
        /// For use in LevelWorldLoad, Spawn's one initial set of enemies
        /// </summary>
        /// <param name="gameTime"></param>
        public static void SpawnInitialEnemies(Level l, int numEnemies)
        {
            Random rand = new Random();
            for (int i = 0; i < numEnemies; i++)
            {
                Sprite virus2 = new Virus("virus2", new Actor(l.GetAnimation("virusPulse.anim")));
                virus2.Pos = new Vector2(rand.Next(0, This.Game.GraphicsDevice.Viewport.Width), rand.Next(0, This.Game.GraphicsDevice.Viewport.Height));
                virus2.AnimationSpeed = 1;
            }
        }


        internal static void LevelWorldLoad(GameTime gameTime)
        {
            Level l = This.Game.CurrentLevel;

            //Initialize Collision Cell Size
            Collision.gridCellHeight = 40;
            Collision.gridCellWidth = 40;
            Collision.createGrid(0, 0, 800, 800);

            /// load background
            l.Background = new Background("default", "bg.bmp");//mBackground = LoadImage("Backgrounds/bg.bmp");

            /** load animations */
            l.AddAnimation(new Animation("viking.anim"));
            l.AddAnimation(new Animation("sun.anim"));
            l.AddAnimation(new Animation("rbc.anim"));
            l.AddAnimation(new Animation("virusPulse.anim"));
            l.AddAnimation(new Animation("antibody.anim"));
            l.AddAnimation(new Animation("shield.anim"));
            l.AddAnimation(new Animation("ship.anim"));

            /** load sprites */


            Sprite vikings1 = new RedBloodCell("rbc", new Actor(l.GetAnimation("rbc.anim")));
            vikings1.Pos = new Vector2(550, 550);
            vikings1.AnimationSpeed= 1;

            Random rand = new Random();

            SpawnInitialEnemies(l, 15);

            //Sprite rbc = new Ship("ship", new Actor(l.GetAnimation("ship.anim")));
            Actor shipActor = new Actor(l.GetAnimation("ship.anim"));
            shipActor.Animations.Add(l.GetAnimation("shield.anim"));
            Sprite ship = new Ship("ship", shipActor);
            
            ship.Pos = new Vector2(50, 50);
            ship.AnimationSpeed = 1;
        }

        internal static void LevelWorldUpdate(GameTime gameTime)
        {
            Level LevelWorld = This.Game.CurrentLevel;

            Sprite player = LevelWorld.GetSprite("ship");

            Background bg = LevelWorld.Background;

            SpawnEnemiesRecurring(gameTime, LevelWorld, 15);
        }
    }
}
