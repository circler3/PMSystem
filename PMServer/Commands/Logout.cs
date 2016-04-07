﻿using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace PMServer.Commands
{
    public class Logout : CommandBase<AppSession, StringRequestInfo>
    {
        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            Common.SessionDict[session.RemoteEndPoint.Address.ToString()] = null ;
        }
    }
}
