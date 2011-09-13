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
            rotationAngle = 0.0f;
            lThumbstick = Vector2.Zero;
            rThumbstick = Vector2.Zero;

            UpdateBehavior += new Behavior(Update);
        }

        private Vector2 shipVelocity;
        private float shipSpeed;
        private PlayerIndex gamepad;
        private bool temporaryShield = false;
        private int temporaryShieldCount = 0;
        private int temporaryShieldMax = 200;
        private float rotationAngle;
        private Vector2 lThumbstick;
        private Vector2 rThumbstick;



        public void Update()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(gamepad);
            if (currentState.IsConnected)
            {

                Pos.X += shipSpeed * currentState.ThumbSticks.Left.X;
                Pos.Y += shipSpeed * -currentState.ThumbSticks.Left.Y;
                

                // Used to decide rotation angle.
                lThumbstick.X = currentState.ThumbSticks.Left.X;
                lThumbstick.Y = currentState.ThumbSticks.Left.Y;


                // Used to figure out the direction to shoot.
                rThumbstick.X = currentState.ThumbSticks.Right.X;
                rThumbstick.Y = currentState.ThumbSticks.Right.X;

                if (lThumbstick.Length() > .2f)
                    rotationAngle = -(float)Math.Atan2(lThumbstick.Y, lThumbstick.X);

                // Draw Method
                //sb.Draw(shipTexture, shipPosition, null, Color.White, rotationAngle, origin, 1.0f , SpriteEffects.None, 0.0f);

                this.mAngle = (180 *rotationAngle) / 3.14159f;

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
            }

            if (!temporaryShield)
            {
                if (Collision.collisionData.Count > 0)
                {
                    foreach (CollisionObject co in this.GetCollision())
                    {
                        if (Collision.collisionData.ContainsKey(co))
                        {
                            foreach (CollisionObject collision in Collision.collisionData[co])
                            {
                                if (collision.parentObject.GetType() == typeof(Virus))
                                {
                                    Pos.X = 50;
                                    Pos.Y = 50;
                                    temporaryShield = true;

                                    mActor.CurrentAnimation = 1;
                                    mActor.Frame = 0;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                temporaryShieldCount++;
                if (temporaryShieldCount > temporaryShieldMax)
                {
                    temporaryShield = false;
                    temporaryShieldCount = 0;

                    mActor.CurrentAnimation = 0;
                    mActor.Frame = 0;
                }
                // DoChangeAnimation
            }

            shipVelocity *= 0.95f;
        }
    }
}
