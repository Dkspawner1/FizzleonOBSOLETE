using Fizzleon.Scenes;
using System;

namespace Fizzleon.Events
{
    public class SceneChangedEventArgs : EventArgs
    {
        public string Message { get; }
        public string PreviousScene { get; }
        public string NewScene { get; }

        public SceneChangedEventArgs(string message, string previousScene, string newScene)
        {
            Message = message;
            PreviousScene = previousScene;
            NewScene = newScene;
        }
    }

    public class SceneChangedEvent
    {
        public event EventHandler<SceneChangedEventArgs> OnSceneChanged;

        public void Raise(IScene previousScene, IScene newScene)
        {
            OnSceneChanged?.Invoke(this, new SceneChangedEventArgs("YourMessageHere", previousScene?.SceneId.ToString(), newScene.SceneId.ToString()));
        }
    }
}
