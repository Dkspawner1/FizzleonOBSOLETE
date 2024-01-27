using System.Runtime.InteropServices;
using Fizzleon.ECS.Systems;
using Fizzleon.Managers;
using Fizzleon.Network;
using MonoGame.Extended.Content;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

public class Game1 : Game
{
    private readonly SceneManager sceneManager;
    public Game1()
    {
        Data.GameTime = new GameTime();
        Data.Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        Window.Title = Data.Window.Title;
        IsMouseVisible = true;
        
        Data.ContentInitializationSystem = ContentInitializationSystem.Create(Content);

        sceneManager = new SceneManager(new TextureLoaderSystem(Content));


    }

    protected override void Initialize()
    {
        Data.Graphics.PreferredBackBufferWidth = Data.Window.Width;
        Data.Graphics.PreferredBackBufferHeight = Data.Window.Height;
        Data.Graphics.ApplyChanges();


        sceneManager.Initialize();

        base.Initialize();

    }

    protected override void LoadContent()
    {
        Data.SpriteBatch = new SpriteBatch(GraphicsDevice);

        // Load content for the SceneManager
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
        GraphicsDevice.Clear(Color.DarkBlue * 0.2f);

        // Draw SceneManager
        sceneManager.Draw();

        base.Draw(gameTime);
    }

    protected override void UnloadContent()
    {
        // Dispose of scenes in the SceneManager
        sceneManager.ForEach(scene => scene.Dispose());

        base.UnloadContent();
    }
}
