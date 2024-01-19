using Fizzleon.Entity;

internal class SpriteComponent : Component
{
    public Texture2D Texture { get; set; }
    public ITransformable Transform { get; set; }

    public SpriteComponent(Texture2D texture)
    {
        Texture = texture;
    }

    public void SetTransform(ITransformable transform)
    {
        Transform = transform;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (spriteBatch != null && !spriteBatch.IsDisposed && Transform != null)
        {
            spriteBatch.Draw(
                Texture,
                Transform.Position,
                null,
                Color.White,
                Transform.Rotation,
                Vector2.Zero,
                Transform.Scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}