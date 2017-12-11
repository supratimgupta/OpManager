using OperationsManager.Models;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Notification.Controllers
{
    public class ChatController : Controller
    {
        private ISessionSvc _sessionSvc;

        private IChatSvc _chatSvc;

        public ChatController(ISessionSvc sessionSvc, IChatSvc chatSvc)
        {
            _sessionSvc = sessionSvc;
            _chatSvc = chatSvc;
        }

        //[Route("getUnread")]
        [HttpGet]
        [AllowAnonymous]        
        public JsonResult GetUnreadMessages()
        {
            List<ChatLogDTO> chats = _chatSvc.GetUnreadMessages(_sessionSvc.GetUserSession().UserMasterId);

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


            return Json(new { data = viewChats, totalCount = viewChats.Sum(vc => vc.UnreadChat) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetChatHistory(int sender, int skipRows)
        {
            List<ChatLogDTO> chats = _chatSvc.GetChatHistory(_sessionSvc.GetUserSession().UserMasterId, sender, skipRows, 10);

            if(chats==null)
            {
                chats = new List<ChatLogDTO>();
            }

            return Json(chats, JsonRequestBehavior.AllowGet);
        }
    }
}