using Duality.Game.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Duality.Game {
    class World {

        public List<Entity> scene = new List<Entity>();

        public int tick = 0;

        public Wave wave;
        public Player playerTop;
        public Player playerBottom;

        private Random r = new Random();
        public int random;

        private int spikeCooldownTime = 60;
        private int spikeCooldownLimit = 20;
        private int spikeCooldownCount = -1;
        private int randomChance = 120;
        private int randomChanceLimit = 40;

        public int totalPoints = 0;

        public bool destroyed = false;

        public int level = 0;
        public bool noAudio;




        public World() {
            //Spike spike = new Spike(Game1.sSpike, new Vector2(200f, 0), this, Player.Side.Top);
            //Add(spike);
            tick = 0;
        }

        public virtual void Update(GameTime gameTime) {

            totalPoints = playerTop.GetPoints() + playerBottom.GetPoints();
            random = r.Next(0, randomChance);


            if (random == 5 && spikeCooldownCount < 0) {
                Spike spike = new Spike(Game1.sSpike, new Vector2(200f, 0), this, Player.Side.Top);
                Add(spike);
                spikeCooldownCount = spikeCooldownTime;
            }
            if (random == 6 && spikeCooldownCount < 0) {
                Spike spike = new Spike(Game1.sSpike, new Vector2(200f, 0), this, Player.Side.Bottom);
                Add(spike);
                spikeCooldownCount = spikeCooldownTime;
            }

            if (spikeCooldownCount >= 0)
                spikeCooldownCount--;

            // Util.Log(spikeCooldownCount.ToString());
            if (tick > 0) {
                if (tick % 450 == 0) {
                    if (randomChance > randomChanceLimit)
                        randomChance -= 5;
                    if (spikeCooldownTime > spikeCooldownLimit)
                        spikeCooldownTime -= 3;

                    Util.Log("Ramp up chances");

                    //if (wave.tickMod > 0.3f)
                    //wave.tickMod -= 0.05f;
                }

                if (tick % 1350 == 0) {
                    wave.tickMod += 0.25f;
                    ClearSpikes();
                    level++;
                    Game1.PlaySound(Game1.sfx.speedup, 0.4f, 0f, noAudio);
                    Util.Log("Ramp up speeds");
                }
            }

            if (destroyed) {
                wave.amplitude += 0.3f;
                wave.frequency -= 0.005f;
                //wave.tickMod += 0.01f;
            }

            tick++;
        }

        public void ClearSpikes() {
            foreach (Entity entity in scene.ToArray()) {

                if (entity.GetType() == typeof(Spike)) {
                    entity.Remove();
                }
            }
        }

        //public void Add(Player player) {
        //    scene.Add(player);
        //    player.position.X = playerSpawn.x;
        //    player.position.Y = playerSpawn.y;
        //    player.direction = dir;
        //}
        //public void Add(Actor actor) {
        //    scene.Add(actor);
        //}
        public void Add(Entity entity) {
            scene.Add(entity);
        }
    }
}
