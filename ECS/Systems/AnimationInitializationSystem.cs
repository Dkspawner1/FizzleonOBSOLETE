using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using Microsoft.Xna.Framework.Content;

namespace Fizzleon.ECS.Systems
{
    public class AnimationInitializationSystem : EntitySystem
    {
        private ComponentMapper<AnimationComponent> animationMapper;
        private readonly ContentManager contentManager;

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
            animation.LoadContent(contentManager);
            Trace.WriteLine($"Entity with AnimationComponent added. EntityId: {entityId}");
        }
    }
}
