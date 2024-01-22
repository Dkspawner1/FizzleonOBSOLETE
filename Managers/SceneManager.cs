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
        //public event EventHandler<RequestSceneChangeEventArgs> RequestSceneChangeRequested;

        private static SceneChangeListener sceneChangeListener;  

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

        // Preps the scene to change and disposes the assets
        private void RequestSceneChange<Tcurrent, TTarget>(Tcurrent currentScene, TTarget targetScene) where Tcurrent : IScene, new() where TTarget : IScene, new()
        {
            Game1.Instance.Components.Clear();
            ChangeScene(targetScene);
            currentScene.Dispose();
        }

        // This event will attempt to load the content after being dispoed during the request
        private void ChangeScene(IScene newScene)
        {
            string sceneChangeMessage = $"Changing to ({newScene.SceneId}): {newScene} test";
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(sceneChangeMessage, CurrentScene?.SceneId.ToString(), newScene.SceneId.ToString()));

            CurrentScene = newScene;

            newScene.LoadContent(Game1.Instance.Content);

            string sceneChangedMessage = $"Changed to ({newScene.SceneId}): {newScene}";
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(sceneChangedMessage, CurrentScene?.SceneId.ToString(), newScene.SceneId.ToString()));
        }
    }
}
