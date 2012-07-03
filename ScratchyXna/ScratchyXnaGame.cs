using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace ScratchyXna
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public abstract class ScratchyXnaGame : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// The singleton game object
        /// </summary>
        public static ScratchyXnaGame ScratchyGame;

        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public Dictionary<string, Costume> costumes = new Dictionary<string, Costume>();
        public Scene activeGameScene;
        public KeyboardInput KeyboardInput = new KeyboardInput();
        public MouseInput MouseInput = new MouseInput();
        public TouchInput TouchInput = new TouchInput();
        public float SpeedMultiplier = 100f;
        public Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>();
        public Dictionary<string, SoundEffect> Sounds = new Dictionary<string, SoundEffect>();
        public Dictionary<string, SoundEffectInstance> SoundInstances = new Dictionary<string, SoundEffectInstance>();
        public Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public GameTime gameTime = new GameTime();
        public Random Random = new Random();
        public PlayerData PlayerData = new PlayerData();

        /// <summary>
        /// Texture used for drawing lines
        /// </summary>
        public Texture2D LineTexture;

        /// <summary>
        /// What type of machine is the game running on
        /// </summary>
        public GamePlatforms Platform = GamePlatforms.Windows;

        /// <summary>
        /// Override to load all of the needed game screens
        /// </summary>
        public abstract void LoadScenes();

        /// <summary>
        /// Constructor
        /// </summary>
        public ScratchyXnaGame()
        {
            ScratchyGame = this;
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

#if WINDOWS_PHONE
            Platform = GamePlatforms.WindowsPhone;
#endif
#if XBOX360
            Platform = GamePlatforms.XBox;
#endif
        }

        /// <summary>
        /// Set the screen size
        /// </summary>
        /// <param name="width">Screen width in pixels</param>
        /// <param name="height">Screen height in pixels</param>
        public void SetScreenSize(float width, float height)
        {
            graphics.PreferredBackBufferHeight = (int)height;
            graphics.PreferredBackBufferWidth = (int)width;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected sealed override void Initialize()
        {
            TouchInput.Init();
            KeyboardInput.Init();
            MouseInput.Init();
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected sealed override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            LineTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            LineTexture.SetData(new[] { new Color(.9f, .9f, .9f, .1f) });

            // TODO: use this.Content to load your game content here

            LoadScenes();
            foreach (var scene in Scenes)
            {
                scene.Value.Init(this);
            }
            if (activeGameScene == null)
            {
                Scenes.First().Value.ShowScene();
            }
        }

        /// <summary>
        /// Add a new scene to the game and give it a name
        /// </summary>
        /// <param name="name">Name of the scene</param>
        public T AddScene<T>(string name) where T : Scene, new()
        {
            T scene = new T();
            string sceneName = (name ?? typeof(T).Name.ToLower().Replace("screen", "").Replace("scene","")).ToLower();
            Scenes.Add(sceneName, scene);
            return scene;
        }
        /// <summary>
        /// Add a new scene to the game
        /// </summary>
        public T AddScene<T>() where T : Scene, new()
        {
            return AddScene<T>(null);
        }


        /// <summary>
        /// Add a costume to the game
        /// </summary>
        /// <param name="scene">Scene that owns it</param>
        /// <param name="CostumeName">Name of the costume</param>
        /// <returns>The costume object</returns>
        public Costume LoadCostume(Scene scene, string CostumeName)
        {
            if (costumes.ContainsKey(CostumeName))
            {
                return costumes[CostumeName];
            }
            Costume costume = new Costume();
            costume.Load(scene, Content, CostumeName);
            costumes[CostumeName] = costume;
            return costume;
        }


        /// <summary>
        /// Add a font to the game
        /// </summary>
        /// <param name="fontName">Name of the font resource</param>
        /// <returns>The font object</returns>
        public SpriteFont LoadFont(string fontName)
        {
            if (Fonts.ContainsKey(fontName))
            {
                return Fonts[fontName];
            }
            SpriteFont font = Content.Load<SpriteFont>("Fonts/" + fontName);
            Fonts[fontName] = font;
            return font;
        }

        /// <summary>
        /// Add a sound to the game
        /// </summary>
        /// <param name="soundName">Name of the sound resource</param>
        /// <returns>The sound object</returns>
        public SoundEffect LoadSound(string soundName)
        {
            soundName = soundName.ToLower();
            if (Sounds.ContainsKey(soundName))
            {
                return Sounds[soundName];
            }
            SoundEffect sound = Content.Load<SoundEffect>("Sounds/" + soundName);
            Sounds[soundName] = sound;
            return sound;
        }

        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="soundName">Name of the sound to play</param>
        /// <param name="loop">Should it keep looping</param>
        /// <param name="volume">Volume from 0.0 to 1.0</param>
        public void PlaySound(string soundName)
        {
            PlaySound(soundName, false, 1.0f, 0.0f, 0.0f);
        }
        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="soundName">Name of the sound to play</param>
        /// <param name="loop">Should it keep looping</param>
        /// <param name="volume">Volume from 0.0 to 1.0</param>
        public void PlaySound(string soundName, bool loop)
        {
            PlaySound(soundName, loop, 1.0f, 0.0f, 0.0f);
        }
        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="soundName">Name of the sound to play</param>
        /// <param name="loop">Should it keep looping</param>
        /// <param name="volume">Volume from 0.0 to 1.0</param>
        public void PlaySound(string soundName, bool loop, float volume)
        {
            PlaySound(soundName, loop, volume, 0.0f, 0.0f);
        }
        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="soundName">Name of the sound to play</param>
        /// <param name="loop">Should it keep looping</param>
        /// <param name="volume">Volume from 0.0 to 1.0</param>
        /// <param name="pan">Pan from -1.0 (left) to 1.0 (right).  0.0 is the center</param>
        /// <param name="pitch">Lower or higher from -1.0 (down one octave) to 1.0 (up one octave). 0 is normal.</param>
        public void PlaySound(string soundName, bool loop, float volume, float pitch, float pan)
        {
            soundName = soundName.ToLower();
            if (Sounds.ContainsKey(soundName))
            {
                if (loop)
                {
                    if (SoundInstances.ContainsKey(soundName) == false)
                    {
                        var soundInstance = Sounds[soundName].CreateInstance();
                        soundInstance.IsLooped = true;
                        soundInstance.Play();
                        SoundInstances.Add(soundName, soundInstance);
                    }
                }
                else
                {
                    Sounds[soundName].Play(volume, pitch, pan);
                }
            }
            else
            {
                throw new Exception("Attempted to play unloaded sound: " + soundName);
            }
        }

        /// <summary>
        /// Stop playing a repeating sound
        /// </summary>
        /// <param name="soundName">Name of the sound to stop</param>
        public void StopSound(string soundName)
        {
            soundName = soundName.ToLower();
            if (SoundInstances.ContainsKey(soundName))
            {
                SoundInstances[soundName].Stop();
                SoundInstances[soundName].Dispose();
                SoundInstances.Remove(soundName);
            }
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected sealed override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            ScratchyGame = null;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected sealed override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;

            // Allows the game to exit
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) || KeyboardInput.KeyPressed(Keys.Escape))
            {
                switch (activeGameScene.BackButtonBehaviour)
                {
                    case BackButtonBehaviours.ExitGame:
                        this.Exit();
                        break;
                    case BackButtonBehaviours.ShowFirstSceneOrExit:
                        if (activeGameScene == Scenes.First().Value)
                        {
                            this.Exit();
                        }
                        else
                        {
                            Scenes.First().Value.ShowScene();
                        }
                        break;
                    case BackButtonBehaviours.ShowPreviousScene:
                        if (activeGameScene.PreviousScene == null)
                        {
                            this.Exit();
                        }
                        else
                        {
                            activeGameScene.PreviousScene.ShowScene();
                        }
                        break;
                    case BackButtonBehaviours.Ignore:
                        // do nothing
                        break;
                }
            }

            KeyboardInput.Update();
#if !XBox
            MouseInput.Update();
            TouchInput.Update();
#endif

            activeGameScene.UpdateScene(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected sealed override void Draw(GameTime gameTime)
        {
            /*todo- depth buffer
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.Clear(ClearOptions.DepthBuffer, Color.CornflowerBlue, 1.0f, 0);
            GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            */
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            activeGameScene.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
