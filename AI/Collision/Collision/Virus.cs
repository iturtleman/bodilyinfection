using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Collision
{
    class Virus
    {
        public Virus(ContentManager cm, Vector2 position)
        {
            virusTexture = cm.Load<Texture2D>("virus");
            virusPosition = position;
            virusVelocity = Vector2.Zero;
        }

        private Texture2D virusTexture;// { get; set; }
        private Vector2 virusPosition;// { get; set; }
        private Vector2 virusVelocity;// { get; set;} 

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(virusTexture, virusPosition, Color.White);
            sb.End();
        }

        public void Update(GameTime gt, GraphicsDeviceManager gfx, Vector2 shipPosition)
        {
            // Create some velocity if the right trigger is down.
            Vector2 virusVelocityAdd = Vector2.Zero;
            Vector2 dirToShip;
            dirToShip.X = shipPosition.X - virusPosition.X;
            dirToShip.Y = shipPosition.Y - virusPosition.Y;

            // Find out what direction we should be thrusting, 
            // using rotation.
            virusVelocityAdd.X = dirToShip.X;
            virusVelocityAdd.Y = dirToShip.Y;
            virusVelocityAdd.Normalize();
            // Now scale our direction by the speed of the virus
            virusVelocityAdd *= 4;

            // Finally, add this vector to our velocity.
            virusVelocity += virusVelocityAdd;

            virusPosition += virusVelocity;

            // Bleed off velocity over time.
            virusVelocity *= 0.15f;

        }
    }
}
