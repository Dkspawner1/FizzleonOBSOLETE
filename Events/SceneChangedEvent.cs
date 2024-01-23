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
}
