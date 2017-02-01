using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs.Users
{
    public class WorkLocationDTO
    {
        public int RowId { get; set; }

        public string LocationName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string ContactNumber { get; set; }

        public string AlternateNumber { get; set; }

        public string Pincode { get; set; }
    }
}
