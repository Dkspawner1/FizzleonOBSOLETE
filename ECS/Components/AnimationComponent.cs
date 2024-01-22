using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Diagnostics;

namespace Fizzleon.ECS.Components
{
    internal class AnimationComponent : SpriteComponent
    {
        public AnimatedSprite AnimatedSprite { get; private set; }
        public SpriteSheet SpriteSheet { get; private set; }
        public string PathToSF { get; }

        public AnimationComponent(string pathToSF, Texture2D texture) : base(texture)
        {
            PathToSF = pathToSF;
        }

        public void LoadContent(ContentManager Content)
        {
            SpriteSheet = Content.Load<SpriteSheet>(PathToSF, new JsonContentLoader());

            AnimatedSprite = new AnimatedSprite(SpriteSheet);
        }

        public void Update(GameTime gameTime)
        {
            AnimatedSprite.Update(gameTime);
        }

        public void Play(string animationName, bool isLooping = true, Action completionAction = null)
        {
            AnimatedSprite.Play(animationName);

        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            AnimatedSprite.Draw(spriteBatch, Transform.Position, Transform.Rotation, Transform.Scale);
        }
    }
}
