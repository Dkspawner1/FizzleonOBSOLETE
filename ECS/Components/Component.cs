namespace Fizzleon.ECS.Components
{
    // This is the base class which each entity will inherit
    // Each entity will be assigned an ID and this class's ID will-Be
    // a copy and reference of the Monogame.Extended.Entities ID 
    public class Component
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
    }
}