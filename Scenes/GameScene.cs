using System.Collections.Generic;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.Scenes
{
    public class GameScene : IScene
    {
        public GameScene(List<EntitySystem> entities, List<EntitySystem> systems)
        {
            Entities = entities;
            Systems = systems;
        }

        public List<EntitySystem> Entities { get; }
        public List<EntitySystem> Systems { get; }

        public void LoadContent()
        {
         
        }

        public void Update(float deltaTime)
        {
          
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            
        }
    }
}