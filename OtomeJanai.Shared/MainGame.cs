using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OtomeJanai.Shared.Common;
using OtomeJanai.Shared.Common.BmFont;
using OtomeJanai.Shared.Common.BmFont.FontModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
#if WINDOWS_UWP
using Windows.System.Threading;
#endif

namespace OtomeJanai.Shared
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    internal class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private FontRenderer _fontRenderer;
        private bool _isInit;
        private SoundEngine _sound;

#if WINDOWS_UWP
        private ThreadPoolTimer _timer;
#else
        private System.Timers.Timer _timer;
#endif



        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
#if WINDOWS_UWP
            _timer = ThreadPoolTimer.CreatePeriodicTimer(TimerElapsed, TimeSpan.FromSeconds(10));
#else
            _timer = new System.Timers.Timer();
            _timer.Interval = 10000;
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
#endif

            _sound = new SoundEngine();
#if ANDROID
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
#endif
        }

        // From Shadowsocks:
        // release any unused pages
        // making the numbers look good in task manager
        // this is totally nonsense in programming
        // but good for those users who care
        // making them happier with their everyday life
        // which is part of user experience
#if WINDOWS_UWP
        private void TimerElapsed(ThreadPoolTimer timer)
        {
            GC.Collect();
        }
#else
        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            GC.Collect();
        }
#endif


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            _fontRenderer = new FontRenderer("Yahei_Light", GraphicsDevice);
            _isInit = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override async void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Test for SoundEngine
            _sound.PlayFromStream(await ContentLoader.GetFileStream("Songs/SE_9670.OGG"), "Songs/SE_9670.OGG");
            _sound.IsLoop = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            InputHandler.Instance.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (!IsActive)
                return;
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            if (_isInit)
            {
                //Test for FontRenderer
                
                _fontRenderer.DrawText(_spriteBatch, 0, 0, "你测试吗？");
                _fontRenderer.DrawText(_spriteBatch, 0, 26, "你写代码吗？");
                _fontRenderer.DrawText(_spriteBatch, 0, 52, "过年写代码你开心吗？");
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            //TODO: Anti-cheats

            _sound.Pause();
        }
        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            _sound.Resume();
        }
    }
}
