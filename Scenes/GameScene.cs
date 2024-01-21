using System.Collections.Generic;
using Fizzleon.ECS;
using Fizzleon.ECS.Components;
using Fizzleon.ECS.Systems;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.Scenes;

using static Data.Game;

public class GameScene : IScene, IGameComponent
{

    public List<Entity> Entities { get; } = new List<Entity>();

    public Data.GameState.GameStates SceneId => Data.GameState.GameStates.GAME;

    public List<EntitySystem> Systems { get; } = new List<EntitySystem>();


    public GameScene()
    {
        Initialize();
    }

    public void Initialize()
    {

        WorldBuilder = new WorldBuilder();
        WorldBuilder.AddSystem(new RenderSystem());
        World = WorldBuilder.Build();
    }

    private Texture2D s;
    public void LoadContent(ContentManager Content)
    {
        s = Game1.Instance.Content.Load<Texture2D>("Textures/Idle Down_Spritesheet (big)");

        // Create entities and add them to the game scene
        var entity1 = World.CreateEntity();
        entity1.Attach(new TransformComponent(new Vector2(200, 200)));
        var sprite1 = new SpriteComponent(s);
        sprite1.SetTransform(entity1.Get<TransformComponent>());
        entity1.Attach(sprite1);

        var entity2 = World.CreateEntity();
        entity2.Attach(new TransformComponent(new Vector2(600, 800)));
        var sprite2 = new SpriteComponent(s);
        sprite2.SetTransform(entity2.Get<TransformComponent>());
        entity2.Attach(sprite2);

        var entity3 = World.CreateEntity();
        entity3.Attach(new TransformComponent(new Vector2(0, 400)));
        var sprite3 = new SpriteComponent(s);
        sprite3.SetTransform(entity3.Get<TransformComponent>());
        entity3.Attach(sprite3);
    }

    public void Update(GameTime gameTime)
    {
        World.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        World.Draw(gameTime);
    }




}
