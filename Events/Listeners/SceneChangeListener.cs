using Fizzleon.Events.Args;
using Fizzleon.Managers;
using System;

namespace Fizzleon.Events.Listeners;

public class SceneChangeListener
{
    private readonly SceneManager sceneManager;

    public SceneChangeListener(SceneManager manager)
    {
        sceneManager = manager;
        sceneManager.SceneChanged += HandleSceneChanged;
    }

    private void HandleSceneChanged(object sender, TransitionEventArgs e)
    {
        // Your logic for handling scene changes
        Trace.WriteLine($"Scene change: {e.PreviousSceneId} -> {e.NewSceneId}");
    }
}
