using PongCliente_Sockets.MVC.Model.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PongServidor_Sockets.Model
{
    class Partida
    {
        public TcpClient client1 { get; set; }
        public TcpClient client2 { get; set; }

        public bool jugandose { get; set; } = false;
        public Player player1 { get; set; }
        public Player player2 { get; set; }
        public StatusBoard statusBoard { get; set; }
        public Ball ball { get; set; }
        public Wall topWall { get; set; }
        public Wall BottomWall { get; set; }
    }
}
