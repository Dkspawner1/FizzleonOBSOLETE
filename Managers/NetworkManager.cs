using Lidgren.Network;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Fizzleon.Managers
{
    public class NetworkManager : IDisposable
    {
        private NetPeerConfiguration configuration;
        private NetServer server;
        private NetClient client;

        public NetworkManager()
        {
            configuration = new NetPeerConfiguration("game");
            server = new NetServer(configuration);
            client = new NetClient(configuration);
        }

        public int StartServer(int startingPort, int maxAttempts, int retryDelayMilliseconds)
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int portToTry = startingPort + attempt;

                // Explicitly release the server resources if the server is already running
                ShutdownServer();

                if (IsPortAvailable(portToTry))
                {
                    configuration.Port = portToTry;
                    server.Start();
                    Trace.WriteLine($"Server started on port {portToTry}");
                    return portToTry;
                }
                else
                {
                    Trace.WriteLine($"Port {portToTry} is in use. Trying the next port.");
                }

                // Add a delay before the next attempt
                System.Threading.Thread.Sleep(retryDelayMilliseconds);
            }

            throw new InvalidOperationException("Unable to find an available port to start the server.");
        }

        private void ShutdownServer()
        {
            if (server.Status == NetPeerStatus.Running)
            {
                // Gracefully wait for the shutdown to complete
                server.Shutdown("Server shutdown");
                System.Threading.Thread.Sleep(1000); // Add a delay (1 second) before restarting
            }
        }

        private bool IsPortAvailable(int port)
        {
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpListeners = ipGlobalProperties.GetActiveTcpListeners();

            foreach (IPEndPoint endpoint in tcpListeners)
                if (endpoint.Port == port)
                    return false;

            return true;
        }

        public void ConnectToServer(string address, int port)
        {
            try
            {
                client.Start();
                client.Connect(address, port);
                Trace.WriteLine($"Client connecting to {address}:{port}");
            }
            catch (SocketException ex)
            {
                Trace.WriteLine($"SocketException: {ex.Message}");
            }
        }

        public void Update()
        {
            HandleMessages(server);
            HandleMessages(client);
        }

        private void HandleMessages(NetPeer netPeer)
        {
            NetIncomingMessage msg;
            while ((msg = netPeer.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.StatusChanged:
                        Trace.WriteLine($"StatusChanged: {msg.SenderConnection.Status}");
                        break;
                    case NetIncomingMessageType.ErrorMessage:
                        Trace.WriteLine("You're fucked!");
                        break;
                    default:
                        Trace.WriteLine($"Unhandled message type: {msg.MessageType}");
                        break;
                }

                netPeer.Recycle(msg);
            }
        }

        public void Dispose()
        {
            ShutdownServer();
            server?.Shutdown("Exiting");
            client?.Shutdown("Exiting");
        }
    }
}
