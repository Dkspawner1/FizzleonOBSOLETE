
using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS.Systems;
public class TransitionSystem : EntityUpdateSystem
{
    private ComponentMapper<TransitionComponent> transitionMapper;

    public TransitionSystem() : base(Aspect.All(typeof(TransitionComponent)))
    {

    }
    public override void Initialize(IComponentMapperService mapperService)
    {
        transitionMapper = mapperService.GetMapper<TransitionComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var transitionComponent = transitionMapper.Get(entity);

            switch (transitionComponent.CurrentTransitionState)
            {
                case TransitionComponent.TransitionState.TransitionIn:
                    transitionComponent.StartTransitionIn();
                    break;

                case TransitionComponent.TransitionState.TransitionOut:
                    transitionComponent.StartTransitionOut();
                    break;


                default:
                    break;
            }
        }
    }
}
