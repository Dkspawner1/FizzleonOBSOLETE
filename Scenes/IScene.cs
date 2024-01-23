using System;
using System.Collections.Generic;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Scenes
{
    public interface IScene : IGameComponent, IDisposable
    {
        public GameStates SceneId { get; }
        public World World { get; set; }
        protected internal void LoadContent();
        protected internal void Update(GameTime gameTime);
        protected internal void Draw(GameTime gameTime);
    }
}