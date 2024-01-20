using System.Data;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Fizzleon.FizzleEntity
{
    internal class RenderSystem : EntityDrawSystem
    {
        private ComponentMapper<TransformComponent> transformMapper;
        private ComponentMapper<SpriteComponent> spriteMapper;

        public RenderSystem()
            : base(Aspect.All(typeof(TransformComponent), typeof(SpriteComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<TransformComponent>();
            spriteMapper = mapperService.GetMapper<SpriteComponent>();
        }

        public override void Draw(GameTime gameTime)
        {
            Data.Game.SpriteBatch.Begin();
            foreach (var entity in ActiveEntities)
            {
                var transform = transformMapper.Get(entity);
                var sprite = spriteMapper.Get(entity);
                sprite.Draw(Data.Game.SpriteBatch);
            }
            Data.Game.SpriteBatch.End();
        }
    }
}