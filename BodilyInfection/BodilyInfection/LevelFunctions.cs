﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BodilyInfection
{
    internal static class LevelFunctions
    {
        internal static void DoNothing() { }

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

        internal static void LevelWorldLoad()
        {
            Level l = This.Game.CurrentLevel;

            /// load background
            l.Background = new Background("default", "bg.bmp");//mBackground = LoadImage("Backgrounds/bg.bmp");

            /** load animations */
            l.AddAnimation(new Animation("viking.anim"));
            l.AddAnimation(new Animation("sun.anim"));
            l.AddAnimation(new Animation("rbc.anim"));


            /** load sprites */
            Sprite vikings1 = new RedBloodCell("viking1", new Actor(l.GetAnimation("viking.anim")));
            vikings1.Pos = new Vector2(0, 0);
            vikings1.AnimationSpeed= 1;

            Sprite vikings2 = new Sprite("viking2", new Actor(l.GetAnimation("viking.anim")));
            vikings2.Pos = new Vector2(350, 300);
            vikings2.AnimationSpeed = 1.5f;

            Sprite sun = new Virus("sun", new Actor(l.GetAnimation("sun.anim")));
            sun.Pos = new Vector2(480, 50);
            sun.AnimationSpeed = 1;

            Sprite rbc = new Ship("rbc", new Actor(l.GetAnimation("rbc.anim")));
            rbc.Pos = new Vector2(50, 50);
            rbc.AnimationSpeed = 1;


        }

        internal static void LevelWorldUpdate()
        {
            Level LevelWorld = This.Game.CurrentLevel;

            Sprite player = LevelWorld.GetSprite("rbc");

            Background bg = LevelWorld.Background;

            /*if (player != null)
            {
                GamePadState currentState = GamePad.GetState(PlayerIndex.One);
                if (currentState.IsConnected)
                {
                    player.Pos += GetLeftThumbState(PlayerIndex.One);
                }
                else // Move with arrow keys
                {
                    KeyboardState keys = Keyboard.GetState();
                    float shipSpeed = 5;
                    if (keys.IsKeyDown(Keys.Up))
                    {
                        player.Pos.Y -= shipSpeed;
                    }
                    else if (keys.IsKeyDown(Keys.Down))
                    {
                        player.Pos.Y += shipSpeed;
                    }

                    if (keys.IsKeyDown(Keys.Left))
                    {
                        player.Pos.X -= shipSpeed;
                    }
                    else if (keys.IsKeyDown(Keys.Right))
                    {
                        player.Pos.X += shipSpeed;
                    }
                }
            }*/

            //if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0)
            //    bg.X -= 1;
            //if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0)
            //    bg.X += 1;
            //if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < 0)
            //    bg.Y += 1;
            //if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0)
            //    bg.Y -= 1;
        }
    }
}
