using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class SessionDTO : DTOs.Users.UserDTO
    {
        public List<DTOs.Users.ActionDTO> ActionList { get; set; }

        public string CurrentRequestedPage { get; set; }
    }
}
