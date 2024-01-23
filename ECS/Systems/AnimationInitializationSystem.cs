using Microsoft.Xna.Framework.Content;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System.Diagnostics;

public class AnimationInitializationSystem : EntitySystem
{
    private ComponentMapper<AnimationComponent> animationMapper;
    private ContentManager contentManager; 

    public AnimationInitializationSystem(ContentManager contentManager)
        : base(Aspect.All(typeof(AnimationComponent)))
    {
        this.contentManager = contentManager; 
    }

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

            if (contentManager != null)
            {
                animation.LoadContent(contentManager);
            }
          
        }
        else
        {
            // Handle the case where AnimationComponent is not attached
        }
    }
}
