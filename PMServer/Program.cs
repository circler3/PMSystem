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
            Ini iniparser = new Ini(@"Config/user.ini");
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
            ServerConfig sc = new ServerConfig() { Port = 2020, TextEncoding = "gb2312" };
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
            Common.SessionDict[session.RemoteEndPoint.Address.ToString()] = null;
        }

        private static void AppServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine(requestInfo.Body);
            session.TrySend(requestInfo.ToString());
        }

        private static void AppServer_NewSessionConnected(AppSession session)
        {
            session.TrySend("WELCOME!");
            if (Common.SessionDict.ContainsKey(session.RemoteEndPoint.Address.ToString()))
            {
                Common.SessionDict[session.RemoteEndPoint.Address.ToString()].Session = session;
            }
        }

        public static void SendTo()
        {

        }
    }
}
