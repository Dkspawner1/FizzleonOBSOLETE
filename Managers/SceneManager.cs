using Fizzleon.Events.Listeners;
using Fizzleon.Events;
using Fizzleon.Scenes;
using static Fizzleon.Core.Data.GameState;
using System.Collections.Generic;
using System;
using Fizzleon.ECS.Components;
using Fizzleon.ECS.Systems;

namespace Fizzleon.Managers;

public class SceneManager(TextureLoaderSystem loaderSystem) : List<IScene>
{
    public IScene CurrentScene { get; set; }

    public event EventHandler<SceneChangedEventArgs> SceneChanged;
    private SceneChangeListener sceneChangeListener;


    protected GameScene gameScene;
    protected MenuScene menuScene;

    protected Dictionary<GameStates, IScene> scenes;

    public void Initialize()
    {
        menuScene = new MenuScene(loaderSystem);
        gameScene = new GameScene(loaderSystem);


        scenes = new Dictionary<GameStates, IScene>
    {
        { GameStates.MENU, menuScene },
        { GameStates.GAME, gameScene }
    };

        foreach (var scene in scenes.Values)
        {
            scene.Initialize();
            Add(scene);
        }

        CurrentScene = menuScene;
        sceneChangeListener = new SceneChangeListener(this);

        
    }
    public void LoadContent()
    {

        CurrentScene.LoadContent();
    }

    public void Update()
    {

        CurrentScene.TransitionComponent.Update();
        CurrentScene.Update(Data.GameTime);

        if (!CurrentScene.IsSceneChangeRequested) return;

        IScene targetScene = (CurrentScene == gameScene) ? menuScene : gameScene;
        RequestSceneChange(CurrentScene, targetScene);
        
        Trace.WriteLine($"Transition state after request: {targetScene.TransitionComponent.CurrentTransitionState}");

    }

    public void Draw()
    {
        CurrentScene.Draw();
    }

    private void RequestSceneChange(IScene currentScene, IScene targetScene)
    {
        Trace.WriteLine($"Requesting scene change from {currentScene.SceneId} to {targetScene.SceneId}");

        // Transition out the current scene
        currentScene.TransitionOut();
        Trace.WriteLine($"Transitioning out of {currentScene.SceneId}");

        // Dispose of the current scene assets
        currentScene.Dispose();
        Trace.WriteLine($"Disposing of {currentScene.SceneId} assets");

        // Set the target scene to transition in
        targetScene.TransitionComponent.CurrentTransitionState = SceneTransitionComponent.TransitionState.TransitionIn;

        // Initialize and load content for the target scene
        targetScene.Initialize();
        targetScene.LoadContent();
        Trace.WriteLine($"Initializing and loading content for {targetScene.SceneId}");

        // Transition into the target scene
        targetScene.TransitionIn();
        Trace.WriteLine($"Transitioning into {targetScene.SceneId}");

        // Perform the actual scene change
        ChangeScene(targetScene);
    }

    private void ChangeScene(IScene newScene)
    {
        string sceneChangeMessage = $"Changing to ({newScene.SceneId}): {newScene} test";
        SceneChanged?.Invoke(this, new SceneChangedEventArgs(sceneChangeMessage, CurrentScene?.SceneId.ToString(), newScene.SceneId.ToString()));

        // Dispose of the previous scene and transition out
        IScene previousScene = CurrentScene;
        CurrentScene = null;
        previousScene.Dispose();
        previousScene.TransitionOut();

        // Initialize, load content, and transition in the new scene
        newScene.Initialize();
        newScene.LoadContent();
        newScene.TransitionIn();
        CurrentScene = newScene;

        string sceneChangedMessage = $"Changed to ({newScene.SceneId}): {newScene}";
        SceneChanged?.Invoke(this, new SceneChangedEventArgs(sceneChangedMessage, previousScene?.SceneId.ToString(), newScene.SceneId.ToString()));
    }
}
