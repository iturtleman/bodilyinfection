﻿using System;
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
        }

        #region Variables

        private Vector2 currentVelocity;

        private float movementSpeed = 2;

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

                // Check the RedBloodCells to see if any are closer than the ship and within
                // attack range.
                List<Sprite> rbcs = This.Game.CurrentLevel.GetSpritesByType("RedBloodCell");
                foreach (Sprite sp in rbcs)

                {
                    float newLength = (Pos - sp.Pos).Length();
                    if (newLength < minDistance && newLength < attackDistance)
                    {
                        minDistance = newLength;
                        minVector = sp.Pos;
                    }
                }

                Vector2 dirToShip = minVector - Pos;

                // Find out what direction we should be thrusting, 
                // using rotation and scale by speed.
                dirToShip.Normalize();
                dirToShip *= movementSpeed;

                // Finally, add this vector to our velocity.
                currentVelocity += dirToShip;

                Pos += currentVelocity;

                // Bleed off velocity over time.
                currentVelocity *= 0.15f;

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
                                        // infect rbc
                                    }
                                }

                                if (collision.Item2.GetType() == typeof(Bullet))
                                {
                                    // Delete self!
                                    // Kill Virus.
                                    This.Game.CurrentLevel.RemoveSprite(this);
                                }
                            }
                        }
                    }
                }
            }

        #endregion
        }
       }
}