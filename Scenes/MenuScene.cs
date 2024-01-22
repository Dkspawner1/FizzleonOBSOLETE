using Fizzleon.ECS.Systems;
using System.Collections.Generic;
using MonoGame.Extended.Entities;

namespace Fizzleon.Scenes;

using static Data.Game;
using static Data.Window;

public class MenuScene : IScene, IGameComponent
{
    public World world { get; set; }

    public List<Entity> Entities { get; } = new();

    public Data.GameState.GameStates SceneId => Data.GameState.GameStates.MENU;

    private WorldBuilder WorldBuilder { get; set; }


    public MenuScene()
    {
        Initialize();
    }

    public void Initialize()
    {
        WorldBuilder = new WorldBuilder();
        WorldBuilder.AddSystem(new RenderSystem());
        world = WorldBuilder.Build();
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
    public bool IsGameSceneRequested = false;
    public void Update(GameTime gameTime)
    {
        oldMouse = mouse;
        mouse = Mouse.GetState();
        mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);


        if (mouseRect.Intersects(buttonsRect[0]) && mouse.LeftButton == ButtonState.Pressed)
            IsGameSceneRequested = true;

        if (mouseRect.Intersects(buttonsRect[2]) && mouse.LeftButton == ButtonState.Pressed)
            Exit = true;

        world.Update(gameTime);
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

