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
            IsDead = false;

            birth = TimeSpan.Zero;
        }

        #region Properties
        // Virus is Harmless for very small amount of time after spawn.
        // To allow player to get away from being killed by a spawning enemy
        public bool Harmless { get; set; }
        public bool Invincible { get; set; }
        public bool Frozen { get; set; }
        public bool IsDead { get; set; }

        #endregion Properties

        #region Variables

        private Vector2 currentVelocity;

        private float movementSpeed = 2;

        private TimeSpan lifeSpan;

        private TimeSpan harmlessTime = new TimeSpan(0, 0, 0, 0, 500);

        private TimeSpan timeOfDeath;

        private TimeSpan explosionLength = new TimeSpan(0, 0, 0, 0, 250);

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
                        if (sp != null && (sp as RedBloodCell).Wounded && !(sp as RedBloodCell).Infected)
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

                #region explosion animation removal
                if ((gameTime.TotalGameTime >= explosionLength + timeOfDeath) && IsDead)
                {

                    This.Game.CurrentLevel.EnemiesDefeated++;
                    This.Game.CurrentLevel.RemoveSprite(this);
                }
                #endregion explosion animation removal

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
                                    if ((collision.Item2 as RedBloodCell).Wounded && !(collision.Item2 as RedBloodCell).Infected)
                                    {
                                        This.Game.CurrentLevel.RemoveSprite(this);
                                    }
                                }

                                if (collision.Item2.GetType() == typeof(Bullet))
                                {
                                    if (!IsDead && !Invincible)
                                    {
                                        // virus is dead, snapshot time of death
                                        Invincible = true;
                                        Harmless = true;

                                        timeOfDeath = gameTime.TotalGameTime;
                                        IsDead = true;
                                    }

                                    if (IsDead)
                                    {
                                        // change the animation if the virus is dead
                                        mActor.Frame = 0;
                                        mActor.CurrentAnimation = 1;
                                    }

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
