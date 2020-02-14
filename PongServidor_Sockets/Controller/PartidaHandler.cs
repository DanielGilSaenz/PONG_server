using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using PongServidor_Sockets.Model;
using System.Diagnostics;

namespace PongServidor_Sockets.Controller
{
    class PartidaHandler
    {
        private static NetworkStream stream1;
        private static NetworkStream stream2;
        private static RecieverHandler recieverHandler1;
        private static RecieverHandler recieverHandler2;

        private const int BYTES_NUM = 512;

        public static void handleClient(TcpListener server, Partida partida)
        {
            Console.WriteLine("Match Found, 2 clients connected");

            stream1 = partida.client1.GetStream();
            stream2 = partida.client2.GetStream();

            sendAll_MatchFound();

            Byte[] bytes1 = new Byte[BYTES_NUM];
            Byte[] bytes2 = new Byte[BYTES_NUM];
            int count = 0;

            recieverHandler1 = new RecieverHandler(stream1, bytes1);
            recieverHandler2 = new RecieverHandler(stream2, bytes2);


            while (true)
            {
                string str1 = recieverHandler1.getMsg();
                string str2 = recieverHandler2.getMsg();

                if (!string.IsNullOrEmpty(str1))
                    Console.WriteLine("stram1: " + str1);
                if (!string.IsNullOrEmpty(str2))
                    Console.WriteLine("stram2: " + str2);

                send(stream2, str1);
                send(stream1, str2);
            }
        }

        /// <summary>If the msg is not null, tries to send it</summary>
        private static void send(NetworkStream stream, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                //Debug.WriteLine(msg);
                byte[] bytes = Encoding.ASCII.GetBytes(msg);
                stream.Write(bytes, 0, bytes.Length);
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
