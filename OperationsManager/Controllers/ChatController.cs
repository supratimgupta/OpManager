using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Controllers
{
    //[RoutePrefix("api/chat")]
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
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetUnreadMessages()
        {
            List<ChatLogDTO> chats = _chatSvc.GetUnreadMessages(_sessionSvc.GetUserSession().UserMasterId);

            List<Models.ChatViewModel> viewChats = new List<Models.ChatViewModel>();
            if(chats!=null && chats.Count>0)
            {
                viewChats = chats.GroupBy(ch => ch.SentByUser.UserMasterId).Select(vc => new Models.ChatViewModel{ 
                            UserName = (vc.First().SentByUser.FName +" " + vc.First().SentByUser.MName +" " +vc.First().SentByUser.LName),
                            UnreadChat = vc.Count(),
                            LastSentAt = vc.Max(oh=>oh.DateTimeLog),
                            LastSentAtString = vc.Max(oh=>oh.DateTimeLog).ToString("yyyy-MM-dd hh:mm:ss")
                            }).ToList();
            }
            

            return Json(new {data = viewChats, totalCount=viewChats.Sum(vc=>vc.UnreadChat)}, JsonRequestBehavior.AllowGet);
        }
    }
}