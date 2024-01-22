using Fizzleon.Managers;
using System;

namespace Fizzleon.Core
{
    using static Data.Game;
    using static Data.Window;

    public class Game1 : Game
    {
        private readonly SceneManager sceneManager;
        public static Game1 Instance { get; private set; }
        private NetworkManager network;

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Instance = this;
            Window.Title = Title;
            IsMouseVisible = true;
            sceneManager = new();
            network = new();
        }


        protected override void Initialize()
        {
            try
            {
                sceneManager.Initialize();
                Graphics.PreferredBackBufferWidth = Width;
                Graphics.PreferredBackBufferHeight = Height;
                Graphics.ApplyChanges();

                var serverPort = network.StartServer(7000, 10, 1000);
                network.ConnectToServer("127.0.0.1", serverPort);
            }
            catch (Exception ex)
            {
                // Handle the exception (log, display an error message, etc.)
                Trace.WriteLine($"Initialization failed: {ex.Message}");
            }
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            sceneManager.LoadContent(Content);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            network.Update();

            if (Data.Window.Exit)
                Exit();

            sceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue * 0.2f);
            sceneManager.Draw(gameTime);

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            network.Dispose();
        }
    }
}

