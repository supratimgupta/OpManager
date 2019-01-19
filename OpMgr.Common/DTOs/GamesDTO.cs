using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
   public class GamesDTO
    {
        public int GamesId { get; set; }

        public string GamesName { get; set; }

        public bool IsActive { get; set; }
        public bool IsSelected { get; set; }
    }
}
