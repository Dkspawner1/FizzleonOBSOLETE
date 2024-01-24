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

    private readonly SceneTransitionComponent transitionComponent;
    public SceneTransitionComponent TransitionComponent => transitionComponent;

    private KeyboardState kb;

    private WorldBuilder worldBuilder;
    private TextureLoaderSystem textureLoaderSystem;

    private List<Player> players { get; set; }

    protected Game1 Instance { get; private set; }

    private ContentManager Content;
    public GameScene(Game1 instance)
    {
        Instance = instance;

        transitionComponent = new SceneTransitionComponent(Instance);

        Content = new ContentManager(instance.Services, "Content");
    }

    public void Initialize()
    {
        worldBuilder = new WorldBuilder()
            .AddSystem(new RenderSystem())
            .AddSystem(new AnimationInitializationSystem(Content))
            .AddSystem(new AnimationUpdateSystem(Content))
            .AddSystem(new TransitionSystem());

        textureLoaderSystem = TextureLoaderSystem.Create(Content);
        worldBuilder.AddSystem(textureLoaderSystem);

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
        players[0].LoadContent(Instance, Content.Load<Texture2D>("Textures/Warrior_Sheet-Effect"), "Textures/Warrior_Sheet-Effect.sf");
        players[1].LoadContent(Instance, Content.Load<Texture2D>("Textures/Mino"), "Textures/Mino.sf");

        textureLoaderSystem.LoadEntities(players.Select(player => player.entity), players.Select(player => player.entity.Get<SpriteComponent>().Texture.Name)) ;
    }

    public void Update()
    {
        HandleInput();
        players[0].Update("runRight", "idleRight");
        players[1].Update("idle-right", "run-right");
        World.Update(new GameTime());
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


        Content.Unload();


        World.Dispose();
    }

    public void TransitionIn()
    {
        // Use the generic TransitionIn method
        TransitionComponent.TransitionIn();
    }

    public void TransitionOut()
    {
        // Use the generic TransitionOut method
        TransitionComponent.TransitionOut();
    }
}
