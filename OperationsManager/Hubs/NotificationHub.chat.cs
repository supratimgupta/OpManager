using Microsoft.AspNet.SignalR;
using Ninject;
using OperationsManager.Helpers;
using OperationsManager.Models;
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
        public void SendMessage(int toUser, int fromUser, string message)
        {
            Connections.UserConnectionRepo uConnRepo = Connections.UserConnectionRepo.GetInstance();

            var sender = uConnRepo[fromUser];
            var reciever = uConnRepo[toUser];

            ChatLogDTO chatLog = new ChatLogDTO();
            chatLog.SentByUser = new UserMasterDTO();
            chatLog.SentByUser.UserMasterId = sender.UserRowId;

            chatLog.SentToUser = new UserMasterDTO();
            chatLog.SentToUser.UserMasterId = reciever.UserRowId;

            chatLog.Message = message;
            chatLog.IsRead = false;

            NinjectControllerFactory ncf = new NinjectControllerFactory();
            IKernel kernel = ncf.GetNinjectKernel();

            StatusDTO<ChatLogDTO> status = kernel.Get<IChatSvc>().SendMessage(chatLog);

            if(status.IsSuccess)
            {
                if (sender != null && reciever != null && !string.IsNullOrEmpty(message))
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

                    if (reciever.ConnectionID != null && reciever.ConnectionID.Count > 0)
                    {
                        foreach (string connectionId in reciever.ConnectionID)
                        {
                            hubContext.Clients.Client(connectionId).sendMessage(sender, message, status.ReturnObj.Id);
                        }
                    }
                }
            }
        }

        public void MessageAck(int chatLogId)
        {
            NinjectControllerFactory ncf = new NinjectControllerFactory();
            IKernel kernel = ncf.GetNinjectKernel();

            kernel.Get<IChatSvc>().MarkAsRead(chatLogId);
        }

        public void ShowTyping(int toUser)
        {
            Connections.UserConnectionRepo uConnRepo = Connections.UserConnectionRepo.GetInstance();
            var reciever = uConnRepo[toUser];

            if(reciever!=null)
            {
                if (reciever.ConnectionID != null && reciever.ConnectionID.Count > 0)
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    foreach (string connectionId in reciever.ConnectionID)
                    {
                        hubContext.Clients.Client(connectionId).showTyping();
                    }
                }
            }
        }

        public void MakeMsgsReadOnServer(int senderRowId, int recieverRowId)
        {
            NinjectControllerFactory ncf = new NinjectControllerFactory();
            IKernel kernel = ncf.GetNinjectKernel();

            kernel.Get<IChatSvc>().MarkAllChatAsReadForUser(senderRowId, recieverRowId);

            this.SendChatsNotRead(recieverRowId);
        }

        public void SendChatsNotRead(int recieverRowId)
        {
            NinjectControllerFactory ncf = new NinjectControllerFactory();
            IKernel kernel = ncf.GetNinjectKernel();

            List<ChatLogDTO> chats = kernel.Get<IChatSvc>().GetUnreadMessages(recieverRowId);

            List<ChatViewModel> viewChats = new List<ChatViewModel>();
            if (chats != null && chats.Count > 0)
            {
                viewChats = chats.GroupBy(ch => ch.SentByUser.UserMasterId).Select(vc => new ChatViewModel
                {
                    UserName = (vc.First().SentByUser.FName + " " + vc.First().SentByUser.MName + " " + vc.First().SentByUser.LName),
                    UserMasterId = vc.First().SentByUser.UserMasterId,
                    UnreadChat = vc.Count(),
                    LastSentAt = vc.Max(oh => oh.DateTimeLog),
                    LastSentAtString = vc.Max(oh => oh.DateTimeLog).ToString("yyyy-MM-dd hh:mm:ss")
                }).ToList();  
            }

            Connections.UserConnectionRepo uConnRepo = Connections.UserConnectionRepo.GetInstance();
            var reciever = uConnRepo[recieverRowId];
            if (reciever != null)
            {
                if (reciever.ConnectionID != null && reciever.ConnectionID.Count > 0)
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                    foreach (string connectionId in reciever.ConnectionID)
                    {
                        hubContext.Clients.Client(connectionId).populateMessageSummaryCL(new { data = viewChats, totalCount = viewChats.Sum(vc => vc.UnreadChat) });
                    }
                }
            }
        }
    }
}