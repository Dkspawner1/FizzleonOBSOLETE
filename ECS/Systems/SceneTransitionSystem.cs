using Fizzleon.ECS.Components;
using Fizzleon.Scenes;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS.Systems
{
    public class SceneTransitionSystem() : EntityUpdateSystem(Aspect.All(typeof(ITransitionable)))
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

                // Update the transition component based on its current state
                switch (transitionComponent.CurrentTransitionState)
                {
                    case SceneTransitionComponent.TransitionState.TransitionIn:
                        transitionComponent.TransitionIn();
                        break;

                    case SceneTransitionComponent.TransitionState.TransitionOut:
                        // Pass the current scene to the TransitionOut method
                        transitionComponent.TransitionOut(Data.CurrentScene);
                        break;

                    default:
                        break;
                }

                // Draw the active transition for each entity
                Data.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                transitionComponent?.DrawTransition(Data.SpriteBatch);
                Data.SpriteBatch.End();
            }
        }


       
    }
}