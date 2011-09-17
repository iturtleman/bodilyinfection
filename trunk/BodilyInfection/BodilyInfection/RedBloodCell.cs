using System;
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
            Wounded = false;
            Infected = false;
        }

        private Vector2 movementVelocity;
        public bool Wounded { get; set; }
        public bool Infected { get; set; }
        private int health = 20;
        public TimeSpan timeToExplode = new TimeSpan(0, 0, 5);  // on infection, it takes 5 seconds for virus to come out.
        TimeSpan timeOfInfection;                               

        public void Update(GameTime gameTime)
        {
            if (!Infected)
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
            }
            else // this is the infect/multiply code
            {
                if (timeOfInfection == TimeSpan.Zero)
                {
                    timeOfInfection = gameTime.TotalGameTime;
                }
                if (gameTime.TotalGameTime > timeOfInfection + timeToExplode)
                {
                    LevelFunctions.SpawnEnemies(delegate() { return new Virus("virus2", new Actor(This.Game.CurrentLevel.GetAnimation("virusPulse.anim"))); }, 5, Pos);
                    This.Game.CurrentLevel.RemoveSprite(this);
                }
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
                                if (Wounded && !((Virus)collision.Item2).Harmless)
                                {
                                    This.Game.CurrentLevel.RemoveSprite(collision.Item2 as Sprite);
                                    Infected = true;
                                }
                            }
                            else if (collision.Item2.GetType() == typeof(Bullet))
                            {
                                if (!Wounded)
                                {
                                    // RBC becomes infectable.
                                    Wounded = true;
                                    mActor.CurrentAnimation = 1;
                                    mActor.Frame = 0;
                                }

                                else
                                {
                                    health--;
                                    if (health <= 0)
                                    {
                                        This.Game.CurrentLevel.RemoveSprite(this);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
