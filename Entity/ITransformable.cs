namespace Fizzleon.Entity;

internal interface ITransformable
{
    Vector2 Position { get; set; }
    float Rotation { get; set; }
    Vector2 Scale { get; set; }
}

