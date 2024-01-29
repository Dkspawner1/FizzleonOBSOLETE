using Fizzleon.ECS.Components;
using Fizzleon.ECS.Entities;
using Fizzleon.ECS.Systems;
using MonoGame.Extended.Entities;
using System;
using System.Collections.Generic;
using static Fizzleon.Core.Data;

namespace Fizzleon.Scenes
{

    public class MenuScene(TextureLoaderSystem textureLoaderSystem) : IScene
    {
        public GameState.GameStates SceneId => GameState.GameStates.MENU;
        public World World { get; set; }
        public bool IsSceneChangeRequested { get; set; }
   
        public SceneTransitionComponent TransitionComponent { get; set; } = new();

        public bool ButtonPressed { get; set; }

        private Entity sceneEntity;

        private static readonly List<ButtonEntity> ButtonEntities = new();


        private readonly SceneTransitionSystem transitionSystem = new();

        private bool isWorldDisposed;

        public bool IsInitialized { get; set; }

        public void Initialize()
        {
            if (World == null || isWorldDisposed)
            {
                var worldBuilder = new WorldBuilder()
                    .AddSystem(textureLoaderSystem)
                    .AddSystem(transitionSystem)
                    .AddSystem(new RenderSystem());

                World = worldBuilder.Build();

                sceneEntity = World.CreateEntity();
                sceneEntity.Attach(TransitionComponent);

                isWorldDisposed = false;

                ButtonEntities.Clear();

                if (ButtonEntities.Count == 0)
                {
                    ButtonEntities.Add(new ButtonEntity(sceneEntity, textureLoaderSystem.Load<Texture2D>("textures/btn0"), new Rectangle(100, 100, 200, 50),World));
                    ButtonEntities.Add(new ButtonEntity(sceneEntity, textureLoaderSystem.Load<Texture2D>("textures/btn1"), new Rectangle(100, 200, 200, 50), World));
                    ButtonEntities.Add(new ButtonEntity(sceneEntity, textureLoaderSystem.Load<Texture2D>("textures/btn2"), new Rectangle(100, 300, 200, 50), World));
                }

                LoadContent();

                isWorldDisposed = false;
#if RELEASE
        TransitionComponent.CurrentTransitionState = SceneTransitionComponent.TransitionState.TransitionIn;
#endif
            }
            else
            {
                IsSceneChangeRequested = false;
                IsInitialized = true;
            }
        }

        public void LoadContent()
        {
            LoadButtons();
            textureLoaderSystem.ReloadTextures(ButtonEntities.Select(buttonEntity => buttonEntity.SceneEntity));
        }
        private void LoadButtons()
        {
            // Iterate through each button entity
            foreach (var buttonEntity in ButtonEntities)
            {
                var spriteComponent = buttonEntity.SpriteComponent;

                if (spriteComponent != null && spriteComponent.Texture != null)
                {
                    var entitySpriteComponent = buttonEntity.SceneEntity.Get<SpriteComponent>();

                    if (entitySpriteComponent == null)
                    {
                        // If not, create and attach a new SpriteComponent
                        buttonEntity.SceneEntity.Attach(new SpriteComponent(spriteComponent.Texture));
                    }
                    else
                    {
                        // If it has a SpriteComponent, simply update the texture
                        entitySpriteComponent.Texture = spriteComponent.Texture;
                    }

                    // Reload the texture into the texture loader system
                    textureLoaderSystem.ReloadTexture(buttonEntity.SceneEntity, spriteComponent.Texture.Name);
                }
                else
                {
                    throw new InvalidOperationException("Button entity has a null texture or SpriteComponent.");
                }
            }
        }

        private MouseState mouse, oldMouse;
        private Rectangle mouseRect;

        public void Update(GameTime gameTime)
        {
            TransitionComponent.Update(gameTime);

            oldMouse = mouse;
            mouse = Mouse.GetState();
            mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            if (!IsSceneChangeRequested)
            {
                if (mouseRect.Intersects(ButtonEntities[0].Rectangle) && mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed)
                {
                    Trace.WriteLine($"Update: Play Button {ButtonEntities.First()} pressed");

                    TransitionComponent.CurrentTransitionState = SceneTransitionComponent.TransitionState.TransitionOut;

                    IsSceneChangeRequested = true;
                    ButtonPressed = true;
                }
                else if (ButtonEntities.Count > 2 && mouseRect.Intersects(ButtonEntities[2].Rectangle) && mouse.LeftButton == ButtonState.Released && oldMouse.LeftButton == ButtonState.Pressed)
                {
                    Trace.WriteLine("Update: Exit button pressed");
                    Window.Exit = true;
                }
            }

            // Only update the world if there is no scene change requested
            if (!IsSceneChangeRequested)
            {
                World.Update(gameTime);
            }
        }
        public void Draw()
        {
            if (World is null)
                return;

            World.Draw(Data.GameTime);
            Graphics.GraphicsDevice.Clear(Color.DarkBlue * 0.2f);

            Data.SpriteBatch.Begin();

            for (var i = 0; i < ButtonEntities.Count; i++)
            {
                if (ButtonEntities[i] != null && ButtonEntities[i].SceneEntity != null)
                {
                    var spriteComponent = ButtonEntities[i].SceneEntity.Get<SpriteComponent>();

                    if (spriteComponent != null && spriteComponent.Texture != null)
                    {
                        Data.SpriteBatch.Draw(spriteComponent.Texture, ButtonEntities[i].Rectangle, Color.White);

                        if (mouseRect.Intersects(ButtonEntities[i].Rectangle))
                        {
                            Data.SpriteBatch.Draw(spriteComponent.Texture, ButtonEntities[i].Rectangle, Color.DarkGray);
                        }
                    }
                    else
                    {
                        Trace.WriteLine("SpriteComponent or its texture is null.");
                    }
                }
                else
                {
                    Trace.WriteLine("ButtonEntity or its SceneEntity is null.");
                }
            }

            TransitionComponent?.DrawTransition(Data.SpriteBatch);

            Data.SpriteBatch.End();
        }


        public void Dispose()
        {
            ButtonEntities.Clear();
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
            Initialize();
            TransitionComponent.TransitionIn();
        }

        public void TransitionOut()
        {
            TransitionComponent.TransitionOut(this);
        }
    }
}
