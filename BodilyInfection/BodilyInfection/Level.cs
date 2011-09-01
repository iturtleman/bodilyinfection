using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BodilyInfection
{
    class Level
    {

        #region Behaviors
        /// <summary>
        /// Level's load action
        /// </summary>
        Behavior LoadBehavior { get; set; }
        /// <summary>
        /// Level's update Behavior
        /// </summary>
        Behavior UpdateBehavior { get; set; }
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
        protected Dictionary<string, Actor> mActors = new Dictionary<string,Actor>();
       
        /// <summary>
        /// This level's Animations
        /// </summary>
        protected List<Animation> mAnims = new List<Animation>();
        #endregion Variables

        #region Methods
        internal void Load()
        {
            throw new NotImplementedException();
        }
        #endregion Methods

    }
}
