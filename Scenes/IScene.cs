using System.Collections.Generic;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.Scenes
{
    public interface IScene
    {

        public List<Entity> Entities { get; }

        public Data.GameState.GameStates SceneId { get; }
        protected List<EntitySystem> Systems { get; }
        protected internal void LoadContent(ContentManager Content);
        protected internal void Update(GameTime gameTime);
        protected internal void Draw(GameTime gameTime);
    }
}