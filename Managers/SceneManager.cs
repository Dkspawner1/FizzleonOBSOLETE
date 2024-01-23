using System;
using System.Collections.Generic;
using Fizzleon.Events;
using Fizzleon.Events.Listeners;
using Fizzleon.Scenes;
using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Managers
{
    public class SceneManager : List<IScene>, IGameComponent
    {
        public event EventHandler<SceneChangedEventArgs> SceneChanged;
        private static SceneChangeListener sceneChangeListener;

        private readonly Game1 Instance;

        public IScene CurrentScene { get; set; }

        protected GameScene gameScene;
        protected MenuScene menuScene;

        protected Dictionary<GameStates, IScene> scenes;
        public SceneManager(Game1 game)
        {
            Instance = game;
        }
        public void Initialize()
        {
            menuScene = new MenuScene(Instance);
            gameScene = new GameScene(Instance);

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

            CurrentScene = menuScene;
            sceneChangeListener = new SceneChangeListener(this);
        }

        public void LoadContent()
        {
            // Load content of the new scene
            CurrentScene.LoadContent();
        }


        public void Update()
        {
            
            switch (CurrentScene.SceneId)
            {
                case GameStates.MENU:
                    menuScene.Update();

                    if (menuScene.IsGameSceneRequested)
                    {
                        RequestSceneChange(menuScene, gameScene);
                        menuScene.IsGameSceneRequested = false;
                    }
                    break;

                case GameStates.GAME:
                    gameScene.Update();
                    if (gameScene.IsMenuSceneRequested)
                    {
                        RequestSceneChange(gameScene, menuScene);
                        gameScene.IsMenuSceneRequested = false;
                    }
                    break;
            }
        }

        public void Draw()
        {
            switch (CurrentScene.SceneId)
            {
                case GameStates.MENU:
                    menuScene.Draw();
                    break;

                case GameStates.GAME:
                    gameScene.Draw();
                    break;

                case GameStates.SETTINGS:
                    break;

                case GameStates.EXIT:
                    break;
            }
        }

        // Preps the scene to change and disposes the assets
        private void RequestSceneChange<Tcurrent, TTarget>(Tcurrent currentScene, TTarget targetScene)
     where Tcurrent : IScene
     where TTarget : IScene
        {
            // Unload content of the current scene
            currentScene.Dispose();

            // Remove components related to the current scene
            for (int i = 0; i < Instance.Components.Count; i++)
            {
                if (Instance.Components[i] is Tcurrent)
                {
                    Instance.Components.RemoveAt(i);
                    i--;
                }
            }

            ChangeScene(targetScene);
        }
        private void ChangeScene(IScene newScene)
        {
            string sceneChangeMessage = $"Changing to ({newScene.SceneId}): {newScene} test";
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(sceneChangeMessage, CurrentScene?.SceneId.ToString(), newScene.SceneId.ToString()));

            // Set CurrentScene to null before unloading the content
            IScene previousScene = CurrentScene;
            CurrentScene = null;

            // Unload content of the previous scene
            previousScene.Dispose();

            // Transition out of the previous scene
            previousScene.TransitionOut();

            // Initialize the new scene
            newScene.Initialize();

            // Load content of the new scene
            newScene.LoadContent();

            // Transition into the new scene
            newScene.TransitionIn();

            // Set CurrentScene to the new scene after loading its content
            CurrentScene = newScene;

            string sceneChangedMessage = $"Changed to ({newScene.SceneId}): {newScene}";
            SceneChanged?.Invoke(this, new SceneChangedEventArgs(sceneChangedMessage, previousScene?.SceneId.ToString(), newScene.SceneId.ToString()));
        }
    }
}
