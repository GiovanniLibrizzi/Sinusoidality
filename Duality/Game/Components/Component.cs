using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game {
    class Component {
        public Entity entity;
        public virtual void Update(GameTime gameTime) { }
    }
}
