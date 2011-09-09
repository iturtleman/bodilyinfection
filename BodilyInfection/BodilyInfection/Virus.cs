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

        public void Update()
        {
            Sprite ship = This.Game.CurrentLevel.GetSprite("rbc");
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
        }

        #endregion
    }
}
