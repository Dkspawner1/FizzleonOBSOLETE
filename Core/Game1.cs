using Fizzleon.Managers;
using Fizzleon.Network;
using System;

namespace Fizzleon.Core
{
    using static Data.Game;
    using static Data.Window;

    public class Game1 : Game
    {
        private readonly SceneManager sceneManager;
        //public static Game1 Instance { get; private set; }

        public static bool Start = false;
        private NetworkConnection networkConnection;


        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = Title;
            IsMouseVisible = true;
            sceneManager = new(this);

            networkConnection = new NetworkConnection();
        }


        protected override void Initialize()
        {
            sceneManager.Initialize();
            Graphics.PreferredBackBufferWidth = Width;
            Graphics.PreferredBackBufferHeight = Height;
            Graphics.ApplyChanges();

            base.Initialize();

            //if (networkConnection.Start())
            //{
            //}
            //else
            //{
            //}

        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            sceneManager.LoadContent();
            
            var textureLoaderSystem = new TextureLoaderSystem(Content);
            
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

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
            sceneManager.ForEach(scene => scene.Dispose());
            Dispose();
        }
    }
}

