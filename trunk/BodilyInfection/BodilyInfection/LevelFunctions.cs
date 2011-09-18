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
        public delegate Sprite EnemyFactory();
        internal static readonly Random rand = new Random();

        internal static void DoNothing(GameTime gameTime) { }

        internal static void ToGameOver(GameTime gameTime)
        {
            Condition oldWin = This.Game.CurrentLevel.WinCondition;
            Behavior oldEnd = This.Game.CurrentLevel.EndBehavior;
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
                virus.Pos = new Vector2(rand.Next(0, This.Game.GraphicsDevice.Viewport.Width), rand.Next(0, This.Game.GraphicsDevice.Viewport.Height));
                virus.AnimationSpeed = 1;
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
                virus.AnimationSpeed = 1;
            }
        }
    }
}
