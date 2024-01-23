using Fizzleon.ECS.Components;
using Fizzleon.ECS.Entities;
using Lidgren.Network;

namespace Fizzleon.Network
{
    // TODO: make JSON-serializable properties to send fast to the server and receive full objects
    public record BindPlayerToNetwork(Player player, NetConnection Connection = null)
    {
        public Vector2 Position { get; set; }
        private double PositionX => Position.X;
        private double PositionY => Position.Y;

        // Empty constructor for record
        public BindPlayerToNetwork() : this(null, null) { }
        public NetConnection Connect() => Connection;


    }
}
