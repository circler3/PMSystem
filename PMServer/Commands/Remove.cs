using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;

namespace PMServer.Commands
{
    public class Remove : CommandBase<AppSession, StringRequestInfo>
    {
        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            var x = requestInfo.Parameters[0];
            Common.SessionDict[session.RemoteEndPoint.Address.ToString()].WorkItems.RemoveAll(w => w.Guid == x);
        }
    }
}
