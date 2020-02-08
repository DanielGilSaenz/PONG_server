using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using PongServidor_Sockets.Model;

namespace PongServidor_Sockets.Controller
{
    class PartidaHandler
    {
        private static NetworkStream stream1;
        private static NetworkStream stream2;

        public static void handleClient(TcpListener server, Partida partida)
        {
            Console.WriteLine("Match Found, 2 clients connected");

            stream1 = partida.client1.GetStream();
            stream2 = partida.client2.GetStream();

            sendAll_MatchFound();

            Byte[] bytes = new Byte[256];
            int count = 0;

            receive(stream1, bytes, count);
            receive(stream2, bytes, count);
            //Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",data.Name, data.CreationTime, data.ThreadNum);
        }

        /// <summary>Checks if there is something to recieve </summary>
        private static string receive(NetworkStream stream, Byte[] bytes, int count)
        {
            if ((count = stream1.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                return Encoding.ASCII.GetString(bytes, 0, count);
            }
            else
            {
                return null;
            }
        }

        /// <summary>Sends a message to all clients in this match to start playing <summary>
        private static void sendAll_MatchFound()
        {
            byte[] msg;
            string matchFound = "MatchFound";
            msg = Encoding.ASCII.GetBytes(matchFound);

            stream1.Write(msg, 0, msg.Length);
            stream2.Write(msg, 0, msg.Length);
        }
    }
}
