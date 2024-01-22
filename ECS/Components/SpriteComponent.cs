namespace Fizzleon.ECS.Components;

internal class SpriteComponent
{
    public Texture2D Texture { get; set; }
    public TransformComponent Transform { get; set; }

    public SpriteComponent(Texture2D texture)
    {
        Texture = texture;
    }

    public void SetTransform(TransformComponent transform) => Transform = transform;

    public void Draw(SpriteBatch spriteBatch)
    {
        if (spriteBatch is { IsDisposed: false } && Transform is { } transformable)
            spriteBatch.Draw(
                Texture,
                transformable.Position,
                null,
                Color.White,
                transformable.Rotation,
                Vector2.Zero,
                transformable.Scale,
                SpriteEffects.None,
                0f
            );
    }
}
