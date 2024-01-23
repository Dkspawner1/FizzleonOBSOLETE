using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using static Fizzleon.Core.Data.GameState;
using System;
using static Fizzleon.ECS.Components.SceneTransitionComponent<Fizzleon.Scenes.IScene>;

namespace Fizzleon.Scenes
{
    public interface IScene : IGameComponent, IDisposable
    {
        public GameStates SceneId { get; }
        public World World { get; set; }
        TransitionState CurrentTransitionState { get; }
        public TransitionComponent TransitionComponent { get; } 

        protected internal void LoadContent();
        protected internal void Update();
        protected internal void Draw();
        protected internal void TransitionIn();
        protected internal void TransitionOut();
    }
}
