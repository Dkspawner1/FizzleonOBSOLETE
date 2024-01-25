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
        Data.Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        Window.Title = Data.Window.Title;
        IsMouseVisible = true;

        var contentLoader = new GameContentLoader(Services, "Content");
        var contentInitializationSystem = ContentInitializationSystem.Create(Content);

        var textureLoaderSystem = TextureLoaderSystem.Create(contentInitializationSystem);

        sceneManager = new SceneManager(textureLoaderSystem);

        Data.Content = Content;

        networkConnection = new NetworkConnection();
        //MouseListener = new MouseListener();
    }

    protected override void Initialize()
    {
        Data.Graphics.PreferredBackBufferWidth = Data.Window.Width;
        Data.Graphics.PreferredBackBufferHeight = Data.Window.Height;
        Data.Graphics.ApplyChanges();

        // Initialize scene manager before base.Initialize()
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

        // Load content after initializing scene manager
        sceneManager.LoadContent();

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // Set Data.GameTime at the beginning of the Update method
        Data.GameTime = gameTime;

        //MouseListener.Update(gameTime);

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

        base.UnloadContent();
    }
}
