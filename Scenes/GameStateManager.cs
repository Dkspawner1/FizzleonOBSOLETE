using System;
using System.Collections.Generic;
using System.Diagnostics;
using MonoGame.Extended.Entities;

using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Scenes
{
    internal class GameStateManager : List<IScene>
    {
        private IScene currentScene;

        private GameScene gameScene;
        private MenuScene menuScene;


        public void InitializeScenes()
        {

            menuScene = new MenuScene();
            gameScene = new GameScene();
        }


        public void AddSceneWithEntities(IScene scene, params Entity[] entities)
        {
            scene.Entities.AddRange(entities);
            Add(scene);
        }


        public void LoadContent(ContentManager Content)
        {
            menuScene = new MenuScene();
            gameScene = new GameScene();

            Add(menuScene);
            Add(gameScene);

            ForEach(scene => scene.LoadContent(Content));
            SetCurrentScene(menuScene);
        }



        public void Update(GameTime gameTime)
        {
            var gameComponents = Game1.Instance.Components;
            
            switch (currentScene.SceneId)
            {
                case GameStates.MENU:
                    menuScene.Update(gameTime);

                    if (menuScene.SwitchToGameScene)
                    {
                        Game1.Instance.Components.Clear();
                        SetCurrentScene(gameScene);
                    }

                    break;
                case GameStates.GAME:
                    gameScene.Update(gameTime);
                    break;
                case GameStates.SETTINGS:
                    break;
                case GameStates.EXIT:
                    break;
                default:
                    break;
            }

        }

 

        public void Draw(GameTime gameTime)
        {
            switch (currentScene.SceneId)
            {
                case GameStates.MENU:
                    menuScene.Draw(gameTime);
                    break;
                case GameStates.GAME:
                    gameScene.Draw(gameTime);
                    break;
                case GameStates.SETTINGS:
                    break;
                case GameStates.EXIT:
                    break;
            }
        }

        public void SetCurrentScene(IScene scene) => currentScene = scene;
    }
}