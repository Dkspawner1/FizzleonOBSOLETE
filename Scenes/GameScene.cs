using Fizzleon.ECS.Components;
using Fizzleon.ECS.Entities;
using Fizzleon.ECS.Systems;
using Fizzleon.Managers;
using MonoGame.Extended.Entities;
using System.Collections.Generic;
using static Fizzleon.Core.Data.GameState;
using static Fizzleon.ECS.Components.SceneTransitionComponent;

public class GameScene : IScene
{
    public GameStates SceneId => GameStates.GAME;
    public World World { get; set; }
    public bool IsSceneChangeRequested { get; set; }

    private Entity sceneEntity;
    public Entity SceneEntity => sceneEntity;

    public SceneTransitionComponent TransitionComponent => sceneTransition;
    private SceneTransitionComponent sceneTransition;

    private KeyboardState kb;

    private WorldBuilder worldBuilder;
    private List<Player> Players { get; set; }

    private readonly TextureLoaderSystem textureLoaderSystem;

    public TransitionState CurrentTransitionState => TransitionState.None;

    private readonly SceneTransitionSystem transitionSystem;

    public GameScene(TextureLoaderSystem textureLoaderSystem, SceneManager sceneManager)
    {
        this.textureLoaderSystem = textureLoaderSystem;
        this.textureLoaderSystem = textureLoaderSystem;

        transitionSystem = new SceneTransitionSystem(sceneManager);
    }
    public void Initialize()
    {
        sceneTransition = new SceneTransitionComponent(textureLoaderSystem.Load<Texture2D>("textures/btn0"));


        worldBuilder = new WorldBuilder()
           .AddSystem(textureLoaderSystem)
           .AddSystem(new RenderSystem())
           .AddSystem(new AnimationInitializationSystem())
           .AddSystem(new AnimationUpdateSystem())
           .AddSystem(transitionSystem);

        World = worldBuilder.Build();

        Players = new List<Player>
        {
            new Player(World, new Vector2(300, 500), 200f),
            new Player(World, new Vector2(700, 200), 175f)
        };
        Players[0].Entity.Attach(new TransformComponent(new Vector2(100,100)));
        Players[1].Entity.Attach(new TransformComponent(new Vector2(100, 100)));




        sceneEntity = World.CreateEntity();
        sceneEntity.Attach(sceneTransition);

        LoadContent();
    }


    public void LoadContent()
    {

        Players[0].LoadContent(textureLoaderSystem, "Textures/Warrior_Sheet-Effect", "Textures/Warrior_Sheet-Effect.sf");
        Players[1].LoadContent(textureLoaderSystem, "Textures/Mino", "Textures/Mino.sf");

        var playerTextures = Players
            .Where(player => player.Entity != null)
            .Select(player => player.Entity.Get<SpriteComponent>()?.Texture)
            .Where(texture => texture != null);

        textureLoaderSystem.LoadEntities(
    Players
        .Where(player => player.Entity != null)
        .Select(player => player.Entity),
        playerTextures.Select(texture => texture.Name));
    }

    public void Update(GameTime gameTime)
    {
        HandleInput();
        Players[0].Update("runRight", "idleRight");
        Players[1].Update("idle-right", "run-right");
        World.Update(gameTime);
        sceneTransition.Update();

    }

    private void HandleInput()
    {
        kb = Keyboard.GetState();
        if (kb.IsKeyDown(Keys.D1))
            IsSceneChangeRequested = true;

    }

    public void Draw()
    {
        World.Draw(Data.GameTime);
    }

    public void Dispose()
    {
        foreach (var player in Players)
        {
            player.Dispose();
        }

        Players.Clear();


        World.Dispose();
    }

    public void TransitionIn()
    {
        TransitionComponent.TransitionIn();
    }

    public void TransitionOut()
    {
        TransitionComponent.TransitionOut();
    }
}
