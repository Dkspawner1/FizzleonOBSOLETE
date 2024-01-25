using Fizzleon.ECS.Components;
using Fizzleon.ECS.Systems;
using Fizzleon.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Entities;
using System;
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


        private SceneTransitionComponent sceneTransition;
        public SceneTransitionComponent TransitionComponent => sceneTransition;

        private readonly TextureLoaderSystem textureLoaderSystem;
        private ContentInitializationSystem contentInitializationSystem;

        private List<Rectangle> buttonsRect = new();
        private static List<MenuComponent> buttons = new List<MenuComponent>();
        private List<Entity> buttonEntities = new List<Entity>();
        private TransitionSystem transitionSystem;


 
        private List<Texture2D> buttonTextures = new List<Texture2D>();
        private List<Rectangle> buttonRectangles = new List<Rectangle>();
        private bool isWorldDisposed = false;

        public MenuScene(TextureLoaderSystem textureLoaderSystem, SceneManager sceneManager)
        {
            this.textureLoaderSystem = textureLoaderSystem;

            contentInitializationSystem = ContentInitializationSystem.Create(Content);

            sceneTransition = new SceneTransitionComponent(textureLoaderSystem);
            transitionSystem = new TransitionSystem(sceneManager);
        }

        public void Initialize()
        {
            // Check if the World is already initialized or disposed
            if (World == null || isWorldDisposed)
            {
                // Create a new World and other necessary components
                var worldBuilder = new WorldBuilder()
      .AddSystem(new RenderSystem())
      .AddSystem(textureLoaderSystem)
      .AddSystem(contentInitializationSystem)
      .AddSystem(transitionSystem);


                World = worldBuilder.Build();
                sceneEntity = World.CreateEntity();
                sceneEntity.Attach(sceneTransition);

                LoadContent();

                isWorldDisposed = false;
            }
            else
            {
                IsSceneChangeRequested = false;
            }
        }

        public void LoadContent()
        {
            buttons.Clear();
            buttonEntities.Clear();

            for (int i = 0; i < buttonTextures.Count; i++)
            {
                var buttonEntity = World.CreateEntity();
                var menuComponent = new MenuComponent(buttonTextures[i], buttonRectangles[i]);
                buttonEntity.Attach(menuComponent);

                textureLoaderSystem.AddMenuEntity(buttonEntity);
            }

            textureLoaderSystem.LoadMenuItems(buttonEntities);

            for (int i = 0; i < buttonEntities.Count; i++)
            {
                var buttonEntity = buttonEntities[i];
                var menuComponent = buttonEntity.Get<MenuComponent>();

                var buttonTexture = textureLoaderSystem.LoadTexture(buttonEntity, $"textures/btn{i}");
                var buttonRectangle = new Rectangle(0, 125 + i * 150, buttonTexture.Width / 4, buttonTexture.Height / 4);

                menuComponent.Texture = buttonTexture;
                menuComponent.Rectangle = buttonRectangle;

                buttons.Add(menuComponent);
            }
            sceneEntity.Get<SceneTransitionComponent>().TransitionIn();
        }

        private MouseState mouse, oldMouse;
        private Rectangle mouseRect;

        public void Update(GameTime gameTime)
        {
            Trace.WriteLine($"Update: CurrentTransitionState = {TransitionComponent.CurrentTransitionState}");

            oldMouse = mouse;
            mouse = Mouse.GetState();
            mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            // Update using the MenuComponent
            for (int i = 0; i < buttons.Count; i++)
            {
                if (mouseRect.Intersects(buttons[i].Rectangle) && mouse.LeftButton == ButtonState.Pressed)
                {
                    Trace.WriteLine($"Update: Button {i} pressed");
                    IsSceneChangeRequested = true;
                }
            }

            if (buttons.Count > 2 && mouseRect.Intersects(buttons[2].Rectangle) && mouse.LeftButton == ButtonState.Pressed)
            {
                Trace.WriteLine("Update: Exit button pressed");
                Window.Exit = true;
            }

            World.Update(gameTime);
        }

        public void Draw()
        {
            Data.SpriteBatch.Begin();

            for (var i = 0; i < buttons.Count; i++)
            {
                Data.SpriteBatch.Draw(buttons[i].Texture, buttons[i].Rectangle, Color.White);


                if (mouseRect.Intersects(buttons[i].Rectangle))
                {
                    Data.SpriteBatch.Draw(buttons[i].Texture, buttons[i].Rectangle, Color.DarkGray);
                }
            }

            sceneEntity.Get<SceneTransitionComponent>().Draw(Data.SpriteBatch);

            Data.SpriteBatch.End();
        }

        public void Dispose()
        {
            buttons.Clear();
            textureLoaderSystem?.Dispose();

            // Check if the World is not null before disposing
            if (World != null)
            {
                World.Dispose();
                World = null;
            }
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
