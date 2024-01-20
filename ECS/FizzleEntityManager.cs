using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS
{
    public class FizzleEntityManager : IGameComponent
    {
        private readonly WorldBuilder worldBuilder;
        private readonly World world;

        public FizzleEntityManager(WorldBuilder worldBuilder)
        {
            this.worldBuilder = worldBuilder;
            this.world = worldBuilder.Build();
        }

        public Entity CreateEntity() => world.CreateEntity();

        public Entity CreateEntityWithComponents(params Component[] components)
        {
            var entity = CreateEntity();
            entity.Attach(components);
            return entity;
        }

        public void AddSystem(EntitySystem system) => worldBuilder.AddSystem(system);
        public void Update(GameTime gameTime) => world.Update(gameTime);

        public void Draw(GameTime gameTime) => world.Draw(gameTime);
        public void Initialize()
        {
        }
    }
}