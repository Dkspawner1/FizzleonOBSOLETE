using Fizzleon.ECS.Components;
using Fizzleon.ECS.Systems;
using Fizzleon.Managers;
using MonoGame.Extended.Entities;
using System.Collections.Generic;
using static Fizzleon.Core.Data;
using static Fizzleon.ECS.Components.SceneTransitionComponent;

namespace Fizzleon.Scenes
{
    public class MenuScene : IScene
    {
        public TransitionState CurrentTransitionState => TransitionState.None;
        public World World { get; set; }
        public GameState.GameStates SceneId => GameState.GameStates.MENU;
        public bool IsSceneChangeRequested { get; set; }

        private Entity sceneEntity;
        public Entity SceneEntity => sceneEntity;

        private TextureLoaderSystem textureLoaderSystem;
        private WorldBuilder worldBuilder { get; set; }

        // Use the generic SceneTransitionComponent
        private SceneTransitionComponent sceneTransition;
        public SceneTransitionComponent TransitionComponent => sceneTransition;

        public Game1 Instance { get; }

        private static List<Texture2D> buttons = new(3);
        private readonly List<Rectangle> buttonsRect = new(buttons.Capacity);

        public MenuScene(Game1 instance)
        {
            Instance = instance;
            sceneTransition = new SceneTransitionComponent(Instance);
        }

        public void Initialize()
        {
            worldBuilder = new WorldBuilder()
                .AddSystem(new RenderSystem())
                .AddSystem(new TransitionSystem());

            // Instantiate textureLoaderSystem before adding it to worldBuilder
            textureLoaderSystem = new TextureLoaderSystem(Instance.Content);
            worldBuilder.AddSystem(textureLoaderSystem);

            World = worldBuilder.Build();
            sceneEntity = World.CreateEntity();

            // Attach the generic SceneTransitionComponent
            sceneEntity.Attach(sceneTransition);

            LoadContent();
        }

        public void LoadContent()
        {
            buttons.Clear();

            for (int i = 0; i < 3; i++)
            {
                buttons.Add(Instance.Content.Load<Texture2D>($"textures/btn{i}"));
                buttonsRect.Add(new Rectangle(0, 125 + i * 150, buttons[i].Width / 4, buttons[i].Height / 4));
            }
            textureLoaderSystem.LoadMenuItems(buttons, buttonsRect);
        }

        private MouseState mouse, oldMouse;
        private Rectangle mouseRect;

        public void Update()
        {
            oldMouse = mouse;
            mouse = Mouse.GetState();
            mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (mouseRect.Intersects(buttonsRect[0]) && mouse.LeftButton == ButtonState.Pressed)
                IsSceneChangeRequested = true;

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

            // Use the generic Draw method
            sceneEntity.Get<SceneTransitionComponent>().Draw(Data.SpriteBatch);

            Data.SpriteBatch.End();
        }

        public void Dispose()
        {
            buttons.Clear();
            textureLoaderSystem.Dispose();
            World.Dispose();
        }

        public void TransitionIn()
        {
            // Use the generic TransitionIn method
            TransitionComponent.TransitionIn();
        }

        public void TransitionOut()
        {
            // Use the generic TransitionOut method
            TransitionComponent.TransitionOut();
        }
    }
}
