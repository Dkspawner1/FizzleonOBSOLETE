using System.Collections.Generic;
using Fizzleon.ECS;
using Fizzleon.ECS.Components;
using Fizzleon.ECS.Systems;
using MonoGame.Extended.Entities;

namespace Fizzleon.Scenes;

public class GameScene : IScene
{
    public World world { get; set; }
    public Data.GameState.GameStates SceneId => Data.GameState.GameStates.GAME;

    private KeyboardState kb;
    public bool IsMenuSceneRequested { get; set; } = false;
    private WorldBuilder WorldBuilder { get; set; }

    private AnimationInitializationSystem animationInitializationSystem;
    private AnimationUpdateSystem animationUpdateSystem;
    private MovementSystem movementSystem;

    public GameScene() { }

    private Entity player;

    public void Initialize()
    {
        WorldBuilder = new WorldBuilder();

        // Pass the ContentManager to AnimationInitializationSystem and AnimationUpdateSystem
        animationInitializationSystem = new AnimationInitializationSystem();
        animationUpdateSystem = new AnimationUpdateSystem(Game1.Instance.Content);

        WorldBuilder.AddSystem(animationInitializationSystem);
        WorldBuilder.AddSystem(animationUpdateSystem);
        WorldBuilder.AddSystem(new RenderSystem());
        movementSystem = new MovementSystem();

        WorldBuilder.AddSystem(movementSystem);

        world = WorldBuilder.Build();

        player = world.CreateEntity();
        player.Attach(new TransformComponent(new Vector2(0, 0)));
        player.Attach(new MovementComponent { Velocity = new Vector2(0, 0) });
    }

    public void LoadContent(ContentManager Content)
    {
        LoadPlayer(Content);
    }
    private void LoadPlayer(ContentManager Content)
    {
        Texture2D playerTexture = Content.Load<Texture2D>("Textures/Warrior_Sheet-Effect");

        var sprite = new SpriteComponent(playerTexture);

        var animation = new AnimationComponent("Textures/Warrior_Sheet-Effect.sf", sprite.Texture);

        animation.LoadContent(Content);

     
        sprite.SetTransform(player.Get<TransformComponent>());
        animation.SetTransform(player.Get<TransformComponent>());

        player.Attach(sprite);
        player.Attach(animation);
    }


    public void Update(GameTime gameTime)
    {
        kb = Keyboard.GetState();
        if (kb.IsKeyDown(Keys.D1))
            IsMenuSceneRequested = true;

        player.Get<AnimationComponent>().Update(gameTime);
        player.Get<MovementComponent>().Velocity = new Vector2(50, 0); 

        world.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {

        world.Draw(gameTime);
    }

 
}
