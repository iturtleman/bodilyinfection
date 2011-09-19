using System;
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
        public Ship(string name, Actor actor)
            : this(name, actor, PlayerIndex.One)
        {

        }

        public Ship(string name, Actor actor, PlayerIndex input)
            : base(name, actor)
        {
            cannon = new Cannon(name + "_cannon", new Actor(This.Game.CurrentLevel.GetAnimation("cannon.anim")), name, input);
            cannon.ZOrder = ZOrder + 1;
            shieldEndTime = TimeSpan.MinValue;
            RemainingLives = DefaultLives;
            this.gamepad = input;
            shipVelocity = Vector2.Zero;
            shipSpeed = 10.0f;
            lThumbstick = Vector2.Zero;
            rThumbstick = Vector2.Zero;
            shieldName = Name + "_shield";
            shieldEndTime = shieldDuration;
            Dead = false;
            EnableShield();


            UpdateBehavior += new Behavior(Update);
            CollisionBehavior += new Behavior(ActOnCollisions);
        }

        Sprite cannon;
        private Vector2 shipVelocity;
        private float shipSpeed;
        private PlayerIndex gamepad;
        private bool shieldOn = false;
        private string shieldName = null;
        private TimeSpan shieldDuration = new TimeSpan(0, 0, 0, 4, 0);
        private TimeSpan shieldEndTime = TimeSpan.MinValue;
        private TimeSpan shootCooldown = new TimeSpan(0, 0, 0, 0, 100);
        private TimeSpan cooldownEndTime = TimeSpan.MinValue;
        private TimeSpan timeOfDeath;
        private TimeSpan explosionLength = new TimeSpan(0, 0, 0, 0, 500);
        private Vector2 lThumbstick;
        private Vector2 rThumbstick;
        public int RemainingLives = 0;
        public readonly int DefaultLives = 4;

        public bool Dead { get; set; }

        public void Update(GameTime gameTime)
        {
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
                    Pos.X += shipSpeed * currentState.ThumbSticks.Left.X;
                    Pos.Y += shipSpeed * -currentState.ThumbSticks.Left.Y;
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

                    // In case you get lost, press A to warp back to the center.
                    if (currentState.Buttons.A == ButtonState.Pressed)
                    {
                        Pos.X = 0;//This.Game.graphics.GraphicsDevice.Viewport.Width / 2;
                        Pos.Y = 0;// This.Game.graphics.GraphicsDevice.Viewport.Height / 2;
                        shipVelocity = Vector2.Zero;
                        This.Game.AudioManager.PlaySoundEffect("ship_spawn");
                    }

                }
                else /* Move with arrow keys */
                {
                    #region Movement
                    KeyboardState keys = Keyboard.GetState();
                    Vector2 velocity = Pos;
                    if (keys.IsKeyDown(Keys.Up))
                    {
                        Pos.Y -= shipSpeed;
                    }
                    else if (keys.IsKeyDown(Keys.Down))
                    {
                        Pos.Y += shipSpeed;
                    }

                    if (keys.IsKeyDown(Keys.Left))
                    {
                        Pos.X -= shipSpeed;
                    }
                    else if (keys.IsKeyDown(Keys.Right))
                    {
                        Pos.X += shipSpeed;
                    }

                    velocity = Pos - velocity;
                    velocity.Normalize();
                    this.Angle = -(float)Math.Atan2(Pos.Y, Pos.X);
                    #endregion Movement
                    Vector2 shootDirKeys = Pos;

                    if (keys.IsKeyDown(Keys.Space) && gameTime.TotalGameTime > cooldownEndTime)
                    {
                        FireBullet(gameTime, shootDirKeys);
                    }
                }
            }
            #region explosion/deletion code
            if ((gameTime.TotalGameTime >= explosionLength + timeOfDeath) && Dead)
            {
                This.Game.AudioManager.PlaySoundEffect("ship_spawn");
                Pos = This.Game.CurrentLevel.PlayerSpawnPoint;
                Dead = false;
                This.Game.CurrentLevel.GetSprite("ship_cannon").mVisible = true;
                SetAnimation(0);
                StartAnim();

                EnableShield();
                shieldEndTime = gameTime.TotalGameTime + shieldDuration;
                RemainingLives--;
                if (RemainingLives <= 0)
                {
                    LevelFunctions.ToGameOver(null);
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

        private void ActOnCollisions(GameTime gameTime)
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
                                if (!shieldOn && !((Virus)collision.Item2).Harmless && !Dead)
                                {
                                    This.Game.CurrentLevel.GetSprite("ship_cannon").mVisible = false;
                                    timeOfDeath = gameTime.TotalGameTime;
                                    SetAnimation(1);
                                    StartAnim();

                                    Dead = true;

                                    This.Game.AudioManager.PlaySoundEffect("ship_explosion");


                                    //break;
                                }
                            }
                            else if (collision.Item2.GetType() == typeof(Background))
                            {
                                Vector2 corner1 = new Vector2(collision.Item3.drawPoints[0].Position.X,
                                                              collision.Item3.drawPoints[0].Position.Y);
                                Vector2 corner2 = new Vector2(collision.Item3.drawPoints[1].Position.X,
                                                              collision.Item3.drawPoints[1].Position.Y);
                                Vector2 c1toc2 = Vector2.Normalize(corner2 - corner1);
                                Vector2 normal = new Vector2(-c1toc2.Y, c1toc2.X);
                                Vector2 animPeg = this.GetAnimation().AnimationPeg;
                                float radius = ((Collision_BoundingCircle)collision.Item1).radius;
                                Pos = (radius - Vector2.Dot(normal, (Pos + animPeg - corner1))) * normal + Pos;
                            }
                        }
                    }
                }
            }
        }

        private void FireBullet(GameTime gameTime, Vector2 shootDir)
        {
            Sprite bullet = new Bullet("bullet",
                                new Actor(This.Game.CurrentLevel.GetAnimation("antibody.anim")),
                                shootDir * 15);
            bullet.Pos = Pos + GetAnimation().AnimationPeg - bullet.GetAnimation().AnimationPeg;
            bullet.AnimationSpeed = 1;
            cooldownEndTime = gameTime.TotalGameTime + shootCooldown;
            This.Game.AudioManager.PlaySoundEffect("gun1", .3f);
        }

        private void EnableShield()
        {
            shieldOn = true;
            Sprite shield = new Shield(shieldName, new Actor(This.Game.CurrentLevel.GetAnimation("shield.anim")), Name);
            Pos = shield.Pos + shield.GetAnimation().AnimationPeg - GetAnimation().AnimationPeg;
            shield.ZOrder = ZOrder + 2;
        }

        private void DisableShield()
        {
            shieldOn = false;
            This.Game.CurrentLevel.RemoveSprite(This.Game.CurrentLevel.GetSprite(shieldName));
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
            UpdateBehavior += Update;
        }

        public void Update(GameTime gameTime)
        {
            Sprite ship = This.Game.CurrentLevel.GetSprite(shipName);
            if (ship != null)
            {
                Pos = ship.Pos + ship.GetAnimation().AnimationPeg - GetAnimation().AnimationPeg;

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
            UpdateBehavior += Update;
        }

        public void Update(GameTime gameTime)
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
                Pos = ship.Pos + ship.GetAnimation().AnimationPeg - GetAnimation().AnimationPeg;
            }
        }
    }
}
