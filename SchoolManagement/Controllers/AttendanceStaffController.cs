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
    public class AttendanceStaffController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        UnitOfWork work1 = new UnitOfWork();
        //
        // GET: /AttendanceStaff/

        //  [Authorize(Roles = "SuperAdmin,Class Teacher,Subject Teacher")]
        [DynamicAuthorize]
        public ActionResult Index(string date, string dateto, string studentid, string present)
        {
            //List<Level> theLevels = work.LevelRepository.Get().ToList();

            List<AttendanceStaff> theAtten = work.AttendanceStaffRepository.Get().OrderByDescending(a => a.DateTaken).ToList();

            if (!String.IsNullOrEmpty(date) && !(String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(date);
                DateTime to = Convert.ToDateTime(dateto);
                theAtten = theAtten.Where(a => a.DateTaken.Date >= from.Date && a.DateTaken.Date <= to.Date).ToList();
            }

            if (!String.IsNullOrEmpty(date)&& (String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(date);
               // DateTime to = Convert.ToDateTime(dateto);
                theAtten = theAtten.Where(a => a.DateTaken.Date == from.Date).ToList();
            }

            if (!String.IsNullOrEmpty(studentid))
            {
                theAtten = theAtten.Where(a => a.StaffID == studentid).ToList();
            }


            //if (!String.IsNullOrEmpty(arm))
            //{
            //    theAtten = theAtten.Where(a => a.arm == arm).ToList();
            //}

            if (!String.IsNullOrEmpty(present))
            {
                bool value = Convert.ToBoolean(present);
                theAtten = theAtten.Where(a => a.Present == value).ToList();
            }



            if ((String.IsNullOrEmpty(date)) && (String.IsNullOrEmpty(dateto)) && (String.IsNullOrEmpty(studentid)) && (String.IsNullOrEmpty(present)))
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

        //
        // GET: /AttendanceStaff/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AttendanceStaff/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AttendanceStaff/Create

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
        //   [Authorize(Roles = "SuperAdmin")]
        [DynamicAuthorize]
        public ActionResult Edit()
        {
            var info = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
            DateTimeOffset theOldDate = TimeZoneInfo.ConvertTime(DateTime.Now, info);

            string stringDate = Convert.ToString(theOldDate);
            DateTime theDate = Convert.ToDateTime(stringDate);

            // List<PrimarySchoolStudent> theStudents = new List<PrimarySchoolStudent>();
            AttendanceStaffViewModel theStaffAttendace = new AttendanceStaffViewModel();
            List<AttendanceStaff> theAttendanceStaff = new List<AttendanceStaff>();
            // // ViewBag.Arm = arm;

            //if (!(string.IsNullOrEmpty(arm)))
            //{
            // List<Attendance> takenAttendance =  work.AttendanceRepository.Get(a => a.DateTaken.Date ==DateTime.Now.Date && a.arm == arm).ToList();
            List<AttendanceStaff> takenAttendanceStaff = work.AttendanceStaffRepository.Get().OrderBy(a => a.DateTaken).ToList();
            takenAttendanceStaff = takenAttendanceStaff.Where(s => s.DateTaken.Date == DateTime.Now.Date).OrderByDescending(a => a.StaffID).ToList();
            if (takenAttendanceStaff.Count() > 0)
            {

                foreach (AttendanceStaff p in takenAttendanceStaff)
                {
                    theAttendanceStaff.Add(new AttendanceStaff() { AttendanceStaffID = p.AttendanceStaffID, DateTaken = p.DateTaken, StaffID = p.StaffID.ToString(), Present = p.Present, NotPresentButTookPermission = p.NotPresentButTookPermission, FirstName = p.FirstName, MiddleName = p.MiddleName, SurName = p.SurName });

                }
                theStaffAttendace.TheAttendanceStaff = theAttendanceStaff;
                return View("Edit", theStaffAttendace);
                /// return View("Edit", theAttendanceStaff);
            }
            //  ViewBag.Arm = "Attendance for " + arm;
            List<PrimarySchoolStaff> theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID != 5000001).ToList();

            foreach (PrimarySchoolStaff p in theStaff)
            {
                theAttendanceStaff.Add(new AttendanceStaff() { DateTaken = DateTime.Now, StaffID = p.UserID.ToString(), Present = false, NotPresentButTookPermission = false, FirstName = p.FirstName, MiddleName = p.Middle, SurName = p.LastName });

            }
            //  }

            theStaffAttendace.TheAttendanceStaff = theAttendanceStaff;
            return View("Edit", theStaffAttendace);
        }

        //
        // POST: /Attendance/Edit/5

        [HttpPost]
        [DynamicAuthorize]
        // [Authorize(Roles = "SuperAdmin")]
        public ActionResult Edit(AttendanceStaffViewModel model)
        {
            try
            {
                 var info = TimeZoneInfo.FindSystemTimeZoneById("W. Central Africa Standard Time");
                 DateTimeOffset theOldDate = TimeZoneInfo.ConvertTime(DateTime.Now, info);

               string stringDate=  Convert.ToString(theOldDate);
               DateTime theDate = Convert.ToDateTime(stringDate);
                //if (!(string.IsNullOrEmpty(attendanceDate)))
                //{
                //    theDate = Convert.ToDateTime(attendanceDate);
                //}
                List<AttendanceStaff> takenAttendanceStaff = work.AttendanceStaffRepository.Get().OrderBy(a => a.DateTaken).ToList();
                takenAttendanceStaff = takenAttendanceStaff.Where(s => s.DateTaken.Date == theDate.Date).OrderByDescending(a => a.StaffID).ToList();
                ////takenAttendanceStaff = takenAttendanceStaff.Where(s => s.DateTaken.Date == DateTime.Now.Date).OrderByDescending(a => a.StaffID).ToList();
                if (takenAttendanceStaff.Count() > 0)
                {
                    //  UnitOfWork work1 = new UnitOfWork();
                    foreach (AttendanceStaff a in model.TheAttendanceStaff)
                    {
                        AttendanceStaff att = work.AttendanceStaffRepository.GetByID(a.AttendanceStaffID);
                        if (att.Present == false && a.Present == true)
                        {
                            a.DateTaken = Convert.ToDateTime(theDate); //DateTime.Now;
                            work1.AttendanceStaffRepository.Update(a);
                        }
                        if (att.NotPresentButTookPermission == false && a.NotPresentButTookPermission == true)
                        {
                            a.DateTaken = theDate; //DateTime.Now;
                            work1.AttendanceStaffRepository.Update(a);
                        }

                        if (att.Present == true && a.Present == false)
                        {
                            a.DateTaken = theDate; //DateTime.Now;
                            work1.AttendanceStaffRepository.Update(a);
                        }
                        if (att.NotPresentButTookPermission == true && a.NotPresentButTookPermission == false)
                        {
                            a.DateTaken = theDate; //DateTime.Now;
                            work1.AttendanceStaffRepository.Update(a);
                        }

                      

                    }
                    work1.Save();
                }
                else
                {
                    foreach (AttendanceStaff a in model.TheAttendanceStaff)
                    {
                        a.DateTaken = DateTime.Now;
                        work.AttendanceStaffRepository.Insert(a);

                    }
                    work.Save();
                }
                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Marked Staff Attendance", UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
                    work.Save();
                }


                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /AttendanceStaff/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /AttendanceStaff/Delete/5

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

        // [Authorize(Roles = "Student")]
        //  public ActionResult Index(string arm, string date, string dateto, string studentid, string present)
        public ActionResult StaffView(string date, string present, string dateto)
        {
            //List<Level> theLevels = work.LevelRepository.Get().ToList();

            string theUserID = User.Identity.Name;

            //  ViewData["arm"] = theItem;
            List<AttendanceStaff> theAtten = work.AttendanceStaffRepository.Get(a => a.StaffID == theUserID).OrderByDescending(a => a.DateTaken).ToList();

            if (!String.IsNullOrEmpty(date) && !(String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(date);
                DateTime to = Convert.ToDateTime(dateto);
                theAtten = theAtten.Where(a => a.DateTaken >= from && a.DateTaken <= to).ToList();
            }

            if (!(String.IsNullOrEmpty(present)))
            {
                bool presentValue = Convert.ToBoolean(present);
                theAtten = theAtten.Where(s => s.Present == presentValue).ToList();
                // DateTime d = Convert.ToDateTime(date);
                // theAtten = null;// theAtten.Where(s => s.DateTaken.Date == d.Date).ToList();

            }


            ViewBag.Count = theAtten.Count();

            return View("StaffView", theAtten);
        }
    }
}
