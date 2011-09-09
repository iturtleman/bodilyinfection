using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Collision
{
    class Ship
    {
        public Ship(ContentManager cm, GraphicsDeviceManager gfx)
        {
            shipVelocity = Vector2.Zero;
            shipTexture = cm.Load<Texture2D>("ship");
            shipPosition.X = gfx.GraphicsDevice.Viewport.Width / 2;
            shipPosition.Y = gfx.GraphicsDevice.Viewport.Height / 2;
            shipSpeed = 10.0f;
            gdm = gfx;
        }

        private Texture2D shipTexture;
        public Vector2 shipPosition;
        private Vector2 shipVelocity;
        private float shipSpeed;

        GraphicsDeviceManager gdm;

        public void Update()
        {
            // Get the game pad state.
            GamePadState currentState = GamePad.GetState(PlayerIndex.One);
            if (currentState.IsConnected)
            {
                
                // Create some velocity if the right trigger is down.
                //Vector2 shipVelocityAdd = Vector2.Zero;

                       
                shipPosition.X += shipSpeed * currentState.ThumbSticks.Left.X;
                shipPosition.Y += shipSpeed * -currentState.ThumbSticks.Left.Y;

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
                    shipPosition.X = gdm.GraphicsDevice.Viewport.Width / 2;
                    shipPosition.Y = gdm.GraphicsDevice.Viewport.Height / 2;
                    shipVelocity = Vector2.Zero;
                }

                //shipPosition += shipVelocity;

                // Bleed off velocity over time.
                //shipVelocity *= 0.95f;
            }
        }
       
        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(shipTexture, shipPosition, Color.White);
            sb.End();
        }
    }
}
