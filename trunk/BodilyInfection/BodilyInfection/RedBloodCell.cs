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
            MovementDir = velocity;
            UpdateBehavior = Update;
            CollisionBehavior = ActOnCollisions;
            Wounded = false;
            Infected = false;
            Dead = false;
        }

        private Vector2 MovementDir
        {
            get
            {
                return movementDir;
            }
            set
            {
                movementDir = value;
                movementDir.Normalize();
            }
        }
        public bool Wounded { get; set; }
        public bool Infected { get; set; }
        public bool Dead { get; set; }
        public bool Reflected { get; set; }


        public Vector2 movementDir;
        private int health = 20;
        public TimeSpan timeToExplode = new TimeSpan(0, 0, 5);  // on infection, it takes 5 seconds for virus to come out.
        public TimeSpan timeOfDeath;
        public TimeSpan explosionLength = new TimeSpan(0, 0, 0, 0, 500);
        TimeSpan timeOfInfection;

        public void Update()
        {
            GameTime gameTime = This.gameTime;
            if (!Infected)
            {
                // Move the sprite by speed.
                Pos += MovementDir * Speed;
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
                    LevelFunctions.Spawn(delegate()
                    {
                        Actor virusActor = new Actor(This.Game.CurrentLevel.GetAnimation("virusPulse.anim"));
                        virusActor.Animations.Add(This.Game.CurrentLevel.GetAnimation("BlueExplosion2.anim"));
                        return new Virus("virus", virusActor);
                    }, 20, Pos);
                    This.Game.AudioManager.PlaySoundEffect("rbc_die");
                    This.Game.CurrentLevel.RemoveSprite(this);
                }
            }

            #region explosion/deletion code
            if (Dead && (gameTime.TotalGameTime >= explosionLength + timeOfDeath))
            {
                //This.Game.CurrentLevel.EnemiesDefeated++;
                GameData.Score -= 2000;
                This.Game.CurrentLevel.RemoveSprite(this);
            }
            #endregion explosion/deletion code
        }

        public void ActOnCollisions()
        {
            GameTime gameTime = This.gameTime;
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
                                    GameData.Score -= 1000;
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
                                    GameData.Score -= 1000;
                                }

                                else
                                {
                                    health--;
                                    if (health <= 0)
                                    {
                                        Dead = true;

                                        timeOfDeath = gameTime.TotalGameTime;
                                        GameData.Score -= 1000;
                                        // change the animation if the rbc is dead
                                        SetAnimation(3);
                                        StartAnim();
                                    }
                                }
                            }
                            else if (collision.Item2.GetType() == typeof(Background_Collision))
                            {
                                /*bool bgCollision = true;*/
                                CollisionObject boundingBox = collision.Item3;
                                /*do
                                {
                                    bgCollision = false;
                                */
                                //move object to new location
                                Vector2 corner1 = new Vector2(boundingBox.drawPoints[0].Position.X,
                                                                  boundingBox.drawPoints[0].Position.Y);
                                Vector2 corner2 = new Vector2(boundingBox.drawPoints[1].Position.X,
                                                              boundingBox.drawPoints[1].Position.Y);
                                Vector2 c1toc2 = Vector2.Normalize(corner2 - corner1);
                                Vector2 normal = new Vector2(-c1toc2.Y, c1toc2.X);
                                normal.Normalize();

                                MovementDir = 2 * (-MovementDir * normal) * normal + MovementDir + (new Vector2(normal.X*.75f,normal.Y*.45f)) ;
                            }
                        }
                    }
                }
            }
        }
    }
}
