using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMServer
{
    class User
    {
        public AppSession Session { get; set; }
        public string Username { get; set; }
        public DateTime LastMessageTime { get; set; }
    }
}
