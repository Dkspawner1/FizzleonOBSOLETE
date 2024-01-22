using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS.Systems
{
    internal class RenderSystem : EntityDrawSystem
    {
        private ComponentMapper<TransformComponent> transformMapper;
        private ComponentMapper<SpriteComponent> spriteMapper;
        private ComponentMapper<AnimationComponent> animationMapper;

        public RenderSystem()
            : base(Aspect.All(typeof(TransformComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<TransformComponent>();
            spriteMapper = mapperService.GetMapper<SpriteComponent>();
            animationMapper = mapperService.GetMapper<AnimationComponent>();
        }

        public override void Draw(GameTime gameTime)
        {
            Data.Game.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

            foreach (var entity in ActiveEntities)
            {
                var transform = transformMapper.Get(entity);
                var sprite = spriteMapper.Get(entity);
                var animation = animationMapper.Get(entity);


                //sprite.Draw(Data.Game.SpriteBatch);
                //animation.Draw(Data.Game.SpriteBatch);

                (animation ?? sprite)?.Draw(Data.Game.SpriteBatch);

            }

            Data.Game.SpriteBatch.End();
        }
    }
}
