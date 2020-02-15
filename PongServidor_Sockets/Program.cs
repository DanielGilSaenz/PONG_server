using PongCliente_Sockets.MVC.Model.Math_Objects;
using PongCliente_Sockets.MVC.Model.Serializable;
using PongServidor_Sockets.Controller;
using PongServidor_Sockets.Model;
using PongServidor_Sockets.Model.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PongServidor_Sockets
{
    class Program
    {
        public static Partida[] partidasPool = new Partida[10];

        static void Main(string[] args)
        {
            
            TcpListener server = null;
            
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 8080;
                IPAddress localAddr = IPAddress.Parse("0.0.0.0");
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

                        if ((partidasPool[index].client1 != null) && (partidasPool[index].client2 != null))
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
            /*
        test:

            Byte[] bytes1 = new Byte[512];
            int count = 0;
            NetworkStream stream = partidasPool[0].client1.GetStream();
            if (false) stream.Read(bytes1, 0, bytes1.Length);

            Jugada j = new Jugada();
            j.player1 = new Player(Key.A,Key.DbeEnterImeConfigureMode, 2147483647, 2147483647, new Point(2147483647, 2147483647), 2147483647);
            j.ball = new Ball(new fPoint(2147483647, 2147483647), new fVector(2147483647, 2147483647), true);
            string msg = j.getAttr(j);
            byte[] bytes = Encoding.ASCII.GetBytes(msg);
            Console.WriteLine("Msg :{0}", msg);
            Console.WriteLine("Bytes count :{0}", bytes.Length);
            Console.ReadKey();
            while (true) ;
            */
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
