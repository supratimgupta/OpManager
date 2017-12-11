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
        //private ISessionSvc _sessionSvc;

        //private Connections.UserConnectionRepo _uConnRepo;

        public override System.Threading.Tasks.Task OnConnected()
        {
            //ISessionSvc sessionSvc = new SessionSvc();
            Connections.UserConnectionRepo uConnRepo = Connections.UserConnectionRepo.GetInstance();

            //Values will be taken from session in this case
            var us=new Connections.UserConnection();
            us.UserName = Context.QueryString["UName"];
            us.ConnectionID = new List<string>();
            us.ConnectionID.Add(Context.ConnectionId);
            us.ICOImagePath = Context.QueryString["ICOImagePath"];
            us.UserRowId = Convert.ToInt32(Context.QueryString["URowId"]);
            bool isNewUser = false;
            uConnRepo.AddUserConnection(us.UserRowId, us, out isNewUser);

            Clients.Client(Context.ConnectionId).signalConnectionId(us.ConnectionID);

            //Clients.User(us.UserRowId.ToString()).signalConnectionId(us.ConnectionID);

            return base.OnConnected();
        }

        public void BroadcastJoiners()
        {
            Connections.UserConnectionRepo uConnRepo = Connections.UserConnectionRepo.GetInstance();

            Dictionary<int, Connections.UserConnection> connectedUsers = uConnRepo.GetAllActiveUsers();

            if(connectedUsers!=null && connectedUsers.Keys.Count>0)
            {
                Clients.All.broadcastUsers(connectedUsers.Values.ToArray());
            }
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            Connections.UserConnectionRepo uConnRepo = Connections.UserConnectionRepo.GetInstance();

            //Values will be taken from session in this case
            var us = new Connections.UserConnection();
            us.UserName = Context.QueryString["UName"];
            us.ConnectionID = new List<string>();
            us.ConnectionID.Add(Context.ConnectionId);
            us.ICOImagePath = Context.QueryString["ICOImagePath"];
            us.UserRowId = Convert.ToInt32(Context.QueryString["URowId"]);
            bool isNewUser = false;
            uConnRepo.AddUserConnection(us.UserRowId, us, out isNewUser);

            Clients.Client(Context.ConnectionId).signalConnectionId(us.ConnectionID);

            //Clients.User(us.UserRowId.ToString()).signalConnectionId(us.ConnectionID);

            return base.OnReconnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            Connections.UserConnectionRepo uConnRepo = Connections.UserConnectionRepo.GetInstance();
            uConnRepo.RemoveUserConnection(Convert.ToInt32(Context.QueryString["URowId"]), Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}