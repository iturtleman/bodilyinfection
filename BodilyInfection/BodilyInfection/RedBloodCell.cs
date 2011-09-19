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
            CollisionBehavior += new Behavior(ActOnCollisions);
            Wounded = false;
            Infected = false;
            Dead = false;
        }

        private Vector2 movementVelocity;
        public bool Wounded { get; set; }
        public bool Infected { get; set; }
        public bool Dead { get; set; }

        private int health = 20;
        public TimeSpan timeToExplode = new TimeSpan(0, 0, 5);  // on infection, it takes 5 seconds for virus to come out.
        public TimeSpan timeOfDeath;
        public TimeSpan explosionLength = new TimeSpan(0, 0, 0, 0, 500);
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
                    This.Game.AudioManager.PlaySoundEffect("rbc_infect");
                }
                if (gameTime.TotalGameTime > timeOfInfection + timeToExplode)
                {
                    LevelFunctions.SpawnEnemies(delegate() 
                    {
                        Actor virusActor = new Actor(This.Game.CurrentLevel.GetAnimation("virusPulse.anim"));
                        virusActor.Animations.Add(This.Game.CurrentLevel.GetAnimation("BlueExplosion2.anim"));
                        return new Virus("virus", virusActor);
                    }, 5, Pos);
                    This.Game.AudioManager.PlaySoundEffect("rbc_die");
                    This.Game.CurrentLevel.RemoveSprite(this);
                }
            }

            #region explosion/deletion code
            if (Dead && (gameTime.TotalGameTime >= explosionLength + timeOfDeath))
            {
                    //This.Game.CurrentLevel.EnemiesDefeated++;
                    This.Game.CurrentLevel.RemoveSprite(this);
            }
            #endregion explosion/deletion code
        }

        public void ActOnCollisions(GameTime gameTime)
        {
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
                                if (Wounded && !Infected && !((Virus)collision.Item2).Harmless)
                                {
                                    Infected = true;
                                    SetAnimation(1);
                                    StartAnim();
                                }
                            }
                            else if (collision.Item2.GetType() == typeof(Bullet))
                            {
                                if (!Wounded)
                                {
                                    // RBC becomes vulnerable.
                                    Wounded = true;
                                    SetAnimation(2);
                                    StartAnim();
                                }

                                else
                                {
                                    health--;
                                    if (health <= 0)
                                    {
                                        Dead = true;

                                        timeOfDeath = gameTime.TotalGameTime;

                                        // change the animation if the rbc is dead
                                        SetAnimation(3);
                                        StartAnim();
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
