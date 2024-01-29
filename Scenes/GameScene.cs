using System.Collections.Generic;
using Fizzleon.ECS.Components;
using Fizzleon.ECS.Entities;
using Fizzleon.ECS.Systems;
using Fizzleon.Managers;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Scenes
{
    public class GameScene(TextureLoaderSystem textureLoaderSystem) : IScene
    {
        public GameStates SceneId => GameStates.GAME;
        public World World { get; set; }

        public SceneTransitionComponent TransitionComponent { get; set; }

        public bool IsSceneChangeRequested { get; set; }

        private List<Player> Players { get; set; }
        private KeyboardState kb;

        private readonly List<ISystem> systemsList = new();
        public bool D1KeyPressed { get; set; }
        public bool IsInitialized { get; set; }

        //TransitionComponent.LoadContent(textureLoaderSystem.Load<Texture2D>("Textures/btn0"));


        public void AddSystem(ISystem system)
        {

            systemsList.Add(system);
        }
        public void Initialize()
        {
            WorldBuilder worldBuilder = new WorldBuilder();
            foreach (var system in systemsList)
            {
                worldBuilder.AddSystem(system);
            }
            World = worldBuilder.Build();

            Players = new List<Player>
            {
                new Player(World, new Vector2(300, 500), 200f),
                new Player(World, new Vector2(700, 200), 175f),
            };

            foreach (var player in Players)
            {
                player.Entity.Attach(new TransformComponent(new Vector2(100, 100)));
                player.LoadContent(textureLoaderSystem, "Textures/Warrior_Sheet-Effect", "Textures/Warrior_Sheet-Effect.sf");
            }

            IsInitialized = true;

        }
        public void LoadContent()
        {
            var playerTextures = Players
                .Select(player => player.Entity?.Get<SpriteComponent>()?.Texture)
                .Where(texture => texture != null);

            // Check if the system is available
            if (systemsList.FirstOrDefault(s => s is TextureLoaderSystem) is TextureLoaderSystem)
            {
                
                textureLoaderSystem.LoadEntities(Players.Select(player => player.Entity), playerTextures.Select(texture => texture.Name));
            }
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();

            foreach (var player in Players)
            {
                player.Update("runRight", "idleRight");
            }

            World.Update(gameTime);
        }

        private void HandleInput()
        {
            kb = Keyboard.GetState();

            // Check if 'D1' key is pressed and cooldown has elapsed
            if (kb.IsKeyDown(Keys.D1) && !IsSceneChangeRequested)
            {
                IsSceneChangeRequested = true;
                Trace.WriteLine("Scene change requested!");
            }
        }

        public void Draw()
        {
            World.Draw(Data.GameTime);
            TransitionComponent?.DrawTransition(Data.SpriteBatch);
        }

        public void Dispose()
        {
            foreach (var player in Players)
            {
                player.Dispose();
            }

            Players.Clear();
            World.Dispose();
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
