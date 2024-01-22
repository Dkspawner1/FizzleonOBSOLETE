
namespace Fizzleon.ECS.Components;
public class PlayerComponent
{
    public float Speed { get; set; }
    public PlayerComponent()
    {
        Speed = 5.0f;
    }
    public void Update(GameTime gameTime)
    {
    }
}