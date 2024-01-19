namespace Fizzleon.Entity;

internal class SpriteComponent : Component
{
    public Texture2D Texture { get; set; }
    public ITransformable Transform { get; set; }
    public SpriteComponent(Texture2D texture) => Texture = texture;
    public void SetTransform(ITransformable transform) => Transform = transform;
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