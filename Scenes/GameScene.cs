using Fizzleon.ECS.Entities;
using Fizzleon.ECS.Systems;
using MonoGame.Extended.Entities;
using System.Collections.Generic;
using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Scenes;
public class GameScene : IScene
{
    public World World { get; set; }
    public GameStates SceneId => GameStates.GAME;

    private KeyboardState kb;
    public bool IsMenuSceneRequested { get; set; } = false;
    private WorldBuilder WorldBuilder { get; set; }

    private List<Player> players;

    public void Initialize()
    {

        WorldBuilder = new WorldBuilder();
        WorldBuilder.AddSystem(new RenderSystem());
        WorldBuilder.AddSystem(new AnimationInitializationSystem());
        WorldBuilder.AddSystem(new AnimationUpdateSystem(Game1.Instance.Content));

        World = WorldBuilder.Build();
        players = new List<Player>
            {
                new Player(World, new Vector2(300, 500), 200f),
                new Player(World, new Vector2(700,200),175f)
            };
    }

    public void LoadContent(ContentManager Content)
    {
        players[0].LoadContent(Game1.Instance.Content, Content.Load<Texture2D>("Textures/Warrior_Sheet-Effect"), "Textures/Warrior_Sheet-Effect.sf");
        players[1].LoadContent(Game1.Instance.Content, Content.Load<Texture2D>("Textures/Mino"), "Textures/Mino.sf");
    }

    public void Update(GameTime gameTime)
    {
        HandleInput();

        players[0].Update(gameTime, "runRight", "idleRight");
        players[1].Update(gameTime, "idle-right", "run-right");

        World.Update(gameTime);
    }

    private void HandleInput()
    {
        kb = Keyboard.GetState();
        if (kb.IsKeyDown(Keys.D1))
            IsMenuSceneRequested = true;
    }
    public void Draw(GameTime gameTime)
    {
        World.Draw(gameTime);
    }

    public void Dispose()
    {
        players.ForEach(p => p.Dispose());
        World.Dispose();
    }
}
