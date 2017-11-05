using OperationsManager.Areas.User.Models;
using OperationsManager.Attributes;
using OperationsManager.Controllers;
using OpMgr.Common.Contracts;
using OpMgr.Common.Contracts.Modules;
using OpMgr.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OperationsManager.Areas.User.Controllers
{
    public class UserController : BaseController
    {
        private IUserSvc _userSvc;
        //private ILogSvc _logSvc;
        private IDropdownRepo _dropDownRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;
        private ISessionSvc _sessionSvc;

        public UserController(IUserSvc userSvc, ISessionSvc sessionSvc, IDropdownRepo dropDownRepo)
        {
            _userSvc = userSvc;
            _dropDownRepo = dropDownRepo;
            _sessionSvc = sessionSvc;
            _uiddlRepo = new Helpers.UIDropDownRepo(_dropDownRepo);

        }

        [HttpGet]
        public ActionResult Search()
        {
            StatusDTO<List<UserMasterDTO>> status = _userSvc.Select(null);

            UserVM userView = userView = new UserVM(); 
            userView.GenderList = _uiddlRepo.getGenderDropDown();
            

            if (status != null && status.ReturnObj != null && status.ReturnObj.Count > 0)
            {
                userView.IsSearchSuccessful = true;// Grid is displayed with records
                userView.UserList = new List<UserVM>(); // instantiating list of Students
                SessionDTO sessionRet = _sessionSvc.GetUserSession();

                //userView.DepartmentList = _uiddlRepo.getDepartmentDropDown();
                userView.LocationList = _uiddlRepo.getLocationDropDown();
                //userView.LocationList = sessionRet.LocationList;

                //Fetch the Gender List

                if (status.IsSuccess && !status.IsException)
                {
                    UserVM searchItem = null;
                    
                    foreach (UserMasterDTO user in status.ReturnObj)
                    {
                        if (user != null)
                        {
                            searchItem = new UserVM(); // instantiating each user

                            searchItem.UserMasterId = user.UserMasterId;
                            searchItem.FName = user.FName;
                            //searchItem.MName = user.MName;
                            searchItem.LName = user.LName;

                            //forming the Name
                            searchItem.Name = searchItem.FName;
                            if (!string.IsNullOrEmpty(searchItem.MName))
                            {
                                searchItem.Name = searchItem.Name + " "+ searchItem.MName;
                            }
                            searchItem.Name = searchItem.Name+ " " + searchItem.LName;

                            searchItem.Gender = user.Gender;
                            searchItem.EmailId = user.EmailId;
                            searchItem.ResidentialAddress = user.ResidentialAddress;
                            searchItem.PermanentAddress = user.PermanentAddress;
                            searchItem.ContactNo = user.ContactNo;
                            searchItem.AltContactNo = user.AltContactNo;
                            //searchItem.BloodGroup = user.BloodGroup;
                            searchItem.Location = new LocationDTO();
                            searchItem.Employee = new EmployeeDetailsDTO();
                            searchItem.Location.LocationDescription = user.Location.LocationDescription;
                            searchItem.Employee.StaffEmployeeId = user.Employee.StaffEmployeeId;

                            searchItem.Role = new RoleDTO();
                            searchItem.Role.RoleDescription = user.Role.RoleDescription;

                            
                            userView.UserList.Add(searchItem);
                            userView.IsSearchSuccessful = true;
                        }
                    }
                }
                else if (status.IsException)
                {
                    throw new Exception(status.ExceptionMessage);
                }
            }

             return View(userView);
        }

        [HttpPost]
        public ActionResult Search(UserVM userView,string Command)
        {
            UserVM uView = null;
            UserMasterDTO user = null;

            if(string.Equals(Command,"Add"))
            {
                return RedirectToAction("Register", "Login", new { area = "Login" });
            }
            
            if (userView != null)
            {
                //Fetch the StandardSection List
                userView.GenderList = _uiddlRepo.getGenderDropDown();
                user = new UserMasterDTO();
                // Search for FName LName and MName
                user.FName = userView.FName;
                //user.MName = userView.MName;
                user.LName = userView.LName;                

                // Search for Gender
                user.Gender = userView.Gender;

                // Search for Location
                user.Location = new LocationDTO();
                user.Location.LocationId = userView.Location.LocationId;

                // Search for Department
                //user.Employee = new EmployeeDetailsDTO();
                //user.Employee.Department = new DepartmentDTO();
                //user.Employee.Department = userView.Employee.Department;

                //search by userempid                
                user.Employee.StaffEmployeeId = userView.Employee.StaffEmployeeId;

                // Search for BloodGroup
                user.BloodGroup = userView.BloodGroup;
                StatusDTO<List<UserMasterDTO>> status = _userSvc.Select(user);
                if (status != null && status.ReturnObj != null && status.ReturnObj.Count > 0)
                {
                   // userView = new UserVM();
                    //userView.IsSearchSuccessful = true;// Grid is displayed with records

                    uView = new UserVM(); // Instantiating Student View model
                    uView.UserList = new List<UserVM>(); // instantiating list of Students

                    //Fetch the Gender List
                    uView.GenderList = _uiddlRepo.getGenderDropDown();
                    uView.LocationList = _uiddlRepo.getLocationDropDown();
                    //uView.DepartmentList = _uiddlRepo.getDepartmentDropDown();

                    if (status.IsSuccess && !status.IsException)
                    {
                        UserVM searchItem = null;
                        foreach (UserMasterDTO u in status.ReturnObj)
                        {
                            if (u != null)
                            {
                                searchItem = new UserVM(); // instantiating each user

                                searchItem.UserMasterId = u.UserMasterId;
                                searchItem.FName = u.FName;
                                searchItem.MName = u.MName;
                                searchItem.LName = u.LName;

                                //forming the Name
                                searchItem.Name = searchItem.FName;
                                if (!string.IsNullOrEmpty(searchItem.MName))
                                {
                                    searchItem.Name = searchItem.Name + " " + searchItem.MName;
                                }
                                searchItem.Name = searchItem.Name + " " + searchItem.LName;

                                searchItem.Gender = u.Gender;
                                searchItem.EmailId = u.EmailId;
                                searchItem.ResidentialAddress = u.ResidentialAddress;
                                searchItem.PermanentAddress = u.PermanentAddress;
                                searchItem.ContactNo = u.ContactNo;
                                searchItem.AltContactNo = u.AltContactNo;
                                searchItem.BloodGroup = u.BloodGroup;

                                searchItem.Role = new RoleDTO();
                                searchItem.Role.RoleDescription = u.Role.RoleDescription;

                                searchItem.Location = new LocationDTO();
                                searchItem.Location.LocationDescription = u.Location.LocationDescription;
                                
                                searchItem.Employee.StaffEmployeeId = u.Employee.StaffEmployeeId;

                                uView.UserList.Add(searchItem);
                                uView.IsSearchSuccessful = true;
                            }
                        }
                    }
                }
               else
                {                    
                    userView.IsSearchSuccessful = false;
                    uView = userView;
                    uView.GenderList = _uiddlRepo.getGenderDropDown();
                    uView.LocationList = _uiddlRepo.getLocationDropDown();
                    uView.DepartmentList = _uiddlRepo.getDepartmentDropDown();
                }
            }

          return View(uView);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult DeleteUser(UserMasterDTO user)
        {
            //UserMasterDTO user = null;
            if (user.UserMasterId!=0)
            {
                //user = new UserMasterDTO();
                //user.UserMasterId = id;
                StatusDTO<UserMasterDTO> status=_userSvc.Delete(user);
                if(status!=null && status.IsSuccess)
                {

                }
            }
            return Json( new { message = "success", status = "true" },JsonRequestBehavior.AllowGet);
        }


    }
}