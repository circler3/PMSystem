using SuperSocket.SocketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Command;

namespace PMServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize the common class
            Common.SessionDict = new System.Collections.Concurrent.ConcurrentDictionary<string, User>();
            Ini iniparser = new Ini(@"Config/users.ini");
            foreach (var n in iniparser.GetKeys("PrevilegedUser"))
            {
                Common.SessionDict[n] = new User()
                {
                    Username = iniparser.GetValue(n, "PrevilegedUser"),
                    Previleged = true,
                };
            }
            foreach (var n in iniparser.GetKeys("NormalUser"))
            {
                Common.SessionDict[n] = new User()
                {
                    Username = iniparser.GetValue(n, "NormalUser"),
                    Previleged = false,
                };
            }
            var appServer = new AppServer();
            appServer.NewSessionConnected += AppServer_NewSessionConnected; ;
            appServer.SessionClosed += AppServer_SessionClosed;
            //使用命令简化模型
            //appServer.NewRequestReceived += AppServer_NewRequestReceived;
            //Setup the appServer
            ServerConfig sc = new ServerConfig() { Port = 2020, TextEncoding = "gb2312", KeepAliveInterval = 10, ClearIdleSession = false };
            if (!appServer.Setup(sc)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");

            while (Console.ReadKey().KeyChar != 'q')
            {
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

        private static void AppServer_SessionClosed(AppSession session, CloseReason value)
        {
            Common.SessionDict[session.RemoteEndPoint.Address.ToString()].Session = null;
        }

        private static void AppServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine(requestInfo.Body);
            session.TrySend(requestInfo.ToString());
        }

        private static void AppServer_NewSessionConnected(AppSession session)
        {
            //session.TrySend("WELCOME!");
            Common.SessionDict[session.RemoteEndPoint.Address.ToString()].Session = session;

            //this is for previledged user who is connecting the server the first time.
            if (Common.SessionDict[session.RemoteEndPoint.Address.ToString()].Previleged)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in Common.SessionDict)
                {
                    sb.Append(item.Key);
                    sb.Append(" ");
                    sb.Append(item.Value.Username);
                    sb.Append(" ");
                }
                session.TrySend(string.Format("{0} {1}\r\n", "Userlist", sb));
                session.TrySend(string.Format("{0} {1}\r\n", "Token", "Previleged"));
                var insp = from c in Common.SessionDict.Values
                           where c.WorkItems != null
                           select c;
                foreach (var n in insp)
                {
                    foreach (var xn in n.WorkItems)
                    {
                        var mes = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}\r\n", "Update", xn.Guid, xn.Name, xn.Description,
                            xn.Percentage, xn.Deadline.ToShortDateString(), xn.Priority, n.Username);
                        session.TrySend(mes);
                    }
                }
            }
            else
            {
                session.TrySend(string.Format("{0} {1} {2}\r\n", "Userlist", session.RemoteEndPoint.Address.ToString(),
                     Common.SessionDict[session.RemoteEndPoint.Address.ToString()].Username));
                foreach (var xn in Common.SessionDict[session.RemoteEndPoint.Address.ToString()].WorkItems)
                {
                    var mes = string.Format("{0} {1} {2} {3} {4} {5} {6} {7}\r\n", "Update", xn.Guid, xn.Name, xn.Description,
                        xn.Percentage, xn.Deadline.ToShortDateString(), xn.Priority, Common.SessionDict[session.RemoteEndPoint.Address.ToString()].Username);
                    session.TrySend(mes);
                }
            }
        }

        public static void SendToPrevileged(string mes)
        {
            var x = from c in Common.SessionDict
                    where c.Value.Previleged == true
                    select c;
            foreach (var n in x)
            {
                if (n.Value.Session == null)
                {
                    return;
                }
                n.Value.Session.TrySend(mes);
            }
        }

        public static void SendToNormal(string mes, string username)
        {
            var x = (from c in Common.SessionDict
                     where c.Value.Username == username
                     select c).SingleOrDefault();
            if (string.IsNullOrEmpty(x.Key)) return;
            if (x.Value.Session == null) return;
            x.Value.Session.TrySend(mes);
        }
    }
}
