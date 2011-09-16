﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BodilyInfection
{
    class RedBloodCell : Sprite
    {
        public RedBloodCell(string name, Actor actor)
            : this(name, actor, new Vector2(3.0f, 3.0f))
        {
            
        }

        public RedBloodCell(string name, Actor actor, Vector2 velocity)
            : base(name, actor)
        {
            movementVelocity = velocity;
            UpdateBehavior += new Behavior(Update);
        }



        private Vector2 movementVelocity;// { get; set; }
        public bool Wounded { get; set; }
        private int health = 10;

        public void Update(GameTime gameTime)
        {

            // Move the sprite by speed.
            Pos.X += movementVelocity.X;

            Pos.Y += movementVelocity.Y;
            
            int MaxX =
                This.Game.GraphicsDevice.Viewport.Width;
            int MinX = 0;
            int MaxY =
                This.Game.GraphicsDevice.Viewport.Height;
            int MinY = 0;

            // Check for bounce.
            if (Pos.X > MaxX)
            {
                movementVelocity.X *= -1;
                Pos.X = MaxX;
            }

            else if (Pos.X < MinX)
            {
                movementVelocity.X *= -1;
                Pos.X = MinX;
            }

            if (Pos.Y > MaxY)
            {
                movementVelocity.Y *= -1;
                Pos.Y = MaxY;
            }

            else if (Pos.Y < MinY)
            {
                movementVelocity.Y *= -1;
                Pos.Y = MinY;
            }

            if (Collision.collisionData.Count > 0)
            {
                foreach (CollisionObject co in this.GetCollision())
                {
                    if (Collision.collisionData.ContainsKey(this))
                    {
                        foreach (Tuple<CollisionObject, WorldObject, CollisionObject> collision in Collision.collisionData[this])
                        {
                            if (collision.Item2.GetType() == typeof(Virus))
                            {
                                if (Wounded)
                                {
                                    // Virus enters RBC
                                    throw new NotImplementedException();
                                    //mActor.CurrentAnimation = 1;
                                    //mActor.Frame = 0;
                                }
                            }
                            else if (collision.Item2.GetType() == typeof(Bullet))
                            {
                                // RBC becomes infectable.
                                //throw new NotImplementedException();
                            }
                        }
                    }
                }
            }
        }
    }
}