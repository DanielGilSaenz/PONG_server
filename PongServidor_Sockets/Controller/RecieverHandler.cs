﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PongServidor_Sockets.Controller
{
    class RecieverHandler
    {
        private string msg;
        private bool stop = false;

        public RecieverHandler(NetworkStream stream, Byte[] bytes)
        {
            startReading(stream, bytes);
        }

        public string getMsg()
        {
            if (msg != null)
            {
                string returned = msg;
                msg = null;
                return returned;
            }
            else
            {
                return msg;
            }
        }

        private void startReading(NetworkStream stream, Byte[] bytes)
        {
            int count;

            new Task(() =>
            {
                while (!stop)
                {
                    count = stream.Read(bytes, 0, bytes.Length);
                    msg = Encoding.ASCII.GetString(bytes, 0, count);
                }
            }).Start();
        }



    }
}
