using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game {
    public class AnimatedSprite {
        public Texture2D texture;
        public Vector2Int frameSize;
        public int frameAmt;
        public int frameCurrent = 0;
        public float speedInit;
        public float speed;
        public int tick = 0;

        public AnimatedSprite(Texture2D texture, Vector2Int frameSize, float speedInit) {
            this.texture = texture;
            this.frameSize = frameSize;
            this.speedInit = speedInit;
            speed = speedInit;
            frameAmt = texture.Width / frameSize.x;
            tick = 0;
        }

        public Rectangle GetRect() {
            //int t = tick;

            if (frameSize.x == texture.Width) {
                return new Rectangle(0, 0, texture.Width, texture.Height);
            }
            if (speed != 0) {

                // Sprite Speed Management
                tick += (int)speed;
                int tickspd = tick % 60 / (int)speed;
                //float tickspd = (tick % (speed));//+Game1.FRAME_RATE/speed));
                // Update current frame
                if (tickspd == 0) frameCurrent++;
                if (frameCurrent == frameAmt) frameCurrent = 0;
                //Util.Log(tickspd.ToString());
            }


            // This will be the drawn portion of the sprite
            return new Rectangle((int)(frameSize.x * frameCurrent), 0, frameSize.x, frameSize.y);
        }

    }
}
