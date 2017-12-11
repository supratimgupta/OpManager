using Microsoft.AspNet.SignalR;
using OperationsManager.Helpers;
using OpMgr.Common.Contracts;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Hubs
{
    public partial class NotificationHub : Hub
    {
        public void SendNotification(int senderRowId, int recieverRowId, string message)
        {
            Connections.UserConnectionRepo uConnRepo = Connections.UserConnectionRepo.GetInstance();

            var sender = uConnRepo[senderRowId];
            var reciever = uConnRepo[recieverRowId];

            if (sender != null && reciever != null && !string.IsNullOrEmpty(message))
            {
                if (message.Length > 50)
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

                    message = "<b>" + sender.UserName + " says: </b>" + message.Substring(0, 50) + "...";
                    if (reciever.ConnectionID != null && reciever.ConnectionID.Count > 0)
                    {
                        foreach (string connectionId in reciever.ConnectionID)
                        {
                            hubContext.Clients.Client(connectionId).sendMessage(message);
                        }
                    }
                }
            }
        }
    }
}