using System;
using Fizzleon.Scenes;

namespace Fizzleon.Events.Args;
    public class SceneChangeEventArgs(IScene currentScene, IScene targetScene) : EventArgs
    {
        public IScene CurrentScene { get; } = currentScene;
        public IScene TargetScene { get; } = targetScene;
    }
