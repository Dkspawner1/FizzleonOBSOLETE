using Fizzleon.ECS.Systems;
using System.Collections.Generic;
using System.Diagnostics;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.Scenes;

using static Data.Game;
using static Data.Window;

internal class MenuScene : IScene, IGameComponent
{

    public List<Entity> Entities { get; } = new List<Entity>();

    public Data.GameState.GameStates SceneId { get; } = Data.GameState.GameStates.MENU;

    public List<EntitySystem> Systems { get; } = new List<EntitySystem>();

    public MenuScene()
    {
        Initialize();
    }

    public void Initialize()
    {
        WorldBuilder = new WorldBuilder();
        WorldBuilder.AddSystem(new RenderSystem());
        World = WorldBuilder.Build();
    }



    private static List<Texture2D> buttons = new(3);
    private readonly List<Rectangle> buttonsRect = new(buttons.Capacity);
    public void LoadContent(ContentManager Content)
    {
        for (int i = 0; i < 3; i++)
        {
            buttons.Add(Content.Load<Texture2D>($"textures/btn{i}"));
            buttonsRect.Add(new Rectangle(0, 125 + i * 150, buttons[i].Width / 4, buttons[i].Height / 4));
        }
    }

    private MouseState mouse, oldMouse;
    private Rectangle mouseRect;
    public bool SwitchToGameScene = false;
    public void Update(GameTime gameTime)
    {
        oldMouse = mouse;
        mouse = Mouse.GetState();
        mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);


        if (mouseRect.Intersects(buttonsRect[0]) && mouse.LeftButton == ButtonState.Pressed)
            SwitchToGameScene = true;

        if (mouseRect.Intersects(buttonsRect[2]) && mouse.LeftButton == ButtonState.Pressed)
            Exit = true;

        World.Update(gameTime);
    }

    public void Draw(GameTime gameTime)
    {
        SpriteBatch.Begin();
        for (var i = 0; i < buttons.Count; i++)
        {
            SpriteBatch.Draw(buttons[i], buttonsRect[i], Color.White);
            if (mouseRect.Intersects(buttonsRect[i]))
                SpriteBatch.Draw(buttons[i], buttonsRect[i], Color.DarkGray);
        }
        SpriteBatch.End();

    }
}

