using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Managers;
internal class GameStateManager : IGameComponent
{
    public SceneManager SceneManager { get; set; }

    public GameStateManager()
    {
        SceneManager = new SceneManager();
    }

    public void Initialize()
    {
        SceneManager.Initialize();
    }
    public void LoadContent(ContentManager Content)
    {
        SceneManager.LoadContent(Content);
    }

    public void Update(GameTime gameTime)
    {
        SceneManager.Update(gameTime);

    }
    public void Draw(GameTime gameTime)
    {
        SceneManager.Draw(gameTime);

    }


    // public override string ToString() => $"{nameof(CurrentScene)}: {CurrentScene}";


}