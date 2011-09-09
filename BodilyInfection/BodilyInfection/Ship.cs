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

            UpdateBehavior += new Behavior(Update);
        }

        private Vector2 shipVelocity;
        private float shipSpeed;
        private PlayerIndex gamepad;


        public void Update()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(gamepad);
            if (currentState.IsConnected)
            {

                // Create some velocity if the right trigger is down.
                //Vector2 shipVelocityAdd = Vector2.Zero;


                Pos.X += shipSpeed * currentState.ThumbSticks.Left.X;
                Pos.Y += shipSpeed * -currentState.ThumbSticks.Left.Y;

                // Now scale our direction by how hard the trigger is down.
                //shipVelocityAdd *= currentState.ThumbSticks.Left.X;

                // Finally, add this vector to our velocity.
                // shipVelocity += shipVelocityAdd;

                //if (currentState.ThumbSticks.Left.X != 0 || currentState.ThumbSticks.Left.Y != 0)
                //    GamePad.SetVibration(PlayerIndex.One, 1, 1);
                //else GamePad.SetVibration(PlayerIndex.One, 0, 0);


                // In case you get lost, press A to warp back to the center.
                if (currentState.Buttons.A == ButtonState.Pressed)
                {
                    Pos.X = This.Game.graphics.GraphicsDevice.Viewport.Width / 2;
                    Pos.Y = This.Game.graphics.GraphicsDevice.Viewport.Height / 2;
                    shipVelocity = Vector2.Zero;
                }

                //Pos += shipVelocity;

                // Bleed off velocity over time.
                //shipVelocity *= 0.95f;
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
            shipVelocity *= 0.95f;
        }
    }
}
