using System;
using Fizzleon.Events.Args;
using Fizzleon.Scenes;

namespace Fizzleon.Managers
{
    public class SceneTransitionManager
    {
        public event EventHandler<SceneChangeEventArgs> SceneChangeRequested;
        public event EventHandler<SceneChangeEventArgs> SceneChangeCompleted;

        public void RequestSceneChange(IScene currentScene, IScene targetScene)
        {
            OnSceneChangeRequested(currentScene, targetScene);
            // Logic For Transition Here: 

            OnSceneChangeCompleted(currentScene, targetScene);
        }

        private void OnSceneChangeRequested(IScene currentScene, IScene targetScene)
        {
            SceneChangeRequested?.Invoke(this, new SceneChangeEventArgs(currentScene, targetScene));
        }

        private void OnSceneChangeCompleted(IScene currentScene, IScene targetScene)
        {
            SceneChangeCompleted?.Invoke(this, new SceneChangeEventArgs(currentScene, targetScene));
        }
    }
}