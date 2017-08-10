using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Models
{
    public class MenuModel
    {
        public string MenuText { get; set; }

        public string MenuImage { get; set; }

        public string UrlToRedirect { get; set; }

        public bool IsSelfRedirectable { get; set; }

        public List<MenuModel> ChildItems { get; set; }

        public bool IsSelected { get; set; }

        public string MenuCode { get; set; }
    }
}