using Fizzleon.ECS.Systems;
using Fizzleon.Managers;
using Fizzleon.Network;
using MonoGame.Extended.Input.InputListeners;

public class Game1 : Game
{
    private readonly SceneManager sceneManager;
    private NetworkConnection networkConnection;

    public Game1()
    {
        Data.GameTime = new GameTime();
        Data.Content = Content;
        Data.Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        Window.Title = Data.Window.Title;
        IsMouseVisible = true;

        // Create ContentInitializationSystem
        var contentInitializationSystem = ContentInitializationSystem.Create(Content);

        // Create SceneManager with required parameters
        var textureLoaderSystem = TextureLoaderSystem.Create( contentInitializationSystem);  

        sceneManager = new SceneManager(textureLoaderSystem, Content);

        networkConnection = new NetworkConnection();
    }

    protected override void Initialize()
    {
        Data.Graphics.PreferredBackBufferWidth = Data.Window.Width;
        Data.Graphics.PreferredBackBufferHeight = Data.Window.Height;
        Data.Graphics.ApplyChanges();


        sceneManager.Initialize();

        base.Initialize();

        if (networkConnection.Start())
        {
            // Handle successful network connection start
        }
        else
        {
            // Handle failed network connection start
        }
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

        // Update SceneManager
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
