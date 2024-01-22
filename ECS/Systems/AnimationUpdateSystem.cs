using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS.Systems
{
    internal class AnimationUpdateSystem : EntityUpdateSystem
    {
        private ComponentMapper<AnimationComponent> animationMapper;
        private ContentManager contentManager; // You may need to inject this from your game class

        public AnimationUpdateSystem(ContentManager contentManager)
            : base(Aspect.All(typeof(AnimationComponent)))
        {
            this.contentManager = contentManager;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            animationMapper = mapperService.GetMapper<AnimationComponent>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ActiveEntities)
            {
                var animation = animationMapper.Get(entity);
                animation.Update(gameTime);
            }
        }
    }
}
