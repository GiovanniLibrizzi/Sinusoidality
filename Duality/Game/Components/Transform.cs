using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game {
    class Transform : Component {

        public Vector2 position = Vector2.Zero;

        public Transform(Vector2 position) {
            this.position = position;
            TransformSystem.Register(this);
        }

        public void AddVelocity(Vector2 v) {
            position.X += v.X;
            position.Y += v.Y;
        }
    }
}
