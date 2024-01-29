using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS.Systems;

public class PlayerMovementSystem()
    : EntityUpdateSystem(Aspect.All(typeof(TransformComponent), typeof(PlayerComponent)))
{
    private ComponentMapper<TransformComponent> transformMapper;
    private ComponentMapper<PlayerComponent> playerMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        transformMapper = mapperService.GetMapper<TransformComponent>();
        playerMapper = mapperService.GetMapper<PlayerComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var transform = transformMapper.Get(entity);
            var player = playerMapper.Get(entity);
            // Move the player entity
            transform.Position += new Vector2(player.Speed * (float)Data.GameTime.ElapsedGameTime.TotalSeconds, 0);
        }
    }

}