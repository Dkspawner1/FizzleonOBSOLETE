namespace Fizzleon.ECS;

internal class TransformComponent 
{
    public Vector2 Position { get; set; }
    public float Rotation { get; set; }
    public Vector2 Scale { get; set; }
    public TransformComponent(Vector2 position)
    {
        Position = position;
        Rotation = 0f;
        Scale = Vector2.One;
    }
   
}