using Fizzleon.ECS;
using Fizzleon.ECS.Components;
using Fizzleon.ECS.Entities;
using Fizzleon.ECS.Systems;
using MonoGame.Extended.Entities;
using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Scenes
{
    public class GameScene : IScene
    {
        public World World { get; set; }
        public GameStates SceneId => GameStates.GAME;

        private KeyboardState kb;
        public bool IsMenuSceneRequested { get; set; } = false;
        private WorldBuilder WorldBuilder { get; set; }

        private Player player;

        public void Initialize()
        {

            WorldBuilder = new WorldBuilder();
            WorldBuilder.AddSystem(new RenderSystem());
            WorldBuilder.AddSystem(new AnimationInitializationSystem());
            WorldBuilder.AddSystem(new AnimationUpdateSystem(Game1.Instance.Content));
            World = WorldBuilder.Build();
            player = new Player(World);
        }

        public void LoadContent(ContentManager Content)
        {

            player.LoadContent(Game1.Instance.Content);
        }



        public void Update(GameTime gameTime)
        {
            HandleInput();
            player.Update(gameTime);

            World.Update(gameTime);
        }

        private void HandleInput()
        {
            kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.D1))
                IsMenuSceneRequested = true;
        }
        public void Draw(GameTime gameTime)
        {
            World.Draw(gameTime);
        }
    }
}
