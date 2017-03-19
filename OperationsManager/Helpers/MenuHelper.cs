using OperationsManager.Models;
using OpMgr.Common.Contracts;
using OpMgr.Common.DTOs;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using System.Web;

namespace OperationsManager.Helpers
{
    public static class MenuHelper
    {
        public static List<MenuModel> GetMenuItems(ISessionSvc sessionSvc)
        {
            SessionDTO sessionDto = sessionSvc.GetUserSession();
            List<ActionDTO> actions = new List<ActionDTO>();
            actions = sessionDto.ActionList;
            List<MenuModel> lstMenu = null;

            if(actions!=null)
            {
                lstMenu = new List<MenuModel>();
                MenuModel menu;
                for (int i=0;i<actions.Count;i++)
                {
                    if(!string.IsNullOrEmpty(actions[i].ActionLink) && string.IsNullOrEmpty(actions[i].HiddenControlId) && string.IsNullOrEmpty(actions[i].DisabledControlId))
                    {
                        string menuText = OpMgr.Resources.Common.MenuResource.ResourceManager.GetString(actions[i].MenuText);
                        if(string.IsNullOrEmpty(actions[i].GroupName))
                        {
                            menu = lstMenu.FirstOrDefault(m => string.Equals(m.MenuCode, actions[i].MenuText));
                            if(menu == null)
                            {
                                menu = new MenuModel();
                                menu.MenuText = menuText;
                                menu.IsSelfRedirectable = true;
                                menu.ChildItems = null;
                                menu.UrlToRedirect = actions[i].ActionLink;
                                menu.MenuCode =  actions[i].MenuText;

                                if(string.Equals(menu.UrlToRedirect, HttpContext.Current.Request.Path, StringComparison.OrdinalIgnoreCase))
                                {
                                    menu.IsSelected = true;
                                }

                                lstMenu.Add(menu);
                            }
                        }
                        else
                        {
                            menu = new MenuModel();
                            menu.MenuText = menuText;
                            menu.IsSelfRedirectable = true;
                            menu.ChildItems = null;
                            menu.UrlToRedirect = actions[i].ActionLink;
                            menu.MenuCode = actions[i].MenuText;

                            if (string.Equals(menu.UrlToRedirect, HttpContext.Current.Request.Path, StringComparison.OrdinalIgnoreCase))
                            {
                                menu.IsSelected = true;
                            }

                            string groupName = OpMgr.Resources.Common.MenuResource.ResourceManager.GetString(actions[i].GroupName);
                            MenuModel addedRoot = lstMenu.FirstOrDefault(m => string.Equals(m.MenuCode, actions[i].GroupName));
                            if(addedRoot==null)
                            {
                                addedRoot = new MenuModel();
                                addedRoot.ChildItems = null;
                                addedRoot.IsSelfRedirectable = false;
                                addedRoot.MenuText = groupName;
                                addedRoot.UrlToRedirect = string.Empty;
                                addedRoot.MenuCode = actions[i].GroupName;
                                lstMenu.Add(addedRoot);
                            }
                            if(addedRoot.ChildItems==null)
                            {
                                addedRoot.ChildItems = new List<MenuModel>();
                                addedRoot.ChildItems.Add(menu);
                            }
                            else
                            {
                                if(addedRoot.ChildItems.FirstOrDefault(m => string.Equals(m.MenuCode, actions[i].MenuText)) == null)
                                {
                                    addedRoot.ChildItems.Add(menu);
                                }
                            }
                        }
                    }
                }
            }

            return lstMenu;
        }


        public static MvcHtmlString GetMenuDesign(List<MenuModel> lstMenu)
        {
            StringBuilder sbMenuHtml = new StringBuilder(string.Empty);

            if (lstMenu != null && lstMenu.Count > 0)
            {
                int subItemCounter = 0;
                for (int i = 0; i < lstMenu.Count; i++)
                {
                    if (lstMenu[i].IsSelfRedirectable || lstMenu[i].ChildItems == null || lstMenu[i].ChildItems==null || lstMenu[i].ChildItems.Count == 0)
                    {
                        if (lstMenu[i].IsSelected)
                        {
                            sbMenuHtml.AppendLine("<li class=\"active\"><a href=\""+lstMenu[i].UrlToRedirect+"\"> " + lstMenu[i].MenuText + "</a></li>");
                        }
                        else
                        {
                            sbMenuHtml.AppendLine("<li class=\"\"><a href=\"" + lstMenu[i].UrlToRedirect + "\"> " + lstMenu[i].MenuText + "</a></li>");
                        }
                    }
                    else
                    {
                        subItemCounter++;
                        var selectedChild = lstMenu[i].ChildItems.FirstOrDefault(m => m.IsSelected);
                        sbMenuHtml.AppendLine("<li class=\"parent\">");
                        sbMenuHtml.AppendLine("<a href=\"#\">");
                        sbMenuHtml.AppendLine("<span data-toggle=\"collapse\" href=\"#sub-item-" + subItemCounter + "\"><svg class=\"glyph stroked chevron-down\"><use xlink:href=\"#stroked-chevron-down\"></use></svg></span> " + lstMenu[i].MenuText);
                        sbMenuHtml.AppendLine("</a>");
                        if (selectedChild != null)
                        {
                            sbMenuHtml.AppendLine("<ul class=\"children\" id=\"sub-item-"+subItemCounter+"\">");
                        }
                        else
                        {
                            sbMenuHtml.AppendLine("<ul class=\"children collapse\" id=\"sub-item-" + subItemCounter + "\">");
                        }
                        for(int j=0;j<lstMenu[i].ChildItems.Count;j++)
                        {
                            if(lstMenu[i].ChildItems[j].IsSelected)
                            {
                                sbMenuHtml.AppendLine("<li class=\"active\">");
                                sbMenuHtml.AppendLine("<a class=\"active\" href=\""+lstMenu[i].ChildItems[j].UrlToRedirect+"\">");
                                sbMenuHtml.AppendLine("<svg class=\"glyph stroked chevron-right\" style=\"color:white;\"><use xlink:href=\"#stroked-chevron-right\" style=\"color:white;\"></use></svg><span style=\"color:white;\"> " + lstMenu[i].ChildItems[j].MenuText+"</span>");
                                sbMenuHtml.AppendLine("</a>");
                                sbMenuHtml.AppendLine("</li>");
                            }
                            else
                            {
                                sbMenuHtml.AppendLine("<li>");
                                sbMenuHtml.AppendLine("<a class=\"\" href=\"" + lstMenu[i].ChildItems[j].UrlToRedirect + "\">");
                                sbMenuHtml.AppendLine("<svg class=\"glyph stroked chevron-right\"><use xlink:href=\"#stroked-chevron-right\"></use></svg> " + lstMenu[i].ChildItems[j].MenuText);
                                sbMenuHtml.AppendLine("</a>");
                                sbMenuHtml.AppendLine("</li>");
                            }
                            
                        }
                        sbMenuHtml.AppendLine("</ul>");
                        sbMenuHtml.AppendLine("</li>");
                    }
                }
            }

            return new MvcHtmlString(sbMenuHtml.ToString());
        }
    }
}