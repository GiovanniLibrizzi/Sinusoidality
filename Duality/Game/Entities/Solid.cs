using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game.Entities {
    class Solid : Entity {
        public Vector2 position;
        public Transform transform;
        public Rectangle rectangle = new Rectangle();

        public Solid(Vector2 position, World world) : base(world) {
            this.position = position;

            transform = new Transform(position);
            AddComponent(transform);

        }
    }
}
