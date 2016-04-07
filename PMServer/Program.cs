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
            var appServer = new AppServer();
            appServer.NewSessionConnected += AppServer_NewSessionConnected; ;
            //使用命令简化模型
            appServer.NewRequestReceived += AppServer_NewRequestReceived;
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
                //foreach (var n in appServer.GetAllSessions())
                //{
                //    var xn = HexToByte(x);
                //    n.TrySend(xn, 0, xn.Length);
                //}
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }

        private static void AppServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            Console.WriteLine(requestInfo.Body);
            session.TrySend(requestInfo.Body);
        }

        private static void AppServer_NewSessionConnected(AppSession session)
        {
            session.TrySend("WELCOME!");
        }



        private static string ByteToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        private static string ByteToHexString2(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        private static byte[] HexToByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
    }
}
