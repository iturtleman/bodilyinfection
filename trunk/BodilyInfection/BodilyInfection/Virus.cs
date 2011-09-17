using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BodilyInfection
{
    class Virus : Sprite
    {
        public Virus(string name, Actor actor)
            : base(name, actor)
        {
            UpdateBehavior += new Behavior(Update);
            Harmless = true;
            Invincible = true;
            Frozen = true;

            birth = TimeSpan.Zero;
        }

        #region Properties
        // Virus is Harmless for very small amount of time after spawn.
        // To allow player to get away from being killed by a spawning enemy
        public bool Harmless { get; set; }
        public bool Invincible { get; set; }
        public bool Frozen { get; set; }


        #endregion Properties

        #region Variables

        private Vector2 currentVelocity;

        private float movementSpeed = 2;


        private TimeSpan lifeSpan;

        private TimeSpan harmlessTime = new TimeSpan(0, 0, 0, 0, 500);

        private TimeSpan birth;

        /// <summary>
        /// The maximum distance in pixels the virus can "see" when detecting a nearby RedBloodCell
        /// </summary>
        private float attackDistance = 100;

        #endregion

        #region Methods

        public void Update(GameTime gameTime)
        {
            if (This.Game.CurrentLevel.GetSprite("ship") != null)
            {
                Sprite ship = This.Game.CurrentLevel.GetSprite("ship");
                Vector2 minVector = ship.Pos;
                float minDistance = (Pos - ship.Pos).Length();
                


                #region harmless timing
                if (birth == TimeSpan.Zero)
                {
                    birth = gameTime.TotalGameTime;
                }

                lifeSpan = gameTime.TotalGameTime - birth;

                if (lifeSpan > harmlessTime)
                {
                    Harmless = false;
                    Invincible = false;
                    Frozen = false;
                }
                #endregion harmless timing

                #region follow rbc
                // Check the RedBloodCells to see if any are closer than the ship and within
                // attack range.
                List<Sprite> rbcs = This.Game.CurrentLevel.GetSpritesByType("RedBloodCell");
                if (Frozen == false)
                {
                    foreach (Sprite sp in rbcs)
                    {
                        if (sp != null && (sp as RedBloodCell).Wounded)
                        {
                            float newLength = (Pos - sp.Pos).Length();
                            if (newLength < minDistance && newLength < attackDistance)
                            {
                                minDistance = newLength;
                                minVector = sp.Pos;
                            }
                        }
                    }
                }
                #endregion follow rbc

                #region follow ship
                Vector2 dirToShip = minVector - Pos;

                dirToShip.Normalize();
                dirToShip *= movementSpeed;

                if (Frozen == false)
                {
                    currentVelocity += dirToShip;

                    Pos += currentVelocity;

                    currentVelocity *= 0.15f;
                }
                #endregion follow ship

                #region collision checking
                if (Collision.collisionData.Count > 0)
                {
                    foreach (CollisionObject co in this.GetCollision())
                    {
                        if (Collision.collisionData.ContainsKey(this))
                        {
                            foreach (Tuple<CollisionObject, WorldObject, CollisionObject> collision in Collision.collisionData[this])
                            {
                                if (collision.Item2.GetType() == typeof(RedBloodCell))
                                {
                                    if ((collision.Item2 as RedBloodCell).Wounded)
                                    {
                                        // Delete self!
                                        This.Game.CurrentLevel.RemoveSprite(this);
                                        // infect rbc (done in RedBloodCell.cs)
                                    }
                                }

                                if (collision.Item2.GetType() == typeof(Bullet) && (Invincible == false))
                                {
                                    This.Game.CurrentLevel.RemoveSprite(this);
                                    This.Game.CurrentLevel.EnemiesDefeated++;
                                }


                            }
                        }
                    }
                }
                #endregion collision checking
            }

        #endregion
        }
       }
}
