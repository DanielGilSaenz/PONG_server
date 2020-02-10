using PongCliente_Sockets.Interfaces;
using PongCliente_Sockets.MVC.Model.Serializable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServidor_Sockets.Model.Serializable
{
    class Jugada : Mostrar
    {
        public Jugada()
        {
        }

        public Jugada(Player player1, Ball ball)
        {
            this.player1 = player1;
            this.ball = ball;
        }

        public Player player1 { get; set; }
        public Ball ball { get; set; }
    }
}
