using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using OpMgr.Common.Contracts;
using System.IO;

namespace OperationsManager.Areas.DataImport.Controllers
{
    public class DataImportController : Controller
    {
        IConfigSvc _configSvc;
        public DataImportController(IConfigSvc configSvc)
        {
            _configSvc = configSvc;
        }

        // GET: DataImport/DataImport
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(Models.DataImportViewModel diVM)
        {
            diVM = new Models.DataImportViewModel();
            try
            {
                if (Request.Files.Count > 0)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        string key = Request.Files.Keys[i];
                        string[] fileNameParts = Request.Files[i].FileName.Split('\\');
                        switch (key)
                        {
                            case "fuStudentFile":
                                Request.Files[i].SaveAs(Path.Combine(_configSvc.StudentDataUploadPath(), fileNameParts[fileNameParts.Length - 1]));
                                break;
                            case "fuStaffFile":
                                Request.Files[i].SaveAs(Path.Combine(_configSvc.StaffDataUploadPath(), fileNameParts[fileNameParts.Length - 1]));
                                break;
                        }
                    }
                    diVM.Message = "Files uploaded successfully.";
                    diVM.Status = true;
                }
                else
                {
                    diVM.Message = "No file in the request.";
                    diVM.Status = false;
                }
            }
            catch(Exception exp)
            {
                diVM.Message = "Error: " + exp.Message;
                diVM.Status = false;
            }
            return View(diVM);
        }
    }
}