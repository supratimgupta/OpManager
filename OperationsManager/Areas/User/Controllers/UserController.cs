using OperationsManager.Areas.User.Models;
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
    public class UserController : Controller
    {
        private IUserSvc _userSvc;
        private ILogSvc _logSvc;
        private IDropdownRepo _dropDownRepo;
        private Helpers.UIDropDownRepo _uiddlRepo;

        public UserController(IUserSvc userSvc,IDropdownRepo dropDownRepo)
        {
            _userSvc = userSvc;
            _dropDownRepo = dropDownRepo;
            _uiddlRepo = new Helpers.UIDropDownRepo(_dropDownRepo);

        }

        [HttpGet]
        public ActionResult Search()
        {
            StatusDTO<List<UserMasterDTO>> status = _userSvc.Select(null);

            UserVM userView = null;

            if (status != null && status.ReturnObj != null && status.ReturnObj.Count > 0)
            {
                userView = new UserVM();

                userView.IsSearchSuccessful = true;// Grid is displayed with records

                userView = new UserVM(); // Instantiating Student View model
                userView.UserList = new List<UserVM>(); // instantiating list of Students

                //Fetch the Gender List
                userView.GenderList = _uiddlRepo.getGenderDropDown();

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
                            searchItem.MName = user.MName;
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
                            searchItem.BloodGroup = user.BloodGroup;

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
        public ActionResult Search(UserVM userView)
        {
            UserVM uView = null;
            UserMasterDTO user = null;

            



            if (userView != null)
            {

                //Fetch the StandardSection List
                userView.GenderList = _uiddlRepo.getGenderDropDown();

                user = new UserMasterDTO();
                

                // Search for FName LName and MName

                user.FName = userView.FName;
                user.MName = userView.MName;
                user.LName = userView.LName;


                

                // Search for Gender
                user.Gender = userView.Gender;

                // Search for Role
                user.Role = new RoleDTO();
                user.Role.RoleDescription = userView.Role.RoleDescription;

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
                }
            }

          return View(uView);
        }
    }
}