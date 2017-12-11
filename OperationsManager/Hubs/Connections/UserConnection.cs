using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Hubs.Connections
{
    public class UserConnection
    {
        public string UserName { get; set; }

        public int UserRowId { get; set; }

        public List<string> ConnectionID { get; set; }

        public string ICOImagePath { get; set; }
    }
}