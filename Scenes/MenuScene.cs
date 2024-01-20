using Fizzleon.ECS.Systems;
using System.Collections.Generic;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.Scenes;

using static Data.Game;
using static Data.Window;

internal class MenuScene : IScene, IGameComponent
{
    private Game1 instance { get; }

    public List<Entity> Entities { get; } = new List<Entity>();

    public  Data.GameState.GameStates SceneId { get; } = Data.GameState.GameStates.MENU;

    public List<EntitySystem> Systems { get; } = new List<EntitySystem>();

    public MenuScene(Game1 instance)
    {
        this.instance = instance;
        Initialize();
    }

    public void Initialize()
    {
        WorldBuilder = new WorldBuilder();
        WorldBuilder.AddSystem(new RenderSystem());
        World = WorldBuilder.Build();
        instance.Components.Add(World);
    }

    private static List<Texture2D> buttons = new(3);
    private readonly List<Rectangle> buttonsRect = new(buttons.Capacity);
    public void LoadContent(ContentManager Content)
    {
        for (int i = 0; i < 3; i++)
        {
            buttons.Add(Content.Load<Texture2D>($"textures/btn{i}"));
            buttonsRect.Add(new Rectangle(0, i * 75, buttons[i].Width / 4, buttons[i].Height / 4));
        }
    }

    public void Update(GameTime gameTime)
    {
        World.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();
        for (int i = 0; i < buttons.Count; i++)
        {
            SpriteBatch.Draw(buttons[i], buttonsRect[i], Color.White);
        }
        SpriteBatch.End();

        World.Draw(gameTime);
    }
}

