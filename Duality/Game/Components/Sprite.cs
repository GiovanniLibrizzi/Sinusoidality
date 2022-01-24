//using Duality.Game.Entities.Solids;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duality.Game {
    class Sprite : Component {
        public Texture2D texture { get; set; }
        public Vector2 scale = new Vector2(1f, 1f);
        public List<AnimatedSprite> spriteList;
        public int spritePrevious = -1;
        public int spriteCurrent;
        public AnimatedSprite animatedSprite;
        public SpriteEffects spriteEffect;
        public int tick = new int();
        public bool visible = true;
        public Color color = Color.White;
        //private static int[] numWidth = { 7, 7, 7, 7, 8, 7, 7, 7, 7, 7 };


        public Sprite(Texture2D texture) {
            this.texture = texture;
            SpriteSystem.Register(this);
        }

        public Sprite(AnimatedSprite animatedSprite) {
            this.texture = animatedSprite.texture;
            this.animatedSprite = new AnimatedSprite(animatedSprite.texture, animatedSprite.frameSize, animatedSprite.speedInit);
            SpriteSystem.Register(this);
        }

        public Sprite(List<AnimatedSprite> spriteList, int spriteCurrent) {
            this.spriteList = spriteList;
            this.spriteCurrent = spriteCurrent;
            animatedSprite = spriteList[spriteCurrent];
            texture = animatedSprite.texture;

            SpriteSystem.Register(this);
        }


        public void Scale(Vector2 sc) {
            scale = new Vector2(Math.Abs(sc.X), Math.Abs(sc.Y));

            // Flip if scale is less than 0
            if (sc.X < 0) {
                spriteEffect = SpriteEffects.FlipHorizontally;
            } else if (sc.Y < 0) {
                spriteEffect = SpriteEffects.FlipVertically;
            } else {
                spriteEffect = SpriteEffects.None;
            }

        }

        public static void DrawNumber(SpriteBatch spriteBatch, Texture2D texture, int number, Vector2 position, Color color) {
            int width = texture.Width / 10;
            int[] digits = number.ToString().Select(t => int.Parse(t.ToString())).ToArray();
            for (int i = 0; i < digits.Length; i++) {
                Rectangle rect = new Rectangle(width * digits[i], 0, width, texture.Height);
                int xGap = 5;//numWidth[digits[i]];// + 1;
                //if (texture == Game1.sNumbersSmall) {
                //    xGap -= 1;
                //}
                spriteBatch.Draw(texture, new Vector2(position.X + (i * xGap), position.Y), rect, color);

            }
        }



        public virtual void Draw(SpriteBatch spriteBatch) {
            tick++;
            Transform t = entity.GetComponent<Transform>();
            // Resets frame count if changing sprites
            if (visible) {
                if (spriteList != null) {
                    if (spritePrevious != spriteCurrent) {
                        animatedSprite = spriteList[spriteCurrent];
                        animatedSprite.speed = spriteList[spriteCurrent].speedInit;
                        animatedSprite.frameCurrent = 0;
                    }
                }

                if (animatedSprite != null) {
                    spriteBatch.Draw(animatedSprite.texture, t.position, animatedSprite.GetRect(), color, 0f, Vector2.Zero, scale, spriteEffect, 0f);
                } else if (texture != null) {
                    spriteBatch.Draw(texture, t.position, null, color, 0f, Vector2.Zero, scale, spriteEffect, 0f);
                }
                //Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale
            }
            spritePrevious = spriteCurrent;
        }

    }
}
