using Fizzleon.Events.Listeners;
using Fizzleon.Events;
using Fizzleon.Scenes;
using static Fizzleon.Core.Data.GameState;
using System.Collections.Generic;
using System;

namespace Fizzleon.Managers;

public class SceneManager : List<IScene>, IGameComponent
{
    public event EventHandler<SceneChangedEventArgs> SceneChanged;
    private SceneChangeListener sceneChangeListener;

    private readonly Game1 Instance;

    public IScene CurrentScene { get; set; }

    protected GameScene gameScene;
    protected MenuScene menuScene;

    protected Dictionary<GameStates, IScene> scenes;

    public SceneManager(Game1 game)
    {
        Instance = game;
    }

    public void Initialize()
    {
        menuScene = new MenuScene(Instance);
        gameScene = new GameScene(Instance);

        scenes = new Dictionary<GameStates, IScene>
        {
            { GameStates.MENU, menuScene },
            { GameStates.GAME, gameScene }
            // Add other scenes as needed
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
        // Load content of the new scene if needed
        // CurrentScene.LoadContent(); // Commented out for now
    }

    public void Update()
    {
        CurrentScene.Update();

        if (CurrentScene is GameScene)
        {
            if (gameScene.IsSceneChangeRequested)
            {
                RequestSceneChange(CurrentScene, menuScene);
                gameScene.IsSceneChangeRequested = false;
            }
        }
        else if (CurrentScene is MenuScene)
        {
            if (menuScene.IsSceneChangeRequested)
            {
                RequestSceneChange(CurrentScene, gameScene);
                menuScene.IsSceneChangeRequested = false;
            }
        }
    }


    public void Draw()
    {
        // Draw the current scene
        CurrentScene.Draw();
    }

    // Preps the scene to change and disposes the assets
    private void RequestSceneChange(IScene currentScene, IScene targetScene)
    {
        currentScene.World.Dispose();
        currentScene.Dispose();
        Instance.Components.Clear();

        targetScene.Initialize();

        ChangeScene(targetScene);
    }

    private void ChangeScene(IScene newScene)
    {
        string sceneChangeMessage = $"Changing to ({newScene.SceneId}): {newScene} test";
        SceneChanged?.Invoke(this, new SceneChangedEventArgs(sceneChangeMessage, CurrentScene?.SceneId.ToString(), newScene.SceneId.ToString()));

        // Set CurrentScene to null before unloading the content
        IScene previousScene = CurrentScene;
        CurrentScene = null;

        // Unload content of the previous scene
        previousScene.Dispose();

        // Transition out of the previous scene
        previousScene.TransitionOut();

        // Initialize the new scene
        newScene.Initialize();

        // Load content of the new scene
        newScene.LoadContent();

        // Transition into the new scene
        newScene.TransitionIn();

        // Set CurrentScene to the new scene after loading its content
        CurrentScene = newScene;

        string sceneChangedMessage = $"Changed to ({newScene.SceneId}): {newScene}";
        SceneChanged?.Invoke(this, new SceneChangedEventArgs(sceneChangedMessage, previousScene?.SceneId.ToString(), newScene.SceneId.ToString()));
    }
}
