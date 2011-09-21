using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BodilyInfection.Engine;

namespace BodilyInfection
{
    internal static class LevelFunctions
    {
        public delegate Sprite EnemyFactory();

        private static readonly Random rand = new Random();

        internal static void DoNothing(GameTime gameTime) { }

        /// <summary>
        /// A list of levels in the order they should be played through
        /// </summary>
        internal static List<string> LevelProgression = new List<string>()
        {
            "Stomach",
            "Lungs",
        };

        internal static void MakeHUD()
        {
            Text scoreText = new Text("scoreText", "Text", "Score:");
            scoreText.Pos = new Vector2(50, This.Game.GraphicsDevice.Viewport.Y);
            scoreText.DisplayColor = Color.AliceBlue;
            scoreText.Static = true;
            Text score = new Text("score", "Text", (This.Game as BodilyInfection).Score.ToString());
            score.Pos = new Vector2(scoreText.GetAnimation().Width + 50, This.Game.GraphicsDevice.Viewport.Y);
            score.DisplayColor = Color.AliceBlue;
            score.Static = true;
            score.UpdateBehavior = delegate(GameTime gameTime)
            {
                score.Content = (This.Game as BodilyInfection).Score.ToString();
            };

            Text livesText = new Text("livesText", "Text", "Lives Remaining:");
            livesText.Pos = new Vector2(This.Game.GraphicsDevice.Viewport.X + This.Game.GraphicsDevice.Viewport.Width - livesText.GetAnimation().Width - 50,
                                        This.Game.GraphicsDevice.Viewport.Y);
            livesText.DisplayColor = Color.Red;
            livesText.Static = true;
            Text lives = new Text("lives", "Text", (This.Game as BodilyInfection).NumberOfLives.ToString());
            lives.Pos = new Vector2(This.Game.GraphicsDevice.Viewport.X + This.Game.GraphicsDevice.Viewport.Width - 50,
                                    This.Game.GraphicsDevice.Viewport.Y);
            lives.DisplayColor = Color.Red;
            lives.Static = true;
            lives.UpdateBehavior = delegate(GameTime gameTime)
            {
                lives.Content = (This.Game as BodilyInfection).NumberOfLives.ToString();
            };
        }

        /// <summary>
        /// A behavior that sends the player to the Stage Clear screen once the level is over.
        /// </summary>
        internal static void ToStageClear()
        {
            This.Game.CurrentLevel.mSprites.Clear();
            This.Game.SetCurrentLevel("stageclear");
        }

        /// <summary>
        /// Forces the level to end and continue to the GameOver screen.
        /// Since it overwrites the "win" condition and unloading behavior
        /// for the current level, it must reset it back to normal after the level ends.
        /// </summary>
        /// <param name="gameTime"></param>
        internal static void GoToGameOver(GameTime gameTime)
        {
            Condition oldWin = This.Game.CurrentLevel.WinCondition;
            UnloadBehavior oldEnd = This.Game.CurrentLevel.EndBehavior;
            This.Game.CurrentLevel.WinCondition = delegate { return true; };
            This.Game.CurrentLevel.EndBehavior = delegate
            {
                // Replace the old win condition
                This.Game.CurrentLevel.WinCondition = oldWin;
                This.Game.CurrentLevel.EndBehavior = oldEnd;
                This.Game.SetCurrentLevel("GameOver");
            };
        }

        /// <summary>
        /// Spawns Enemies created by the EnemyFactory at random locations on the screen
        /// </summary>
        /// <param name="gameTime"></param>
        public static void SpawnEnemies(EnemyFactory constructEnemy, int numEnemies)
        {
            for (int i = 0; i < numEnemies; i++)
            {             
                Sprite virus = constructEnemy();
                virus.Pos = This.Game.CurrentLevel.Camera.Pos + 
                    new Vector2(rand.Next(0, This.Game.GraphicsDevice.Viewport.Width),
                        rand.Next(0, This.Game.GraphicsDevice.Viewport.Height));
            }
        }

        /// <summary>
        /// Spawns Enemies created by the EnemyFactory centered within a radius 
        /// proportional to the number of enemies around a specified position
        /// </summary>
        /// <param name="constructEnemy"></param>
        /// <param name="numEnemies"></param>
        /// <param name="position"></param>
        public static void SpawnEnemies(EnemyFactory constructEnemy, int numEnemies, Vector2 position)
        {
            for (int i = 0; i < numEnemies; i++)
            {
                Sprite virus = constructEnemy();
                double radius = Math.Max(virus.GetAnimation().Height, virus.GetAnimation().Width) * numEnemies / 4.0;
                double x = rand.NextDouble() * radius * 2 - radius;
                int mod = (rand.Next(0, 2) - 1);
                double y = Math.Sqrt(radius * radius - x * x) * mod;
                virus.Pos = position + new Vector2((float)x, (float)y);
            }
        }
    }
}
