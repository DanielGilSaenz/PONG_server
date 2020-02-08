using PongCliente_Sockets.Interfaces;
using PongCliente_Sockets.MVC.Model.Math_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongServidor_Sockets.Model.Serializable
{
    class PlayerPos : Mostrar
    {
        public PlayerPos(Point pos)
        {
            this.pos = pos;
        }

        public Point pos { get; set; }
    }
}
