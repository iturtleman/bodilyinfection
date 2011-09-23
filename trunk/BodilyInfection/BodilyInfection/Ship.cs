﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BodilyInfection
{
    class Ship : Sprite
    {
        #region constructors
        public Ship(string name, Actor actor)
            : this(name, actor, PlayerIndex.One)
        {

        }

        public Ship(string name, Actor actor, PlayerIndex input)
            : base(name, actor)
        {
            cannon = new Cannon(name + "_cannon", new Actor((This.Game.CurrentLevel != This.Game.NextLevel && This.Game.NextLevel != null ? This.Game.NextLevel : This.Game.CurrentLevel).GetAnimation("cannon.anim")), name, input);
            cannon.ZOrder = ZOrder + 1;
            shieldEndTime = TimeSpan.MinValue;
            RemainingLives = DefaultLives;
            this.gamepad = input;
            shipVelocity = Vector2.Zero;
            shipSpeed = 10.0f;
            lThumbstick = Vector2.Zero;
            rThumbstick = Vector2.Zero;
            Dead = false;
            shieldName = Name + "_shield";
            Dead = false;
            EnableShield();


            UpdateBehavior = Update;
            CollisionBehavior = ActOnCollisions;
        }

        public Ship(string name, Actor actor, PlayerIndex input, string cannonAnimName)
            : base(name, actor)
        {
            cannon = new Cannon(cannonAnimName, new Actor((This.Game.CurrentLevel != This.Game.NextLevel && This.Game.NextLevel != null ? This.Game.NextLevel : This.Game.CurrentLevel).GetAnimation("cannon.anim")), name, input);
            cannon.ZOrder = ZOrder + 1;
            shieldEndTime = TimeSpan.MinValue;
            RemainingLives = DefaultLives;
            this.gamepad = input;
            shipVelocity = Vector2.Zero;
            shipSpeed = 10.0f;
            lThumbstick = Vector2.Zero;
            rThumbstick = Vector2.Zero;
            Dead = false;
            shieldName = Name + "_shield";
            Dead = false;
            EnableShield();


            UpdateBehavior = Update;
            CollisionBehavior = ActOnCollisions;
        }

        #endregion

        #region variables
        Sprite cannon;
        private Vector2 shipVelocity;
        private float shipSpeed;
        private PlayerIndex gamepad;
        private bool shieldOn = false;
        private string shieldName = null;
        private TimeSpan shieldDuration = new TimeSpan(0, 0, 0, 4, 0);
        private TimeSpan shieldEndTime = TimeSpan.MinValue;
        private TimeSpan shootCooldown = new TimeSpan(0, 0, 0, 0, 60);
        private TimeSpan cooldownEndTime = TimeSpan.MinValue;
        private TimeSpan timeOfDeath;
        private TimeSpan explosionLength = new TimeSpan(0, 0, 0, 0, 500);
        private Vector2 lThumbstick;
        private Vector2 rThumbstick;
        public int RemainingLives = 0;
        public readonly int DefaultLives = 4;
        #endregion

        public bool Dead { get; set; }

        public void Update()
        {
            GameTime gameTime = This.gameTime;
            if (shieldEndTime == TimeSpan.MinValue)
            {
                shieldEndTime = gameTime.TotalGameTime + shieldDuration;
            }
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(gamepad);
            if (!Dead)
            {
                if (currentState.IsConnected)
                {
                    #region Movement
                    Move(new Vector2(
                        Pos.X + shipSpeed * currentState.ThumbSticks.Left.X,
                        Pos.Y + shipSpeed * -currentState.ThumbSticks.Left.Y)
                    );
                    #endregion

                    #region Shooting
                    // Used to figure out the direction to shoot.
                    rThumbstick.X = currentState.ThumbSticks.Right.X;
                    rThumbstick.Y = -currentState.ThumbSticks.Right.Y;

                    if (rThumbstick.Length() != 0)
                    {
                        if (gameTime.TotalGameTime > cooldownEndTime)
                        {
                            Vector2 shootDir = rThumbstick;
                            shootDir.Normalize();

                            FireBullet(gameTime, shootDir);
                        }
                    }
                    #endregion

                    #region Rotation
                    // Used to decide rotation angle.
                    lThumbstick.X = currentState.ThumbSticks.Left.X;
                    lThumbstick.Y = -currentState.ThumbSticks.Left.Y;

                    if ((lThumbstick.X != 0 || lThumbstick.Y != 0) /*&& lThumbstick.Length() > .3f*/)
                    {
                        //if (lThumbstick.Length() > .2f)
                        this.Angle = (float)Math.Atan2(lThumbstick.Y, lThumbstick.X) + ((float)Math.PI / 2);
                    }
                    #endregion
#if NOCHEATS
#else

                    // In case you get lost, press A to warp back to the center.
                    if (currentState.Buttons.A == ButtonState.Pressed)
                    {
                        Pos = new Vector2();
                        shipVelocity = Vector2.Zero;
                        This.Game.AudioManager.PlaySoundEffect("ship_spawn");
                    }
#endif

                }
                else /* Move with arrow keys */
                {
                    #region Movement
                    KeyboardState keys = Keyboard.GetState();
                    Vector2 velocity = new Vector2();
                    if (keys.IsKeyDown(Keys.Up))
                    {
                        velocity.Y -= 1;
                    }
                    else if (keys.IsKeyDown(Keys.Down))
                    {
                        velocity.Y += 1;
                    }

                    if (keys.IsKeyDown(Keys.Left))
                    {
                        velocity.X -= 1;
                    }
                    else if (keys.IsKeyDown(Keys.Right))
                    {
                        velocity.X += 1;
                    }

                    if (keys.IsKeyDown(Keys.W))
                    {
                        Pos.Y -= shipSpeed;
                    }
                    else if (keys.IsKeyDown(Keys.S))
                    {
                        Pos.Y += shipSpeed;
                    }

                    if (keys.IsKeyDown(Keys.A))
                    {
                        Pos.X -= shipSpeed;
                    }
                    else if (keys.IsKeyDown(Keys.D))
                    {
                        Pos.X += shipSpeed;
                    }
                    velocity.Normalize();
                    this.Angle = -(float)Math.Atan2(Pos.Y, Pos.X);
                    #endregion Movement

                    if ((keys.IsKeyDown(Keys.Up) || keys.IsKeyDown(Keys.Down) || keys.IsKeyDown(Keys.Left) || keys.IsKeyDown(Keys.Right)) && gameTime.TotalGameTime > cooldownEndTime)
                    {
                        FireBullet(gameTime, velocity);
                    }
                }
            }
            #region explosion/deletion code
            if ((gameTime.TotalGameTime >= explosionLength + timeOfDeath) && Dead)
            {

                GameData.NumberOfLives--;
                if (GameData.NumberOfLives >= 0)
                {
                    Pos = (This.Game.CurrentLevel as BodilyInfectionLevel).PlayerSpawnPoint;
                    Dead = false;
                    This.Game.CurrentLevel.GetSprite("ship_cannon").mVisible = true;
                    SetAnimation(0);
                    StartAnim();
                    This.Game.AudioManager.PlaySoundEffect("ship_spawn");
                    EnableShield();
                    shieldEndTime = gameTime.TotalGameTime + shieldDuration;
                }
                else
                {
                    LevelFunctions.GoToGameOver();
                }

                //cannon.mVisible = true;
            }
            #endregion explosion/deletion code

            if (shieldOn && gameTime.TotalGameTime > shieldEndTime)
            {
                DisableShield();
            }

            shipVelocity *= 0.95f;
        }

        internal void Move(Vector2 pos)
        {
            Pos = pos;
            Sprite turret = This.Game.CurrentLevel.GetSprite("ship_cannon");
            Sprite shield = This.Game.CurrentLevel.GetSprite("ship_shield");
            if (turret != null)
            {
                turret.Pos = Pos + GetAnimation().AnimationPeg - turret.GetAnimation().AnimationPeg;
                if (shield != null)
                    shield.Pos = Pos + GetAnimation().AnimationPeg - shield.GetAnimation().AnimationPeg;
            }
        }

        private void ActOnCollisions()
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
                                if (!shieldOn && !((Virus)collision.Item2).Harmless && !Dead)
                                {
                                    This.Game.CurrentLevel.GetSprite("ship_cannon").mVisible = false;
                                    timeOfDeath = gameTime.TotalGameTime;
                                    SetAnimation(1);
                                    StartAnim();

                                    Dead = true;

                                    This.Game.AudioManager.PlaySoundEffect("ship_explosion", .7f);


                                    //break;
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
                                Vector2 animPeg = this.GetAnimation().AnimationPeg;
                                float radius = ((Collision_BoundingCircle)collision.Item1).radius;
                                Pos = (radius - Vector2.Dot(normal, (Pos + animPeg - corner1))) * normal + Pos;

                                //test for collisions in new position
                                /* List<Vector2> gridLocations = collision.Item1.gridLocations(this);
                                 foreach (Vector2 v in gridLocations)
                                 {
                                     List<WorldObject> worldObjectList = new List<WorldObject>();
                                     try
                                     {
                                         worldObjectList = Collision.bucket[v];
                                     }
                                     catch { };
                                     foreach (WorldObject worldObject in worldObjectList)
                                     {
                                         if (worldObject.GetType() == typeof(Background))
                                         {
                                             List<Tuple<CollisionObject, CollisionObject>> detectedCollisions = Collision.detectCollision(this, worldObject);
                                             if (detectedCollisions.Count != 0)
                                             {
                                                 bgCollision = true;
                                                 boundingBox = detectedCollisions[0].Item2;
                                                 break;
                                             }
                                         }
                                     }
                                     if (bgCollision)
                                         break;
                                 }
                             }
                             while (bgCollision);*/
                            }
                        }
                    }
                }
            }
        }

        private void FireBullet(GameTime gameTime, Vector2 shootDir)
        {
            Sprite bullet = new Bullet("bullet",
                                new Actor((This.Game.CurrentLevel != This.Game.NextLevel && This.Game.NextLevel != null ? This.Game.NextLevel : This.Game.CurrentLevel).GetAnimation("antibody.anim")),
                                shootDir * 15);
            bullet.Pos = Pos + GetAnimation().AnimationPeg - bullet.GetAnimation().AnimationPeg;
            cooldownEndTime = gameTime.TotalGameTime + shootCooldown;
            This.Game.AudioManager.PlaySoundEffect("gun1", .1f);
        }

        private void EnableShield()
        {
            shieldOn = true;
            Sprite shield = new Shield(shieldName, new Actor((This.Game.CurrentLevel != This.Game.NextLevel && This.Game.NextLevel != null ? This.Game.NextLevel : This.Game.CurrentLevel).GetAnimation("shield.anim")), Name);
            shield.Pos = Pos + GetAnimation().AnimationPeg - shield.GetAnimation().AnimationPeg;
            shield.ZOrder = ZOrder + 2;
        }

        private void DisableShield()
        {
            shieldOn = false;
            This.Game.CurrentLevel.RemoveSprite((This.Game.CurrentLevel != This.Game.NextLevel && This.Game.NextLevel != null ? This.Game.NextLevel : This.Game.CurrentLevel).GetSprite(shieldName));
        }
    }

    class Cannon : Sprite
    {
        private string shipName;
        private PlayerIndex gamepad;

        public Cannon(string name, Actor actor, string shipName, PlayerIndex input)
            : base(name, actor)
        {
            this.shipName = shipName;
            gamepad = input;
            UpdateBehavior = Update;
        }

        public void Update()
        {
            Sprite ship = This.Game.CurrentLevel.GetSprite(shipName);
            if (ship != null)
            {
                //Pos = ship.Pos + ship.GetAnimation().AnimationPeg - GetAnimation().AnimationPeg;

                #region Rotation
                GamePadState currentState = GamePad.GetState(gamepad);
                if (currentState.IsConnected)
                {
                    Vector2 rThumbstick;
                    // Used to decide rotation angle.
                    rThumbstick.X = currentState.ThumbSticks.Right.X;
                    rThumbstick.Y = -currentState.ThumbSticks.Right.Y;

                    if ((rThumbstick.X != 0 || rThumbstick.Y != 0) /*&& lThumbstick.Length() > .3f*/)
                    {
                        //if (lThumbstick.Length() > .2f)
                        this.Angle = (float)Math.Atan2(rThumbstick.Y, rThumbstick.X) + ((float)Math.PI / 2);
                    }
                }
                #endregion
            }
        }
    }

    class Shield : Sprite
    {
        private string shipName;

        public Shield(string name, Actor actor, string shipName)
            : base(name, actor)
        {
            this.shipName = shipName;
            UpdateBehavior = Update;
        }

        public void Update()
        {
            Sprite ship = This.Game.CurrentLevel.GetSprite(shipName);
            if (ship == null)
            {
                // Ship is gone, make self invisible
                mVisible = false;
                return;
            }
            else
            {
                mVisible = true;
                //Pos = ship.Pos + ship.GetAnimation().AnimationPeg - GetAnimation().AnimationPeg;
            }
        }
    }
}
