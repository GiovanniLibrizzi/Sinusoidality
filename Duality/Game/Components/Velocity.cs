using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game {
    class Velocity : Component {

        public Vector2 velocity = Vector2.Zero;

        public Velocity(Vector2 velocity) {
            this.velocity = velocity;
            VelocitySystem.Register(this);
        }

        /*        public virtual void Update(GameTime gameTime) {
                    Velocity v = entity.GetComponent<Velocity>();

                }*/
    }
}
