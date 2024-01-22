using System.Collections.Generic;
using System.Diagnostics;
using System.Security;
using Fizzleon.ECS;
using Fizzleon.ECS.Components;
using Fizzleon.ECS.Systems;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.Scenes;

public class GameScene : IScene, IGameComponent
{
    public World world { get; set; }

    public Data.GameState.GameStates SceneId => Data.GameState.GameStates.GAME;

    private KeyboardState kb;
    public bool IsMenuSceneRequested { get; set; } = false;
    private WorldBuilder WorldBuilder { get; set; }

    public GameScene() => Initialize();



    // Player entity
    private Entity player;

    public void Initialize()
    {
        WorldBuilder = new WorldBuilder();
        WorldBuilder.AddSystem(new RenderSystem());
        world = WorldBuilder.Build();

        // Bind the player to the world
        player = world.CreateEntity();
        // Attach the transform component to the player entity
        player.Attach(new TransformComponent(new Vector2(0, 0)));
        // Attach the movement component to the player entity
        player.Attach(new MovementComponent { Velocity = new Vector2(0, 0) });

    }
    public void LoadContent(ContentManager Content)
    {
        // Create the player's texture
        Texture2D playerTexture = Content.Load<Texture2D>("Textures/Idle Down_Spritesheet (big)");
        // Create a sprite component for the player containing a texture2D 
        var sprite1 = new SpriteComponent(playerTexture);
        // Attach the sprite component to the player entity
        sprite1.SetTransform(player.Get<TransformComponent>());
        // Add the sprite component to the player entity
        player.Attach(sprite1);
    }

    public void Update(GameTime gameTime)
    {
        kb = Keyboard.GetState();
        if (kb.IsKeyDown(Keys.D1))
            IsMenuSceneRequested = true;

        world.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        world.Draw(gameTime);
    }
}
