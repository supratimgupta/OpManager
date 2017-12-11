using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.Contracts.Modules
{
    public interface IChatSvc
    {
        List<DTOs.ChatLogDTO> GetUnreadMessages(int toUserId);

        StatusDTO<ChatLogDTO> SendMessage(DTOs.ChatLogDTO chat);

        List<DTOs.ChatLogDTO> GetChatHistory(int toUserId, int fromUserId, int skipRows, int takeRow);

        bool MarkAsRead(int chatLogId);

        bool MarkAllChatAsReadForUser(int senderRowId, int recieverRowId);
    }
}
