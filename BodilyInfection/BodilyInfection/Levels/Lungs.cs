using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BodilyInfection.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace BodilyInfection.Levels
{
    class Lungs
    {
        #region Timer Variables
        internal static TimeSpan SpawnWaitTime = new TimeSpan(0, 0, 0, 4, 0);
        internal static TimeSpan PreviousSpawn = new TimeSpan(0, 0, 0, 0, 0);
        #endregion Timer Variables

        private static int EnemiesDefeatedWinCondition = 100;

        internal static void Load(Level lastLevel)
        {
            BodilyInfectionLevel l = This.Game.CurrentLevel as BodilyInfectionLevel;

            /// load background
            l.AddAnimation(new BackgroundAnimation("lungs.anim"));
            l.Background = new Background("lungs", new Actor(l.GetAnimation("lungs.anim")));

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

            LevelFunctions.MakeHUD();
        }

        internal static void Update(GameTime gameTime)
        {
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
        }

        internal static bool CompletionCondition()
        {
            return This.Game.CurrentLevel.EnemiesDefeated >= EnemiesDefeatedWinCondition;
        }
    }
}
