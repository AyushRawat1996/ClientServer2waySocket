using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class ServerSocket
    {
        private int port;
        private Socket serverSocket;

        public ServerSocket(int port)
        {
            this.port = port;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);

            /* Associates a Socket with a local endpoint. */
            serverSocket.Bind(serverEndPoint);

            /*Places a Socket in a listening state.
             * The maximum length of the pending connections queue is 100 
             */
            serverSocket.Listen(100);
        }

        public void start()
        {
            Console.WriteLine("Starting the Server");
            Console.Title = "SERVER SIDE";
            /* Accept Connection Requests */
            Socket accepted = serverSocket.Accept();

            while (accepted.Connected) {
                int bufferSize = accepted.SendBufferSize;
                byte[] buffer = new byte[bufferSize];

                /* Receives data from a bound Socket into a receive buffer. It return the number of bytes received. */
                int bytesRead = accepted.Receive(buffer);

                byte[] formatted = new byte[bytesRead];

                for (int i = 0; i < bytesRead; i++)
                {
                    formatted[i] = buffer[i];
                }

                String receivedData = Encoding.ASCII.GetString(formatted);
                
                Console.WriteLine("Client: " + receivedData);
                if(receivedData.Length.Equals(0)) {
                    Console.WriteLine("Stopping Server");
                    break;
                }
                Console.Write("Type a message: ");

                String response = Console.ReadLine();
                if (response == "Bye")
                {
                    Console.WriteLine("Stopping Server");
                    break;
                }
                byte[] resp = Encoding.UTF8.GetBytes(response);
                accepted.Send(resp, 0, resp.Length, 0);
            }

            serverSocket.Close();
            Console.WriteLine("Press some key to close");
            Console.Read();
        }
    }
    class Server
    {
        static void Main(string[] args)
        {
            ServerSocket server = new ServerSocket(1234);
            server.start();
        }
    }
}