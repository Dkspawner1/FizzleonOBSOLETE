using Fizzleon.Scenes;
using System;

namespace Fizzleon.Events
{
    public class SceneChangedEventArgs(string message, string previousScene, string newScene) : EventArgs
    {
        public string Message { get; } = message;
        public string PreviousScene { get; } = previousScene;
        public string NewScene { get; } = newScene;
    }
}
