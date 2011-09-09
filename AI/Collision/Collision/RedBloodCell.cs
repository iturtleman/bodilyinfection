using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Collision
{
    class RedBloodCell
    {
        public RedBloodCell(ContentManager cm, Random random, GraphicsDeviceManager gfx)
        {
            rbcTexture = cm.Load<Texture2D>("rbc");
            rbcPosition.X = random.Next(0, gfx.GraphicsDevice.Viewport.Width);
            rbcPosition.Y = random.Next(0, gfx.GraphicsDevice.Viewport.Height);
            rbcSpeed = new Vector2(10.0f, 10.0f);

            // Generate random numbers for movement pattern

            randomX = ((float)random.Next(1, 10) / 100) + ((float)random.Next(1, 10) / 1000);
            randomY = ((float)random.Next(1, 10) / 100) + ((float)random.Next(1, 10) / 1000);
        }

        private Texture2D rbcTexture;// { get; set; }
        private Vector2 rbcPosition;// { get; set; }
        private Vector2 rbcSpeed;// { get; set; }
        private float randomX, randomY;

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(rbcTexture, rbcPosition, Color.White);
            sb.End();
        }

        public void Update(GameTime gt, GraphicsDeviceManager gfx) {
 
            // Move the sprite by speed, scaled by elapsed time.
            rbcPosition.X +=
                rbcSpeed.X * ((float)gt.ElapsedGameTime.TotalSeconds + randomX);

            rbcPosition.Y +=
                rbcSpeed.Y * ((float)gt.ElapsedGameTime.TotalSeconds + randomY);

            int MaxX =
                gfx.GraphicsDevice.Viewport.Width - rbcTexture.Width;
            int MinX = 0;
            int MaxY =
                gfx.GraphicsDevice.Viewport.Height - rbcTexture.Height;
            int MinY = 0;

            // Check for bounce.
            if (rbcPosition.X > MaxX)
            {
                rbcSpeed.X *= -1;
                rbcPosition.X = MaxX;
            }

            else if (rbcPosition.X < MinX)
            {
                rbcSpeed.X *= -1;
                rbcPosition.X = MinX;
            }

            if (rbcPosition.Y > MaxY)
            {
                rbcSpeed.Y *= -1;
                rbcPosition.Y = MaxY;
            }

            else if (rbcPosition.Y < MinY)
            {
                rbcSpeed.Y *= -1;
                rbcPosition.Y = MinY;
            }
        }
    }
}
