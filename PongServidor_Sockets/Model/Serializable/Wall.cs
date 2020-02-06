﻿using PongCliente_Sockets.Interfaces;
using PongCliente_Sockets.MVC.Model.Math_Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PongCliente_Sockets.MVC.Model.Serializable
{

    class Wall : Mostrar, ICloneable, ICompareBool
    {
        public Line line { get; set; }

        public Wall() { }

        public Wall(Line line)
        {
            this.line = line;
        }

        public bool Compare(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Wall)) return false;
            Wall o = (Wall)obj;

            if (o.line.Compare(line)) return true;
            else return false;
        }

        public object Clone()
        {
            return new Wall((Line)line.Clone());
        }
    }
}
