﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodilyInfection
{
    class Level
    {
        #region Constructor
        public Level()
        {
            LoadBehavior = () => { };
            UpdateBehavior = () => { };
            EndBehavior = () => { };
        }
        public Level(string n, Behavior loadBehave, Behavior updateBehave, Behavior endBehavior)
        {
            mName = n;
            LoadBehavior = loadBehave;
            UpdateBehavior = updateBehave;
        }
        #endregion Constructor

        #region Behaviors
        /// <summary>
        /// Level's load action
        /// </summary>
        public Behavior LoadBehavior { get; set; }
        /// <summary>
        /// Level's update Behavior
        /// </summary>
        public Behavior UpdateBehavior { get; set; }
        /// <summary>
        /// Level's End Behavior
        /// </summary>
        public Behavior EndBehavior { get; set; }
        #endregion Behaviors

        #region Properties
        /// <summary>
        /// Get's level's name
        /// </summary>
        public string Name { get { return mName; } }
        /// <summary>
        /// Gets and Sets Level's current Background
        /// </summary>
        public Background Background { get; set; }
        #endregion Properties

        #region Variables
        /// <summary>
        /// Level's name
        /// </summary>
        string mName = "";

        /// <summary>
        /// Vector of all WorldObjects drawn on the level.
        /// </summary>
        protected List<WorldObject> mSprites = new List<WorldObject>();

        /// <summary>
        /// This level's actors. 
        /// </summary>
        protected Dictionary<string, Actor> mActors = new Dictionary<string, Actor>();

        /// <summary>
        /// This level's Animations
        /// </summary>
        protected List<Animation> mAnims = new List<Animation>();

        #endregion Variables

        #region Methods

        #region Actions
        internal void Load()
        {
            LoadBehavior();
        }
        internal void Update()
        {
            UpdateBehavior();
        }
        #endregion Actions

        #region Draw
        internal void DrawScene(Microsoft.Xna.Framework.GameTime gameTime)
        {
            This.Game.spriteBatch.Begin();
            /** Draw BG */
            if (Background != null)
                Background.Draw();

            /** Draw Sprites*/
            DrawSprites(gameTime);

            /** Draw Boundary Data */
            DrawCollisions();
            This.Game.spriteBatch.End();
        }
        internal void DrawSprites(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (var sprite in mSprites)
            {
                sprite.Draw(gameTime);
            }
        }

        void DrawCollisions()
        {
            if(Game.ShowCollisions){
                Background.DrawCollisions();
                foreach (var sprite in mSprites)
                {
                    sprite.DrawCollisions();
                }
            }
        }
        #endregion Drawing

        public void AddSprite(Sprite sp)
        {
            mSprites.Add(sp);
        }

        public Sprite GetSprite(string name)
        {
            return (mSprites.Find(delegate(WorldObject s) { return s.Name == name; }) as Sprite);
        }

        public void RemoveSprite(Sprite sp)
        {
            mSprites.Remove(sp);
        }

        public void AddAnimation(Animation anim)
        {
            mAnims.Add(anim);
        }

        public Animation GetAnimation(string name)
        {
            return mAnims.Find(delegate(Animation a) { return a.Name == name; });
        }

        public void RemoveAnimation(Animation anim)
        {
            mAnims.Remove(anim);
        }

        public void AddActor(string name, Actor actor)
        {
            mActors[name]= actor;
        }

        public void RemoveActor(string name)
        {
            mActors.Remove(name);
        }
        #endregion Methods
    }
}