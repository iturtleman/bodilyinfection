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
            this.gamepad = input;
            shipVelocity = Vector2.Zero;
            shipSpeed = 10.0f;
            lThumbstick = Vector2.Zero;
            rThumbstick = Vector2.Zero;

            UpdateBehavior += new Behavior(Update);
        }

        private Vector2 shipVelocity;
        private float shipSpeed;
        private PlayerIndex gamepad;
        private bool shieldOn = false;   //ctrlf
        private TimeSpan shieldDuration = new TimeSpan(0, 0, 0, 4, 0);
        private TimeSpan shieldEndTime = TimeSpan.MinValue;
        private TimeSpan shootCooldown = new TimeSpan(0, 0, 0, 0, 100/*500*/);
        private TimeSpan cooldownEndTime = TimeSpan.MinValue;
        private Vector2 lThumbstick;
        private Vector2 rThumbstick;

        public void Update(GameTime gameTime)
        {
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
                rThumbstick.Y = currentState.ThumbSticks.Right.Y;

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
                    }
                }
                #endregion

                #region Rotation
                // Used to decide rotation angle.
                lThumbstick.X = currentState.ThumbSticks.Left.X;
                lThumbstick.Y = currentState.ThumbSticks.Left.Y;

                if ((lThumbstick.X != 0 || lThumbstick.Y != 0) && lThumbstick.Length() > .3f)
                {
                    //if (lThumbstick.Length() > .2f)
                    this.Angle = -(float)Math.Atan2(lThumbstick.Y, lThumbstick.X);
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
                    bullet.Pos = Pos;
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
                                    Pos.X = 50;
                                    Pos.Y = 50;
                                    shieldOn = true;

                                    mActor.CurrentAnimation = 1;
                                    mActor.Frame = 0;
                                    shieldEndTime = gameTime.TotalGameTime + shieldDuration;
                                    //This.Game.CurrentLevel.RemoveSprite(this);

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
                    shieldOn = false;

                    mActor.CurrentAnimation = 0;
                    mActor.Frame = 0;
                }
            }

            shipVelocity *= 0.95f;
        }
    }
}
