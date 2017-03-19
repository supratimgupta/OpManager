using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpMgr.Common.DTOs
{
    public class BookMasterDTO
    {
        public UserMasterDTO UserMaster { get; set; }

        public int BookMasterId { get; set; }

        public UserMasterDTO CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public UserMasterDTO UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public bool Active { get; set; }

        [Required(ErrorMessage = "Book Name is required")]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Author Name is required")]
        public string AuthorName1 { get; set; }

        public string AuthorName2 { get; set; }

        [Required(ErrorMessage = "Purchase Date is required")]
        public DateTime? PurchaseDate { get; set; }

        [Required(ErrorMessage = "Acc No is required")]
        public string AccNo { get; set; }

        [Required(ErrorMessage = "Acc Date is required")]
        public DateTime? AccDate { get; set; }

        [Required(ErrorMessage = "Call No is required")]
        public string CALLNO { get; set; }

        [Required(ErrorMessage = "ISBN No is required")]
        public string ISBNNo { get; set; }

        [Required(ErrorMessage = "Edition is required")]
        public string Edition { get; set; }

        [Required(ErrorMessage = "Publisher is required")]
        public string Publisher { get; set; }

        public DateTime? PublishingYear { get; set; }

        [Required(ErrorMessage = "Place is required")]
        public string Place { get; set; }

        public string Series { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public string Price { get; set; }

        public string NOFCD { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public LocationDTO Location { get; set; }

        [Required(ErrorMessage = "Book Category is required")]
        public BookCategoryDTO BookCategory { get; set; }

        public BookTypeDTO BookType { get; set; }
    }
}
