using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;



namespace Collision
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Effect effect;
        SpriteFont font;
        public const int numberOfObjects = 1;
        Matrix viewMatrix;
        Matrix projectionMatrix;
        private const int screenHeight = 800;
        private const int screenWidth = 800;
        TimeSpan timeFill;
        private Collision collision = new Collision();
        public List<Objects> objects;

        //---------AI Stuff----------
        private Ai ai = new Ai();
        int numberOfRbcs = 5;
        int numberOfViruses = 10;
        Random random = new Random();   // used for random movement of red blood cells
        private List<RedBloodCell> rbcs;
        private List<Virus> viruses;
        private Ship ship;
        //---------------------------


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Vincent's AI";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            device = graphics.GraphicsDevice;


            effect = Content.Load<Effect>("effects");
            SetUpCamera();
            font = Content.Load<SpriteFont>("myFont");

            SetUpObjects();

            collision.gridCellHeight = 40;
            collision.gridCellWidth = 40;
            collision.createGrid(0, 0, 800, 800);
            collision.drawGrid = true;


        }

        protected override void UnloadContent()
        {
        }

        private Circle generateCircle(Random r)
        {
            Circle circle = new Circle();

            circle.location = new Vector2( (float)r.Next(screenWidth), (float)r.Next(screenHeight) );

            circle.velocityX = 0;
            circle.velocityY = 0;

            circle.radius = 15f;

            circle.points = new VertexPositionColor[5];
            circle.points[0].Position = new Vector3(circle.location.X, circle.location.Y + circle.radius, 0f);
            circle.points[0].Color = Color.Red;
            circle.points[1].Position = new Vector3(circle.location.X - circle.radius, circle.location.Y, 0f);
            circle.points[1].Color = Color.Red;
            circle.points[2].Position = new Vector3(circle.location.X, circle.location.Y - circle.radius, 0f);
            circle.points[2].Color = Color.Red;
            circle.points[3].Position = new Vector3(circle.location.X + circle.radius, circle.location.Y, 0f);
            circle.points[3].Color = Color.Red;
            circle.points[4].Position = new Vector3(circle.location.X, circle.location.Y + circle.radius, 0f);
            circle.points[4].Color = Color.Red;

            return circle;
        }

        private void SetUpObjects()
        {
            Random r = new Random();
            objects = new List<Objects>();
            rbcs = new List<RedBloodCell>();
            viruses = new List<Virus>();

            for (int i = 0; i < numberOfObjects; i++)
            {
                Circle circle = generateCircle(r);
                objects.Add(circle);
            }

            for (int i = 0; i < numberOfRbcs; i++)
            {
                RedBloodCell rbc = new RedBloodCell(Content, random, graphics);
                rbcs.Add(rbc);
            }

            for (int i = 0; i < numberOfViruses; i++)
            {
                Virus virus = new Virus(Content, random, graphics);
                viruses.Add(virus);
            }

            ship = new Ship(Content, graphics);
        }

        private void DrawText(GameTime time)
        {
            spriteBatch.Begin();
            if (time.ElapsedGameTime.Milliseconds != 0)
                spriteBatch.DrawString(font
                    , (1000 / time.ElapsedGameTime.Milliseconds).ToString()
                    , new Vector2(20, 45), Color.White);
            else
                spriteBatch.DrawString(font, "Too High", new Vector2(20, 45), Color.White);

            spriteBatch.DrawString(font, "You can write text like this.", new Vector2(20, 65), Color.White);
            spriteBatch.DrawString(font, "Speed of your code: " + timeFill.Milliseconds.ToString(), new Vector2(20, 85), Color.White);
            spriteBatch.End();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            DateTime startFill = DateTime.Now;
            //code you want to test time of
           
            //------------------------------------------------

            foreach (RedBloodCell i in rbcs)
            {
                i.Update(gameTime, graphics);
            }

            foreach (Virus i in viruses)
            {
                i.Update(gameTime, graphics, ship.shipPosition);
            }

            ship.Update();
            //------------------------------------------------
            DateTime stopFill = DateTime.Now;


            timeFill = stopFill - startFill;
            base.Update(gameTime);
        }

        private void SetUpCamera()
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(screenWidth / 2, screenHeight / 2, 1200), new Vector3(screenWidth / 2, screenHeight / 2, 0), new Vector3(0, 1, 0));
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 1.0f, 2000.0f);
        }

        protected override void Draw(GameTime gameTime)
        {
            device.Clear(Color.DarkSlateBlue);

            effect.CurrentTechnique = effect.Techniques["ColoredNoShading"];
            effect.Parameters["xView"].SetValue(viewMatrix);
            effect.Parameters["xProjection"].SetValue(projectionMatrix);
            effect.Parameters["xWorld"].SetValue(Matrix.Identity);


            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                collision.draw(device);
                for (int i = 0; i < numberOfObjects; i++)
                    device.DrawUserPrimitives(PrimitiveType.LineStrip, objects[i].points, 0, 4);

            }

            DrawText(gameTime);

            foreach (RedBloodCell i in rbcs)
            {
                i.Draw(spriteBatch);
            }

            foreach (Virus i in viruses)
            {
                i.Draw(spriteBatch);
            }

            ship.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}