
using System;
using System.Diagnostics;
using Fizzleon.Managers;

namespace Fizzleon.Events.Listeners;
public class SceneChangeListener
{
    public SceneChangeListener(SceneManager sceneManager) => sceneManager.SceneChanged += HandleSceneChanged;


    private void HandleSceneChanged(object sender, SceneChangedEventArgs e) => Trace.WriteLine(e.NewScene.ToString());
}