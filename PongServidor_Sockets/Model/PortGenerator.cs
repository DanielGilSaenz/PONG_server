using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace PongServidor_Sockets.Model
{
    class PortGenerator
    {
        /// <summary> The port to start looking for ports </summary>
        public const int DEFAULT_STARTING_PORT = 8090;

        /// <summary> Gets the next free port starting from  the startPort* or the defaltStartPort in it's defect </summary>
        public void getNextFreePort(out int freePort, int startPort = DEFAULT_STARTING_PORT)
        {
            int port = startPort;
            bool isAvailable = false;

            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            while (!isAvailable)
            {
                foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
                {
                    if (!tcpi.LocalEndPoint.Port.Equals(port))
                    {
                        freePort = port;
                        isAvailable = true;
                        return;
                    }
                }
                port++;
                if (port == 65535)
                {
                    freePort = -1;
                    return;
                }
            }
            freePort = -1;
            return;
        }
    }
}
