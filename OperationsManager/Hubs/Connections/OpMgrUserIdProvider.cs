using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Hubs.Connections
{
    public class OpMgrUserIdProvider : IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            // your logic to fetch a user identifier goes here.

            // for example:
            UserConnectionRepo uConnRepo = UserConnectionRepo.GetInstance();
            var userId = uConnRepo[Convert.ToInt32(request.QueryString["URowId"])];
            return userId==null?string.Empty:userId.UserRowId.ToString();
        }
    }
}