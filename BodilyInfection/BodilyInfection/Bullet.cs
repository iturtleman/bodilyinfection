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
        }

        public Bullet(string name, Actor actor, Vector2 velocity)
            : base(name, actor)
        {
            movementVelocity = velocity;
            UpdateBehavior += new Behavior(Update);
        }

        private Vector2 movementVelocity;

        public void Update(GameTime gameTime)
        {
            // Fire in straight line from position of ship when bullet was created. (scaled by velocity)
            Pos.X += movementVelocity.X;
            Pos.Y += movementVelocity.Y;

            if (Pos.X > This.Game.GraphicsDevice.Viewport.Width ||
                Pos.Y > This.Game.GraphicsDevice.Viewport.Height ||
                Pos.X < 0 ||
                Pos.Y < 0)
            {
                This.Game.CurrentLevel.RemoveSprite(this);
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
                                // Do some damage!
                                This.Game.CurrentLevel.RemoveSprite(this);
                                // Somehow it only deletes the bullet when the *ship* runs over something... no idea why.
                            }
                        }
                    }
                }
            }
        }   
    }
}
