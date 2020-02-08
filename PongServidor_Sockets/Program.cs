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

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();

                    new Task(() => PartidaHandler.handleClient(server, nextPartida)).Start();

                    if (nextPartida == null) continue;
                    else
                    {
                        
                    }
                    
                };
            }
            catch
            {

            }
        }

        private static ref Partida nextFreePartida()
        {
            Partida nullPartida;
            for (int i = 0; i < partidasPool.Length; i++)
            {
                if (!partidasPool[i].jugandose) return ref partidasPool[i];
            }
            return ref nullPartida;
        }
    }
}
