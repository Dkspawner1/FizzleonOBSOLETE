using Fizzleon.Entity;
using MonoGame.Extended;
using MonoGame.Extended.Entities;

namespace Fizzleon.Core;

using static Data.Game;
using static Data.Window;

public class Game1 : Game
{
    public Game1()
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        Window.Title = Title;
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
        Graphics.PreferredBackBufferWidth = Width;
        Graphics.PreferredBackBufferHeight = Height;
        Graphics.ApplyChanges();

        WorldBuilder = new WorldBuilder();
        WorldBuilder.AddSystem(new RenderSystem());
        World = WorldBuilder.Build();
        Components.Add(World);

        base.Initialize();
    }

    private Texture2D s;
    protected override void LoadContent()
    {
        s = Content.Load<Texture2D>("Textures/Idle Down_Spritesheet (big)");

        SpriteBatch = new SpriteBatch(GraphicsDevice);

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


    }

    protected override void Update(GameTime gameTime)
    {
        World.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkBlue * 0.2f);
        
    World.Draw(gameTime);

        base.Draw(gameTime);
    }
}
