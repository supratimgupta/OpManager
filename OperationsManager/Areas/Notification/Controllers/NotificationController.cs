using OperationsManager.Controllers;
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
    public class NotificationController : BaseController
    {

        private INotificationSvc _notiSvc;

        private ISessionSvc _sessionSvc;

        public NotificationController(INotificationSvc notiSvc, ISessionSvc sessionSvc) 
        {
            _notiSvc = notiSvc;
            _sessionSvc = sessionSvc;
        }

        // GET: Notification/Notification
        [AllowAnonymous]
        public ActionResult ViewAll()
        {
            NotificationDTO nDTO = new NotificationDTO();
            nDTO.User = new UserMasterDTO();
            nDTO.User.UserMasterId = _sessionSvc.GetUserSession().UserMasterId;
            List<NotificationDTO> lstNotifications = _notiSvc.Select(nDTO).ReturnObj;

            Models.NotificationVM lstNotiVM = new Models.NotificationVM();
            if(lstNotifications!=null && lstNotifications.Count>0)
            {
                lstNotiVM.Notifications = new List<Models.NotificationVM>();
                Models.NotificationVM item = null;
                foreach(NotificationDTO noDTO in lstNotifications)
                {
                    item = new Models.NotificationVM();
                    item.NotificationText = noDTO.NotificationText;
                    item.NotificationReminderId = noDTO.NotificationReminderId;
                    item.MarkAsRead = false;

                    lstNotiVM.Notifications.Add(item);
                }
            }

            return View(lstNotiVM);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ViewAll(Models.NotificationVM notiVM)
        {
            if (notiVM != null && notiVM.Notifications != null && notiVM.Notifications.Count>0)
            {
                NotificationDTO nDTO = null;
                foreach(Models.NotificationVM nVM in notiVM.Notifications)
                {
                    try
                    {
                        nDTO = new NotificationDTO();
                        nDTO.NotificationReminderId = nVM.NotificationReminderId;
                        nDTO.Viewed = nVM.MarkAsRead;

                        _notiSvc.Update(nDTO);
                    }
                    catch { }
                }
            }

            SessionDTO session = _sessionSvc.GetUserSession();

            NotificationDTO nDTO1 = new NotificationDTO();
            nDTO1.User = new UserMasterDTO();
            nDTO1.User.UserMasterId = session.UserMasterId;
            session.Notifications = _notiSvc.Select(nDTO1).ReturnObj;
            if (session.Notifications != null)
            {
                session.NotificationCounts = session.Notifications.Count;
            }
            else
            {
                session.NotificationCounts = 0;
            }
            _sessionSvc.SetUserSession(session);

            return RedirectToAction("ViewAll");
        }

        [AllowAnonymous]
        public ActionResult Chat(int user)
        {
            return View();
        }
    }
}