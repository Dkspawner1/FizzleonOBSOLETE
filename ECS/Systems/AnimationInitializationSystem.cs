using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS.Systems;

public class AnimationInitializationSystem() : EntitySystem(Aspect.All(typeof(AnimationComponent)))
{
    private ComponentMapper<AnimationComponent> animationMapper;

    public override void Initialize(IComponentMapperService mapperService)
    {
        animationMapper = mapperService.GetMapper<AnimationComponent>();
    }

    protected override void OnEntityAdded(int entityId)
    {
        var animation = animationMapper.Get(entityId);

        if (animation != null)
        {
            Trace.WriteLine($"Entity with AnimationComponent added. EntityId: {entityId}");
        }
        else
        {
            // Handle the case where AnimationComponent is not attached
        }
    }
}