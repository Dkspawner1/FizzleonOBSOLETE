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
    public TransitionState CurrentTransitionState => TransitionState.None;

    private Entity sceneEntity;
    public Entity SceneEntity => sceneEntity;

    private KeyboardState kb;

    private WorldBuilder worldBuilder;
    private List<Player> players { get; set; }

    private readonly TextureLoaderSystem textureLoaderSystem;
    private ContentInitializationSystem contentInitializationSystem;
    public SceneTransitionComponent TransitionComponent => transitionComponent;
    private readonly SceneTransitionComponent transitionComponent;
    private TransitionSystem transitionSystem;

    public GameScene(TextureLoaderSystem textureLoaderSystem, SceneManager sceneManager)
    {
        this.textureLoaderSystem = textureLoaderSystem;

        contentInitializationSystem = ContentInitializationSystem.Create(Data.Content);

        // Initialize the transition component
        transitionComponent = new SceneTransitionComponent(textureLoaderSystem);

        // Pass the SceneManager instance to the TransitionSystem
        transitionSystem = new TransitionSystem(sceneManager);
    }

    public void Initialize()
    {
        var contentInitializationSystem = ContentInitializationSystem.Create(Data.Content);
        var textureLoaderSystem = TextureLoaderSystem.Create(contentInitializationSystem);
        var sceneTransitionComponent = new SceneTransitionComponent(textureLoaderSystem);

        worldBuilder = new WorldBuilder()
           .AddSystem(contentInitializationSystem)
           .AddSystem(textureLoaderSystem)
           .AddSystem(new RenderSystem())
           .AddSystem(new AnimationInitializationSystem(Data.Content))
           .AddSystem(new AnimationUpdateSystem(Data.Content))
           .AddSystem(transitionSystem);

        World = worldBuilder.Build();

        players = new List<Player>
        {
            new Player(World, new Vector2(300, 500), 200f),
            new Player(World, new Vector2(700, 200), 175f)
        };

        sceneEntity = World.CreateEntity();

        sceneEntity.Attach(transitionComponent);

        LoadContent();
    }


    public void LoadContent()
    {
        players[0].LoadContent(textureLoaderSystem, "Textures/Warrior_Sheet-Effect", "Textures/Warrior_Sheet-Effect.sf");
        players[1].LoadContent(textureLoaderSystem, "Textures/Mino", "Textures/Mino.sf");

        textureLoaderSystem.LoadEntities(
            players
                .Where(player => player.entity != null)
                .Select(player => player.entity),
            players
                .Where(player => player.entity != null && player.entity.Has<SpriteComponent>())
                .Select(player => player.entity.Get<SpriteComponent>()?.Texture?.Name)
                .Where(textureName => textureName != null)
        );
    }

    public void Update(GameTime gameTime)
    {
        HandleInput();
        players[0].Update("runRight", "idleRight");
        players[1].Update("idle-right", "run-right");
        World.Update(gameTime);
    }

    private void HandleInput()
    {
        kb = Keyboard.GetState();
        if (kb.IsKeyDown(Keys.D1))
            IsSceneChangeRequested = true;

    }

    public void Draw()
    {
        World.Draw(new GameTime());
    }

    public void Dispose()
    {
        foreach (var player in players)
        {
            player.Dispose();
        }

        players.Clear();


        Data.Content.Unload();


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
