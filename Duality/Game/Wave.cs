using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game {
    class Wave : Entity {
        private float origin = Game1.SCREEN_HEIGHT / 2;
        private float maxFreq = 0.0196349f;
        public float frequency = 0.05f;
        public float amplitude = 10.0f;
        public int pxWavelength;
        public int waveAmt = 1;
        private int tick = 0;
        public float tickMod = 1f;

        public float realMod = 1f;



        public Wave(World world) : base(world) {
            pxWavelength = (int)((2 * Math.PI) / frequency);
            waveAmt = (int)Math.Floor((decimal)(Game1.SCREEN_WIDTH / pxWavelength))+1;
        }

        public override void Update(GameTime gameTime) {
            realMod = Util.Lerp(realMod, tickMod, 0.07f);
            tick++;
        }


        public float WaveEqn(int x) {
            return (float)Math.Sin((((x + (tick* realMod)) % (pxWavelength * waveAmt)) * frequency)) * amplitude + origin;
        }

        private float WaveEqn(int x, float frequency, float amplitude) {
            return (float)Math.Sin((x * frequency)) * amplitude + origin;
        }


        public int XOffset(int x) {
            return (int)((x+((2*pxWavelength)/Math.PI)) % (pxWavelength * waveAmt) );
        }

   
        public void DrawWave(SpriteBatch spriteBatch, Texture2D pixel) {
            //spriteBatch.Draw(pixel, new Vector2(320/2, (float)Math.Sin(time/20)*10+180/2), pixel.Bounds, Color.Green);
           

            Rectangle waveRect = new Rectangle(0, 0, 1, 180);
            for (int i = 0; i < pxWavelength*waveAmt; i++) {
                
                float xOffset = ((i + (tick* realMod)) % (pxWavelength*waveAmt));
                float ySin = WaveEqn(i, frequency, amplitude);
                
                spriteBatch.Draw(pixel, new Vector2(xOffset, ySin), waveRect, Color.White);
            }
            //Util.Log(pxWavelength.ToString());
        }
    }
}
