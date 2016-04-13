using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using System;

namespace PMServer.Commands
{
    public class Update : CommandBase<AppSession, StringRequestInfo>
    {
        public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
        {
            //            var mes = string.Format("{0} {1} {2} {3} {4} {5} {6}\r\n", "Remove", w.Guid, w.Name, w.Description, w.Percentage, w.Deadline.ToShortDateString(), w.Priority);
            if (requestInfo.Parameters.Length < 6)
            {
                return;
            }
            var x = requestInfo.Parameters[0];
            var workItem = Common.SessionDict[session.RemoteEndPoint.Address.ToString()].WorkItems.Find(w => w.Guid == x);
            if (workItem == null)
            {
                workItem = new WorkItem() { Guid = x };
                Common.SessionDict[session.RemoteEndPoint.Address.ToString()].WorkItems.Add(workItem);
            }

            workItem.Name = requestInfo.Parameters[1];
            workItem.Description = requestInfo.Parameters[2];
            workItem.Percentage = Convert.ToInt32(requestInfo.Parameters[3]);
            workItem.Deadline = Convert.ToDateTime(requestInfo.Parameters[4]);
            workItem.Priority = Convert.ToInt32(requestInfo.Parameters[5]);
            Console.WriteLine(requestInfo.Key + " " + requestInfo.Body);
            Program.SendToPrevileged(requestInfo.Key + " " + requestInfo.Body);
        }
    }
}
