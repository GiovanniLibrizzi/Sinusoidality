using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game.Entities {
    class Player : Actor {

        public enum Side {
            Top,
            Bottom,
        }

        public enum pState {
            Idle,
            Jump,
            Hit
        }

        private List<Input.Btn> sideBtn = new List<Input.Btn>() { Input.Up, Input.Down };
        private List<int> sideDir = new List<int>() { -1, 1 };
        private List<Color> sideColor = new List<Color>() { Color.White, Color.Black };
        private List<Game1.sfx> sideSfx = new List<Game1.sfx>() { Game1.sfx.jumpUp, Game1.sfx.jumpDown };



        public int tick = 0;
        public Side side;

        private pState statePrev = pState.Idle;
        private pState state = pState.Idle;

        private float yFloor;

        private int points = 0;

        public Player(Texture2D texture, Vector2 position, Side side, World world) : base(texture, position, world) {
            this.side = side;

            sprite.color = sideColor[(int)side];
           

            jspd = 3.6f;


        }

        public override void Update(GameTime gameTime) {
            // Initialize in runtime
            if (tick == 0) {
                position.X = (Game1.SCREEN_WIDTH - 64 ) - (texture.Width / 2);

            }
            int yOffset;
            int floorMargin = 5;

            if (side == Side.Top) { 
                yOffset = -texture.Height - floorMargin;
            } else {
                yOffset = floorMargin-1;
            }

            yFloor = world.wave.WaveEqn(world.wave.XOffset((int)position.X)-18) + yOffset;

            switch (state) {
                case pState.Idle:
                    position.Y = yFloor;
                    velocity.Y = 0f;

                    if (Input.keyPressed(sideBtn[(int)side]) || Input.keyDown(sideBtn[(int)side])) {
                        Jump(jspd*sideDir[(int)side]);
                        Game1.PlaySound(sideSfx[(int)side], 1f, 0f, world.noAudio);
                        StateGoto(pState.Jump);
                    }
                    touchingGround = true;
                    
                    
                    break;

                case pState.Jump:
                    Gravity(side);
                    // Variable jump height
                    if ((side == Side.Top && velocity.Y < 0) || (side == Side.Bottom && velocity.Y > 0)) {
                        if (!Input.keyPressed(sideBtn[(int)side])) {
                            velocity.Y = Util.Lerp(velocity.Y, 0, 0.02f);
                        }
                        if (!Input.keyDown(sideBtn[(int)side])) {
                            //Util.Log("RELEASED");
                            velocity.Y = Util.Lerp(velocity.Y, 0, 0.2f);
                        }
                    }
                    if ((side == Side.Top && position.Y >= yFloor-1) || (side == Side.Bottom && position.Y <= yFloor+1)) {
                        StateGoto(pState.Idle);
                    }
                    
                    break;

                case pState.Hit:
                    for (int i = 0; i < 4; i++) {
                        if (tick % 8 == i) {
                            sprite.color = Color.Red;
                        } else {
                            sprite.color = sideColor[(int)side];
                        }
                    }
                    if (tick % 4 == 0) {
                        sprite.color = Color.Red;
                    }
                    break;
            }



            //Gravity();

            position += velocity;

            transform.position = position;
            tick += 1;
        }

        public void StateGoto(pState newState) {
            tick = 0;
            statePrev = state;
            state = newState;
        }

        public void AddPoints(int amt) {
            points += amt;
        }
        public int GetPoints() {
            return points;
        }
    }
}
