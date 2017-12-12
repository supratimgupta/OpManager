using Microsoft.AspNet.SignalR;
using Ninject;
using OperationsManager.Helpers;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
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
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

                NinjectControllerFactory ncf = new NinjectControllerFactory();
                IKernel kernel = ncf.GetNinjectKernel();

                NotificationDTO notiDTO = new NotificationDTO();
                notiDTO.User = new UserMasterDTO();
                notiDTO.User.UserMasterId = recieverRowId;

                var test = kernel.Get<INotificationSvc>().Select(notiDTO);

                if(test.ReturnObj!=null && test.ReturnObj.Count>0)
                {
                    string[] messages = test.ReturnObj.Select(m => m.NotificationText).ToArray();

                    if(message.Length>0)
                    {
                        if (reciever.ConnectionID != null && reciever.ConnectionID.Count > 0)
                        {
                            foreach (string connectionId in reciever.ConnectionID)
                            {
                                hubContext.Clients.Client(connectionId).sendMessage(messages);
                            }
                        }
                    }                    
                }
            }
        }
    }
}