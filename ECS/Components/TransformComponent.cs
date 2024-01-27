namespace Fizzleon.ECS.Components;

public class TransformComponent(Vector2 position)
{
    public Vector2 Position { get; set; } = position;
    public float Rotation { get; set; } = 0f;
    public Vector2 Scale { get; set; } = Vector2.One;
}