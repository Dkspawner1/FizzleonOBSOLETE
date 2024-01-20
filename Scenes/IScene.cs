using System.Collections.Generic;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.Scenes
{
    public interface IScene
    {
        protected List<EntitySystem> Entities { get; }
        protected List<EntitySystem> Systems { get; }
        protected void LoadContent();
        protected void Update(float deltaTime);
        protected void Draw(SpriteBatch spriteBatch);
    }
}