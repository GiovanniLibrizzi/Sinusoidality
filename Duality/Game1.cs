using Duality.Game;
using Duality.Game.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Duality {
    public class Game1 : Microsoft.Xna.Framework.Game {
        // Rendering and resolution
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public const int SCREEN_WIDTH = 320;
        public const int SCREEN_HEIGHT = 180;
        public const int FRAME_RATE = 60;
        int backbufferWidth, backbufferHeight;
        private Matrix globalTransformation;

        public static int wScr = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int hScr = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;


        Rectangle CANVAS = new Rectangle(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);

        public static int GridSize = 16;
        public static int ScreenWidth = SCREEN_WIDTH * 3;
        public static int ScreenHeight = SCREEN_HEIGHT * 3;
        private RenderTarget2D renderTarget;

        bool noAudio = false;


        // Textures
        private Texture2D pixel;
        private Texture2D circle;
        public static Texture2D sSpike;
        public static Texture2D sNumbers;

        // hud text
        private Texture2D sEndTitle;
        private Texture2D sEndScore;
        private Texture2D sEndHigh;
        private Texture2D sEndSpace;
        private Texture2D sEndNewhigh;

        private Texture2D sTitle;

        // Sounds
        public static List<SoundEffect> soundEffects;
        public enum sfx {
            jumpUp,
            jumpDown,
            confirm,
            death,
            title,
            speedup,
            click
        }
        Song song;

        PointsManager pointsManager;



        //Objects
        World world;
        Wave wave;

        Player playerTop;
        Player playerBottom;


        // Variables
        public int tick;
        float delta;

        int[] scale = { 1, 3, 4, 5, 6, -1 };
        int scaleInc = 0;

        float fadeAlpha = 0;
        float fadeAlphaBlack = 0;
        bool fadeOut = false;
        bool fadeIn = false;
        float fadeOutSpd = 0.07f;
        float fadeInSpd = 0.02f;
        int waitStart;
        bool toGame = false;
        bool newHighScore = false;

        public int highScore = 0;

        // Game state
        public enum GameState {
            TitleScreen,
            InGame,
            EndScreen,
        }
        public GameState state = GameState.TitleScreen;



        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        public void Resolution(int w, int h, bool b) {
            graphics.PreferredBackBufferWidth = w;
            graphics.PreferredBackBufferHeight = h;
            ScreenWidth = w;
            ScreenHeight = h;
            graphics.IsFullScreen = b;
            graphics.ApplyChanges();
        }

        protected override void Initialize() {
            // TODO: Add your initialization logic here

            Window.Title = "Sinusoidality";

            base.Initialize();
        }

        protected override void LoadContent() {
            renderTarget = new RenderTarget2D(GraphicsDevice, CANVAS.Width, CANVAS.Height);

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = new Texture2D(GraphicsDevice, 1, 1);


            // TODO: use this.Content to load your game content here

            // Sprites
            pixel = Content.Load<Texture2D>("pixel");
            circle = Content.Load<Texture2D>("circle");
            sSpike = Content.Load<Texture2D>("spike");
            sNumbers = Content.Load<Texture2D>("s_numbers");

            // end text
            sEndTitle = Content.Load<Texture2D>("s_end_title");
            sEndScore = Content.Load<Texture2D>("s_end_score");
            sEndHigh = Content.Load<Texture2D>("s_end_high");
            sEndSpace = Content.Load<Texture2D>("s_end_space");
            sEndNewhigh = Content.Load<Texture2D>("s_end_newhigh");

            sTitle = Content.Load<Texture2D>("s_title");



            // Audio
            try {
                soundEffects = new List<SoundEffect>() {
                    Content.Load<SoundEffect>("snd_jump_up"),
                    Content.Load<SoundEffect>("snd_jump_down"),
                    Content.Load<SoundEffect>("snd_confirm"),
                    Content.Load<SoundEffect>("snd_death"),
                    Content.Load<SoundEffect>("snd_title"),
                    Content.Load<SoundEffect>("snd_speedup"),
                    Content.Load<SoundEffect>("snd_click")
                };


                song = Content.Load<Song>("sinusoundal");
                PlaySound(sfx.title, 1f, 0f, noAudio);
                //MediaPlayer.Play(song);
                //MediaPlayer.IsRepeating = true;
                //MediaPlayer.Volume = 0.5f;
                
                MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
            } catch (Microsoft.Xna.Framework.Audio.NoAudioHardwareException) {
                noAudio = true;
            }
            pointsManager = new PointsManager();
            pointsManager = PointsManager.Load();



            ScalePresentationArea();
        }

        public void ToGame() {

            state = GameState.InGame;
            world = new World();

            wave = new Wave(world);
            world.wave = wave;
            world.Add(wave);

            playerTop = new Player(circle, new Vector2(0,0), Player.Side.Top, world);
            world.playerTop = playerTop;
            world.Add(playerTop);

            playerBottom = new Player(circle, new Vector2(0,0), Player.Side.Bottom, world);
            world.playerBottom = playerBottom;
            world.Add(playerBottom);
            world.noAudio = noAudio;

            if (!noAudio) {
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = 1f;
            }
            newHighScore = false;

        }
        public static void PlaySound(sfx sound, float vol, float pitch, bool noAudio) {
            if (!noAudio) {
                soundEffects[(int)sound].Play(vol, pitch, 0f);
            }
        }
        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e) {
            // 0.0f is silent, 1.0f is full volume
            //MediaPlayer.Play(song);
        }


        public void ScalePresentationArea() {
            //Work out how much we need to scale our graphics to fill the screen
            backbufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            backbufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            float horScaling = backbufferWidth / SCREEN_WIDTH;
            float verScaling = backbufferHeight / SCREEN_HEIGHT;
            Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);
            globalTransformation = Matrix.CreateScale(screenScalingFactor);
        }

        protected override void Update(GameTime gameTime) {
            Input.GetState();
            tick++;

            delta = (float)gameTime.TotalGameTime.TotalSeconds*1000;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit(); 

            if (Input.keyPressed(Keys.F)) {
                if (scaleInc < scale.Length - 1) {
                    scaleInc++;
                } else {
                    scaleInc = 0;
                }
                int scaleCur = scale[scaleInc];
                if (scaleCur > 0) {
                    Resolution(SCREEN_WIDTH * scaleCur, SCREEN_HEIGHT * scaleCur, false);
                } else {
                    graphics.HardwareModeSwitch = false;
                    Resolution(wScr, hScr, true);
                }
            }

            switch (state) {
                case GameState.TitleScreen:
                    if (Input.keyPressed(Input.Reset)) {
                        toGame = true;
                        PlaySound(sfx.confirm, 1f, 0f, noAudio);
                    }
                    break;

                case GameState.InGame:

                    if (world != null) {
                        world.Update(gameTime);
                        foreach (Entity entity in world.scene.ToArray()) {
                            entity.Update(gameTime);
                            foreach (Component component in entity.components) {
                                component.Update(gameTime);
                            }
                        }

                    }

                    

                    //if (Input.keyPressed(Input.Reset)) {
                    //    toGame = true;
                    //}
                    break;

                case GameState.EndScreen:
                    if (Input.keyPressed(Input.Reset)) {
                        tick += 40 * 6;
                        toGame = true;
                        PlaySound(sfx.confirm, 1f, 0f, noAudio);

                    }
                    break;
            }


            if (toGame) {
                fadeOut = true;
                if (fadeAlphaBlack > .995f) {
                    fadeOut = false;
                    ToGame();
                    fadeIn = true;
                    toGame = false;
                }
            }

            if (fadeOut) {
                fadeAlphaBlack = Util.Lerp(fadeAlphaBlack, 1f, fadeOutSpd);
                if (fadeAlphaBlack >= .993f) {
                    fadeAlphaBlack = 1f;
                    fadeOut = false;
                }
            }
            if (fadeIn) {
                fadeAlphaBlack = Util.Lerp(fadeAlphaBlack, 0f, fadeInSpd);
                if (fadeAlphaBlack <= 0.001f) {
                    fadeAlphaBlack = 0;
                    fadeIn = false;
                }
            }

            
            //if (world.totalPoints > pointsManager.GetHighScore()) {
            //    highScore = world.totalPoints;
            //    pointsManager.Update(highScore);
            //    PointsManager.Save(pointsManager);
            //} else {
            //    highScore = pointsManager.GetHighScore();
            //}

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);//, transformMatrix: _camera.Transform);


            

            switch (state) {
                case GameState.TitleScreen:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Draw(sTitle, new Vector2(0, 0), Color.White);
                    break;

                case GameState.InGame:
                    if (wave != null) {
                        wave.DrawWave(spriteBatch, pixel);
                    }


                    if (world != null) {
                        List<Player> players = new List<Player>();
                        foreach (Entity entity in world.scene) {
                            if (entity.HasComponent<Sprite>()) {
                                entity.GetComponent<Sprite>().Draw(spriteBatch);
                                //spriteBatch.Draw(entity.GetComponent<Sprite>().texture, entity.GetComponent<Transform>().position, Color.White);
                            }
                            if (entity.GetType() == typeof(Player)) {
                                players.Add((Player)entity);
                            }
                        }

                        // draws players on top
                        foreach (Player p in players) {
                            p.GetComponent<Sprite>().Draw(spriteBatch);
                        }
                        if (!world.destroyed) {
                            // point numbers
                            Sprite.DrawNumber(spriteBatch, sNumbers, world.playerTop.GetPoints(), new Vector2(playerTop.position.X + 2, playerTop.position.Y + 5), Color.Black);
                            Sprite.DrawNumber(spriteBatch, sNumbers, world.playerBottom.GetPoints(), new Vector2(playerBottom.position.X + 2, playerBottom.position.Y + 5), Color.White);
                        } else {
                            fadeAlpha = Util.Lerp(fadeAlpha, 1f, 0.02f);
                            spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), Color.White * fadeAlpha, 0f, new Vector2(0, 0), new Vector2(320, 180), SpriteEffects.None, 0f);

                            if (fadeAlpha > 0.993f) {
                                fadeAlpha = 1f;
                                waitStart = tick;
                                //if (world.totalPoints > highScore) {


                                    
                                    if (world.totalPoints > pointsManager.GetHighScore()) {
                                        highScore = world.totalPoints;
                                        pointsManager.Update(highScore);
                                        PointsManager.Save(pointsManager);
                                        newHighScore = true;
                                    } else {
                                        highScore = pointsManager.GetHighScore();
                                    }


                                
                                state = GameState.EndScreen;

                            }
                        }
                    }
                    

                    break;

                case GameState.EndScreen:
                    //List<Texture2D> endTex = new List<Texture2D>() { sEndTitle, sEndScore, }
                    GraphicsDevice.Clear(Color.White);
                    
                    fadeAlpha = 0;
                    int delay = 40;

                    // This is stupidly redundant but I am not feeling like making it look pretty :/
                    if (tick > waitStart && tick < waitStart + (delay * 6)+1) {
                        if ((tick-waitStart) % delay == 0) {
                            PlaySound(sfx.click, 1f, 0f, noAudio);
                        }
                    }
                    if (tick > waitStart+ delay) {
                        spriteBatch.Draw(sEndTitle, new Vector2(133, 49), Color.White);
                    }
                    if (tick > waitStart + delay * 2) {
                        spriteBatch.Draw(sEndScore, new Vector2(112, 71), Color.White);
                    }
                    if (tick > waitStart + delay * 3) {
                        Sprite.DrawNumber(spriteBatch, sNumbers, world.totalPoints, new Vector2(166, 71), Color.Black);
                    }
                    if (tick > waitStart + delay * 4) {
                        spriteBatch.Draw(sEndHigh, new Vector2(109, 88), Color.White);
                    }
                    if (tick > waitStart + delay * 5) {
                        Sprite.DrawNumber(spriteBatch, sNumbers, highScore, new Vector2(166, 88), Color.Black);
                    }

                    if (newHighScore) {
                        if (tick > waitStart + delay * 6) {
                            float ySpace = (float)Math.Sin((tick * .085f)) * 1;
                            spriteBatch.Draw(sEndNewhigh, new Vector2(211+ySpace, ySpace + 71), Color.White);
                            if ((tick - waitStart) == (delay * 6) + 1)
                                PlaySound(sfx.confirm, 1f, 0.25f, noAudio);
                        }
                        if (tick > waitStart + delay * 7) {
                            float ySpace = (float)Math.Sin((tick * .07f)) * 1;
                            spriteBatch.Draw(sEndSpace, new Vector2(146, ySpace + 108), Color.White);
                        }
                    } else {
                        if (tick > waitStart + delay * 6) {
                            float ySpace = (float)Math.Sin((tick * .07f)) * 1;
                            spriteBatch.Draw(sEndSpace, new Vector2(146, ySpace + 108), Color.White);
                        }
                    }


                    break;
            }

            //Util.Log(fadeIn.ToString());
            spriteBatch.Draw(pixel, new Vector2(0, 0), new Rectangle(0, 0, 1, 1), Color.Black * fadeAlphaBlack, 0f, new Vector2(0, 0), new Vector2(320, 180), SpriteEffects.None, 0f);


            spriteBatch.End();

                
            // Don't mess with
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
