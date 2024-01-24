using Fizzleon.Library;
using Lidgren.Network;
using System;

namespace Fizzleon.Network
{
    public class NetworkConnection
    {
        private NetClient client;

        public bool Start()
        {
            var loginInformation = new NetworkLoginInformation() { Name = "Fizzle" };

            client = new NetClient(new NetPeerConfiguration("fizzleon") { Port = 7000 });
            client.Start();
            var output = client.CreateMessage();
            output.Write((byte)PacketType.Login);
            output.WriteAllProperties(loginInformation);
            client.Connect("localhost", 7001, output);
            return EsablishInfo();
        }

        private bool EsablishInfo()
        {
            var time = DateTime.Now;
            NetIncomingMessage incoming = null;
            while (true)
            {
                if (DateTime.Now.Subtract(time).Seconds > 5)
                    return false;

                if ((incoming = client.ReadMessage()) == null) continue;
                switch (incoming.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        var data = incoming.ReadByte();
                        if (data == (byte)PacketType.Login)
                        {
                            var accepted = incoming.ReadBoolean();
                            return accepted;
                        }
                        else
                        {
                            // Handle other packet types if needed
                            return false;
                        }
                }
            }
        }
    }
}
