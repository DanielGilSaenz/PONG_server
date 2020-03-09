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
using System.Threading;

namespace PongServidor_Sockets.Controller
{
    class PartidaHandler
    {
        private NetworkStream stream1;
        private NetworkStream stream2;
        private PortGenerator portGenerator = new PortGenerator();

        private const int BYTES_NUM = 512;

        private int t { get; set; }

        public void handleClient(TcpListener server, Partida partida, int t)
        {
            this.t = t;
            Console.WriteLine("Match Found, 1 client connected" + " t:" + t);

            stream1 = partida.client1.GetStream();
            stream2 = partida.client2.GetStream();

            do
            {
                send(stream1, "MatchFound");
            } while (!waitForMsg(1500, "OK", stream1));

            do
            {
                send(stream2, "MatchFound");
            } while (!waitForMsg(1500, "OK", stream2));

            do
            {
                send(stream1, "p1");
            } while (!waitForMsg(1500, "OK", stream1));

            do
            {
                send(stream2, "p2");
            } while (!waitForMsg(1500, "OK", stream2));


            send(stream1, "StartGame");
            send(stream1, "StartGame");

            send(stream2, "StartGame");
            send(stream2, "StartGame");

            string str1;
            string str2;
            while (true)
            {
                str1 = read(stream1, 100);
                str2 = read(stream2, 100);
                send(stream1, str2);
                send(stream2, str1);
                str1 = null;
                str2 = null;                
            }
            

        }

        public void handleClientOnlyOne(TcpListener server, Partida partida, int t)
        {
            this.t = t;
            Console.WriteLine("Match Found, 1 client connected" + " t:" + t);

            stream1 = partida.client1.GetStream();

            do
            {
                send(stream1, "MatchFound");
            } while (!waitForMsg(5000, "OK", stream1));

            do
            {
                send(stream1, "p1");
            } while (!waitForMsg(5000, "OK", stream1));

            do
            {
                send(stream1, "StartGame");
            } while (!waitForMsg(5000, "OK", stream1));

            string str1;
            while (true)
            {
                try
                {
                    str1 = read(stream1, 100);
                    str1 = null;
                    //send(stream1, str1);
                }
                catch
                {
                    return;
                }
                
            }
            Console.WriteLine("Desconnected");

        }

        /// <summary>If the msg is not null, tries to send it</summary>
        private void send(NetworkStream stream, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                Console.WriteLine("[W]" + msg + " t:" + t);
                //Debug.WriteLine(msg);
                byte[] bytes = Encoding.ASCII.GetBytes(msg);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        private string read(NetworkStream stream, int timeout)
        {
            try
            {
                Byte[] bytes = new Byte[BYTES_NUM];
                stream.ReadTimeout = timeout;
                int count = stream.Read(bytes, 0, bytes.Length);
                string response = Encoding.ASCII.GetString(bytes, 0, count);
                if (response != null) Console.WriteLine("[R]" + response + " t:" + t);
                return response;
            }
            catch
            {
                return null;
            }
            
        }

        private void getNextPort(NetworkStream oldStream, out NetworkStream newStream)
        {
            // Maybe useless
            int freePort;
            portGenerator.getNextFreePort(out freePort);
            send(oldStream, "newPort:" + freePort);
            //newStream = new NetworkStream();
            throw new NotImplementedException();
        }

        /// <summary> Return true if the response is equal to the msg before the timeout</summary>
        private bool waitForMsg(int timeout, string msg, NetworkStream stream)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            string response;
            while (stopwatch.ElapsedMilliseconds < timeout)
            {
                response = read(stream, 100);
                if (response == msg) return true;
            }
            return false;
        }
    }
}
