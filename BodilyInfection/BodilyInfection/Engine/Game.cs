using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BodilyInfection
{
    /// \todo move this somewhere else
    public delegate void Behavior(GameTime gameTime);
    public delegate bool Condition();

    /// <summary>
    /// High-level controller for the game
    /// This class is tasked with the following:
    /// - creating the SDL screen
    /// - loading levels from appropriate locations
    /// - switching levels as appropriate
    /// </summary>
    internal partial class Game : Microsoft.Xna.Framework.Game
    {
        #region Properties
        /// <summary>
        /// Decides whether or not to draw bounding boxes
        /// </summary>
        public static bool ShowCollisionData { get { return Collision.ShowCollisionData; } set { Collision.ShowCollisionData = value; } }
        /// <summary>
        /// Decides whether or not to draw FPS
        /// </summary>
        public bool ShowFPS { get; set; }

        /// <summary>
        /// Gets or sets the current level being played.
        /// </summary>
        internal Level CurrentLevel
        {
            get
            {
                if (mCurrentLevel < mLevels.Count)
                {
                    return mLevels[mCurrentLevel];
                }
                else
                {
                    return null;
                }
            }
        }

        internal AudioManager AudioManager
        {
            get
            {
                return mAudioManager;
            }
        }

        /// <summary>
        /// Gets the index of the current level being played.
        /// </summary>
        int LevelIndex
        {
            get
            {
                return mCurrentLevel;
            }

        }

        /// <summary>
        /// Gets the total count of levels.
        /// </summary>
        int LevelCount { get { return mLevels.Count; } }

        /// <summary>
        /// Returns the current FPS of the game
        /// </summary>
        int FPS { get { return currentFPS; } }
        #endregion Properties

        #region Variables

        #region FPS calc
        /// <summary>
        /// \todo implement or get rid of these
        /// </summary>
        int currentFPS;
        #endregion FPS calc

        int mCurrentLevel;/**< Current Level index. */
        AudioManager mAudioManager = new AudioManager();
        List<Level> mLevels = new List<Level>();
        Dictionary<string, Behavior> mLevelBehaviors = new Dictionary<string, Behavior>();
        #endregion Variables

        #region premade things
        internal GraphicsDeviceManager graphics;
        internal SpriteBatch spriteBatch;
        #endregion

        #region Constructor
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        #endregion Constructor

        #region Initialization
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // todo: Add your initialization logic here

            base.Initialize();

            //Initialize Collision Cell Size
            Collision.gridCellHeight = 40;
            Collision.gridCellWidth = 40;
            Collision.createGrid(0, 0, 800, 800);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // todo: use this.Content to load your game content here
            LoadResources();

            SetCurrentLevel("TitleScreen");
        }
        #endregion Initialization

        #region Destruction
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // todo: Unload any non ContentManager content here
        }
        #endregion Destruction

        #region Updating
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            #region Handle input
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                ShowCollisionData = !ShowCollisionData;
            }


            #endregion Handle input


            if (CurrentLevel != null)
                CurrentLevel.Update(gameTime);

            #region FPS
            if (gameTime.ElapsedGameTime.Milliseconds > 0)
                currentFPS = 1000 / gameTime.ElapsedGameTime.Milliseconds;
            #endregion FPS

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (CurrentLevel != null)
                CurrentLevel.Draw(gameTime);

            #region DrawFPS
            if (ShowFPS)
            {

            }
            #endregion DrawFPS

            base.Draw(gameTime);
        }
        #endregion Updating

        #region Methods
        /// <summary>
        /// Sets the current level.
        /// </summary>
        /// <param name="name"></param>
        public void SetCurrentLevel(string name)
        {
            for (int i = 0, count = mLevels.Count; i < count; i++)
            {
                if (mLevels[i].Name == name)
                {
                    mCurrentLevel = i;
                    mLevels[mCurrentLevel].Load();
                    return;
                }
            }
            Console.WriteLine(string.Format("Level {0} does not exist", name));
        }
        #endregion Methods
    }
}
