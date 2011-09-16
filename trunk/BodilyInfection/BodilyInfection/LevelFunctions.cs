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

        internal static void LevelWorldLoad(GameTime gameTime)
        {
            Level l = This.Game.CurrentLevel;

            //Initialize Collision Cell Size
            Collision.gridCellHeight = 4000;
            Collision.gridCellWidth = 4000;
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

            Sprite sun = new Virus("sun", new Actor(l.GetAnimation("virusPulse.anim")));
            sun.Pos = new Vector2(480, 50);
            sun.AnimationSpeed = 1;

            //Sprite rbc = new Ship("ship", new Actor(l.GetAnimation("ship.anim")));
            Actor shipActor = new Actor(l.GetAnimation("ship.anim"));
            shipActor.Animations.Add(l.GetAnimation("antibody.anim"));
            Sprite ship = new Ship("ship", shipActor);
            
            ship.Pos = new Vector2(50, 50);
            ship.AnimationSpeed = 1;
        }

        internal static void LevelWorldUpdate(GameTime gameTime)
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
