using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OperationsManager.Areas.Library.Models;
using OpMgr.Common.DTOs;

namespace OperationsManager.Areas.Library.Controllers
{
    public class LibraryController : Controller
    {
        private IBookMasterSvc _bookmasterSvc;

        private ILogSvc _logger;

        private ISessionSvc _sessionSvc;

        private IDropdownRepo _ddlRepo;

        private Helpers.UIDropDownRepo _uiddlRepo;

       
        public LibraryController(IBookMasterSvc bookmasterSvc, IDropdownRepo ddlRepo, ISessionSvc sessionSvc)
        {
            _bookmasterSvc = bookmasterSvc;
            _ddlRepo = ddlRepo;
            //new OpMgr.DataAccess.Implementations.DropdownRepo(new OpMgr.Configurations.Implementations.ConfigSvc());
            _uiddlRepo = new Helpers.UIDropDownRepo(_ddlRepo);
            //_logger = logger;

            _sessionSvc = sessionSvc;
        }

        // GET: Library/Library
        [HttpGet]
        public ActionResult RegisterBooks(string mode, string id)
        {
            LibraryViewModel lvModel = new LibraryViewModel();
            lvModel.MODE = mode;
            if (mode != null && string.Equals(mode, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                //Populate edit data using id passed in URL, if id==null then show error message
                StatusDTO<BookMasterDTO> dto = _bookmasterSvc.Select(Convert.ToInt32(id));
                lvModel.BookMasterId = dto.ReturnObj.BookMasterId;
                lvModel.BookName = dto.ReturnObj.BookName;
                lvModel.AuthorName1 = dto.ReturnObj.AuthorName1;
                lvModel.AuthorName2 = dto.ReturnObj.AuthorName2;
                lvModel.PurchaseDate = dto.ReturnObj.PurchaseDate;
                lvModel.AccNo = dto.ReturnObj.AccNo;
                lvModel.AccDate = dto.ReturnObj.AccDate;
                lvModel.CALLNO = dto.ReturnObj.CALLNO;
                lvModel.ISBNNo = dto.ReturnObj.ISBNNo;
                lvModel.Edition = dto.ReturnObj.Edition;
                lvModel.Publisher = dto.ReturnObj.Publisher;
                lvModel.PublishingYear = dto.ReturnObj.PublishingYear;
                lvModel.Place = dto.ReturnObj.Place;
                lvModel.Series = dto.ReturnObj.Series;
                lvModel.Price = dto.ReturnObj.Price;
                lvModel.NOFCD = dto.ReturnObj.NOFCD;
                lvModel.BookCategory = dto.ReturnObj.BookCategory;
                lvModel.Location = dto.ReturnObj.Location;

            }            
            lvModel.BookCategoryList = _uiddlRepo.getBookCategoryDropDown();
            lvModel.LocationList = _uiddlRepo.getLocationDropDown();
            return View(lvModel);

        }

        [HttpPost]
        public ActionResult RegisterBooks(LibraryViewModel lvModel)

        {
            if (string.Equals(lvModel.MODE, "EDIT", StringComparison.OrdinalIgnoreCase))
            {
                //Call update
                _bookmasterSvc.Update(lvModel);
            }
            else
            {
                //Call insert
                _bookmasterSvc.Insert(lvModel);
            }
            
            lvModel.BookCategoryList = _uiddlRepo.getBookCategoryDropDown();
            lvModel.LocationList = _uiddlRepo.getLocationDropDown();
            return View(lvModel);
        }
    }
}