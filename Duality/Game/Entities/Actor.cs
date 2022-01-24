using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Game.Entities {
    class Actor : Entity {
        public Vector2 position;
        public Texture2D texture;
        public Transform transform;
        public Vector2 velocity;
        public Sprite sprite;
        public float jspd;
        public float gravity = 0.13f;
        public float gravityDef = 0.13f;

        public Rectangle collisionBox;


        public bool touchingGround;

        public enum Dir {
            Right = 1,
            Left = -1,
        }
        public Dir direction;


        public Actor(Texture2D texture, Vector2 position, World world) : base(world) {

            this.texture = texture;

            transform = new Transform(position);
            AddComponent(transform);

            sprite = new Sprite(texture);
            AddComponent(sprite);
        }


        // Moves actor downwards
        protected void Gravity(Player.Side side) {
            if (side == Player.Side.Top) {
                if (velocity.Y < 10) {
                    velocity.Y += gravity;
                }
            } else {
                //if (velocity.Y > 10) {
                    velocity.Y -= gravity;
                //}
            }
        }

        protected void Jump(float jspd) {
            //Game1.PlaySound(Game1.sfx.jump, 1f, 0f, world.noAudio);
            velocity.Y = jspd;
            touchingGround = false;
        }


        protected Entity IsTouching(Type entity) {
            //entity.GetType();
            Vector2 posA = position;
            Rectangle boxA = collisionBox;
            foreach (Entity e in world.scene.OfType<Entity>().ToArray()) {

                if (entity.Equals(e.GetType())) {
                    Vector2 posB = e.GetComponent<Transform>().position;
                    Rectangle boxB = e.GetComponent<Sprite>().texture.Bounds;
                    //if (e.GetType() == typeof(Gold)) {
                    //    Gold a = (Gold)e;
                    //    boxB = a.collisionBox;
                    //}
                    if (posA.X + boxA.Left < posB.X + boxB.Right &&
                           posA.X + boxA.Right > posB.X + boxB.Left &&
                           posA.Y + boxA.Top < posB.Y + boxB.Bottom &&
                           posA.Y + boxA.Bottom > posB.Y + boxB.Top) {
                        return e;
                    }
                }
            }
            return null;
        }

        public static bool Colliding(Vector2 posA, Rectangle boxA, Vector2 posB, Rectangle boxB) {
            return (posA.X < posB.X + boxB.Width &&
                    posA.X + boxA.Width > posB.X &&
                    posA.Y < posB.Y + boxB.Height &&
                    posA.Y + boxA.Height > posB.Y);
        }


        }
}
