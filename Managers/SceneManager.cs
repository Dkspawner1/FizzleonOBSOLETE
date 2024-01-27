using Fizzleon.Events.Listeners;
using Fizzleon.Events;
using Fizzleon.Scenes;
using static Fizzleon.Core.Data.GameState;
using System.Collections.Generic;
using System;
using Fizzleon.ECS.Systems;

namespace Fizzleon.Managers;

public class SceneManager : List<IScene>
{
    public IScene CurrentScene { get; set; }
    public ContentInitializationSystem ContentInitializationSystem { get; private set; }

    public event EventHandler<SceneChangedEventArgs> SceneChanged;
    private SceneChangeListener sceneChangeListener;


    protected GameScene gameScene;
    protected MenuScene menuScene;

    protected Dictionary<GameStates, IScene> scenes;

    private TextureLoaderSystem textureLoaderSystem;

    // Constructor with textureLoaderSystem parameter
    public SceneManager(TextureLoaderSystem textureLoaderSystem, ContentManager contentManager)
    {
        if (textureLoaderSystem == null)
            throw new ArgumentNullException(nameof(textureLoaderSystem));

        this.textureLoaderSystem = textureLoaderSystem;
      

    }
    public void SetTextureLoaderSystem(TextureLoaderSystem textureLoaderSystem)
    {
        if (textureLoaderSystem == null)
            throw new ArgumentNullException(nameof(textureLoaderSystem));

        this.textureLoaderSystem = textureLoaderSystem;
    }
    public void Initialize()
    {
        menuScene = new MenuScene(textureLoaderSystem, this);
        gameScene = new GameScene(textureLoaderSystem, this);

        // Add other scenes as needed

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

        CurrentScene.LoadContent();
    }

    public void Update()
    {
        if (CurrentScene == null)
        {
            // Handle the case where CurrentScene is null, possibly log an error
            return;
        }

        CurrentScene.TransitionComponent.Update();
        CurrentScene.Update(Data.GameTime);

        if (CurrentScene.IsSceneChangeRequested)
        {
            IScene targetScene = (CurrentScene == gameScene) ? menuScene : gameScene;
            RequestSceneChange(CurrentScene, targetScene);
            CurrentScene.IsSceneChangeRequested = false;
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
        Trace.WriteLine($"Requesting scene change from {currentScene.SceneId} to {targetScene.SceneId}");

        currentScene.TransitionOut();
        Trace.WriteLine($"Transitioning out of {currentScene.SceneId}");

        currentScene.Dispose();
        Trace.WriteLine($"Disposing of {currentScene.SceneId} assets");

        targetScene.Initialize();
        targetScene.LoadContent();
        Trace.WriteLine($"Initializing and loading content for {targetScene.SceneId}");

        targetScene.TransitionIn();
        Trace.WriteLine($"Transitioning into {targetScene.SceneId}");

        ChangeScene(targetScene);
    }

    private void ChangeScene(IScene newScene)
    {
        string sceneChangeMessage = $"Changing to ({newScene.SceneId}): {newScene} test";
        SceneChanged?.Invoke(this, new SceneChangedEventArgs(sceneChangeMessage, CurrentScene?.SceneId.ToString(), newScene.SceneId.ToString()));

        IScene previousScene = CurrentScene;
        CurrentScene = null;

        previousScene.Dispose();
        previousScene.TransitionOut();

        newScene.Initialize();
        newScene.LoadContent();
        newScene.TransitionIn();

        CurrentScene = newScene;

        string sceneChangedMessage = $"Changed to ({newScene.SceneId}): {newScene}";
        SceneChanged?.Invoke(this, new SceneChangedEventArgs(sceneChangedMessage, previousScene?.SceneId.ToString(), newScene.SceneId.ToString()));
    }
}
