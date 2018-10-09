using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
   public class CoCurricularDTO
    {
        public int CoCurricularId { get; set; }

        public string CoCurricularName { get; set; }

        public bool IsActive { get; set; }
        public bool IsSelected { get; set; }
    }
}
