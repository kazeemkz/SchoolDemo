//using CsQuery.Engine.PseudoClassSelectors;
using JobHustler.DAL;
using JobHustler.Models;
using SchoolManagement.DAL;
using SchoolManagement.Model;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class ParentController : Controller
    {
        // UnitOfWork work = new UnitOfWork();
        //
        // GET: /Parent/
        UnitOfWork work = new UnitOfWork();
        [DynamicAuthorize]
        public ActionResult Index(string sortOrder, string currentFilter, string first, string middle, string last, int? page)
        {
            int pageSize = 50;
            int pageNumber = (page ?? 1);

            var parent = from s in work.ParentRepository.Get().OrderByDescending(a => a.Date)
                         select s;
            if (User.IsInRole("Parent"))
            {
                ViewBag.Count = parent.Count();
                Int32 UserName = Convert.ToInt32(User.Identity.Name);
                parent = parent.Where(a => a.UserID == UserName);
                return View("View Parents List", parent.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                if (!(string.IsNullOrEmpty(first)))
                {
                    parent = parent.Where(a => a.FirstName.ToLower().Contains(first.ToLower()));//|| a.LastName.Contains(name) || a.MiddleName.Contains(name));
                }
                if (!(string.IsNullOrEmpty(middle)))
                {
                    parent = parent.Where(a => a.MiddleName.ToLower().Contains(middle.ToLower()));//|| a.LastName.Contains(name) || a.MiddleName.Contains(name));
                }
                if (!(string.IsNullOrEmpty(last)))
                {
                    parent = parent.Where(a => a.LastName.ToLower().Contains(last.ToLower()));//|| a.LastName.Contains(name) || a.MiddleName.Contains(name));
                }
                ViewBag.Count = parent.Count();
                return View("View Parents List", parent.ToPagedList(pageNumber, pageSize));
            }

        }

        //
        // GET: /Parent/Details/5
        [DynamicAuthorize]
        public ActionResult Details(string id)
        {
          Int32  theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
          Parent theParent = work.ParentRepository.GetByID(theId);
            List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.ParentID == theId).ToList();

            //   Parent theParent = theParentt[0];

            // PrimarySchoolStudent pri = 
            //  Parent theParent = work.ParentRepository.GetByID(id);
            int counter = 0;
            foreach (PrimarySchoolStudent s in theStudents)
            {

                if (counter == 0)
                {
                    theParent.StudentIDOne = Convert.ToString(s.UserID);
                }
                if (counter == 1)
                {
                    theParent.StudentIDTwo = Convert.ToString(s.UserID);
                }

                if (counter == 2)
                {
                    theParent.StudentIDThree = Convert.ToString(s.UserID);
                }
                if (counter == 3)
                {
                    theParent.StudentIDFour = Convert.ToString(s.UserID);
                }
                if (counter == 4)
                {
                    theParent.StudentIDFive = Convert.ToString(s.UserID);
                }
                counter = counter + 1;
            }
            return View(theParent);
        }

        //
        // GET: /Parent/Create
        // [ValidateAntiForgeryToken]
        [DynamicAuthorize]
        public ActionResult Create()
        {
            return View("Create Parent");
        }

        //
        // POST: /Parent/Create
        [DynamicAuthorize]
        [HttpPost]
        public ActionResult Create(Parent model)
        {

            try
            {
                List<PrimarySchoolStudent> theP = new List<PrimarySchoolStudent>();
                if (!(string.IsNullOrEmpty(model.StudentIDOne)))
                {
                    int id = Convert.ToInt32(model.StudentIDOne);
                    List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == id).ToList();
                    PrimarySchoolStudent theStudent = theStudents[0];
                    if (theStudent != null)
                    {
                        theP.Add(theStudent);
                        // model.ThePrimarySchoolStudent.
                    }

                }
                if (!(string.IsNullOrEmpty(model.StudentIDTwo)))
                {
                    int id = Convert.ToInt32(model.StudentIDTwo);
                    List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == id).ToList();
                    PrimarySchoolStudent theStudent = theStudents[0];
                    if (theStudent != null)
                    {
                        theP.Add(theStudent);
                        // model.ThePrimarySchoolStudent.
                    }

                }
                if (!(string.IsNullOrEmpty(model.StudentIDThree)))
                {
                    int id = Convert.ToInt32(model.StudentIDThree);
                    List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == id).ToList();
                    PrimarySchoolStudent theStudent = theStudents[0];
                    if (theStudent != null)
                    {
                        theP.Add(theStudent);
                        // model.ThePrimarySchoolStudent.
                    }

                }
                if (!(string.IsNullOrEmpty(model.StudentIDFour)))
                {
                    int id = Convert.ToInt32(model.StudentIDFour);
                    List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == id).ToList();
                    PrimarySchoolStudent theStudent = theStudents[0];
                    if (theStudent != null)
                    {
                        theP.Add(theStudent);
                        // model.ThePrimarySchoolStudent.
                    }

                }
                if (!(string.IsNullOrEmpty(model.StudentIDFive)))
                {
                    int id = Convert.ToInt32(model.StudentIDFive);
                    List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == id).ToList();
                    PrimarySchoolStudent theStudent = theStudents[0];
                    if (theStudent != null)
                    {
                        theP.Add(theStudent);
                        // model.ThePrimarySchoolStudent.
                    }

                }
                model.ThePrimarySchoolStudent = theP;
                int theNewID;
                int theNewUserID;
                List<Parent> thePerson = work.ParentRepository.Get().ToList();
                Int32 totalParents = thePerson.Count();
                if (thePerson.Count() == 0)
                {
                    theNewUserID = 1000;// UserIDLengthAdjuster.NumberAdjuster("1");
                }
                else
                {
                    Parent Paren = thePerson.Last();
                    // Paren.UserID;
                    //theNewID = Paren.PersonID + 1;
                    theNewUserID = Paren.UserID + 1;// UserIDLengthAdjuster.NumberAdjuster(theNewID.ToString());
                }

                model.UserID = Convert.ToInt32(theNewUserID);
                model.Date = DateTime.Now;
                model.Role = "Parent";
                work.ParentRepository.Insert(model);
                work.Save();
                string password = PaddPassword.Padd(model.FirstName.ToLower() + model.MiddleName.ToLower() + model.LastName.ToLower());
                Membership.CreateUser(model.UserID.ToString(), password);

                Roles.AddUserToRole(model.UserID.ToString(), "Parent");
                Tweaker.AdjustTimer(model.UserID.ToString());

                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "A Parent was Created, frist name:-" + " " + model.FirstName + " " + "Last Name :- " + model.LastName, UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
                    work.Save();
                }
                return RedirectToAction("Index");
                //}
                //else
                //{

                //    return View("Create Parent");
                //}
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Parent/Edit/5
        [DynamicAuthorize]
        public ActionResult Edit(string id)
        {
           Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            smContext sm = new smContext();
            List<Parent> theParentt = sm.Parent.Include("ThePrimarySchoolStudent").Where(a => a.ParentID == theId).ToList();
            List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.ParentID == theId).ToList();

            Parent theParent = theParentt[0];
            // PrimarySchoolStudent pri = 
            //  Parent theParent = work.ParentRepository.GetByID(id);
            int counter = 0;
            foreach (PrimarySchoolStudent s in theStudents)
            {

                if (counter == 0)
                {
                    theParent.StudentIDOne = Convert.ToString(s.UserID);
                }
                if (counter == 1)
                {
                    theParent.StudentIDTwo = Convert.ToString(s.UserID);
                }

                if (counter == 2)
                {
                    theParent.StudentIDThree = Convert.ToString(s.UserID);
                }
                if (counter == 3)
                {
                    theParent.StudentIDFour = Convert.ToString(s.UserID);
                }
                if (counter == 4)
                {
                    theParent.StudentIDFive = Convert.ToString(s.UserID);
                }
                counter = counter + 1;
            }

            return View("Edit Parent", theParent);
        }

        //
        // POST: /Parent/Edit/5
        //   [ValidateAntiForgeryToken]
        [HttpPost]
        [DynamicAuthorize]
        public ActionResult Edit(Parent model)
        {
            try
            {
                work.ParentRepository.Update(model);
                work.Save();
                Parent theParent = work.ParentRepository.GetByID(model.ParentID);
                int pID = theParent.ParentID;

                List<PrimarySchoolStudent> thePri = work.PrimarySchoolStudentRepository.Get(a => a.ParentID == pID).ToList();
                foreach (PrimarySchoolStudent p in thePri)
                {
                    p.Parent = null;
                    p.ParentID = null;
                    work.PrimarySchoolStudentRepository.Update(p);
                }

                work.Save();
                int parentId = model.ParentID;
                List<PrimarySchoolStudent> theP = new List<PrimarySchoolStudent>();
                if (!(string.IsNullOrEmpty(model.StudentIDOne)))
                {
                    int id = Convert.ToInt32(model.StudentIDOne);
                    List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == id).ToList();
                    PrimarySchoolStudent theStudent = theStudents[0];
                    if (theStudent != null)
                    {
                        theStudent.ParentID = parentId;
                        work.PrimarySchoolStudentRepository.Update(theStudent);
                        theP.Add(theStudent);
                    }

                }
                if (!(string.IsNullOrEmpty(model.StudentIDTwo)))
                {
                    int id = Convert.ToInt32(model.StudentIDTwo);
                    List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == id).ToList();
                    PrimarySchoolStudent theStudent = theStudents[0];
                    if (theStudent != null)
                    {
                        theStudent.ParentID = parentId;
                        work.PrimarySchoolStudentRepository.Update(theStudent);
                        theP.Add(theStudent);

                        // model.ThePrimarySchoolStudent.
                    }

                }
                if (!(string.IsNullOrEmpty(model.StudentIDThree)))
                {
                    int id = Convert.ToInt32(model.StudentIDThree);
                    List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == id).ToList();
                    PrimarySchoolStudent theStudent = theStudents[0];
                    if (theStudent != null)
                    {
                        theStudent.ParentID = parentId;
                        work.PrimarySchoolStudentRepository.Update(theStudent);
                        theP.Add(theStudent);
                    }

                }
                if (!(string.IsNullOrEmpty(model.StudentIDFour)))
                {
                    int id = Convert.ToInt32(model.StudentIDFour);
                    List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == id).ToList();
                    PrimarySchoolStudent theStudent = theStudents[0];
                    if (theStudent != null)
                    {
                        theStudent.ParentID = parentId;
                        work.PrimarySchoolStudentRepository.Update(theStudent);
                        theP.Add(theStudent);
                    }

                }
                if (!(string.IsNullOrEmpty(model.StudentIDFive)))
                {
                    int id = Convert.ToInt32(model.StudentIDFive);
                    List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == id).ToList();
                    PrimarySchoolStudent theStudent = theStudents[0];
                    if (theStudent != null)
                    {
                        theStudent.ParentID = parentId;
                        work.PrimarySchoolStudentRepository.Update(theStudent);
                        theP.Add(theStudent);
                    }

                }
                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Parent Information Updated User ID:-" + model.UserID, UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
                    work.Save();
                }
                work.Save();
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View("Edit Parent");
            }
        }

        //
        // GET: /Parent/Delete/5
        [DynamicAuthorize]
        public ActionResult Delete(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            Parent theParent = work.ParentRepository.GetByID(theId);
            return View(theParent);
        }

        //
        // POST: /Parent/Delete/5
        [DynamicAuthorize]
        [HttpPost]
        public ActionResult Delete(Parent model)
        {
            try
            {
                // TODO: Add delete logic here
                string firstName = model.FirstName;
                string lastName = model.LastName;
                Parent theParent = work.ParentRepository.GetByID(model.ParentID);
                int pID = theParent.ParentID;

                List<PrimarySchoolStudent> thePri = work.PrimarySchoolStudentRepository.Get(a => a.ParentID == pID).ToList();
                foreach (PrimarySchoolStudent p in thePri)
                {

                    p.Parent = null;
                    p.ParentID = null;
                }
                work.ParentRepository.Delete(theParent);
                work.Save();
                Membership.DeleteUser(Convert.ToString(theParent.UserID));
                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Parent was Deleted First Name:-" + firstName +"  Last Name:-"+lastName, UserID = User.Identity.Name };
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
