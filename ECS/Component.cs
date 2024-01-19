namespace Fizzleon.ECS
{
    public class Component
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true; // Components are active by default

    }
}