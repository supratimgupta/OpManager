using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OperationsManager.Areas.Student.Models
{
    public class StudentVM:StudentDTO
    {
        public string Name { get; set; }
    }
}