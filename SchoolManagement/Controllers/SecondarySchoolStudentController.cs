using JobHustler.DAL;
using JobHustler.Models;
using SchoolManagement.Model;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SchoolManagement.Controllers
{
    public class SecondarySchoolStudentController : Controller
    {
        //
        // GET: /SecondarySchoolStudent/
        UnitOfWork work = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult CodeEntryPartial()
        {
            // if (Request.IsAjaxRequest())
            // {
            return PartialView("CodeEntryPartial");
            // }
            return View();

        }

        [AllowAnonymous]
        public ViewResult ValidateCode(string code)
        {
            if (code == "555")
            {
                RedirectToAction("Create", "SecondarySchoolStudent");
                return View("Create");

            }
            else
            {
                ViewBag.ID = 1;
                return View("Login");
                //  RedirectToAction("Login", "Account", new { returnUrl ="/"});
            }
            // return View();

        }

        //
        // GET: /SecondarySchoolStudent/Details/5
        //[DynamicAuthorize]
        [Authorize]
        public ActionResult Details(string id, int tracker = 0)
        {
            if (tracker == 1)
            {
                //ViewData["Success"] = "Show the Dialog";
                ViewBag.Success = "Show the Dialog";
                //  ViewData["Success"] = "Show the Dialog";
            }
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            SecondarySchoolStudent theSec = work.SecondarySchoolStudentRepository.GetByID(theId);
            // //return View("Details2", theStudent);


            return View(theSec);
        }

        //
        // GET: /SecondarySchoolStudent/Create
        // [NonAction]
        // [NonAction]
        public ActionResult Create(string pass)
        {
            //if (!(string.IsNullOrEmpty(pass)))
            //{

            //    if (pass == "555")
            //    {
            return View();
            //  }
            // else
            //    {
            //        return RedirectToAction("Login", "Account");
            //    }

            //}
            //else
            //{
            //    return RedirectToAction("Login", "Account");
            //}
        }

        //
        // POST: /SecondarySchoolStudent/Create

        [HttpPost]
        public ActionResult Create(SecondarySchoolStudent model)
        {
            try
            {
                model.EnrollmentDate = DateTime.Now;
                if (!(ModelState.IsValid))
                {
                    return View(model);
                }

                model.IsApproved = false;
                // model.DateEntered = DateTime.Today;

                //  model.DateApproved = DateTime.Now;

                //  model.EnrollmentDate = DateTime.Now;

                work.SecondarySchoolStudentRepository.Insert(model);
                work.Save();
                //  }
                // TODO: Add insert logic here

                // ViewData["Success"] = "Dear " + model.FirstName + "Your data has been saved successfully, we shall get back at you soon";
                // return Content("Data added successfully");
                return RedirectToAction("Create", "Photo", new { id = SchoolManagement.Models.Encript.EncryptString(model.PersonID.ToString(), true) });
               // return RedirectToAction("Create", "Photo", new { id = model.PersonID });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SecondarySchoolStudent/Edit/5
        [DynamicAuthorize]
        public ActionResult Edit(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();

            theItem.Add(new SelectListItem() { Text = "None", Value = "" });
            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
            theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
            //theItem.Add(new SelectListItem() { Text = "Suspended", Value = "Suspended" });
            theItem.Add(new SelectListItem() { Text = "Expelled", Value = "Expelled" });
            theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });
            ViewData["Level"] = theItem;
            SecondarySchoolStudent theStudent = work.SecondarySchoolStudentRepository.GetByID(theId);
            if (((!(string.IsNullOrEmpty(theStudent.InitialLevel))) && (theStudent.IsApproved == false)))
            {
                if (theStudent.InitialLevel.Contains("KG") || theStudent.InitialLevel.StartsWith("NURS") || theStudent.InitialLevel.StartsWith("PRIMARY"))
                {
                    ViewBag.StudentID = theStudent.UserID;
                }
                if (theStudent.InitialLevel.StartsWith("JSS"))
                {
                    ViewBag.StudentID = 30000000 + theStudent.PersonID;
                }
                if (theStudent.InitialLevel.StartsWith("SSS"))
                {
                    ViewBag.StudentID = 40000000 + theStudent.PersonID;
                }
            }
            ViewBag.InitialLevel = theStudent.InitialLevel;
            return View(theStudent);
        }

        //
        // POST: /SecondarySchoolStudent/Edit/5
        [DynamicAuthorize]
        [HttpPost]
        public ActionResult Edit(SecondarySchoolStudent model)
        {
            try
            {
                // TODO: Add update logic here

                if (ModelState.IsValid)
                {
                    //  TryUpdateModel(
                    // PrimarySchoolStudent theStudent =   work2.PrimarySchoolStudentRepository.GetByID(model.PersonID);
                    model.Role = "Student";
                    if (model.IsApproved == true)
                    {
                        if (model.PresentLevel == null)
                        {
                            model.IsApproved = false;
                            List<Level> theLevels = work.LevelRepository.Get().ToList();
                            List<SelectListItem> theItem = new List<SelectListItem>();

                            theItem.Add(new SelectListItem() { Text = "None", Value = "" });
                            foreach (var level in theLevels)
                            {
                                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
                            }
                            theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
                            theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
                            //theItem.Add(new SelectListItem() { Text = "Suspended", Value = "Suspended" });
                            theItem.Add(new SelectListItem() { Text = "Expelled", Value = "Expelled" });
                            theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });
                            ViewData["Level"] = theItem;
                            ModelState.AddModelError("", "Assign a Class to the Approved Student First");
                            return View("Edit", model);
                        }
                        if (model.LevelType == null)
                        {
                            model.IsApproved = false;
                            ModelState.AddModelError("", "Assign a Class Arm to the Approved Student First");

                            List<Level> theLevels = work.LevelRepository.Get().ToList();
                            List<SelectListItem> theItem = new List<SelectListItem>();

                            theItem.Add(new SelectListItem() { Text = "None", Value = "" });
                            foreach (var level in theLevels)
                            {
                                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
                            }
                            theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
                            theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
                            //theItem.Add(new SelectListItem() { Text = "Suspended", Value = "Suspended" });
                            theItem.Add(new SelectListItem() { Text = "Expelled", Value = "Expelled" });
                            theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });
                            ViewData["Level"] = theItem;
                            return View("Edit", model);
                        }
                        if (!string.IsNullOrEmpty(model.LevelType) && !string.IsNullOrEmpty(model.PresentLevel))
                        {
                            string[] theLevel = model.LevelType.Split(':');
                            if (theLevel.Count() == 2)
                            {
                                model.LevelType = model.LevelType;
                            }
                            else
                            {
                                model.PresentLevel = theLevel[0];
                            }
                        }
                        //if (model.PresentLevel.Contains("KG") || model.PresentLevel.StartsWith("NURS") || model.PresentLevel.StartsWith("PRIMARY"))
                        //{
                        //    // model.p
                        //    MembershipUser user = Membership.GetUser(Convert.ToString(20000000 + model.PersonID), false);
                        //    if (user == null)
                        //    {
                        //        model.RepeatTimes = 0;
                        //        model.UserID = 20000000 + model.PersonID;
                        //        model.Role = "Student";
                        //        model.IsApproved = true;
                        //        model.DateApproved = DateTime.Now;
                        //        Membership.CreateUser(model.UserID.ToString(), model.FirstName + model.Middle + model.LastName, model.FatherEmail);
                        //        // Roles.RemoveUserFromRole(model.UserID.ToString(), theStudent.Role);
                        //        Roles.AddUserToRole(model.UserID.ToString(), model.Role);
                        //        Tweaker.AdjustTimer(model.UserID.ToString());

                        //    }
                        //}

                        if (model.PresentLevel.StartsWith("JSS"))
                        {
                            // model.p
                            MembershipUser user = Membership.GetUser(Convert.ToString(30000000 + model.PersonID), false);
                            if (user == null)
                            {
                                model.RepeatTimes = 0;
                                model.UserID = 30000000 + model.PersonID;
                                model.Role = "Student";
                                model.ClassGivenEntryPoint = model.PresentLevel;
                                model.IsApproved = true;
                                model.DateApproved = DateTime.Now;
                                string password = PaddPassword.Padd(model.FirstName.ToLower() + model.Middle.ToLower() + model.LastName.ToLower());
                                Membership.CreateUser(model.UserID.ToString(), password, model.FatherEmail);
                                // Roles.RemoveUserFromRole(model.UserID.ToString(), theStudent.Role);
                                Roles.AddUserToRole(model.UserID.ToString(), model.Role);
                                Tweaker.AdjustTimer(model.UserID.ToString());

                            }
                        }

                        if (model.PresentLevel.StartsWith("SSS"))
                        {
                            // model.p
                            MembershipUser user = Membership.GetUser(Convert.ToString(40000000 + model.PersonID), false);
                            if (user == null)
                            {
                                model.RepeatTimes = 0;
                                model.UserID = 40000000 + model.PersonID;
                                model.Role = "Student";
                                model.ClassGivenEntryPoint = model.PresentLevel;
                                model.IsApproved = true;
                                model.DateApproved = DateTime.Now;
                                string password = PaddPassword.Padd(model.FirstName.ToLower() + model.Middle.ToLower() + model.LastName.ToLower());
                                Membership.CreateUser(model.UserID.ToString(), password, model.FatherEmail);
                                // Roles.RemoveUserFromRole(model.UserID.ToString(), theStudent.Role);
                                Roles.AddUserToRole(model.UserID.ToString(), model.Role);
                                Tweaker.AdjustTimer(model.UserID.ToString());

                            }
                        }


                    }
                    if (model.IsApproved)
                    {
                        work.SecondarySchoolStudentRepository.Update(model);
                    }
                    work.Save();
                    if (User.Identity.Name != "5000001")
                    {
                        AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Student Information has been updated with Student ID -,  First Name:-" + model.UserID, UserID = User.Identity.Name };
                        work.AuditTrailRepository.Insert(audit);
                        work.Save();
                    }

                }
                return RedirectToAction("Index", "PrimarySchoolStudent");
            }
            catch
            {
                List<Level> theLevels = work.LevelRepository.Get().ToList();
                List<SelectListItem> theItem = new List<SelectListItem>();

                theItem.Add(new SelectListItem() { Text = "None", Value = "" });
                foreach (var level in theLevels)
                {
                    theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
                }
                ViewData["Level"] = theItem;
                return View("Edit", model);
            }
        }

        //
        // GET: /SecondarySchoolStudent/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SecondarySchoolStudent/Delete/5

        // POST: /PrimarySchoolStudent/Delete/5
        [DynamicAuthorize]
        [HttpPost]
        public ActionResult Delete(PrimarySchoolStudent model)
        {
            try
            {
                // TODO: Add delete logic here
                SecondarySchoolStudent theStudent = work.SecondarySchoolStudentRepository.GetByID(model.PersonID);
                string firstName = model.FirstName;
                string lastName = model.LastName;
                work.SecondarySchoolStudentRepository.Delete(theStudent);
                Membership.DeleteUser(Convert.ToString(theStudent.UserID));
                work.Save();
                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Student was Deleted,  First Name:-" + firstName + " Last Name:-" + lastName, UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
                    work.Save();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
