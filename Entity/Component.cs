namespace Fizzleon.Entity
{
    public class Component
    {
        // Common properties or methods for components
        // For example, you might want to include an ID or IsActive property.
        public int Id { get; set; }
        public bool IsActive { get; set; }

        // Constructor
        public Component()
        {
            IsActive = true; // Components are active by default
        }
    }
}