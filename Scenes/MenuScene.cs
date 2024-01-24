using Fizzleon.ECS.Components;
using Fizzleon.ECS.Systems;
using Fizzleon.Managers;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Input.InputListeners;
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

        private TextureLoaderSystem textureLoaderSystem;
        private WorldBuilder worldBuilder { get; set; }

        // Use the generic SceneTransitionComponent
        private SceneTransitionComponent sceneTransition;
        public SceneTransitionComponent TransitionComponent => sceneTransition;

        public Game1 Instance { get; }

        private List<Rectangle> buttonsRect = new();
        private static List<MenuComponent> buttons = new List<MenuComponent>();
        private List<Entity> buttonEntities = new List<Entity>();



        public MenuScene(Game1 instance)
        {
            Instance = instance;
            sceneTransition = new SceneTransitionComponent(Instance);
            textureLoaderSystem = TextureLoaderSystem.Create(Content);

        }


        private bool isWorldDisposed = false;
        public void Initialize()
        {
            // Check if the World is already initialized or disposed
            if (World == null || isWorldDisposed)
            {
                // Create a new World and other necessary components
                var worldBuilder = new WorldBuilder()
                    .AddSystem(new RenderSystem())
                    .AddSystem(new TransitionSystem());
                var inputHandlingSystem = new InputHandlingSystem(Data.MouseListener, buttonsRect);
                worldBuilder.AddSystem(inputHandlingSystem);
                textureLoaderSystem = TextureLoaderSystem.Create(Content);
                worldBuilder.AddSystem(textureLoaderSystem);

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

            for (int i = 0; i < 3; i++)
            {
                var buttonEntity = World.CreateEntity();

                var spriteComponent = new SpriteComponent(Content.Load<Texture2D>("textures/btn0"));
                buttonEntity.Attach(spriteComponent);

                // Create and attach a new MenuComponent to the buttonEntity
                var menuComponent = new MenuComponent(spriteComponent?.Texture, new Rectangle(100,100,100,100)); // You might need to provide a proper constructor
                buttonEntity.Attach(menuComponent);

                // Add the entity to the buttonEntities list using the TextureLoaderSystem
                textureLoaderSystem.AddMenuEntity(buttonEntity);
            }

            // Now call LoadMenuItems to ensure SpriteComponents are attached
            textureLoaderSystem.LoadMenuItems(buttonEntities);

            for (int i = 0; i < buttonEntities.Count; i++)
            {
                var buttonEntity = buttonEntities[i];

                // Retrieve the MenuComponent from the buttonEntity
                var menuComponent = buttonEntity.Get<MenuComponent>();

                // Load the texture for the MenuComponent using the TextureLoaderSystem
                var buttonTexture = textureLoaderSystem.LoadTexture(buttonEntity, $"textures/btn{i}");
                var buttonRectangle = new Rectangle(0, 125 + i * 150, buttonTexture.Width / 4, buttonTexture.Height / 4);

                // Update the properties of the MenuComponent
                menuComponent.Texture = buttonTexture;
                menuComponent.Rectangle = buttonRectangle;

                buttons.Add(new MenuComponent(buttonTexture, buttonRectangle));
            }
        }

        private MouseState mouse, oldMouse;
        private Rectangle mouseRect;

        public void Update()
        {
            oldMouse = mouse;
            mouse = Mouse.GetState();
            mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            // Update using the MenuComponent
            for (int i = 0; i < buttons.Count; i++)
            {
                if (mouseRect.Intersects(buttons[i].Rectangle) && mouse.LeftButton == ButtonState.Pressed)
                {
                    IsSceneChangeRequested = true;
                }
            }

            if (buttons.Count >= 3 && mouseRect.Intersects(buttons[2].Rectangle) && mouse.LeftButton == ButtonState.Pressed)
            {
                Window.Exit = true;
            }

            World.Update(Data.GameTime);
        }
        public void Draw()
        {
            Data.SpriteBatch.Begin();

            for (var i = 0; i < buttons.Count; i++)
            {
                Data.SpriteBatch.Draw(buttons[i].Texture, buttons[i].Rectangle, Color.White);

                // If the mouse intersects with a button, you might want to highlight it
                if (mouseRect.Intersects(buttons[i].Rectangle))
                {
                    Data.SpriteBatch.Draw(buttons[i].Texture, buttons[i].Rectangle, Color.DarkGray);
                }
            }

            // Draw other components if needed

            // Draw the scene transition component or any other overlays
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
