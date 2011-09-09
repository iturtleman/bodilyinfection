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
        //public RedBloodCell(ContentManager cm, Vector2 position, Vector2 velocity)
        public RedBloodCell(string name, Actor actor): base(name, actor)
        {
            //rbcTexture = cm.Load<Texture2D>("rbc");
            //rbcPosition = position;
            rbcSpeed = new Vector2(10.0f, 10.0f);
            //randomVelocity = velocity;

        }

        private Texture2D rbcTexture;// { get; set; }
        private Vector2 rbcPosition;// { get; set; }
        private Vector2 rbcSpeed;// { get; set; }
        private Vector2 randomVelocity;

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();
            sb.Draw(rbcTexture, rbcPosition, Color.White);
            sb.End();
        }

        public void Update(GameTime gt, GraphicsDeviceManager gfx)
        {

            // Move the sprite by speed, scaled by elapsed time.
            rbcPosition.X +=
                rbcSpeed.X * ((float)gt.ElapsedGameTime.TotalSeconds + randomVelocity.X);

            rbcPosition.Y +=
                rbcSpeed.Y * ((float)gt.ElapsedGameTime.TotalSeconds + randomVelocity.Y);

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
