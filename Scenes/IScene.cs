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

    void Initialize();
    void LoadContent();
    void Update();
    void Draw();
    void TransitionIn();
    void TransitionOut();

    bool IsSceneChangeRequested { get; set; }
}