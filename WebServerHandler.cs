using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Tizen.Network.WiFi;

namespace GreentegCoreApp1
{
    class WebServerHandler
    {
        public WebServerHandler()
        {
            TcpListener server = new TcpListener(2636);
            server.Start();
        }
    }
}
