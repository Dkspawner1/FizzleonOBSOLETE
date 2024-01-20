using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using MonoGame.Extended.Entities.Systems;

using static Fizzleon.Core.Data.GameState;
using static Fizzleon.Core.Data.GameState.GameStates;

namespace Fizzleon.Scenes
{
    internal class GameStateManager : IScene
    {
        private readonly List<IScene> scenes = new List<IScene>();
        private IScene currentScene;
      

        public void AddScene(IScene scene)
        {
            scenes.Add(scene);
        }

        List<EntitySystem> IScene.Entities => null;

        List<EntitySystem> IScene.Systems => null;

        public void LoadContent()
        {
            var interfaceMethods = typeof(IScene)
                .GetInterfaces()
                .Select(x => typeof(IScene).GetInterfaceMap(x))
                .SelectMany(x => x.TargetMethods).ToArray();

            var propsNotFromInterface = typeof(IScene)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy)
                .Where(x => !x.GetAccessors(true).Any(y => interfaceMethods.Contains(y))).ToArray();

            Trace.WriteLine(propsNotFromInterface.Length);
        }

        public void Update(float deltaTime)
        {
            switch (CurrentState)
            {
                case MENU:
                    break;
                case GAME:
                    break;
                case SETTINGS:
                    break;
                case EXIT:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch (CurrentState)
            {
                case MENU:
                    break;
                case GAME:
                    break;
                case SETTINGS:
                    break;
                case EXIT:
                    break;
            }
        }
    }
}