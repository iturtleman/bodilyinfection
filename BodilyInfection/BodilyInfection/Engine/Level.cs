using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BodilyInfection
{
    class Level
    {

        #region Constructor
        public Level()
        {
            LoadBehavior = (GameTime gameTime) => { };
            UpdateBehavior = (GameTime gameTime) => { };
            EndBehavior = (GameTime gameTime) => { };
        }
        public Level(string n, Behavior loadBehavior, Behavior updateBehavior, Behavior endBehavior, Condition winCondition)
        {
            mName = n;
            LoadBehavior = loadBehavior;
            UpdateBehavior = updateBehavior;
            EndBehavior = endBehavior;
            WinCondition = winCondition;
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

        /// <summary>
        /// A Condition to check if the level has been won
        /// </summary>
        public Condition WinCondition { get; set; }

        #region Properties
        /// <summary>
        /// Get's level's name
        /// </summary>
        public string Name { get { return mName; } }
        /// <summary>
        /// Gets and Sets Level's current Background
        /// </summary>
        public Background Background { get; set; }

        /// <summary>
        /// A count of the enemies defeated in this level
        /// </summary>
        public int EnemiesDefeated { get; set; }
        #endregion Properties

        #region Variables
        /// <summary>
        /// Level's name
        /// </summary>
        string mName = "";

        /// <summary>
        /// Vector of all WorldObjects drawn on the level.
        /// </summary>
        internal List<WorldObject> mSprites = new List<WorldObject>();

        /// <summary>
        /// This level's actors. 
        /// </summary>
        protected Dictionary<string, Actor> mActors = new Dictionary<string, Actor>();

        /// <summary>
        /// This level's Animations
        /// </summary>
        protected List<Animation> mAnims = new List<Animation>();

        protected List<WorldObject> ToAdd = new List<WorldObject>();
        protected List<WorldObject> ToRemove = new List<WorldObject>();

        public Vector2 PlayerSpawnPoint = new Vector2(50, 50);
        #endregion Variables

        #region Updating
        internal void Load()
        {
            This.Game.AudioManager.Stop();
            EnemiesDefeated = 0;
            LoadBehavior(null);
        }

        internal void Update(GameTime gameTime)
        {
            if (WinCondition())
            {
                mSprites.Clear();
                EndBehavior(gameTime);
                return;
            }
            mSprites.Sort();
            Collision.update();
            UpdateBehavior(gameTime);
            foreach (Sprite sp in mSprites)
            {
                sp.UpdateBehavior(gameTime);
            }
            foreach (var item in ToRemove)
            {
                 mSprites.Remove(item);
            }
            ToRemove.Clear();
            foreach (var item in ToAdd)
            {
                mSprites.Add(item);
            }
            ToAdd.Clear();
        }
        #endregion Updating

        #region Methods



        #region Draw
        internal void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            This.Game.spriteBatch.Begin();
            /** Draw BG */
            if (Background != null)
                Background.Draw();

            /** Draw Sprites*/
            DrawSprites(gameTime);

            /** Draw Boundary Data */
            Collision.Draw();
            This.Game.spriteBatch.End();
        }
        internal void DrawSprites(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (var sprite in mSprites)
            {
                sprite.Draw(gameTime);
            }
        }

        #endregion Drawing

        #region Management
        public void AddSprite(Sprite sp)
        {
            ToAdd.Add(sp);
        }

        public Sprite GetSprite(string name)
        {
            return (mSprites.Find(delegate(WorldObject s) { return s.Name == name; }) as Sprite);
        }

        /// <summary>
        /// Retrieves all sprites with the specified type.
        /// @todo Get only sprites within a certain distance of a point, for efficiency's sake.
        ///     Possibly could make use of Bruce's collision code.
        /// </summary>
        /// <param name="typename">The type name to select by.</param>
        /// <returns></returns>
        public List<Sprite> GetSpritesByType(string typename)
        {
            return (mSprites.FindAll(
                delegate(WorldObject s) { return s.GetType().Name == typename; }).ConvertAll<Sprite>(
                    delegate(WorldObject s) { return s as Sprite; }));
        }

        public void RemoveSprite(Sprite sp)
        {
            ToRemove.Add(sp);
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
            mActors[name] = actor;
        }

        public void RemoveActor(string name)
        {
            mActors.Remove(name);
        }
        #endregion Management
        #endregion Methods
    }
}
