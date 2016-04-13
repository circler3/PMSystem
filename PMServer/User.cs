using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMServer
{
    class User
    {
        public User()
        {
            WorkItems = new List<WorkItem>();
        }
        public AppSession Session { get; set; }
        public string Username { get; set; }
        public DateTime LastMessageTime { get; set; }
        public List<WorkItem> WorkItems { get; set; }
        public bool Previleged { get; set; }
    }
}
