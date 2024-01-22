
using Fizzleon.Scenes;
using System;

namespace Fizzleon.Events;

public class RequestSceneChangeEventArgs : EventArgs
{
    public IScene CurrentScene { get; }
    public IScene TargetScene { get; }
    public RequestSceneChangeEventArgs(IScene currentScene, IScene targetScene)
    {
        CurrentScene = currentScene;
        TargetScene = targetScene;
    }
}

