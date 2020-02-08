using PongServidor_Sockets.Controller;
using PongServidor_Sockets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PongServidor_Sockets
{
    class Program
    {
        public static Partida[] partidasPool = new Partida[3];

        static void Main(string[] args)
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 8080;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, port);
                server.Start();
                Console.WriteLine("server started with " + server.LocalEndpoint.ToString());

                for (int i = 0; i < partidasPool.Length; i++)
                {
                    partidasPool[i] = new Partida();
                }

                Console.WriteLine("Waiting for clients to connect");
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    int index = nextFreePartida();
                    if (index >= 0)
                    {
                        if (partidasPool[index].client1 == null) partidasPool[index].client1 = client;
                        else if (partidasPool[index].client2 == null) partidasPool[index].client2 = client;

                        if ((partidasPool[index].client1!= null) && (partidasPool[index].client2 != null))
                        {
                            partidasPool[index].jugandose = true;
                            new Task(() => PartidaHandler.handleClient(server, partidasPool[index])).Start();
                        }                        
                    }
                };
            }
            catch
            {

            }
        }

        /// <summary>Gets the index of the next free match</summary>
        private static int nextFreePartida()
        {
            for (int i = 0; i < partidasPool.Length; i++)
            {
                if (!partidasPool[i].jugandose) return i;
            }
            return -1;
        }
    }
}
