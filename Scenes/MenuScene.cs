using Fizzleon.ECS.Components;
using Fizzleon.ECS.Entities;
using Fizzleon.ECS.Systems;
using Fizzleon.Managers;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using static Fizzleon.Core.Data;

namespace Fizzleon.Scenes
{
    
    public class MenuScene : IScene
    {
        public GameState.GameStates SceneId => GameState.GameStates.MENU;
        public World World { get; set; }
        public bool IsSceneChangeRequested { get; set; }


        private Entity sceneEntity;
        public Entity SceneEntity => sceneEntity;

        private static readonly List<ButtonEntity> buttonEntities = new();

        private readonly TextureLoaderSystem textureLoaderSystem;


        public SceneTransitionComponent TransitionComponent { get; set; }


        private readonly SceneTransitionSystem transitionSystem;

        private bool isWorldDisposed;

        public MenuScene(TextureLoaderSystem textureLoaderSystem)
        {
            this.textureLoaderSystem = textureLoaderSystem;
            TransitionComponent = new SceneTransitionComponent(Data.ContentInitializationSystem.Load<Texture2D>("textures/btn0"));
            transitionSystem = new SceneTransitionSystem();
        }


        public void Initialize()
        {
            if (World == null || isWorldDisposed)
            {
                var worldBuilder = new WorldBuilder()
                    .AddSystem(textureLoaderSystem)
                    .AddSystem(new RenderSystem())
                    .AddSystem(transitionSystem);

                World = worldBuilder.Build();
               
                sceneEntity = World.CreateEntity();
                sceneEntity.Attach(TransitionComponent);

                buttonEntities.Clear();

                if (buttonEntities.Count == 0)
                {
                    buttonEntities.Add(new ButtonEntity(sceneEntity, textureLoaderSystem.Load<Texture2D>("textures/btn0"), new Rectangle(100, 100, 200, 50)));
                    buttonEntities.Add(new ButtonEntity(sceneEntity, textureLoaderSystem.Load<Texture2D>("textures/btn1"), new Rectangle(100, 200, 200, 50)));
                    buttonEntities.Add(new ButtonEntity(sceneEntity, textureLoaderSystem.Load<Texture2D>("textures/btn2"), new Rectangle(100, 300, 200, 50)));
                }

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
            LoadButtons();
          
        }
        private void LoadButtons()
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
            TransitionComponent.TransitionIn();
        }

        public void TransitionOut()
        {
            TransitionComponent.TransitionOut();
        }
    }
}
