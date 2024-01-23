using System;
using System.Collections.Generic;
using MonoGame.Extended.Entities;
using Fizzleon.ECS.Systems;
using static Fizzleon.ECS.Components.SceneTransitionComponent<Fizzleon.Scenes.IScene>;
using static Fizzleon.Core.Data;
using Fizzleon.ECS.Components;

namespace Fizzleon.Scenes
{
    public class MenuScene : IScene
    {
        public TransitionState CurrentTransitionState => TransitionState.None;
        public World World { get; set; }
        public GameState.GameStates SceneId => GameState.GameStates.MENU;

        private Entity sceneEntity;
        public Entity SceneEntity => sceneEntity;

        private TextureLoaderSystem textureLoaderSystem;
        private WorldBuilder worldBuilder { get; set; }
        private TransitionComponent transitionComponent;
        public TransitionComponent TransitionComponent => transitionComponent;

        public Game1 Instance { get; }

        private static List<Texture2D> buttons = new(3);
        private readonly List<Rectangle> buttonsRect = new(buttons.Capacity);

        private readonly SceneTransitionComponent<MenuScene> sceneTransition;
        public MenuScene(Game1 instance)
        {
            Instance = instance;
            sceneTransition = new SceneTransitionComponent<MenuScene>(Instance, this);
        }

        public void Initialize()
        {
            worldBuilder = new WorldBuilder();
            worldBuilder.AddSystem(new RenderSystem());
            textureLoaderSystem = new TextureLoaderSystem(Instance.Content);
            worldBuilder.AddSystem(textureLoaderSystem);
            worldBuilder.AddSystem(new TransitionSystem());

            World = worldBuilder.Build();
            sceneEntity = World.CreateEntity();
            transitionComponent = new TransitionComponent(Instance, "Textures/Warrior_Sheet-Effect");
            sceneEntity.Attach(sceneTransition);
            sceneEntity.Attach(transitionComponent);

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
        public bool IsGameSceneRequested { get; set; } = false;

        public void Update()
        {
            oldMouse = mouse;
            mouse = Mouse.GetState();
            mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRect.Intersects(buttonsRect[0]) && mouse.LeftButton == ButtonState.Pressed)
                IsGameSceneRequested = true;

            if (mouseRect.Intersects(buttonsRect[2]) && mouse.LeftButton == ButtonState.Pressed)
                Window.Exit = true;

            World.Update(Data.GameTime);
        }

        public void Draw()
        {
            Data.SpriteBatch.Begin();
            for (var i = 0; i < buttons.Count; i++)
            {
                Data.SpriteBatch.Draw(buttons[i], buttonsRect[i], Color.White);
                if (mouseRect.Intersects(buttonsRect[i]))
                    Data.SpriteBatch.Draw(buttons[i], buttonsRect[i], Color.DarkGray);
            }
            sceneEntity.Get<SceneTransitionComponent<MenuScene>>().Draw(Data.SpriteBatch);

            Data.SpriteBatch.End();
        }

        public void Dispose()
        {
            buttons.Clear();
            World.Dispose();
            textureLoaderSystem.Dispose();
        }

        public void TransitionIn()
        {
            sceneEntity.Get<SceneTransitionComponent<MenuScene>>().TransitionIn();
        }

        public void TransitionOut()
        {
            sceneEntity.Get<SceneTransitionComponent<MenuScene>>().TransitionOut();
        }
    }
}
