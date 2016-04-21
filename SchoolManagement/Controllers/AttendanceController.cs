using JobHustler.DAL;
using JobHustler.Models;
using SchoolManagement.Model;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class AttendanceController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        //
        // GET: /Attendance/
        //    @if (role == "Student" || role == "Class Teacher" || role == "Subject Teacher" || role == "SuperAdmin")
        // [Authorize(Roles = "SuperAdmin,Class Teacher,Subject Teacher")]
        [DynamicAuthorize]
        public ActionResult Index(string arm, string date, string dateto, string studentid, string present)
        {
            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();
            theItem.Add(new SelectListItem() { Text = "None", Value = "" });

            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }

            //  ViewData["Level"] = theItem;

            ViewData["arm"] = theItem;
            List<Attendance> theAtten = work.AttendanceRepository.Get().OrderByDescending(a => a.DateTaken).ToList();

            if (!String.IsNullOrEmpty(date) && !(String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(date);
                DateTime to = Convert.ToDateTime(dateto);
                theAtten = theAtten.Where(a => a.DateTaken >= from && a.DateTaken <= to).ToList();
            }

            if (!String.IsNullOrEmpty(studentid))
            {
                theAtten = theAtten.Where(a => a.StudentID == studentid).ToList();
            }


            if (!String.IsNullOrEmpty(arm))
            {
                theAtten = theAtten.Where(a => a.arm == arm).ToList();
            }

            if (!String.IsNullOrEmpty(present))
            {
                bool value = Convert.ToBoolean(present);
                theAtten = theAtten.Where(a => a.Present == value).ToList();
            }



            if (String.IsNullOrEmpty(arm) && (String.IsNullOrEmpty(date)) && (String.IsNullOrEmpty(dateto)) && (String.IsNullOrEmpty(studentid)) && (String.IsNullOrEmpty(present)))
            {
                //theAtten = theAtten.Where(s => s.arm == arm).ToList();
                //DateTime d = Convert.ToDateTime(date);
                theAtten = null;// theAtten.Where(s => s.DateTaken.Date == d.Date).ToList();
                ViewBag.Count = 0; //; theAtten.Count();

                return View(theAtten);

            }



            //if (!String.IsNullOrEmpty(SexString))
            //{
            //    students = students.Where(s => s.Sex.Equals(SexString));
            //}


            if (!String.IsNullOrEmpty(date))
            {
                //  DateTime d = Convert.ToDateTime(date);
                //  theAtten = theAtten.Where(s => s.DateTaken.Date == d.Date).ToList();
            }


            ViewBag.Count = theAtten.Count();

            return View(theAtten);
        }

       // [Authorize(Roles = "Student,Parent")]
         [DynamicAuthorize]
        //  public ActionResult Index(string arm, string date, string dateto, string studentid, string present)
        public ActionResult StudentView(string date, string present, string dateto, string studentID)
        {
            Int32 theUser = Convert.ToInt32(User.Identity.Name);
            List<PrimarySchoolStudent> thSchoolStudents;
            if (User.IsInRole("Parent"))
            {
                string userid = User.Identity.Name;
                List<Parent> theP = work.ParentRepository.Get(a => a.UserID == theUser).ToList();
                Parent theRealParent = theP[0];
                int idParent = theRealParent.ParentID;
                thSchoolStudents = work.PrimarySchoolStudentRepository.Get(a => a.ParentID == idParent).ToList();
                List<SelectListItem> theItem = new List<SelectListItem>();
                // theItem.Add(new SelectListItem() { Text = "None", Value = "" });

                foreach (var s in thSchoolStudents)
                {
                    theItem.Add(new SelectListItem() { Text = s.LastName + " " + s.FirstName, Value = s.PersonID.ToString() });
                    ViewData["Students"] = theItem;
                }
            }
            //List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<Attendance> theAtten = new List<Attendance>();
            if (User.IsInRole("Student"))
            {
                // 
                string theUserID = User.Identity.Name;

                //  ViewData["arm"] = theItem;
                theAtten = work.AttendanceRepository.Get(a => a.StudentID == theUserID).OrderByDescending(a => a.DateTaken).ToList();

            }
            else
            {
                //List<Parent> theP = work.ParentRepository.Get(a => a.UserID == theUser).ToList();
                //Parent theRealParent = theP[0];
                //int idParent = theRealParent.ParentID;
                //thSchoolStudents = work.PrimarySchoolStudentRepository.Get(a => a.ParentID == idParent).ToList();
                //if (thSchoolStudents.Count() > 0)
                //{
                //    PrimarySchoolStudent p = thSchoolStudents[0];
                //    string id = p.UserID.ToString();
                if (studentID != null)
                {
                    theAtten = work.AttendanceRepository.Get().OrderByDescending(a => a.DateTaken).ToList();
                }
                else
                {
                    theAtten = new List<Attendance>();
                }
                //    theAtten = theAtten.Where(a => a.StudentID == id).ToList();
                //}
                //else
                //{
                //    theAtten = new List<Attendance>();
                //}

            }
            if (User.IsInRole("Student"))
            {
                if (!String.IsNullOrEmpty(date) && !(String.IsNullOrEmpty(dateto)))
                {
                    DateTime from = Convert.ToDateTime(date);
                    DateTime to = Convert.ToDateTime(dateto);
                    theAtten = theAtten.Where(a => a.DateTaken >= from && a.DateTaken <= to).ToList();
                }

                if (!String.IsNullOrEmpty(studentID))
                {
                    int id = Convert.ToInt32(studentID);
                    PrimarySchoolStudent p = work.PrimarySchoolStudentRepository.GetByID(id);
                    string stringId = p.UserID.ToString();
                    theAtten = theAtten.Where(a => a.StudentID == stringId).ToList();
                }

                if (!(String.IsNullOrEmpty(present)))
                {
                    bool presentValue = Convert.ToBoolean(present);
                    theAtten = theAtten.Where(s => s.Present == presentValue).ToList();
                    // DateTime d = Convert.ToDateTime(date);
                    // theAtten = null;// theAtten.Where(s => s.DateTaken.Date == d.Date).ToList();

                }
            }
            else
            {


                if (!String.IsNullOrEmpty(date) && !(String.IsNullOrEmpty(dateto)) && !(String.IsNullOrEmpty(studentID)))
                {
                    int id = Convert.ToInt32(studentID);
                    PrimarySchoolStudent p = work.PrimarySchoolStudentRepository.GetByID(id);
                    int ids = p.UserID;
                    string stringId = ids.ToString();
                    DateTime from = Convert.ToDateTime(date);
                    DateTime to = Convert.ToDateTime(dateto);
                    theAtten = theAtten.Where(a => a.DateTaken >= from && a.DateTaken <= to && a.StudentID == stringId).ToList();
                }

                if (!String.IsNullOrEmpty(studentID))
                {
                    int id = Convert.ToInt32(studentID);
                    PrimarySchoolStudent p = work.PrimarySchoolStudentRepository.GetByID(id);
                    string stringId = p.UserID.ToString();
                    theAtten = theAtten.Where(a => a.StudentID == stringId).ToList();
                }

                if (!String.IsNullOrEmpty(present) && !(String.IsNullOrEmpty(studentID)))
                {
                    bool presentValue = Convert.ToBoolean(present);
                    theAtten = theAtten.Where(s => s.Present == presentValue).ToList();
                }
            }

            ViewBag.Count = theAtten.Count();

            return View(theAtten);
        }
        //
        // GET: /Attendance/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Attendance/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Attendance/Create

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
        // GET: /Attendance/Edit/5
        // [Authorize(Roles = "SuperAdmin,Class Teacher,Subject Teacher")]
        [DynamicAuthorize]
        public ActionResult Edit(string arm)
        {
            List<Level> theLevels = work.LevelRepository.Get().ToList();

            if (User.IsInRole("Subject Teacher") || User.IsInRole("Class Teacher"))
            {

                List<SelectListItem> theItemClasses = new List<SelectListItem>();
                theItemClasses.Add(new SelectListItem() { Text = "None", Value = "" });

                string theUserName = User.Identity.Name;
                Int32 theUserName1 = Convert.ToInt32(theUserName);
                PrimarySchoolStaff theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == theUserName1).First();
                string theTeachersClass = theStaff.ClassTeacherOf;
                string[] splitClasses = theTeachersClass.Split('-');
                List<string> listOfClasses = new List<string>();
                listOfClasses = splitClasses.ToList();

                foreach (var c in theLevels)
                {
                    foreach (var cla in listOfClasses)
                    {
                        string classType = c.LevelName + ":" + c.Type;
                        if (classType == cla)
                        {
                            theItemClasses.Add(new SelectListItem() { Text = classType, Value = classType });
                        }
                    }
                }


                ViewData["arm"] = theItemClasses;
            }

            if (User.IsInRole("SuperAdmin"))
            {
                List<SelectListItem> theItem = new List<SelectListItem>();
                theItem.Add(new SelectListItem() { Text = "None", Value = "" });

                foreach (var level in theLevels)
                {
                    theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
                }
                ViewData["arm"] = theItem;

            }
            List<PrimarySchoolStudent> theStudents = new List<PrimarySchoolStudent>();
            AttendanceViewModel theStudentsAttendace = new AttendanceViewModel();
            List<Attendance> theAttendance = new List<Attendance>();
            // ViewBag.Arm = arm;

            if (!(string.IsNullOrEmpty(arm)))
            {
                // List<Attendance> takenAttendance =  work.AttendanceRepository.Get(a => a.DateTaken.Date ==DateTime.Now.Date && a.arm == arm).ToList();
                List<Attendance> takenAttendance = work.AttendanceRepository.Get().OrderByDescending(a => a.DateTaken).ToList();
                takenAttendance = takenAttendance.Where(s => s.DateTaken.Date == DateTime.Now.Date && s.arm == arm).ToList();
                if (takenAttendance.Count() > 0)
                {
                    ModelState.AddModelError("", "Attendance has already been taken for this Class Today!");
                    return View("Edit", theStudentsAttendace);
                }
                //  ViewBag.Arm = "Attendance for " + arm;
                theStudents = work.PrimarySchoolStudentRepository.Get(a => a.LevelType == arm).ToList();

                foreach (PrimarySchoolStudent p in theStudents)
                {
                    theAttendance.Add(new Attendance() { arm = p.LevelType, DateTaken = DateTime.Now, StudentID = p.UserID.ToString(), Present = false, FirstName = p.FirstName, MiddleName = p.Middle, SurName = p.LastName });

                }
            }

            theStudentsAttendace.TheAttendance = theAttendance;
            return View("Edit", theStudentsAttendace);
        }

        //
        // POST: /Attendance/Edit/5

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Class Teacher,Subject Teacher")]
        public ActionResult Edit(AttendanceViewModel model)
        {
            try
            {
                foreach (Attendance a in model.TheAttendance)
                {
                    a.DateTaken = DateTime.Now;
                    work.AttendanceRepository.Insert(a);

                }
                work.Save();
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Attendance/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Attendance/Delete/5

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
