using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game.Entities {
    class SolidTexture : Solid {
        Texture2D texture;
        public Sprite sprite;
        public SolidTexture(Texture2D texture, Vector2 position, World world) : base(position, world) {

            this.texture = texture;
            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            transform = new Transform(position);
            AddComponent(transform);

            sprite = new Sprite(texture);
            AddComponent(sprite);
        }
    }
}
