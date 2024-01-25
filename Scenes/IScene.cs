using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using static Fizzleon.Core.Data;
using System;
using static Fizzleon.ECS.Components.SceneTransitionComponent;

public interface IScene : IDisposable
{
    GameState.GameStates SceneId { get; }
    TransitionState CurrentTransitionState { get; }
    World World { get; set; }
    Entity SceneEntity { get; }
    SceneTransitionComponent TransitionComponent { get; }

    abstract void Initialize();
    abstract void LoadContent();
    abstract void Update(GameTime gameTime);
    abstract void Draw();
    abstract void TransitionIn();
    abstract void TransitionOut();

    bool IsSceneChangeRequested { get; set; }
}