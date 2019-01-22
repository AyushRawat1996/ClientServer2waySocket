using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace client
{
    public class ClientSocket
    {
        private Socket clientSocket;
        private int port;

        public ClientSocket(int port)
        {   //using Constructor to initiate the Socket
            this.port = port;
            Console.WriteLine("Starting client socket");
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Console.Title = "CLIENT SIDE";
        }

        // Separate function to Connect with the Server
        public void connect() {
            try
            {
                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                clientSocket.Connect(serverEndPoint);
                Console.WriteLine("Coonected to Server");
            }
            catch (Exception e) {
                Console.WriteLine("Error while connectiong to server {0}", e.Message);
            }
        }

        // Function to start message transmission
        public void start()
        {
            while (true) {
                try
                {
                    //SEND----------------------------------------------------------
                    Console.Write("Type a message: ");
                    //Reading the data from Console Window
                    String data = Console.ReadLine();
                    
                    //Encoding the data
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    //using send function yo send the data
                    clientSocket.Send(bytes);
                    if (data == "Bye") {
                        break;
                    }
                    //RECIEVE-------------------------------------------------------
                    //Setting the Buffer size of recieved data
                    int receiveBufferSize = clientSocket.ReceiveBufferSize;
                    byte[] buffer = new byte[receiveBufferSize];

                    int receivedBytes = clientSocket.Receive(buffer);
                    byte[] receivedData = new byte[receivedBytes];

                    for (int i = 0; i < receivedBytes; i++)
                    {
                        receivedData[i] = buffer[i];
                    }

                    String received = Encoding.UTF8.GetString(receivedData);

                    Console.WriteLine("Server : {0}", received);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Connection termnated with error: {0}", e.Message);
                    Console.WriteLine("Closing connection");
                    clientSocket.Close();
                    Console.Read();
                }
            }

            Console.WriteLine("Closing connection");
            clientSocket.Close();
            Console.Read();
        }
    }

    class Client
    {
        static void Main(string[] args)
        {
            ClientSocket clientSocket = new ClientSocket(1234);
            clientSocket.connect();
            clientSocket.start();

        }
    }
}