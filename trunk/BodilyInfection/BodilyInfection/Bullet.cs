using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BodilyInfection
{
    class Bullet : Sprite
    {
        public Bullet(string name, Actor actor)
            : this(name, actor, new Vector2(3.0f, 3.0f))
        {
            Sprite ship = This.Game.CurrentLevel.GetSprite("ship");
            Pos = ship.Pos;

            // Get the game pad state.
            currentState = GamePad.GetState(gamepad);

            // Save X and Y values of thumbstick at the time of bullet creation
            rThumbstick.X = currentState.ThumbSticks.Right.X;
            rThumbstick.Y = currentState.ThumbSticks.Right.Y;

        }

        public Bullet(string name, Actor actor, Vector2 velocity)
            : base(name, actor)
        {
            movementVelocity = velocity;
            UpdateBehavior += new Behavior(Update);
        }

        private Vector2 movementVelocity;
        private PlayerIndex gamepad;
        GamePadState currentState;
        Vector2 rThumbstick;

        public void Update()
        {
            if (currentState.IsConnected)
            {
                // Fire in straight line from position of ship when bullet was created. (scaled by velocity)
                Pos.X += movementVelocity.X * rThumbstick.X;
                Pos.Y += movementVelocity.Y * rThumbstick.Y;
            }

            if (Collision.collisionData.Count > 0)
            {
                foreach (CollisionObject co in this.GetCollision())
                {
                    if (Collision.collisionData.ContainsKey(this))
                    {
                        foreach (Tuple<CollisionObject, WorldObject, CollisionObject> collision in Collision.collisionData[this])
                        {
                            if (collision.Item2.GetType() == typeof(Virus) ||
                                collision.Item2.GetType() == typeof(RedBloodCell))
                            {
                                // Bullet disappears, it's hit something
                                throw new NotImplementedException();
                            }
                        }
                    }
                }
            }
        }   
    }
}
