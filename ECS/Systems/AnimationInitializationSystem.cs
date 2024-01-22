using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using System;
using System.Diagnostics;

namespace Fizzleon.ECS.Systems
{
    public class AnimationInitializationSystem : EntitySystem
    {
        private ComponentMapper<AnimationComponent> animationMapper;

        public AnimationInitializationSystem()
            : base(Aspect.All(typeof(AnimationComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            animationMapper = mapperService.GetMapper<AnimationComponent>();
        }

        protected override void OnEntityAdded(int entityId)
        {
            var animation = animationMapper.Get(entityId);
            animation.LoadContent(Game1.Instance.Content);
            Trace.WriteLine($"Entity with AnimationComponent added. EntityId: {entityId}");

        }
    }
}
