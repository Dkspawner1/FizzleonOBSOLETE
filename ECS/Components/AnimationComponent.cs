using System;
using Fizzleon.ECS.Systems;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

namespace Fizzleon.ECS.Components;

public class AnimationComponent(TextureLoaderSystem textureLoaderSystem, string pathToXnb, string pathToSf)
    : SpriteComponent(textureLoaderSystem.Load<Texture2D>(pathToXnb))
{
    public AnimatedSprite AnimatedSprite { get; set; } = textureLoaderSystem.LoadAnimation(pathToSf);

    public void Update()
    {
        
        AnimatedSprite.Update(Data.GameTime);
    }

    public void Play(string animationName, Action completionAction = null)
    {
        AnimatedSprite.Play(animationName, completionAction);
    }

    public override void Draw()
    {
        AnimatedSprite.Draw(Data.SpriteBatch, Transform.Position, Transform.Rotation, Transform.Scale);
    }
}