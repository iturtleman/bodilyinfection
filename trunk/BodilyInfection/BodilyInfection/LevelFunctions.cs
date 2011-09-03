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
        internal static void LevelWorldLoad()
        {
            Level l = This.Game.CurrentLevel;

            /// load background
            l.Background = new Background("default", "bg.bmp");//mBackground = LoadImage("Backgrounds/bg.bmp");

            /** load animations */
            l.AddAnimation(new Animation("viking.anim"));
            l.AddAnimation(new Animation("sun.anim"));

            /** load sprites */
            Sprite vikings1 = new Sprite("viking1", new Actor(l.GetAnimation("viking.anim")));
            vikings1.Pos = new Vector2(0, 0);
            vikings1.Speed= 1;
            l.AddSprite(vikings1);

            Sprite vikings2 = new Sprite("viking2", new Actor(l.GetAnimation("viking.anim")));
            vikings2.Pos = new Vector2(350, 300);
            vikings2.Speed = 1.5f;
            l.AddSprite(vikings2);

            Sprite sun = new Sprite("sun", new Actor(l.GetAnimation("sun.anim")));
            sun.Pos = new Vector2(480, 50);
            sun.Speed = 1;
            l.AddSprite(sun);


        }

        internal static void LevelWorldUpdate()
        {
            Level LevelWorld = This.Game.CurrentLevel;

            Sprite player = LevelWorld.GetSprite("viking1");

            Background bg = LevelWorld.Background;

            if (player != null)
            {
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                    player.Pos.X -= 1;
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                    player.Pos.X += 1;
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                    player.Pos.Y += 1;
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                    player.Pos.Y -= 1;
            }

            //if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0)
            //    bg.Pos.X -= 1;
            //if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0)
            //    bg.Pos.X += 1;
            //if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < 0)
            //    bg.Pos.Y += 1;
            //if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0)
            //    bg.Pos.Y -= 1;
        }
    }
}
