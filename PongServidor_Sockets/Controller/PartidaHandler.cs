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
        public static void handleClient(TcpListener server, Partida partida)
        {
            NetworkStream stream1 = partida.client1.GetStream();
            NetworkStream stream2 = partida.client2.GetStream();
            byte[] msg;

            Console.WriteLine("Game time it is!! Match Found");
            string matchFound = "MatchFound";
            msg = Encoding.ASCII.GetBytes(matchFound);

            stream1.Write(msg, 0, msg.Length);
            stream2.Write(msg, 0, msg.Length);
            


            string gameIsOn = "Lets GO!! Start the game.";
            msg = Encoding.ASCII.GetBytes(gameIsOn);           
            
            stream1.Write(msg, 0, msg.Length);
            stream2.Write(msg, 0, msg.Length);


            int i;
            Byte[] bytes = new Byte[256];
            String data = null;
            while(true)
            {
                if ((i = stream1.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received1: {0}", data);
                }
                if ((i = stream2.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received2: {0}", data);
                }
            }
            //Console.WriteLine("Task #{0} created at {1}, ran on thread #{2}.",data.Name, data.CreationTime, data.ThreadNum);
        }
    }
}
