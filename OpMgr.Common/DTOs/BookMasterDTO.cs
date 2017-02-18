using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class BookMasterDTO
    {
        public int BookMasterId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        public string BookName { get; set; }

        public string AuthorName1 { get; set; }

        public string AuthorName2 { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public string AccNo { get; set; }

        public DateTime? AccDate { get; set; }

        public string CALLNO { get; set; }

        public string ISBNNo { get; set; }

        public string Edition { get; set; }

        public string Publisher { get; set; }

        public DateTime? PublishingYear { get; set; }

        public string Place { get; set; }

        public string Series { get; set; }

        public string Price { get; set; }

        public string NOFCD { get; set; }

        public LocationDTO Location { get; set; }

        public BookCategoryDTO BookCategory { get; set; }

        public BookTypeDTO BookType { get; set; }
    }
}
