using Fizzleon.ECS.Components;
using Fizzleon.ECS.Entities;
using Fizzleon.ECS.Systems;
using Fizzleon.Managers;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using static Fizzleon.Core.Data;
using static SceneTransitionComponent;

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

        private static List<ButtonEntity> buttonEntities = new List<ButtonEntity>();


        private readonly TextureLoaderSystem textureLoaderSystem;
        private ContentInitializationSystem contentInitializationSystem;


        private SceneTransitionComponent sceneTransition;
        public SceneTransitionComponent TransitionComponent => sceneTransition;
        private SceneTransitionSystem transitionSystem;

        private bool isWorldDisposed = false;

        public MenuScene(TextureLoaderSystem textureLoaderSystem, SceneManager sceneManager, ContentInitializationSystem contentInitializationSystem)
        {
            this.textureLoaderSystem = textureLoaderSystem;
            this.contentInitializationSystem = contentInitializationSystem;

            this.textureLoaderSystem = textureLoaderSystem;

            contentInitializationSystem = ContentInitializationSystem.Create(Content);

            sceneTransition = new SceneTransitionComponent(contentInitializationSystem.Load<Texture2D>("textures/btn0"));

            transitionSystem = new SceneTransitionSystem(sceneManager);
        }

        public void Initialize()
        {
            if (World == null || isWorldDisposed)
            {
                var worldBuilder = new WorldBuilder()
                    .AddSystem(new RenderSystem())
                    .AddSystem(textureLoaderSystem)
                    .AddSystem(contentInitializationSystem)
                    .AddSystem(transitionSystem);

                World = worldBuilder.Build();
                sceneEntity = World.CreateEntity();
                sceneEntity.Attach(sceneTransition);

                buttonEntities.Clear();

                buttonEntities.Add(new ButtonEntity(sceneEntity, textureLoaderSystem.LoadTexture("textures/btn0"), new Rectangle(100, 100, 200, 50)));
                buttonEntities.Add(new ButtonEntity(sceneEntity, textureLoaderSystem.LoadTexture("textures/btn1"), new Rectangle(100, 200, 200, 50)));
                buttonEntities.Add(new ButtonEntity(sceneEntity, textureLoaderSystem.LoadTexture("textures/btn2"), new Rectangle(100, 300, 200, 50)));

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
            // Iterate through each button entity
            foreach (var buttonEntity in buttonEntities)
            {
                // Check if the button entity has a valid texture
                if (buttonEntity.Texture != null)
                {
                    // Reload the texture into the texture loader system
                    textureLoaderSystem.ReloadTexture(buttonEntity.SceneEntity, buttonEntity.Texture.Name);

                    // Access the SpriteComponent of the button entity
                    var spriteComponent = buttonEntity.SceneEntity.Get<SpriteComponent>();

                    // Check if the entity has a SpriteComponent
                    if (spriteComponent == null)
                    {
                        // If not, create and attach a new SpriteComponent
                        spriteComponent = new SpriteComponent(buttonEntity.Texture);
                        buttonEntity.SceneEntity.Attach(spriteComponent);
                    }
                    else
                    {
                        // If it already has a SpriteComponent, update its texture
                        spriteComponent.Texture = buttonEntity.Texture;
                    }

                    // Additional logic for the SpriteComponent if needed
                }
                else
                {
                    throw new InvalidOperationException("Button entity has a null texture.");
                }
            }
        }

        private MouseState mouse, oldMouse;
        private Rectangle mouseRect;

        public void Update(GameTime gameTime)
        {
            Trace.WriteLine($"Update: CurrentTransitionState = {TransitionComponent.CurrentTransitionState}");

            oldMouse = mouse;
            mouse = Mouse.GetState();
            mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            for (int i = 0; i < buttonEntities.Count; i++)
            {
                if (mouseRect.Intersects(buttonEntities[i].Rectangle) && mouse.LeftButton == ButtonState.Pressed)
                {
                    Trace.WriteLine($"Update: Button {i} pressed");
                    IsSceneChangeRequested = true;
                }
            }

            if (buttonEntities.Count > 2 && mouseRect.Intersects(buttonEntities[2].Rectangle) && mouse.LeftButton == ButtonState.Pressed)
            {
                Trace.WriteLine("Update: Exit button pressed");
                Window.Exit = true;
            }

        }

        public void Draw()
        {
            TransitionComponent.Update();

            Data.SpriteBatch.Begin();

            for (var i = 0; i < buttonEntities.Count; i++)
            {
                Data.SpriteBatch.Draw(buttonEntities[i].Texture, buttonEntities[i].Rectangle, Color.White);

                if (mouseRect.Intersects(buttonEntities[i].Rectangle))
                {
                    Data.SpriteBatch.Draw(buttonEntities[i].Texture, buttonEntities[i].Rectangle, Color.DarkGray);
                }
            }

            Data.SpriteBatch.End();
        }

        public void Dispose()
        {
            buttonEntities.Clear();
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
