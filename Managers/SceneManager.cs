using System;
using System.Collections.Generic;
using Fizzleon.Events;
using Fizzleon.Events.Listeners;
using Fizzleon.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Managers
{
    public class SceneManager : List<IScene>, IGameComponent
    {
        public event EventHandler<SceneChangedEventArgs> SceneChanged;
        private SceneChangeListener sceneChangeListener;
        public IScene CurrentScene { get; set; }

        protected GameScene gameScene;
        protected MenuScene menuScene;

        protected Dictionary<GameStates, IScene> scenes;

        public void Initialize()
        {
            menuScene = new MenuScene();
            gameScene = new GameScene();

            scenes = new Dictionary<GameStates, IScene>
            {
                { GameStates.MENU, menuScene },
                { GameStates.GAME, gameScene }
            };

            foreach (var scene in scenes.Values)
            {
                scene.Initialize();
                Add(scene);
            }

            Add(menuScene);
            Add(gameScene);
            sceneChangeListener = new SceneChangeListener(this);
        }

        public void LoadContent(ContentManager Content)
        {
            ForEach(scene => scene.LoadContent(Content));
            CurrentScene = menuScene;
        }

        public void Update(GameTime gameTime)
        {
            switch (CurrentScene.SceneId)
            {
                case GameStates.MENU:
                    menuScene.Update(gameTime);

                    if (menuScene.IsGameSceneRequested)
                    {
                        RequestSceneChange(menuScene, gameScene);
                        menuScene.IsGameSceneRequested = false;
                    }
                    break;

                case GameStates.GAME:
                    gameScene.Update(gameTime);
                    if (gameScene.IsMenuSceneRequested)
                    {
                        RequestSceneChange(gameScene, menuScene);
                        gameScene.IsMenuSceneRequested = false;
                    }
                    break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            switch (CurrentScene.SceneId)
            {
                case GameStates.MENU:
                    menuScene.Draw(gameTime);
                    break;

                case GameStates.GAME:
                    gameScene.Draw(gameTime);
                    break;

                // Handle other scenes as needed

                case GameStates.SETTINGS:
                    break;

                case GameStates.EXIT:
                    break;
            }
        }

        private void RequestSceneChange(in IScene currentScene, IScene targetScene)
        {
            // Cleanup and change the scene
            Game1.Instance.Components.Clear();
            ChangeScene(targetScene);
            currentScene.Dispose();
        }

        private void ChangeScene(IScene newScene)
        {
            SceneChanged?.Invoke(this, new SceneChangedEventArgs($"Changing to ({newScene.SceneId}): {newScene}"));
            CurrentScene = newScene;

            // Load content for the new scene if needed
            newScene.LoadContent(Game1.Instance.Content); // Assuming ContentManagerInstance is available

            SceneChanged?.Invoke(this, new SceneChangedEventArgs($"Changed to ({newScene.SceneId}): {newScene}"));
        }
    }
}
