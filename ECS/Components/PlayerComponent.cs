
namespace Fizzleon.ECS.Components;
public class PlayerComponent
{
    public float Speed { get; set; }
    public PlayerComponent(float speed) => Speed = speed;

}