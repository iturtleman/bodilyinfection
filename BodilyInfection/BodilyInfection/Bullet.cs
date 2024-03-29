﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace BodilyInfection
{
    class Bullet : Sprite
    {
        public Bullet(string name, Actor actor)
            : this(name, actor, new Vector2(3.0f, 3.0f))
        {
            Sprite ship = This.Game.CurrentLevel.GetSprite("ship");
            Pos = ship.Pos;
        }

        public Bullet(string name, Actor actor, Vector2 velocity)
            : base(name, actor)
        {
            movementVelocity = velocity;
            UpdateBehavior =Update;
        }

        private Vector2 movementVelocity;
        Stopwatch timer = new Stopwatch();

        public void Update()
        {
            // Fire in straight line from position of ship when bullet was created. (scaled by velocity)
            Pos.X += movementVelocity.X;
            Pos.Y += movementVelocity.Y;

            int buffer = 250;
            Vector2 mod = Pos - This.Game.CurrentLevel.Camera.Pos;
            if (mod.X > This.Game.GraphicsDevice.Viewport.Width + buffer ||
                mod.Y > This.Game.GraphicsDevice.Viewport.Height + buffer ||
                mod.X < -buffer ||
                mod.Y < -buffer)
            {
                This.Game.CurrentLevel.RemoveSprite(this);
            }


            if (Collision.collisionData.Count > 0)
            {
                foreach (CollisionObject co in this.GetCollision())
                {
                    if (Collision.collisionData.ContainsKey(this))
                    {
                        foreach (Tuple<CollisionObject, WorldObject, CollisionObject> collision in Collision.collisionData[this])
                        {
                            if (collision.Item2.GetType() == typeof(Virus) || collision.Item2.GetType() == typeof(RedBloodCell) || collision.Item2.GetType() == typeof(Background_Collision))
                            {
                                //Remove Bullet
                                This.Game.CurrentLevel.RemoveSprite(this);

                                //Notify that RBC was hit via vibration.
                                //GamePad.SetVibration(PlayerIndex.One, 1.0f, 1.0f);

                                //timer.Start(); 
                                //while ((timer.ElapsedMilliseconds / 1000) > 1000000) { }

                                //    GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                                //    timer.Reset();
                            }
                        }
                    }
                }
            }
        }   
    }
}
