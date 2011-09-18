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
            Sprite cannon = new Cannon(name + "_cannon", new Actor(This.Game.CurrentLevel.GetAnimation("cannon.anim")), name, input);
            cannon.ZOrder = ZOrder + 1;

            RemainingLives = DefaultLives;
            this.gamepad = input;
            shipVelocity = Vector2.Zero;
            shipSpeed = 10.0f;
            lThumbstick = Vector2.Zero;
            rThumbstick = Vector2.Zero;
            shieldName = Name + "_shield";
            shieldEndTime = shieldDuration;
            EnableShield();


            UpdateBehavior += new Behavior(Update);
        }

        private Vector2 shipVelocity;
        private float shipSpeed;
        private PlayerIndex gamepad;
        private bool shieldOn = false;
        private string shieldName = null;
        private TimeSpan shieldDuration = new TimeSpan(0, 0, 0, 4, 0);
        private TimeSpan shieldEndTime = TimeSpan.MinValue;
        private TimeSpan shootCooldown = new TimeSpan(0, 0, 0, 0, 100);
        private TimeSpan cooldownEndTime = TimeSpan.MinValue;
        private Vector2 lThumbstick;
        private Vector2 rThumbstick;
        public int RemainingLives = 0;
        public readonly int DefaultLives = 4;


        public void Update(GameTime gameTime)
        {
            if (shieldEndTime == TimeSpan.MinValue)
            {
                shieldEndTime = gameTime.TotalGameTime + shieldDuration;
            }
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(gamepad);
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

                        Sprite bullet = new Bullet("bullet",
                            new Actor(This.Game.CurrentLevel.GetAnimation("antibody.anim")),
                            shootDir * 15);
                        bullet.Pos = Pos;
                        bullet.AnimationSpeed = 1;
                        cooldownEndTime = gameTime.TotalGameTime + shootCooldown;
                        //this.Angle = (float)Math.Atan2(rThumbstick.X, -rThumbstick.Y);
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
                    Pos.X = This.Game.graphics.GraphicsDevice.Viewport.Width / 2;
                    Pos.Y = This.Game.graphics.GraphicsDevice.Viewport.Height / 2;
                    shipVelocity = Vector2.Zero;
                }

            }
            else /* Move with arrow keys */
            {
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

                Vector2 shootDirKeys = Pos;

                if (keys.IsKeyDown(Keys.Space) && gameTime.TotalGameTime > cooldownEndTime)
                {
                    Sprite bullet = new Bullet("bullet",
                            new Actor(This.Game.CurrentLevel.GetAnimation("antibody.anim")),
                            velocity * 15);
                    bullet.Pos = Pos + GetAnimation().AnimationPeg - bullet.GetAnimation().AnimationPeg;
                    bullet.AnimationSpeed = 1;
                    cooldownEndTime = gameTime.TotalGameTime + shootCooldown;
                }
            }

            if (!shieldOn)
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
                                    if (!shieldOn && !((Virus)collision.Item2).Harmless)
                                    {
                                        // mActor.CurrentAnimation = 1;
                                        Pos = This.Game.CurrentLevel.PlayerSpawnPoint;

                                        EnableShield();
                                        shieldEndTime = gameTime.TotalGameTime + shieldDuration;
                                        RemainingLives--;
                                        if (RemainingLives <= 0)
                                        {
                                            LevelFunctions.ToGameOver(null);
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (gameTime.TotalGameTime > shieldEndTime)
                {
                    DisableShield();
                }
            }

            shipVelocity *= 0.95f;
        }

        private void EnableShield()
        {
            shieldOn = true;
            Sprite shield = new Shield(shieldName, new Actor(This.Game.CurrentLevel.GetAnimation("shield.anim")), Name);
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
