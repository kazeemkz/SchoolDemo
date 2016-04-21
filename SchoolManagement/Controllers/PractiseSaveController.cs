using JobHustler.DAL;
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
    [Authorize]
    public class PractiseSaveController : Controller
    {
        UnitOfWork work = new UnitOfWork();

        //
        // GET: /PractiseSave/

        public ActionResult Index(string studentID, string level = "", string term = "", int id = 0)
        {

            if (User.IsInRole("Parent"))
            {
                Int32 theUser = Convert.ToInt32(User.Identity.Name);
                List<PrimarySchoolStudent> thSchoolStudents;
                //  string userid = User.Identity.Name;
                List<Parent> theP = work.ParentRepository.Get(a => a.UserID == theUser).ToList();
                Parent theRealParent = theP[0];
                int idParent = theRealParent.ParentID;
                thSchoolStudents = work.PrimarySchoolStudentRepository.Get(a => a.ParentID == idParent).ToList();
                List<SelectListItem> theItem = new List<SelectListItem>();
                foreach (var s in thSchoolStudents)
                {
                    theItem.Add(new SelectListItem() { Text = s.LastName + " " + s.FirstName, Value = s.PersonID.ToString() });
                    ViewData["Students"] = theItem;
                }
            }
            if (User.IsInRole("Student"))
            {
                Membership.GetUser(true);
                string theUser = User.Identity.Name.ToString();
                ViewBag.Level = "Student ID -" + theUser + " Class -" + level + "Term -" + term;
                List<PractiseSave> thePractiseExam = work.PractiseSaveRepository.Get(a => a.Level.Equals(level) && a.Term.Equals(term) && a.StudentID.Equals(theUser)).ToList();
                return View(thePractiseExam);
            }

            if (User.IsInRole("Parent"))
            {
                // int userID = Convert.ToInt32(User.Identity.Name);
                Int32 theUser = Convert.ToInt32(User.Identity.Name);
                Parent theP = work.ParentRepository.Get(a => a.UserID == theUser).First();
                if (studentID != null)
                {
                    int ids = Convert.ToInt32(studentID);
                    PrimarySchoolStudent theStudents = work.PrimarySchoolStudentRepository.GetByID(ids);
                    if (theStudents != null)
                    {
                        string userID = Convert.ToString(theStudents.UserID);
                        ViewBag.Level = "Student ID -" + userID + " Class -" + level + "Term -" + term;
                        List<PractiseSave> thePractiseExam = work.PractiseSaveRepository.Get(a => a.Level.Equals(level) && a.Term.Equals(term) && a.StudentID.Equals( userID)).ToList();
                        return View(thePractiseExam);
                    }
                    else
                    {
                        string userID = Convert.ToString(theStudents.UserID);
                        ViewBag.Level = "Student ID -" + userID + " Class -" + level + "Term -" + term;
                        List<PractiseSave> thePractiseExam = work.PractiseSaveRepository.Get(a => a.Level.Equals(level) && a.Term.Equals(term) && a.StudentID == "0").ToList();
                        return View(thePractiseExam);
                    }

                }
                else
                {
                    //string userID = "0";// Convert.ToString(theStudents.UserID);
                  //  ViewBag.Level = "Student ID -" + theUser + " Class -" + level + "Term -" + term;
                    ViewBag.Level = "Student ID -" + " " + " Class -" + level + "Term -" + term;
                    List<PractiseSave> thePractiseExam = work.PractiseSaveRepository.Get(a => a.Level.Equals(level) && a.Term.Equals(term) && a.StudentID == "0").ToList();
                    return View(thePractiseExam);
                }
            }


            if (User.IsInRole("Teacher"))
            {
                Membership.GetUser(true);
                ViewBag.ID = id;
                PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(id);
                string StudentID = Convert.ToString(id);
                List<PractiseSave> thePractiseExam = new List<PractiseSave>();
                if (theStudent != null)
                {
                    Int32 theTeacher = Convert.ToInt32(User.Identity.Name);
                    PrimarySchoolStaff theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == theTeacher).First();
                    string theStudentID = theStudent.UserID.ToString();
                    thePractiseExam = work.PractiseSaveRepository.Get(a => a.LevelType.Equals(theStaff.ClassTeacherOf) && a.Term.Equals(term) && a.StudentID.Equals(theStudentID)).ToList();
                }
                return View(thePractiseExam);
            }
            else
            {
                Membership.GetUser(true);
                ViewBag.ID = id;
                PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(id);
                string StudentID = Convert.ToString(id);
                List<PractiseSave> thePractiseExam = new List<PractiseSave>();
                if (theStudent != null)
                {
                    Int32 theTeacher = Convert.ToInt32(User.Identity.Name);
                    PrimarySchoolStaff theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == theTeacher).First();
                    string theStudentID = theStudent.UserID.ToString();
                    thePractiseExam = work.PractiseSaveRepository.Get(a => a.Level.Equals(theStudent.PresentLevel) && a.Term.Equals(term) && a.StudentID.Equals(theStudentID)).ToList();
                }
                return View(thePractiseExam);
            }
        }

        //
        // GET: /PractiseSave/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /PractiseSave/Create

        public ActionResult Create(string level, string term)
        {


            //  ExamSubjectReg theExam = new ExamSubjectReg();

            //foreach (var exam in thePractiseExam)
            //{

            //}

            return View();
        }

        //
        // POST: /PractiseSave/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /PractiseSave/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /PractiseSave/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /PractiseSave/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /PractiseSave/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
