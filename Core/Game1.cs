using Fizzleon.ECS.Systems;
using Fizzleon.Managers;
using MonoGame.Extended.Screens;
using System;
using static Fizzleon.Core.Data;
using ContentInitializationSystem = Fizzleon.ECS.Systems.ContentInitializationSystem;

namespace Fizzleon.Core;

public class Game1 : Game
{
    private readonly SceneManager sceneManager;


    public Game1()
    {
        Data.GameTime = new GameTime();


        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        Window.Title = Data.Window.Title;


        IsMouseVisible = true;

        Data.ContentManager = Content;
        Data.ContentInitializationSystem = ContentInitializationSystem.Create(Data.ContentManager);

        sceneManager = new SceneManager(Graphics);
    }



    protected override void Initialize()
    {
        Graphics.PreferredBackBufferWidth = Data.Window.Width;
        Graphics.PreferredBackBufferHeight = Data.Window.Height;
        Graphics.ApplyChanges();

#if DEBUG
        Window.AllowUserResizing = true;
#endif

        sceneManager.InitializeScenes();
        sceneManager.SubscribeToSceneEvents();

        base.Initialize();
    }


    protected override void LoadContent()
    {
        Data.SpriteBatch = new SpriteBatch(GraphicsDevice);


        sceneManager.LoadContent();

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        Data.GameTime = gameTime;

        if (Data.Window.Exit)
            Exit();

        sceneManager.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        sceneManager.Dispose();

        base.UnloadContent();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            sceneManager?.Dispose();

        base.Dispose(disposing);
    }
}