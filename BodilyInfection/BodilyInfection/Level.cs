using System;
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
        }
        public Level(string n, Behavior loadBehave, Behavior updateBehave)
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
        protected Behavior LoadBehavior { get; set; }
        /// <summary>
        /// Level's update Behavior
        /// </summary>
        protected Behavior UpdateBehavior { get; set; }
        #endregion Behaviors

        #region Properties
        /// <summary>
        /// Get's level's name
        /// </summary>
        public string Name { get { return mName; } }
        /// <summary>
        /// Gets and Sets Level's background
        /// </summary>
        Background Background { get; set; }
        #endregion Properties

        #region Variables
        /// <summary>
        /// Level's name
        /// </summary>
        string mName = "";

        /// <summary>
        /// Vector of all sprites on the level. \todo Maybe make into Dictonary? This should be a list of World objects once implemented. \todo add accessor or move to public \todo make a Dictonary by name (for collisions) \todo make a list ordered by Zorder (for drawing)
        /// </summary>
        protected List<Sprite> mSprites = new List<Sprite>();

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

        #region Drawing
        public void DrawSprites()
        {
            for (int i = 0; i < mSprites.Count; i++)
            {
                mSprites[i].Draw();
            }
        }

        //void DrawCollisions()
        //{
        //    if(Game::game()->ShowCollisions){
        //        mBackground->drawCollisions();
        //        for (size_t i=0; i<mSprites.size(); ++i) {
        //            mSprites[i]->drawCollisions();
        //        }
        //    }
        //}
        #endregion Drawing

        public void AddSprite(Sprite sp)
        {
            mSprites.Add(sp);
        }

        public Sprite GetSprite(string name)
        {
            return mSprites.Find(delegate(Sprite s) { return s.Name == name; });
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
