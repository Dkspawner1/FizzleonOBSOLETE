using System.Collections.Generic;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

using static Fizzleon.Core.Data.GameState;

namespace Fizzleon.Scenes
{
    public interface IScene : IGameComponent
    {
        public GameStates SceneId { get; }
        protected internal World World { get; set; }
        protected internal void LoadContent(ContentManager Content);
        protected internal void Update(GameTime gameTime);
        protected internal void Draw(GameTime gameTime);
    }
}