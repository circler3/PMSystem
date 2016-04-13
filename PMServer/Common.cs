using SuperSocket.SocketBase;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace PMServer
{
    class Common
    {
        public static ConcurrentDictionary<string, User> SessionDict { get; set; }
    }
}
