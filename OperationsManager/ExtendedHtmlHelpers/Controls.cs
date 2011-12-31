using OperationsManager.Helpers;
using OpMgr.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace OperationsManager.ExtendedHtmlHelpers
{
    public static class Controls
    {
        static ISessionSvc _sessionSvc;
        static Controls()
        {
            _sessionSvc = new SessionSvc();
        }

        private static object GetPropValue(object src, string propName)
        {
            object value = null;
            if (src.GetType().GetProperty(propName) != null)
            {
                value = src.GetType().GetProperty(propName).GetValue(src, null);
            }
            return value;
        }

        public static MvcHtmlString OpMgrDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string labelText, object htmlAttributes = null)
        {
            if (htmlAttributes != null)
            {
                object objId = GetPropValue(htmlAttributes, "id");
                if (objId != null)
                {
                    string id = objId.ToString();
                    OpMgr.Common.DTOs.SessionDTO session = _sessionSvc.GetUserSession();
                    var disabledControl = session.ActionList.FirstOrDefault(a => string.Equals(a.DisabledControlId, id) && string.Equals(a.ParentAction.URL, session.CurrentRequestedPage));
                    var hiddenControl = session.ActionList.FirstOrDefault(a => string.Equals(a.HiddenControlId, id) && string.Equals(a.ParentAction.URL, session.CurrentRequestedPage));
                    if (hiddenControl != null)
                    {
                        return new MvcHtmlString("");
                    }
                    if (disabledControl != null)
                    {
                        return FormLine(helper.LabelFor(expression, labelText).ToString(), helper.DropDownListFor(expression, selectList, htmlAttributes).ToString(), true, false);
                    }
                }

            }
            return FormLine(helper.LabelFor(expression, labelText).ToString(), helper.DropDownListFor(expression, selectList, htmlAttributes).ToString(), false, false);
        }

        public static MvcHtmlString OpMgrTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string labelText, object htmlAttributes = null)
        {
            if (htmlAttributes != null)
            {
                object objId = GetPropValue(htmlAttributes, "id");
                if (objId != null)
                {
                    string id = objId.ToString();
                    OpMgr.Common.DTOs.SessionDTO session = _sessionSvc.GetUserSession();
                    var disabledControl = session.ActionList.FirstOrDefault(a => string.Equals(a.DisabledControlId, id) && string.Equals(a.ParentAction.URL, session.CurrentRequestedPage));
                    var hiddenControl = session.ActionList.FirstOrDefault(a => string.Equals(a.HiddenControlId, id) && string.Equals(a.ParentAction.URL, session.CurrentRequestedPage));
                    if (hiddenControl != null)
                    {
                        return new MvcHtmlString("");
                    }
                    if (disabledControl != null)
                    {
                        return FormLine(helper.LabelFor(expression, labelText).ToString(), helper.TextBoxFor(expression, htmlAttributes).ToString(), true, false);
                    }
                }

            }
            return FormLine(helper.LabelFor(expression, labelText).ToString(), helper.TextBoxFor(expression, htmlAttributes).ToString(), false, false);
        }

        public static MvcHtmlString OpMgrTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string labelText, object htmlAttributes = null)
        {
            if (htmlAttributes != null)
            {
                object objId = GetPropValue(htmlAttributes, "id");
                if (objId != null)
                {
                    string id = objId.ToString();
                    OpMgr.Common.DTOs.SessionDTO session = _sessionSvc.GetUserSession();
                    var disabledControl = session.ActionList.FirstOrDefault(a => string.Equals(a.DisabledControlId, id) && string.Equals(a.ParentAction.URL, session.CurrentRequestedPage));
                    var hiddenControl = session.ActionList.FirstOrDefault(a => string.Equals(a.HiddenControlId, id) && string.Equals(a.ParentAction.URL, session.CurrentRequestedPage));
                    if (hiddenControl != null)
                    {
                        return new MvcHtmlString("");
                    }
                    if (disabledControl != null)
                    {
                        return FormLine(helper.LabelFor(expression, labelText).ToString(), helper.TextAreaFor(expression, htmlAttributes).ToString(), true, false);
                    }
                }

            }
            return FormLine(helper.LabelFor(expression, labelText).ToString(), helper.TextAreaFor(expression, htmlAttributes).ToString(), false, false);
        }

        public static MvcHtmlString OpMgrCheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, bool>> expression, string labelText, object htmlAttributes = null)
        {
            if (htmlAttributes != null)
            {
                object objId = GetPropValue(htmlAttributes, "id");
                if (objId != null)
                {
                    string id = objId.ToString();
                    OpMgr.Common.DTOs.SessionDTO session = _sessionSvc.GetUserSession();
                    var disabledControl = session.ActionList.FirstOrDefault(a => string.Equals(a.DisabledControlId, id) && string.Equals(a.ParentAction.URL, session.CurrentRequestedPage));
                    var hiddenControl = session.ActionList.FirstOrDefault(a => string.Equals(a.HiddenControlId, id) && string.Equals(a.ParentAction.URL, session.CurrentRequestedPage));
                    if (hiddenControl != null)
                    {
                        return new MvcHtmlString("");
                    }
                    if (disabledControl != null)
                    {
                        return FormLine(helper.LabelFor(expression, labelText).ToString(), helper.CheckBoxFor(expression, htmlAttributes).ToString(), true, false);
                    }
                }

            }
            return FormLine(helper.LabelFor(expression, labelText).ToString(), helper.CheckBoxFor(expression, htmlAttributes).ToString(), false, false);
        }

        private static MvcHtmlString FormLine(string labelContent, string fieldContent, bool isDisabled, bool isHidden, object htmlAttributes = null)
        {
            if(isHidden)
            {
                new MvcHtmlString("");
            }
            var editorLabel = new TagBuilder("div");
            editorLabel.AddCssClass("editor-label");
            editorLabel.InnerHtml += labelContent;

            var editorField = new TagBuilder("div");
            editorField.AddCssClass("editor-field");
            if(isDisabled)
            {
                editorField.AddCssClass("disabledDiv");
            }
            editorField.InnerHtml += fieldContent;

            var container = new TagBuilder("div");
            //if (htmlAttributes != null)
                //container.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            //container.AddCssClass("form-line");
            container.InnerHtml += editorLabel;
            container.InnerHtml += editorField;

            return MvcHtmlString.Create(container.ToString());
        }

        //public static MvcHtmlString HelpTextFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string customText = null)
        //{
        //    // Can do all sorts of things here -- eg: reflect over attributes and add hints, etc...
        //} 
    }
}