namespace Fizzleon.ECS.Components;

public class SpriteComponent(Texture2D texture) 
{
    public Texture2D Texture { get; set; } = texture;
    public TransformComponent Transform { get; set; }   

    public virtual void Draw()
    {
        if (Data.SpriteBatch is { IsDisposed: false })
            Data.SpriteBatch.Draw(
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
