using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game.Entities {
    class Spike : Actor {

        private int tick = 0;
        private Player.Side side;
        private float yFloor;

        private bool collected = false;
        

        public Spike(Texture2D texture, Vector2 position, World world, Player.Side side) : base(texture, position, world) {
            collisionBox = new Rectangle(0, 1, 2, 5);

            this.side = side;
            //Util.Log(position.ToString());
            this.position = position;

            if (side == Player.Side.Bottom) {
                sprite.Scale(new Vector2(1f, -1f));
                sprite.color = Color.Black;
                collisionBox = new Rectangle(1, 15, 2, 7);


            } else {
                sprite.color = Color.White;
            }
            //sprite.color = Color.Red;

        }



        public override void Update(GameTime gameTime) {
            Player playerTouching = (Player)IsTouching(typeof(Player));
            if (playerTouching != null) {
                if (playerTouching.side == side) {
                    if (playerTouching.side == Player.Side.Top && playerTouching.velocity.Y >= 0 || playerTouching.side == Player.Side.Bottom && playerTouching.velocity.Y <= 0) {
                        playerTouching.StateGoto(Player.pState.Hit);
                        if (!world.destroyed) {
                            Game1.PlaySound(Game1.sfx.death, .6f, 0f, world.noAudio);
                            world.destroyed = true;
                            if (!world.noAudio) {
                                MediaPlayer.Stop();
                            }
                            world.ClearSpikes();
                        }
                    }
                    
                    
                }
            }

            if (side == Player.Side.Top) {
                if (!collected && (world.playerTop.position.Y < position.Y && world.playerTop.position.X <= position.X)) {
                    collected = true;
                    world.playerTop.AddPoints(1);
                }
            } else {
                if (!collected && (world.playerBottom.position.Y > position.Y && world.playerBottom.position.X <= position.X)) {
                    collected = true;
                    world.playerBottom.AddPoints(1);
                }
            }

           
            //Util.Log(yFloor.ToString());
            if (tick == 0) {
                position.X = -10;
                yFloor = world.wave.WaveEqn((world.wave.XOffset((int)position.X))) - 2;
            }
            position.X += (world.wave.realMod);
            //position.X = (float)Math.Round(position.X);


            //position.X -= .25f;
            //Util.Log(world.wave.coords.Length.ToString());
            //if (position.X >= 0 && tick != 0)
            //yFloor = world.wave.WaveEqn(((int)position.X) + 2) - 4;

            if (side == Player.Side.Top) {
                position.Y = yFloor - 7 - (world.level * 2f);
            } else {
                position.Y = yFloor - 14 + (world.level * 2f);
            }



            if (position.X > Game1.SCREEN_WIDTH+24) {
                Remove();
            }
            transform.position = position;
            tick++;
            //Util.Log(position.X.ToString());

        }
    }
}
