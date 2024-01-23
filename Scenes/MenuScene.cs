using Fizzleon.ECS.Systems;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

using static Fizzleon.Core.Data.Game;
using static Fizzleon.Core.Data.Window;
using System.Reflection;

namespace Fizzleon.Scenes
{
    public class MenuScene : IScene, IDisposable
    {
        public World World { get; set; }

        public Data.GameState.GameStates SceneId => Data.GameState.GameStates.MENU;

        private WorldBuilder WorldBuilder { get; set; }
        public Game Instance { get; }

        private TextureLoaderSystem textureLoaderSystem;

        private static List<Texture2D> buttons = new(3);
        private readonly List<Rectangle> buttonsRect = new(buttons.Capacity);

        public MenuScene(Game instance)
        {
            Instance = instance;
            WorldBuilder = new WorldBuilder();
            WorldBuilder.AddSystem(new RenderSystem());
            textureLoaderSystem = new TextureLoaderSystem(Instance.Content);
            WorldBuilder.AddSystem(textureLoaderSystem);
            World = WorldBuilder.Build();
        }

        public void Initialize()
        {
            LoadContent();
        }

        public void LoadContent()
        {
            for (int i = 0; i < 3; i++)
            {
                buttons.Add(Instance.Content.Load<Texture2D>($"textures/btn{i}"));
                buttonsRect.Add(new Rectangle(0, 125 + i * 150, buttons[i].Width / 4, buttons[i].Height / 4));
            }
            textureLoaderSystem.LoadMenuItems(buttons, buttonsRect);

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

            World.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Data.Game.SpriteBatch.Begin();
            for (var i = 0; i < buttons.Count; i++)
            {
                Data.Game.SpriteBatch.Draw(buttons[i], buttonsRect[i], Color.White);
                if (mouseRect.Intersects(buttonsRect[i]))
                    Data.Game.SpriteBatch.Draw(buttons[i], buttonsRect[i], Color.DarkGray);
            }
            Data.Game.SpriteBatch.End();
        }

        public void Dispose()
        {
            buttons.Clear();
            World.Dispose();
            textureLoaderSystem.Dispose();
        }
    }
}
