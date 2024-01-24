using Fizzleon.Managers;
using Fizzleon.Network;
using MonoGame.Extended.Input.InputListeners;
using System;

namespace Fizzleon.Core
{
    using static Data;
    using static Data.Window;

    public class Game1 : Game
    {
        private readonly SceneManager sceneManager;
        private NetworkConnection networkConnection;
        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = Title;
            IsMouseVisible = true;
            sceneManager = new(this);

            //Data.Content = new ContentManager();
            Data.Content = Content;
            networkConnection = new NetworkConnection();
            MouseListener = new MouseListener();
        }


        protected override void Initialize()
        {
            sceneManager.Initialize();
            Graphics.PreferredBackBufferWidth = Width;
            Graphics.PreferredBackBufferHeight = Height;
            Graphics.ApplyChanges();

            base.Initialize();

            if (networkConnection.Start())
            {
            }
            else
            {
            }


        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            sceneManager.LoadContent();
            
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            MouseListener.Update(gameTime);
            if (Data.Window.Exit)
                Exit();
            
            sceneManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue * 0.2f);
            sceneManager.Draw();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            sceneManager.ForEach(scene => scene.Dispose());
        }
    }
}

