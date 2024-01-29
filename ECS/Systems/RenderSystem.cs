using Fizzleon.ECS.Components;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.ECS.Systems
{
    public class RenderSystem() : EntityDrawSystem(Aspect.All(typeof(TransformComponent)))
    {
        private ComponentMapper<TransformComponent> transformMapper;
        private ComponentMapper<SpriteComponent> spriteMapper;
        private ComponentMapper<AnimationComponent> animationMapper;

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<TransformComponent>();
            spriteMapper = mapperService.GetMapper<SpriteComponent>();
            animationMapper = mapperService.GetMapper<AnimationComponent>();
        }

        public override void Draw(GameTime gameTime)
        {
            Data.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);

            foreach (var entityId in ActiveEntities)
            {
                // Get the entity using the entityId
                var entity = GetEntity(entityId);

                var transform = transformMapper.Get(entity);
                var sprite = spriteMapper.Get(entity);
                var animation = animationMapper.Get(entity);

                //sprite.Transform = transform;

                // Ensure both transform and sprite components are present
                if (transform != null && sprite != null)
                {
                    var position = transform.Position;
                    var rotation = transform.Rotation;
                    var scale = transform.Scale;

                    // Draw static sprite
                    sprite.Draw();

                    // Draw animated sprite
                    animation?.Draw(); 
                }
            }

            Data.SpriteBatch.End();
        }
    }
}
