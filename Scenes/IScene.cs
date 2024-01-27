using System;
using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using static Fizzleon.Core.Data;

namespace Fizzleon.Scenes
{
    public interface IScene : IDisposable
    {
        GameState.GameStates SceneId { get; }
        World World { get; set; }
        Entity SceneEntity { get; }
        SceneTransitionComponent TransitionComponent { get; set; }

        void Initialize();
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw();
        void TransitionIn();
        void TransitionOut();

        bool IsSceneChangeRequested { get; set; }
    }
}