using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS.Systems;

public class SceneTransitionSystem()
    : EntityUpdateSystem(Aspect.All(typeof(SceneTransitionComponent)))
{
    private ComponentMapper<SceneTransitionComponent> transitionMapper;


    public override void Initialize(IComponentMapperService mapperService)
    {
        transitionMapper = mapperService.GetMapper<SceneTransitionComponent>();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var entity in ActiveEntities)
        {
            var transitionComponent = transitionMapper.Get(entity);

            switch (transitionComponent.CurrentTransitionState)
            {
                case SceneTransitionComponent.TransitionState.TransitionIn:
                    transitionComponent.TransitionIn();
                    break;

                case SceneTransitionComponent.TransitionState.TransitionOut:
                    transitionComponent.TransitionOut();
                    break;

                default:
                    break;
            }
        }

        Data.SpriteBatch.Begin();

        DrawTransition();

        Data.SpriteBatch.End();
        
    }

    public void DrawTransition()
    {
        foreach (var entity in ActiveEntities)
        {
            var transitionComponent = transitionMapper.Get(entity);
            transitionComponent.Draw(Data.SpriteBatch);
        }
    }
}