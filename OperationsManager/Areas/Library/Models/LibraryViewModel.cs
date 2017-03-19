using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.Library.Models
{
    public class LibraryViewModel : BookMasterDTO
    {
        public string MODE { get; set; }
        public SelectList LocationList { get; set; }
        public SelectList BookCategoryList { get; set; }
    }
}