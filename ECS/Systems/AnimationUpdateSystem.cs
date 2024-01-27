using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS.Systems
{
    public class AnimationUpdateSystem() : EntityUpdateSystem(Aspect.All(typeof(AnimationComponent)))
    {
        private ComponentMapper<AnimationComponent> animationMapper;

        public override void Initialize(IComponentMapperService mapperService)
        {
            animationMapper = mapperService.GetMapper<AnimationComponent>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {

                var animation = animationMapper.Get(entity);
                animation.Update();
            }
        }
    }
}
