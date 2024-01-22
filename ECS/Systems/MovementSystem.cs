using Fizzleon.ECS;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Entities;
using Fizzleon.ECS.Components;

public class MovementSystem : EntityUpdateSystem
{
    private ComponentMapper<TransformComponent> transformMapper;
    private ComponentMapper<MovementComponent> movementMapper;

    public MovementSystem()
        : base(Aspect.All(typeof(TransformComponent), typeof(MovementComponent)))
    {
    }

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
            transform.Position += movement.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
