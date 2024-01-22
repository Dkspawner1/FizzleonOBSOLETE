
using System;
using System.Collections.Generic;
using Fizzleon.Events;
using Fizzleon.Events.Listeners;
using Fizzleon.Scenes;
using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Managers;
public class SceneManager : List<IScene>, IGameComponent
{
    public event EventHandler<SceneChangedEventArgs> SceneChanged;
    private SceneChangeListener sceneChangeListener;
    public IScene CurrentScene { get; set; }

    protected GameScene gameScene;
    protected MenuScene menuScene;

    protected Dictionary<GameStates, IScene> scenes;

    public void Initialize()
    {
        menuScene = new MenuScene();
        gameScene = new GameScene();

        scenes = new Dictionary<GameStates, IScene>{
        { GameStates.MENU, menuScene },
        { GameStates.GAME, gameScene }};
        
        foreach (var scene in scenes.Values)
        {
            scene.Initialize();
            Add(scene);

        }

        Add(menuScene);
        Add(gameScene);
        sceneChangeListener = new SceneChangeListener(this);
    }
    public SceneManager()
    {

    }
    public void LoadContent(ContentManager Content)
    {
        ForEach(scene => scene.LoadContent(Content));
        CurrentScene = menuScene;
    }
    public void Update(GameTime gameTime)
    {
        switch (CurrentScene.SceneId)
        {
            case GameStates.MENU:
                menuScene.Update(gameTime);

                if (menuScene.IsGameSceneRequested)
                {
                    Game1.Instance.Components.Clear();
                    ChangeScene(gameScene);
                    menuScene.IsGameSceneRequested = !menuScene.IsGameSceneRequested;
                }
                break;
            case GameStates.GAME:
                gameScene.Update(gameTime);
                if (gameScene.IsMenuSceneRequested)
                {
                    Game1.Instance.Components.Clear();
                    ChangeScene(menuScene);
                    gameScene.IsMenuSceneRequested = !gameScene.IsMenuSceneRequested;
                }
                break;
            case GameStates.SETTINGS:
                break;
            case GameStates.EXIT:
                break;
            default:
                break;
        }

    }

    private void ChangeScene(IScene newScene)
    {
        SceneChanged?.Invoke(this, new SceneChangedEventArgs($"Changing to ({newScene.SceneId}):{newScene.ToString()}"));
        // TODO: once we find a way to unload content we will load it again here
        // newScene.LoadContent(ContentManagerInstance); // Assuming ContentManagerInstance is available
        CurrentScene = newScene;
        SceneChanged?.Invoke(this, new SceneChangedEventArgs($"Changed to ({newScene.SceneId}):{newScene.ToString()}"));

    }

    public void Draw(GameTime gameTime)
    {
        switch (CurrentScene.SceneId)
        {
            case GameStates.MENU:
                menuScene.Draw(gameTime);
                break;
            case GameStates.GAME:
                gameScene.Draw(gameTime);
                break;
            case GameStates.SETTINGS:
                break;
            case GameStates.EXIT:
                break;
        }
    }



}