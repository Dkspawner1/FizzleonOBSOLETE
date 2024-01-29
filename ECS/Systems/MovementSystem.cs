using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS.Systems;

public class MovementSystem() : EntityUpdateSystem(Aspect.All(typeof(TransformComponent), typeof(MovementComponent)))
{
    private ComponentMapper<TransformComponent> transformMapper;
    private ComponentMapper<MovementComponent> movementMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        transformMapper = mapperService.GetMapper<TransformComponent>();
        movementMapper = mapperService.GetMapper<MovementComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var transform = transformMapper.Get(entity);
            var movement = movementMapper.Get(entity);

            // Example: Move the entity based on velocity
            transform.Position += movement.Velocity * (float)Data.GameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}