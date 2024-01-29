using System;
using System.Collections.Generic;
using Fizzleon.ECS.Systems;
using static Fizzleon.Core.Data.GameState;
using Fizzleon.ECS.Components;
using Fizzleon.Events.Args;
using Fizzleon.Scenes;

namespace Fizzleon.Managers
{
    public class SceneManager(GraphicsDeviceManager graphics) : IDisposable
    {
        public IScene CurrentScene { get; private set; }
        private Dictionary<GameStates, IScene> Scenes { get; set; } = new();

        private SceneTransitionSystem TransitionSystem { get; set; } = new();
        private SceneTransitionManager TransitionManager { get; set; } = new();
        public event EventHandler<TransitionEventArgs> SceneChanged;

        public void SubscribeToSceneEvents()
        {
            TransitionManager.SceneChangeRequested += OnSceneChangeRequested;
            TransitionManager.SceneChangeCompleted += OnSceneChangeCompleted;
        }
        public void InitializeScenes()
        {

            TextureLoaderSystem textureLoaderSystem = new TextureLoaderSystem(Data.ContentManager);

            // Initialize and load content for the menu scene
            Scenes[GameStates.MENU] = new MenuScene(textureLoaderSystem);
            Scenes[GameStates.MENU].Initialize();

            // ... other scene initialization logic
            Scenes[GameStates.GAME] = new GameScene(textureLoaderSystem);
            Scenes[GameStates.GAME].Initialize();

            // Set the initial scene
            CurrentScene = Scenes[GameStates.MENU];
            TransitionSystem.Initialize(CurrentScene.World);
            TransitionSystem.Initialize(Scenes[GameStates.GAME].World);


        }
        public void LoadContent()
        {
            // Set up event handlers
            TransitionManager.SceneChangeRequested += OnSceneChangeRequested;
            TransitionManager.SceneChangeCompleted += OnSceneChangeCompleted;
            CurrentScene.LoadContent();
        }
        private void OnSceneChangeRequested(object sender, SceneChangeEventArgs e)
        {
            IScene targetScene = Scenes[e.TargetScene.SceneId];
            TransitionToScene(targetScene);
        }

        private void OnSceneChangeCompleted(object sender, SceneChangeEventArgs e)
        {
            IScene targetScene = Scenes[e.TargetScene.SceneId];
            ChangeScene(targetScene);
        }

        private void TransitionToScene(IScene targetScene)
        {
            if (targetScene.TransitionComponent == null)
            {
                targetScene.TransitionComponent = new SceneTransitionComponent(); // Initialize if null.
            }

            targetScene.TransitionComponent.CurrentTransitionState = SceneTransitionComponent.TransitionState.TransitionIn;


            if (!targetScene.IsInitialized)
            {
                targetScene.Initialize();
            }

            targetScene.LoadContent();

            // Dispose of the current scene assets after transitioning out
            CurrentScene.TransitionComponent.TransitionOutComplete += (s, args) =>
            {
                CurrentScene.Dispose();
            };
        }

        private void ChangeScene(IScene newScene)
        {
            // Invoke the SceneChanged event
            SceneChanged?.Invoke(this, new TransitionEventArgs(
                SceneTransitionComponent.TransitionState.TransitionOut,
                SceneTransitionComponent.TransitionState.TransitionIn,
                $"Changing to ({newScene.SceneId}): {newScene}",
                CurrentScene?.SceneId.ToString(),
                newScene.SceneId.ToString()
            ));

            // Dispose of the previous scene and transition out
            CurrentScene?.Dispose();
            CurrentScene?.TransitionOut();

            // Initialize, load content, and transition in the new scene
            newScene.Initialize();
            newScene.LoadContent();
            newScene.TransitionIn();
            CurrentScene = newScene;

            // Reset the scene change flag
            CurrentScene.IsSceneChangeRequested = false;

            // Invoke the SceneChanged event again for the new scene
            SceneChanged?.Invoke(this, new TransitionEventArgs(
                SceneTransitionComponent.TransitionState.TransitionOut,
                SceneTransitionComponent.TransitionState.TransitionIn,
                $"Changed to ({newScene.SceneId}): {newScene}",
                CurrentScene?.SceneId.ToString(),
                newScene.SceneId.ToString()
            ));
        }

     

        public void Update()
        {
            Data.CurrentScene = CurrentScene;

            // Check if a scene change is requested
            if (CurrentScene.IsSceneChangeRequested)
            {
                IScene targetScene = (CurrentScene == Scenes[GameStates.GAME]) ? Scenes[GameStates.MENU] : Scenes[GameStates.GAME];

                // Check the source scene and trigger the transition only when needed
                if ((CurrentScene == Scenes[GameStates.MENU] && ((MenuScene)CurrentScene).ButtonPressed) ||
                    (CurrentScene == Scenes[GameStates.GAME] && ((GameScene)CurrentScene).D1KeyPressed))
                {
                    TransitionToScene(targetScene);
                    ResetInputFlags();
                    Trace.WriteLine($"Update: Transitioning from {CurrentScene.SceneId} to {targetScene.SceneId}");
                }
            }

            // Update and transition the current scene
            CurrentScene.Update(Data.GameTime);
            TransitionSystem.Update(Data.GameTime);

            CurrentScene.Draw();
            DrawScenes();
        }

        private void DrawScenes()
        {
            // Draw each scene and transition components
            Data.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            foreach (var scene in Scenes.Values)
            {
                scene.TransitionComponent?.DrawTransition(Data.SpriteBatch);
            }
            Data.SpriteBatch.End();
        }

        private void ResetInputFlags()
        {
            if (CurrentScene == Scenes[GameStates.MENU])
                ((MenuScene)CurrentScene).ButtonPressed = false;
            else if (CurrentScene == Scenes[GameStates.GAME]) ((GameScene)CurrentScene).D1KeyPressed = false;
        }

        public void Dispose()
        {
            foreach (var scene in Scenes.Values)
            {
                scene?.Dispose();
                TransitionManager.SceneChangeRequested -= OnSceneChangeRequested;
                TransitionManager.SceneChangeCompleted -= OnSceneChangeCompleted;
            }

            CurrentScene?.Dispose();
            TransitionSystem?.Dispose();
            graphics?.Dispose();
        }
    }
}
