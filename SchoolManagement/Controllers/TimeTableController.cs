using SchoolManagement.Model;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.DAL;
using JobHustler.DAL;
using System.Text;
using JobHustler.Models;

namespace SchoolManagement.Controllers
{
    public class TimeTableController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        //
        // GET: /TimeTable/
         [DynamicAuthorize]
        public ActionResult Index(string arm, string studentID)
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
                List<SelectListItem> theItem1 = new List<SelectListItem>();
                foreach (var s in thSchoolStudents)
                {
                    theItem1.Add(new SelectListItem() { Text = s.LastName + " " + s.FirstName, Value = s.PersonID.ToString() });
                    ViewData["Students"] = theItem1;
                }
            }
            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();
            theItem.Add(new SelectListItem() { Text = "None", Value = "" });

            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }

            ViewData["arm"] = theItem;

            List<List<TimeTable>> theTimeTable1 = TimeTableCollisionAviodance.DisplayTimeTable(arm);
            ViewBag.TimeTable = theTimeTable1;
            int userId = 0;// Convert.ToInt32(User.Identity.Name);
            if (User.IsInRole("Parent"))
            {

                Int32 theUser = Convert.ToInt32(User.Identity.Name);
                Parent theP = work.ParentRepository.Get(a => a.UserID == theUser).First();
                if (studentID != null)
                {
                    int id = Convert.ToInt32(studentID);
                    //  userId = Convert.ToInt32(User.Identity.Name);
                    List<PrimarySchoolStudent> theStaff = work.PrimarySchoolStudentRepository.Get(a => a.PersonID == id).ToList();
                    if (theStaff.Count() > 0)
                    {
                        PrimarySchoolStudent theRealStudent = theStaff[0];
                        string theArm = theRealStudent.LevelType;
                        List<List<TimeTable>> theTimeTable = TimeTableCollisionAviodance.DisplayTimeTable(theArm);
                        ViewBag.TimeTable = theTimeTable;
                        return View();
                    }

                }
            }




            if (string.IsNullOrEmpty(arm) && User.IsInRole("Student"))
            {
                userId = Convert.ToInt32(User.Identity.Name);
                List<PrimarySchoolStudent> theStaff = work.PrimarySchoolStudentRepository.Get(a => a.UserID == userId).ToList();
                if (theStaff.Count() > 0)
                {
                    PrimarySchoolStudent theRealStudent = theStaff[0];
                    string theArm = theRealStudent.LevelType;
                    List<List<TimeTable>> theTimeTable = TimeTableCollisionAviodance.DisplayTimeTable(theArm);
                    ViewBag.TimeTable = theTimeTable;
                    return View();
                }
            }
            return View();
        }

        public ActionResult StaffSubjectView(string arm)
        {
            //List<Level> theLevels = work.LevelRepository.Get().ToList();
            //List<SelectListItem> theItem = new List<SelectListItem>();
            //theItem.Add(new SelectListItem() { Text = "None", Value = "" });

            //foreach (var level in theLevels)
            //{
            //    theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            //}

            //ViewData["arm"] = theItem;

            int userId = Convert.ToInt32(User.Identity.Name);
            List<PrimarySchoolStaff> theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == userId).ToList();
            PrimarySchoolStaff theRealStaff = theStaff[0];
            List<List<TimeTable>> theTimeTable = TimeTableCollisionAviodance.DisplayTimeTableCustomisedForStaff(theRealStaff.PersonID);

            ViewBag.TimeTable = theTimeTable;
            ViewBag.Name = theRealStaff.LastName + " " + theRealStaff.FirstName;
            return View("StaffSubjectView");
        }


        public ActionResult StudentTimeTableView(string studentID)
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
            int userId = 0;//Convert.ToInt32(User.Identity.Name);
            // int userID = 0; //=// Convert.ToInt32(User.Identity.Name);
            if (User.IsInRole("Parent"))
            {
                // int userID = Convert.ToInt32(User.Identity.Name);
                Int32 theUser = Convert.ToInt32(User.Identity.Name);
                Parent theP = work.ParentRepository.Get(a => a.UserID == theUser).First();
                if (studentID != null)
                {
                    int id = Convert.ToInt32(studentID);
                    PrimarySchoolStudent theStudents = work.PrimarySchoolStudentRepository.GetByID(id);
                    if (theStudents != null)
                    {
                        userId = theStudents.UserID;
                        // work.PrimarySchoolStudentRepository.Get(a=>a.UserID == ids).First();
                    }
                    else
                    {
                        userId = -1;
                    }

                }
                else
                {
                    userId = -1;
                }
            }
            else
            {
                //  userID = Convert.ToInt32(User.Identity.Name);
                userId = Convert.ToInt32(User.Identity.Name);

            }
            //int userId = Convert.ToInt32(User.Identity.Name);
            List<PrimarySchoolStudent> theStaff = work.PrimarySchoolStudentRepository.Get(a => a.UserID == userId).ToList();
            List<List<TimeTable>> theTimeTable = new List<List<TimeTable>>();
            if (theStaff.Count() > 0)
            {
                PrimarySchoolStudent theRealStudent = theStaff[0];
                string theArm = theRealStudent.LevelType;
                theTimeTable = TimeTableCollisionAviodance.DisplayTimeTable(theArm);
            }

            //  ViewBag.TimeTable = theTimeTable;
            // ViewBag.Name = theRealStaff.LastName + " " + theRealStaff.FirstName;
            return View("Index", theTimeTable);
        }
        //
        // GET: /TimeTable/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /TimeTable/Create
        [DynamicAuthorize]
        public ActionResult Create()
        {
            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();

            theItem.Add(new SelectListItem() { Text = "None", Value = "" });
            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            ViewData["Level"] = theItem;


            List<Subject> theSubject = work.SubjectRepository.Get().ToList();
            List<SelectListItem> theSubjectsItem = new List<SelectListItem>();

            theSubjectsItem.Add(new SelectListItem() { Text = "None", Value = "" });
            foreach (var sub in theSubject)
            {
                theSubjectsItem.Add(new SelectListItem() { Text = sub.Name, Value = sub.Name });
            }
            ViewData["Subject"] = theSubjectsItem;



            List<PrimarySchoolStaff> staff = work.PrimarySchoolStaffRepository.Get().OrderBy(a => a.LastName).ToList();
            List<SelectListItem> theStaffItem = new List<SelectListItem>();

            theStaffItem.Add(new SelectListItem() { Text = "None", Value = "" });
            foreach (var s in staff)
            {
                theStaffItem.Add(new SelectListItem() { Text = s.LastName + " " + s.Middle + " " + s.FirstName, Value = s.LastName + " " + s.Middle + " " + s.FirstName });
            }
            ViewData["Staff"] = theStaffItem;
            return View();
        }

        //
        // POST: /TimeTable/Create

        [HttpPost]
        [DynamicAuthorize]
        public ActionResult Create(TimeTable model)
        {
            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();

            theItem.Add(new SelectListItem() { Text = "None", Value = "" });
            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            ViewData["Level"] = theItem;


            List<Subject> theSubject = work.SubjectRepository.Get().ToList();
            List<SelectListItem> theSubjectsItem = new List<SelectListItem>();

            theSubjectsItem.Add(new SelectListItem() { Text = "None", Value = "" });
            foreach (var sub in theSubject)
            {
                theSubjectsItem.Add(new SelectListItem() { Text = sub.Name, Value = sub.Name });
            }
            ViewData["Subject"] = theSubjectsItem;



            List<PrimarySchoolStaff> staff = work.PrimarySchoolStaffRepository.Get().OrderBy(a => a.LastName).ToList();
            List<SelectListItem> theStaffItem = new List<SelectListItem>();

            theStaffItem.Add(new SelectListItem() { Text = "None", Value = "" });
            foreach (var s in staff)
            {

                theStaffItem.Add(new SelectListItem() { Text = s.LastName + " " + s.Middle + " " + s.FirstName, Value = s.LastName + " " + s.Middle + " " + s.FirstName });
            }
            ViewData["Staff"] = theStaffItem;
            try
            {

                string v = model.Subject + ";" + model.Class + ";" + model.StratTimeHour + ";" + model.StratTimeMinute + ";" + model.Teacher + ";" + model.Day;

                string theReturnedValue = TimeTableCollisionAviodance.CheckDaySlotAvialability(v);
                string theReturnedValueSubject = TimeTableCollisionAviodance.CheckSubjectSlotAvialability(v);
                string theReturnedValueStaff = TimeTableCollisionAviodance.CheckStaffSlotAvialability(v);
                if (!(string.IsNullOrEmpty(theReturnedValue)))
                {
                    // ModelState.AddModelError("", "Create a Class Subjects First for Class " + theStudent.PresentLevel);
                    // return View(theReg);
                    ModelState.AddModelError("", theReturnedValue);

                    return View(model);
                }

                int start = Convert.ToInt32(model.StratTimeHour);
                int finish = Convert.ToInt32(model.EndTimeHour);

                if (finish < start)
                {
                    ModelState.AddModelError("", "Start Time of a Subject must be greater than its finish time!");

                    return View(model);
                }

                if (!(string.IsNullOrEmpty(theReturnedValueSubject)))
                {
                    ModelState.AddModelError("", theReturnedValueSubject);

                    return View(model);
                }

                if (!(string.IsNullOrEmpty(theReturnedValueStaff)))
                {
                    ModelState.AddModelError("", theReturnedValueStaff);

                    return View(model);
                }
                //var theValue = $('#Subject').val() + ";" + $('#Class').val() + ";" + $('#StratTimeHour').val() + ";" + $('#StratTimeMinute').val() + ";" + $('#Teacher').val() + ";" + $('#Day').val();
                TeachingSubject ts = new TeachingSubject();
                ts.SubjectName = model.Subject;

                TeachingDay td = new TeachingDay();
                td.StratTimeHour = model.StratTimeHour;
                td.StratTimeMinute = model.StratTimeMinute;
                td.EndTimeHour = model.EndTimeHour;
                td.EndTimeMinute = model.EndTimeMinute;

                if (model.Day == "SUNDAY")
                {
                    td.theDay = Day.SUNDAY;
                }

                if (model.Day == "MONDAY")
                {
                    td.theDay = Day.MONDAY;
                }

                if (model.Day == "TUESDAY")
                {
                    td.theDay = Day.TUESDAY;
                }

                if (model.Day == "WEDNESDAY")
                {
                    td.theDay = Day.WEDNESDAY;
                }

                if (model.Day == "THURSDAY")
                {
                    td.theDay = Day.THURSDAY;
                }

                if (model.Day == "FRIDAY")
                {
                    td.theDay = Day.FRIDAY;
                }


                if (model.Day == "SATURDAY")
                {
                    td.theDay = Day.SATURDAY;
                }

                TeachingClass tc = new TeachingClass();
                tc.ClassName = model.Class;


                List<TeachingSubject> tss = new List<TeachingSubject>();
                tss.Add(ts);

                td.TeachingSubject = tss;

                List<TeachingDay> tdd = new List<TeachingDay>();
                tdd.Add(td);
                tc.TheTeachingDay = tdd; ;


                List<TeachingClass> tt = new List<TeachingClass>();
                tt.Add(tc);
                Teacher t = new Teacher();
                //  work.PrimarySchoolStaffRepository.ge
                t.TeacherName = model.Teacher;

                string[] theTeacher = model.Teacher.Split(' ');
                string firstName = theTeacher[2];
                string lastName = theTeacher[0];
                List<PrimarySchoolStaff> priStaff = work.PrimarySchoolStaffRepository.Get(a => a.LastName.Equals(lastName) && a.FirstName.Equals(firstName)).ToList();
                t.ThePersonID = priStaff[0].PersonID;
                t.TheTeachingClass = tt;
                work.TeacherRepository.Insert(t);
                work.Save();

                // td.theDay =  model.Day;
                // TODO: Add insert logic here

                return RedirectToAction("Create");
                //  return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /TimeTable/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /TimeTable/Edit/5

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
        // GET: /TimeTable/Delete/5
        [DynamicAuthorize]
        public ActionResult Delete(int id)
        {
            TimeTable theTable = new TimeTable();
            TeachingSubject theSubject = work.TeachingSubjectRepository.GetByID(id);

            theTable.Day = theSubject.TeachingDay.theDay.ToString();
            theTable.EndTimeHour = theSubject.TeachingDay.EndTimeHour;
            theTable.EndTimeMinute = theSubject.TeachingDay.EndTimeMinute;
            theTable.StratTimeHour = theSubject.TeachingDay.StratTimeHour;
            theTable.StratTimeMinute = theSubject.TeachingDay.StratTimeMinute;
            theTable.Subject = theSubject.SubjectName;
            theTable.Teacher = theSubject.TeachingDay.TeachingClass.Teacher.TeacherName;
            theTable.Class = theSubject.TeachingDay.TeachingClass.ClassName;
            theTable.TeachingSubjectID = theSubject.TeachingSubjectID;
            return View(theTable);
        }

        //
        // POST: /TimeTable/Delete/5

        [HttpPost]
        [DynamicAuthorize]
        public ActionResult Delete(TimeTable model)
        {
            try
            {
                int id = model.TeachingSubjectID;
                // TODO: Add delete logic here
                TimeTable theTable = new TimeTable();
                TeachingSubject theSubject = work.TeachingSubjectRepository.GetByID(id);
                int TeachingDayID = theSubject.TeachingDay.TeachingDayID;
                int TeachingClassID = theSubject.TeachingDay.TeachingClass.TeachingClassID;
                int TeacherID = theSubject.TeachingDay.TeachingClass.Teacher.TeacherID;

                work.TeachingSubjectRepository.Delete(theSubject.TeachingSubjectID);
                work.TeachingDayRepository.Delete(TeachingDayID);
                work.TeachingClassRepository.Delete(TeachingClassID);
                work.TeacherRepository.Delete(TeacherID);
                work.Save();


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        //public ActionResult CheckSubjectSlotAvialability(string subject, string level, string StartHourclinet, string StartMinuteclinet)
        public ActionResult CheckDaySlotAvialability(string levels)
        {
            //var theValue = $('#Subject').val() + ";" + $('#Class').val() + ";" + $('#StratTimeHour').val() + ";" + $('#StratTimeMinute').val() + ";" + $('#Teacher').val() + ";" + $('#Day').val();
            string[] theLEvel = levels.Split(';');
            string subject = theLEvel[0];
            string level = theLEvel[1];
            string StartHourclinet = theLEvel[2];
            string StartMinuteclinet = theLEvel[3];
            string day1 = theLEvel[5];
            //  string day1 = theLEvel[5];
            Day day = DayHelper.GetDay(day1);
            smContext db = new smContext();


            List<TeachingDay> theTeachingDay = db.TeachingDay.Include("TeachingSubject").Where(a => a.TeachingClass.ClassName == level && a.theDay == day).ToList();
            foreach (TeachingDay td in theTeachingDay)
            {
             TeachingClass theTeachingClass =   db.TeachingClass.Where(a => a.TeachingClassID == td.TeachingClassID).First();
             string theClass = theTeachingClass.ClassName;
               // string theClass = td.TeachingClass.ClassName;
                foreach (TeachingSubject ts in td.TeachingSubject)
                {
                    String Day = ts.TeachingDay.theDay.ToString();
                    string theSubject = ts.SubjectName;
                    string EndHour = ts.TeachingDay.EndTimeHour;
                    string EndMinutes = ts.TeachingDay.EndTimeMinute;

                    string StartHour = ts.TeachingDay.StratTimeHour;
                    string StartMinutes = ts.TeachingDay.StratTimeMinute;

                    StringBuilder EndHourEndMinutes = new StringBuilder();

                    EndHourEndMinutes.Append(EndHour);
                    EndHourEndMinutes.Append(EndMinutes);


                    int time = Convert.ToInt16(EndHourEndMinutes.ToString()); //= Convert.ToInt16(EndHour) + Convert.ToInt16(EndMinutes);
                    StringBuilder StartHourStartMinuteclinets = new StringBuilder();
                    StartHourStartMinuteclinets.Append(StartHourclinet).Append(StartMinuteclinet);
                    int timeClient = Convert.ToInt16(StartHourStartMinuteclinets.ToString());//Convert.ToInt16(StartHourclinet) + Convert.ToInt16(StartMinuteclinet);

                    if (timeClient < time)
                    {
                        string theAlert = theSubject + "  has been fixed  for " + theClass + " which starts " + StartHour + ":" + StartMinutes + " ends at " + EndHour + ":" + EndMinutes;
                        return Json(theAlert, JsonRequestBehavior.AllowGet);
                    }
                }




            }

            //  }


            return Json("", JsonRequestBehavior.AllowGet);
        }





        //public ActionResult CheckSubjectSlotAvialability(string subject, string level, string StartHourclinet, string StartMinuteclinet)
        public ActionResult CheckSubjectSlotAvialability(string levels)
        {
            string[] theLEvel = levels.Split(';');
            string subject = theLEvel[0];
            string level = theLEvel[1];
            string StartHourclinet = theLEvel[2];
            string StartMinuteclinet = theLEvel[3];
            string day1 = theLEvel[4];
            //  string day1 = theLEvel[5];
            Day day = DayHelper.GetDay(day1);
            smContext db = new smContext();

            // db.TeachingClass.Include("TheTeachingDay").Where(a => a.ClassName == level).ToList();

            //  List<TeachingSubject> theTeachingSubject = work.TeachingSubjectRepository.Get().ToList();
            List<TeachingClass> theTeachingClass = db.TeachingClass.Include("TheTeachingDay").Where(a => a.ClassName == level).ToList(); //work.TeachingClassRepository.Get(a => a.ClassName == level).ToList();
            foreach (TeachingClass c in theTeachingClass)
            {

                //  foreach (TeachingDay td in c.TheTeachingDay)
                // {
                List<TeachingDay> theTeachingDay = db.TeachingDay.Include("TeachingSubject").Where(a => a.TeachingClass.TeachingClassID == c.TeachingClassID && a.theDay == day).ToList();

                foreach (TeachingDay td in theTeachingDay)
                {
                    foreach (TeachingSubject ts in td.TeachingSubject)
                    {
                        String Day = ts.TeachingDay.theDay.ToString();
                        string theSubject = ts.SubjectName;
                        string EndHour = ts.TeachingDay.EndTimeHour;
                        string EndMinutes = ts.TeachingDay.EndTimeMinute;

                        string StartHour = ts.TeachingDay.StratTimeHour;
                        string StartMinutes = ts.TeachingDay.StratTimeMinute;

                        StringBuilder EndHourEndMinutes = new StringBuilder();

                        EndHourEndMinutes.Append(EndHour);
                        EndHourEndMinutes.Append(EndMinutes);


                        int time = Convert.ToInt16(EndHourEndMinutes.ToString()); //= Convert.ToInt16(EndHour) + Convert.ToInt16(EndMinutes);
                        StringBuilder StartHourStartMinuteclinets = new StringBuilder();
                        StartHourStartMinuteclinets.Append(StartHourclinet).Append(StartMinuteclinet);
                        int timeClient = Convert.ToInt16(StartHourStartMinuteclinets.ToString());//Conve

                        if (theSubject == subject && timeClient < time)
                        {
                            string theAlert = theSubject + " " + "has been fixed  for this class and which ends at " + EndHour + ":" + EndMinutes;
                            return Json(theAlert, JsonRequestBehavior.AllowGet);
                        }
                    }
                    // tdy.su

                    //   theTeachingDay.
                    //   foreach(TeachingSubj)




                }

            }


            return Json("", JsonRequestBehavior.AllowGet);
        }

        //public ActionResult CheckSubjectSlotAvialability(string subject, string level, string StartHourclinet, string StartMinuteclinet)
        public ActionResult CheckStaffSlotAvialability(string levels)
        {
            string[] theLEvel = levels.Split(';');
            string subject = theLEvel[0];
            string level = theLEvel[1];
            string StartHourclinet = theLEvel[2];
            string StartMinuteclinet = theLEvel[3];
            string teacher = theLEvel[4];
            //  string[] split = teacher.Split(' ');
            // int teacherID = Convert.ToInt32(split[0]);
            string day1 = theLEvel[5];
            Day day = DayHelper.GetDay(day1);
            smContext db = new smContext();

            List<Teacher> Teacher = db.Teacher.Include("TheTeachingClass").Where(a => a.TeacherName == teacher).ToList();
            foreach (Teacher t in Teacher)
            {

                //  List<TeachingSubject> theTeachingSubject = work.TeachingSubjectRepository.Get().ToList();
                // List<TeachingClass> theTeachingClass = db.TeachingClass.Include("TheTeachingDay").Where(a => a.ClassName == level && a.Teacher.TeacherID == t.TeacherID).ToList(); //work.TeachingClassRepository.Get(a => a.ClassName == level).ToList();
                List<TeachingClass> theTeachingClass = db.TeachingClass.Include("TheTeachingDay").Where(a => a.Teacher.TeacherID == t.TeacherID).ToList(); //work.TeachingClassRepository.Get(a => a.ClassName == level).ToList();
                foreach (TeachingClass c in theTeachingClass)
                {
                    string ClassName = c.ClassName;
                    //  foreach (TeachingDay td in c.TheTeachingDay)
                    // {
                    List<TeachingDay> theTeachingDay = db.TeachingDay.Include("TeachingSubject").Where(a => a.TeachingClass.TeachingClassID == c.TeachingClassID && a.theDay == day).ToList();

                    foreach (TeachingDay td in theTeachingDay)
                    {
                        foreach (TeachingSubject ts in td.TeachingSubject)
                        {
                            String Day = ts.TeachingDay.theDay.ToString();
                            string theSubject = ts.SubjectName;
                            string EndHour = ts.TeachingDay.EndTimeHour;
                            string EndMinutes = ts.TeachingDay.EndTimeMinute;

                            string StartHour = ts.TeachingDay.StratTimeHour;
                            string StartMinutes = ts.TeachingDay.StratTimeMinute;

                            StringBuilder EndHourEndMinutes = new StringBuilder();

                            EndHourEndMinutes.Append(EndHour);
                            EndHourEndMinutes.Append(EndMinutes);


                            int time = Convert.ToInt16(EndHourEndMinutes.ToString()); //= Convert.ToInt16(EndHour) + Convert.ToInt16(EndMinutes);
                            StringBuilder StartHourStartMinuteclinets = new StringBuilder();
                            StartHourStartMinuteclinets.Append(StartHourclinet).Append(StartMinuteclinet);
                            int timeClient = Convert.ToInt16(StartHourStartMinuteclinets.ToString());//Conve

                            if (theSubject == subject && timeClient < time)
                            {
                                string theAlert = theSubject + " " + "has been fixed  for " + teacher + " on " + Day + " for class " + ClassName + " which starts at " + StartHour + ":" + StartMinutes + " and which ends at " + EndHour + ":" + EndMinutes;
                                return Json(theAlert, JsonRequestBehavior.AllowGet);
                            }

                            if (theSubject != subject && timeClient < time)
                            {
                                string theAlert = theSubject + " " + "has been fixed  for " + teacher + " on " + Day + " for class " + ClassName + " which starts at " + StartHour + ":" + StartMinutes + " and which ends at " + EndHour + ":" + EndMinutes;
                                // string theAlert = teacher + " " + "has been fixed  for this day and ends " + EndHour + ":" + EndMinutes;
                                return Json(theAlert, JsonRequestBehavior.AllowGet);
                            }
                        }
                        // tdy.su

                        //   theTeachingDay.
                        //   foreach(TeachingSubj)




                    }

                }
            }


            return Json("", JsonRequestBehavior.AllowGet);
        }


    }
}
