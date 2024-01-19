
namespace Fizzleon.ECS;
public class MovementComponent : Component
{
    public Vector2 Velocity { get; set; } = Vector2.Zero;
    public Vector2 Acceleration { get; set; } = Vector2.Zero;
}
