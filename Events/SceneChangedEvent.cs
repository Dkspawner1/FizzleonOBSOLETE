using System;

namespace Fizzleon.Events;

public class SceneChangedEventArgs : EventArgs
{
    // public string PreviousScene { get; set; }
    public string NewScene { get; set; }
    public SceneChangedEventArgs(string newScene)
    {
        NewScene = newScene;
    }
}
public class SceneChangedEvent
{
    public event EventHandler<SceneChangedEventArgs> OnSceneChanged;

    public void Raise(string newScene) => OnSceneChanged?.Invoke(this, new SceneChangedEventArgs(newScene));

}