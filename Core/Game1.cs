using Fizzleon.Scenes;

namespace Fizzleon.Core;

using static Data.Game;
using static Data.Window;

public class Game1 : Game
{
    private readonly GameStateManager gameStateManager;

    public Game1()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        Window.Title = Title;
        IsMouseVisible = true;
        gameStateManager = new(this);
    }

    protected override void Initialize()
    {
        Graphics.PreferredBackBufferWidth = Width;
        Graphics.PreferredBackBufferHeight = Height;
        Graphics.ApplyChanges();

        gameStateManager.InitializeScenes();


        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        gameStateManager.LoadContent(Content);

        base.LoadContent();

    }

    protected override void Update(GameTime gameTime)
    {
        gameStateManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkBlue * 0.2f);
        gameStateManager.Draw(gameTime);

        base.Draw(gameTime);
    }
}
