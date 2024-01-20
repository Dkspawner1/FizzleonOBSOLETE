using System.Collections.Generic;
using System.Diagnostics;
using MonoGame.Extended.Entities;

using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Scenes
{
    internal class GameStateManager : List<IScene>
    {
        private readonly Game1 instance;
        private IScene currentScene;

        private GameScene gameScene;
        private MenuScene menuScene;

        public GameStateManager(Game1 instance) => this.instance = instance;

        public void InitializeScenes()
        {
            menuScene = new MenuScene(instance);
            gameScene = new GameScene(instance);
        }


        public void AddSceneWithEntities(IScene scene, params Entity[] entities)
        {
            scene.Entities.AddRange(entities);
            Add(scene);
        }

        public void AddScene(IScene scene) => Add(scene);

        public void LoadContent(ContentManager Content)
        {
            menuScene = new MenuScene(instance);
            gameScene = new GameScene(instance);

            AddScene(menuScene);
            AddScene(gameScene);
            ForEach(scene => scene.LoadContent(Content));
            SetCurrentScene(menuScene);
        }



        public void Update(GameTime gameTime)
        {
            switch (currentScene)
            {
                case MenuScene:
                    currentScene = menuScene;
                    currentScene?.Update(gameTime);
                    break;
                case GameScene:
                    currentScene?.Update(gameTime);
                    break;
            }

        }

        public void Draw(GameTime gameTime)
        {
            switch (currentScene)
            {
                case MenuScene:
                    currentScene?.Draw(gameTime);
                    break;
                case GameScene:
                    currentScene?.Draw(gameTime);
                    break;
            }
        }

        public void SetCurrentScene(IScene scene)
        {
            currentScene = scene;
        }
    }
}