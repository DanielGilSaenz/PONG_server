using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using PongServidor_Sockets.Model;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace PongServidor_Sockets.Controller
{
    class PartidaHandler
    {
        private static NetworkStream stream1;
        private static NetworkStream stream2;
        private static RecieverHandler recieverHandler1;
        private static RecieverHandler recieverHandler2;
        private static PortGenerator portGenerator = new PortGenerator();

        private const int BYTES_NUM = 512;

        public static void handleClient(TcpListener server, Partida partida)
        {
            
            Console.WriteLine("Match Found, 2 clients connected");

            stream1 = partida.client1.GetStream();
            stream2 = partida.client2.GetStream();

            
            // TODO Hay que mirar como hacer lo de enviar y recibir de manera sincrona pero que sea efficiente
            // TODO Si generar una task cada vez que se envia
            // TODO [!] Pendiente el testing

            prepareMatch(stream1, "p1");
            prepareMatch(stream2, "p2");

            sendStartGame(stream1);
            sendStartGame(stream2);

            Byte[] bytes1 = new Byte[BYTES_NUM];
            Byte[] bytes2 = new Byte[BYTES_NUM];

            string str1;
            string str2;

            while (bothConnected(partida))
            {
                // TODO reciever handles seems to not work at all
                str1 = read(stream1, 100);
                str2 = read(stream2, 100);

                send(stream1, str2);
                send(stream2, str1);
            }
            Console.WriteLine("Desconnected");

        }

        /// <summary>Checks if both of the clients are still connected</summary>
        private static bool bothConnected(Partida partida)
        {
            if (isConnected(partida.client1) && isConnected(partida.client2)) return true;
            else return false;
        }

        /// <summary>Checks if the client is still connected</summary>
        private static bool isConnected(TcpClient client)
        {
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation c in tcpConnections)
            {
                TcpState stateOfConnection = c.State;

                if (c.LocalEndPoint.Equals(client.Client.LocalEndPoint) && c.RemoteEndPoint.Equals(client.Client.RemoteEndPoint))
                {
                    if (stateOfConnection == TcpState.Established)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

            }

            return false;
        }

        /// <summary>If the msg is not null, tries to send it</summary>
        private static void send(NetworkStream stream, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                Console.WriteLine(msg);
                Console.WriteLine(msg);
                //Debug.WriteLine(msg);
                byte[] bytes = Encoding.ASCII.GetBytes(msg);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        private static string read(NetworkStream stream, int timeout)
        {
            Byte[] bytes = new Byte[BYTES_NUM];
            stream.ReadTimeout = timeout;
            int count = stream.Read(bytes, 0, bytes.Length);
            string response = Encoding.ASCII.GetString(bytes, 0, count);
            return response;
        }

        private static void getNextPort(NetworkStream oldStream, out NetworkStream newStream)
        {
            // Maybe useless
            int freePort;
            portGenerator.getNextFreePort(out freePort);
            send(oldStream, "newPort:" + freePort);
            //newStream = new NetworkStream();
            throw new NotImplementedException();
        }

        /// <summary> Prepares both of the players to play </summary>
        private static void prepareMatch(NetworkStream stream, string playerNumber)
        {
            new Task(() =>
            {
                Byte[] bytes = new Byte[BYTES_NUM];
                stream.ReadTimeout = 100;
                int count;
                string response = "";

                // Wrtites a macth found to know the client is ready
                bytes = Encoding.ASCII.GetBytes("MatchFound");
                stream.Write(bytes, 0, bytes.Length);

                bytes = new Byte[BYTES_NUM];
                while (response != "OK")
                {
                    count = stream.Read(bytes, 0, bytes.Length);
                    response = Encoding.ASCII.GetString(bytes, 0, count);
                    bytes = new Byte[BYTES_NUM];
                }

                // Wrtites the player number in order to configure the client
                bytes = Encoding.ASCII.GetBytes(playerNumber);
                stream.Write(bytes, 0, bytes.Length);

                bytes = new Byte[BYTES_NUM];
                while (response != "OK")
                {
                    count = stream.Read(bytes, 0, bytes.Length);
                    response = Encoding.ASCII.GetString(bytes, 0, count);
                    bytes = new Byte[BYTES_NUM];
                }

            }).Start();
        }

        private static void sendStartGame(NetworkStream stream)
        {
            new Task(() =>
            {
                Byte[] bytes = new Byte[BYTES_NUM];
                stream.ReadTimeout = 100;
                int count;
                string response = "";

                bytes = Encoding.ASCII.GetBytes("StartGame");
                stream.Write(bytes, 0, bytes.Length);

            }).Start();
        }
    }
}
