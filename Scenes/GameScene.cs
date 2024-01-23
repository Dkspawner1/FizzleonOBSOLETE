using Fizzleon.ECS.Components;
using Fizzleon.ECS.Entities;
using Fizzleon.ECS.Systems;
using Fizzleon.Scenes;
using MonoGame.Extended.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using static Fizzleon.Core.Data.GameState;
using static Fizzleon.ECS.Components.SceneTransitionComponent<Fizzleon.Scenes.IScene>;

public class GameScene : IScene
{
    public GameStates SceneId => GameStates.GAME;
    public World World { get; set; }

    public TransitionState CurrentTransitionState => TransitionState.None;

    private Entity sceneEntity;
    public Entity SceneEntity => sceneEntity;

    private TransitionComponent transitionComponent;
    public TransitionComponent TransitionComponent => transitionComponent;
    private readonly SceneTransitionComponent<GameScene> sceneTransition;

    private KeyboardState kb;
    public bool IsMenuSceneRequested { get; set; } = false;

    private WorldBuilder worldBuilder;
    private TextureLoaderSystem textureLoaderSystem;

    private List<Player> players { get; set; }

    protected Game1 Instance { get; private set; }
    private ContentManager Content;


    public GameScene(Game1 instance)
    {
        Instance = instance;
        sceneTransition = new SceneTransitionComponent<GameScene>(Instance, this);

        Content = new ContentManager(instance.Services, "Content");
    }

    public void Initialize()
    {
        worldBuilder = new WorldBuilder();

        var animationInitializationSystem = new AnimationInitializationSystem(Content);
        worldBuilder.AddSystem(animationInitializationSystem);

        worldBuilder.AddSystem(new AnimationUpdateSystem(Content));
        worldBuilder.AddSystem(new TransitionSystem());
        textureLoaderSystem = new TextureLoaderSystem(Content);
        worldBuilder.AddSystem(textureLoaderSystem);

        World = worldBuilder.Build();
        players = new List<Player>
        {
            new Player(World, new Vector2(300, 500), 200f),
            new Player(World, new Vector2(700, 200), 175f)
        };

        sceneEntity = World.CreateEntity();

        transitionComponent = new TransitionComponent(Instance, "Textures/Warrior_Sheet-Effect");

        sceneEntity.Attach(transitionComponent);

        sceneEntity.Attach(sceneTransition);

        LoadContent();
    }

    public void LoadContent()
    {
        players[0].LoadContent(Instance, Content.Load<Texture2D>("Textures/Warrior_Sheet-Effect"), "Textures/Warrior_Sheet-Effect.sf");
        players[1].LoadContent(Instance, Content.Load<Texture2D>("Textures/Mino"), "Textures/Mino.sf");

        textureLoaderSystem.LoadEntities(players.Select(player => player.entity));
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
            IsMenuSceneRequested = true;
    }

    public void Draw()
    {
        World.Draw(new GameTime()); // Pass a new GameTime instance
    }

    public void Dispose()
    {
        foreach (var player in players)
        {
            player.Dispose();
        }

        players.Clear();

        // Unload content specific to the current scene
        Content.Unload();

        // Dispose the World
        World.Dispose();
    }

    public void TransitionIn()
    {
        sceneEntity.Get<SceneTransitionComponent<GameScene>>().TransitionIn();
    }

    public void TransitionOut()
    {
        sceneEntity.Get<SceneTransitionComponent<GameScene>>().TransitionOut();
    }
}
