using Fizzleon.ECS.Entities;
using Fizzleon.ECS.Systems;
using Fizzleon.Scenes;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;

using static Fizzleon.Core.Data.GameState;

public class GameScene : IScene, IDisposable
{
    public World World { get; set; }
    public GameStates SceneId => GameStates.GAME;

    private KeyboardState kb;
    public bool IsMenuSceneRequested { get; set; } = false;
    private WorldBuilder WorldBuilder { get; set; }

    private List<Player> players;
    public Game Instance { get; }

    private TextureLoaderSystem textureLoaderSystem;

    public GameScene(Game instance)
    {
        Instance = instance;
    }

    public void Initialize()
    {
        WorldBuilder = new WorldBuilder();
        WorldBuilder.AddSystem(new RenderSystem());
        WorldBuilder.AddSystem(new AnimationInitializationSystem(Instance.Content));
        WorldBuilder.AddSystem(new AnimationUpdateSystem(Instance.Content));

        textureLoaderSystem = new TextureLoaderSystem(Instance.Content);
        WorldBuilder.AddSystem(textureLoaderSystem);

        World = WorldBuilder.Build();
        players = new List<Player>
        {
            new Player(World, new Vector2(300, 500), 200f),
            new Player(World, new Vector2(700, 200), 175f)
        };
    }

    public void LoadContent()
    {
        players[0].LoadContent(Instance, Instance.Content.Load<Texture2D>("Textures/Warrior_Sheet-Effect"), "Textures/Warrior_Sheet-Effect.sf");
        players[1].LoadContent(Instance, Instance.Content.Load<Texture2D>("Textures/Mino"), "Textures/Mino.sf");

        textureLoaderSystem.LoadEntities(players.Select(player => player.entity));
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
        foreach (var player in players)
        {
            player.Dispose();
        }

        players.Clear();

        // Unload content specific to the current scene
        Instance.Content.Unload();

        // Dispose the World
        World.Dispose();
    }
}
