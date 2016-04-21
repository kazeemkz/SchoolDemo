using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using JobHustler.DAL;
using SchoolManagement.DAL;
using SchoolManagement.Model;
using PagedList;
using SchoolManagement.Models;
using MvcMembership;
using MvcMembership.Settings;
using System.Web.Security;
using JobHustler.Models;

namespace SchoolManagement.Controllers
{
    // [Authorize]
    public class PrimarySchoolStudentController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        // UnitOfWork work2 = new UnitOfWork();
        [DynamicAuthorize]
        public ViewResult Index(string arm, string sortOrder, string PrimarySec, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page)
        {

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
            ViewData["arm"] = theItem;

            
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            ViewBag.ClassSortParm = sortOrder == "class" ? "class desc" : "class";
            ViewBag.GenderSortParm = sortOrder == "gender" ? "gender desc" : "gender";

            if (Request.HttpMethod == "GET")
            {
                // searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            var students = from s in work.PrimarySchoolStudentRepository.Get()
                           select s;



            if (!String.IsNullOrEmpty(ApprovedString))
            {
                bool theValue = Convert.ToBoolean(ApprovedString);
                students = students.Where(s => s.IsApproved == theValue);
            }


            if (!String.IsNullOrEmpty(SexString))
            {
                students = students.Where(s => s.Sex.Equals(SexString));
            }


            if (!String.IsNullOrEmpty(PrimarySec))
            {
                List<PrimarySchoolStudent> thePeople = new List<PrimarySchoolStudent>();
                List<SecondarySchoolStudent> theSec = new List<SecondarySchoolStudent>();

                if (PrimarySec == "Primary")
                {


                    //  List<PrimarySchoolStudent> thePeopleDatabase = students;//.Where(s is SchoolManagement.Model.PrimarySchoolStudent);// work.PrimarySchoolStudentRepository.Get().ToList();
                    foreach (var t in students)
                    {
                        if (t is SchoolManagement.Model.SecondarySchoolStudent)
                        {

                        }
                        else
                        {
                            thePeople.Add(t);
                        }

                    }

                    students = thePeople;
                }



                else
                {
                    foreach (var t in students)
                    {
                        if (t is SchoolManagement.Model.SecondarySchoolStudent)
                        {
                            thePeople.Add(t);
                        }
                        else
                        {

                        }

                    }
                    students = thePeople;
                }
                // students = students.Where(s => s..Equals(SexString));
            }







            if (!String.IsNullOrEmpty(LevelString))
            {
                //students = students.Where(s => s.PresentLevel.Equals(LevelString) && s.PresentLevel !=null);
                students = students.Where(s => s.PresentLevel == LevelString);
            }


            if (!String.IsNullOrEmpty(arm))
            {
                //students = students.Where(s => s.PresentLevel.Equals(LevelString) && s.PresentLevel !=null);
                students = students.Where(s => s.LevelType == arm);
            }



            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstName.ToUpper().Contains(searchString.ToUpper()) || s.Middle.ToUpper().Contains(searchString.ToUpper()));
            }




            if (!String.IsNullOrEmpty(StudentIDString))
            {
                int theID = Convert.ToInt32(StudentIDString);
                students = students.Where(s => s.UserID == theID);
            }

            //if (!String.IsNullOrEmpty(SexString))
            //{
            //    students = students.Where(s => s.Sex.Equals(SexString));
            //}

            switch (sortOrder)
            {
                case "Name desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;

                case "class":
                    students = students.OrderBy(s => s.PresentLevel);
                    break;
                case "class desc":
                    students = students.OrderByDescending(s => s.PresentLevel);
                    break;

                case "gender":
                    students = students.OrderBy(s => s.Sex);
                    break;
                case "gender desc":
                    students = students.OrderByDescending(s => s.Sex);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            if (User.IsInRole("Student"))
            {

                ViewBag.Count = 1;
                int UserName = Convert.ToInt32(User.Identity.Name);
                // List<PrimarySchoolStaff> theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == UserName).ToList();
                //PrimarySchoolStaff theStaf = theStaff[0];
                students = students.Where(a => a.UserID == UserName && a.IsApproved == true);
                return View(students.ToPagedList(pageNumber, pageSize));


            }
            if (User.IsInRole("Parent"))
            {

                ViewBag.Count = 1;
                Int32 UserName = Convert.ToInt32(User.Identity.Name);
                List<Parent> theParentt = work.ParentRepository.Get(a => a.UserID == UserName).ToList();
                Parent theRealParent;
                // if (theParentt.Count() > 0)
                //{
                theRealParent = theParentt[0];
                //}
                // List<PrimarySchoolStaff> theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == UserName).ToList();
                //PrimarySchoolStaff theStaf = theStaff[0];
                students = students.Where(a => a.ParentID == theRealParent.ParentID && a.IsApproved == true);
                return View(students.ToPagedList(pageNumber, pageSize));

            }
            else
            {
                ViewBag.Count = students.Count();
                return View(students.ToPagedList(pageNumber, pageSize));
            }
        }


        [DynamicAuthorize]
        public ViewResult Index2(string arm, string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page)
     
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            ViewBag.ClassSortParm = sortOrder == "class" ? "class desc" : "class";
            ViewBag.GenderSortParm = sortOrder == "gender" ? "gender desc" : "gender";


            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();
            theItem.Add(new SelectListItem() { Text = "None", Value = "" });

            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
            theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
            theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });

            ViewData["arm"] = theItem;

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            var students = from s in work.PrimarySchoolStudentRepository.Get()
                           select s;


            if (!String.IsNullOrEmpty(SexString))
            {
                students = students.Where(s => s.Sex==(SexString));
            }

            if (!String.IsNullOrEmpty(LevelString))
            {
                students = students.Where(s => s.PresentLevel ==(LevelString));
            }

            if (!String.IsNullOrEmpty(arm))
            {
                //students = students.Where(s => s.PresentLevel.Equals(LevelString) && s.PresentLevel !=null);
                students = students.Where(s => s.LevelType == arm);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstName.ToUpper().Contains(searchString.ToUpper()) || s.Middle.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!String.IsNullOrEmpty(StudentIDString))
            {
                int theID = Convert.ToInt32(StudentIDString);
                students = students.Where(s => s.UserID == theID);
            }

            //if (!String.IsNullOrEmpty(SexString))
            //{
            //    students = students.Where(s => s.Sex.Equals(SexString));
            //}
            switch (sortOrder)
            {
                case "Name desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;

                case "class":
                    students = students.OrderBy(s => s.PresentLevel);
                    break;
                case "class desc":
                    students = students.OrderByDescending(s => s.PresentLevel);
                    break;

                case "gender":
                    students = students.OrderBy(s => s.Sex);
                    break;
                case "gender desc":
                    students = students.OrderByDescending(s => s.Sex);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);


            if (User.IsInRole("Teacher"))
            {

                int UserName = Convert.ToInt32(User.Identity.Name);
                List<PrimarySchoolStaff> theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == UserName).ToList();
                PrimarySchoolStaff theStaf = theStaff[0];
                students = students.Where(a => a.LevelType == theStaf.ClassTeacherOf && a.IsApproved == true);
                return View("Index2", students.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View("Index2", students.ToPagedList(pageNumber, pageSize));
            }
        }


        //[DynamicAuthorize]
        //public ViewResult Index3(string arm, string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page
        //    , string paidschoolfess, string term)
        //{


        //    List<Level> theLevels = work.LevelRepository.Get().ToList();
        //    List<SelectListItem> theItem = new List<SelectListItem>();
        //    theItem.Add(new SelectListItem() { Text = "None", Value = "" });

        //    foreach (var level in theLevels)
        //    {
        //        theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
        //    }

        //    ViewData["arm"] = theItem;
        //    theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
        //    theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
        //    theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });

        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
        //    ViewBag.ClassSortParm = sortOrder == "class" ? "class desc" : "class";
        //    ViewBag.GenderSortParm = sortOrder == "gender" ? "gender desc" : "gender";

        //    if (Request.HttpMethod == "GET")
        //    {
        //        // searchString = currentFilter;
        //    }
        //    else
        //    {
        //        page = 1;
        //    }
        //    ViewBag.CurrentFilter = searchString;

        //    var students = from s in work.PrimarySchoolStudentRepository.Get()
        //                   select s;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
        //                               || s.FirstName.ToUpper().Contains(searchString.ToUpper()) || s.Middle.ToUpper().Contains(searchString.ToUpper()));
        //    }

        //    if (!String.IsNullOrEmpty(SexString))
        //    {
        //        students = students.Where(s => s.Sex.Equals(SexString));
        //    }

        //    if (!String.IsNullOrEmpty(LevelString))
        //    {
        //        students = students.Where(s => s.PresentLevel == LevelString);
        //    }

        //    if (!String.IsNullOrEmpty(arm))
        //    {
        //        //students = students.Where(s => s.PresentLevel.Equals(LevelString) && s.PresentLevel !=null);
        //        students = students.Where(s => s.LevelType == arm);
        //    }

        //    if (!String.IsNullOrEmpty(StudentIDString))
        //    {
        //        int theID = Convert.ToInt32(StudentIDString);
        //        students = students.Where(s => s.UserID == theID);
        //    }

        //    if (!String.IsNullOrEmpty(SexString))
        //    {
        //        students = students.Where(s => s.Sex.Equals(SexString));
        //    }

        //    //if (!String.IsNullOrEmpty(ApprovedString))
        //    //{
        //    //    bool theValue = Convert.ToBoolean(ApprovedString);
        //    //    students = students.Where(s => s.IsApproved == theValue);
        //    //}


        //    if (!String.IsNullOrEmpty(term) && !String.IsNullOrEmpty(LevelString) && !String.IsNullOrEmpty(paidschoolfess))
        //    {

        //        bool paidschoolfess1 = Convert.ToBoolean(paidschoolfess);
        //        if (paidschoolfess1)
        //        {
        //            List<PrimarySchoolStudent> theStudent = new List<PrimarySchoolStudent>();
        //            List<SchoolFeePayment> theStudentPayment = work.SchoolFeePaymentRepository.Get(a => a.Term.Equals(term) && a.Level.Equals(LevelString)).ToList();

        //            foreach (var payment in theStudentPayment)
        //            {
        //                List<PrimarySchoolStudent> thestudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID.Equals(payment.StudentID)).ToList();
        //                PrimarySchoolStudent theRealStudent = thestudent[0];
        //                if (!(theStudent.Contains(theRealStudent)))
        //                    theStudent.Add(theRealStudent);
        //            }
        //            // bool theValue = Convert.ToBoolean(ApprovedString);
        //            students = theStudent; //students.Where(s => s.IsApproved == theValue);
        //        }
        //        else
        //        {
        //            //student that have paid
        //            List<PrimarySchoolStudent> theStudentThatHavePaid = new List<PrimarySchoolStudent>();

        //            List<PrimarySchoolStudent> theStudentThatHaveNotPaid = new List<PrimarySchoolStudent>();
        //            List<SchoolFeePayment> theStudentPayment = work.SchoolFeePaymentRepository.Get(a => a.Term.Equals(term) && a.Level.Equals(LevelString)).ToList();
        //            //student in a given class
        //            List<PrimarySchoolStudent> allStudentes = work.PrimarySchoolStudentRepository.Get(a => a.PresentLevel.Equals(LevelString)).ToList();
        //            foreach (var payment in theStudentPayment)
        //            {
        //                List<PrimarySchoolStudent> thestudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID.Equals(payment.StudentID)).ToList();
        //                PrimarySchoolStudent theRealStudent = thestudent[0];
        //                if (!(theStudentThatHavePaid.Contains(theRealStudent)))
        //                    theStudentThatHavePaid.Add(theRealStudent);
        //            }
        //            // bool theValue = Convert.ToBoolean(ApprovedString);
        //            foreach (var student in allStudentes)
        //            {
        //                if (theStudentThatHavePaid.Contains(student))
        //                {

        //                }
        //                else
        //                {
        //                    theStudentThatHaveNotPaid.Add(student);
        //                }
        //            }
        //            //  theStudent.;
        //            students = theStudentThatHaveNotPaid; //students.Where(s => s.IsApproved == theValue);
        //        }
        //    }

        //    //if (!String.IsNullOrEmpty(term) || !String.IsNullOrEmpty(LevelString) || !String.IsNullOrEmpty(paidschoolfess))
        //    //{

        //    //}

        //    //  ,bool paidschoolfess, string term)
        //    switch (sortOrder)
        //    {
        //        case "Name desc":
        //            students = students.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            students = students.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "Date desc":
        //            students = students.OrderByDescending(s => s.EnrollmentDate);
        //            break;

        //        case "class":
        //            students = students.OrderBy(s => s.PresentLevel);
        //            break;
        //        case "class desc":
        //            students = students.OrderByDescending(s => s.PresentLevel);
        //            break;

        //        case "gender":
        //            students = students.OrderBy(s => s.Sex);
        //            break;
        //        case "gender desc":
        //            students = students.OrderByDescending(s => s.Sex);
        //            break;
        //        default:
        //            students = students.OrderBy(s => s.LastName);
        //            break;
        //    }

        //    int pageSize = 100;
        //    int pageNumber = (page ?? 1);

        //    // if (User.IsInRole("Teacher"))
        //    // {

        //    // int UserName = Convert.ToInt32(User.Identity.Name);
        //    // List<PrimarySchoolStaff> theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == UserName).ToList();
        //    // PrimarySchoolStaff theStaf = theStaff[0];
        //    students = students.Where(a => a.IsApproved == true);
        //    ViewBag.Count = students.Count();
        //    return View("Index3", students.ToPagedList(pageNumber, pageSize));
        //    // }
        //    // else
        //    // {
        //    //   return View("Index3", students.ToPagedList(pageNumber, pageSize));
        //    // }
        //}




        //[Authorize(Roles = "Student, Parent")]
        //public ViewResult ViewYourFees(string studentID, string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page
        //    , string paidschoolfess, string term)
        //{

        //    ViewBag.CurrentSort = sortOrder;
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
        //    ViewBag.ClassSortParm = sortOrder == "class" ? "class desc" : "class";
        //    ViewBag.GenderSortParm = sortOrder == "gender" ? "gender desc" : "gender";

        //    if (Request.HttpMethod == "GET")
        //    {
        //        // searchString = currentFilter;
        //    }
        //    else
        //    {
        //        page = 1;
        //    }
        //    ViewBag.CurrentFilter = searchString;

        //    var students = from s in work.PrimarySchoolStudentRepository.Get()
        //                   select s;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
        //                               || s.FirstName.ToUpper().Contains(searchString.ToUpper()) || s.Middle.ToUpper().Contains(searchString.ToUpper()));
        //    }

        //    if (!String.IsNullOrEmpty(SexString))
        //    {
        //        students = students.Where(s => s.Sex.Equals(SexString));
        //    }

        //    if (!String.IsNullOrEmpty(LevelString))
        //    {
        //        students = students.Where(s => s.PresentLevel == LevelString);
        //    }

        //    if (!String.IsNullOrEmpty(StudentIDString))
        //    {
        //        int theID = Convert.ToInt32(StudentIDString);
        //        students = students.Where(s => s.UserID == theID);
        //    }

        //    if (!String.IsNullOrEmpty(SexString))
        //    {
        //        students = students.Where(s => s.Sex.Equals(SexString));
        //    }



        //    if (!String.IsNullOrEmpty(term) && !String.IsNullOrEmpty(LevelString) && !String.IsNullOrEmpty(paidschoolfess))
        //    {

        //        bool paidschoolfess1 = Convert.ToBoolean(paidschoolfess);
        //        if (paidschoolfess1)
        //        {
        //            List<PrimarySchoolStudent> theStudent = new List<PrimarySchoolStudent>();
        //            List<SchoolFeePayment> theStudentPayment = work.SchoolFeePaymentRepository.Get(a => a.Term.Equals(term) && a.Level.Equals(LevelString)).ToList();

        //            foreach (var payment in theStudentPayment)
        //            {
        //                List<PrimarySchoolStudent> thestudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID.Equals(payment.StudentID)).ToList();
        //                PrimarySchoolStudent theRealStudent = thestudent[0];
        //                if (!(theStudent.Contains(theRealStudent)))
        //                    theStudent.Add(theRealStudent);
        //            }
        //            // bool theValue = Convert.ToBoolean(ApprovedString);
        //            students = theStudent; //students.Where(s => s.IsApproved == theValue);
        //        }
        //        else
        //        {
        //            //student that have paid
        //            List<PrimarySchoolStudent> theStudentThatHavePaid = new List<PrimarySchoolStudent>();

        //            List<PrimarySchoolStudent> theStudentThatHaveNotPaid = new List<PrimarySchoolStudent>();
        //            List<SchoolFeePayment> theStudentPayment = work.SchoolFeePaymentRepository.Get(a => a.Term.Equals(term) && a.Level.Equals(LevelString)).ToList();
        //            //student in a given class
        //            List<PrimarySchoolStudent> allStudentes = work.PrimarySchoolStudentRepository.Get(a => a.PresentLevel.Equals(LevelString)).ToList();
        //            foreach (var payment in theStudentPayment)
        //            {
        //                List<PrimarySchoolStudent> thestudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID.Equals(payment.StudentID)).ToList();
        //                PrimarySchoolStudent theRealStudent = thestudent[0];
        //                if (!(theStudentThatHavePaid.Contains(theRealStudent)))
        //                    theStudentThatHavePaid.Add(theRealStudent);
        //            }
        //            // bool theValue = Convert.ToBoolean(ApprovedString);
        //            foreach (var student in allStudentes)
        //            {
        //                if (theStudentThatHavePaid.Contains(student))
        //                {

        //                }
        //                else
        //                {
        //                    theStudentThatHaveNotPaid.Add(student);
        //                }
        //            }
        //            //  theStudent.;
        //            students = theStudentThatHaveNotPaid; //students.Where(s => s.IsApproved == theValue);
        //        }
        //    }

           
        //    switch (sortOrder)
        //    {
        //        case "Name desc":
        //            students = students.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            students = students.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "Date desc":
        //            students = students.OrderByDescending(s => s.EnrollmentDate);
        //            break;

        //        case "class":
        //            students = students.OrderBy(s => s.PresentLevel);
        //            break;
        //        case "class desc":
        //            students = students.OrderByDescending(s => s.PresentLevel);
        //            break;

        //        case "gender":
        //            students = students.OrderBy(s => s.Sex);
        //            break;
        //        case "gender desc":
        //            students = students.OrderByDescending(s => s.Sex);
        //            break;
        //        default:
        //            students = students.OrderBy(s => s.LastName);
        //            break;
        //    }

        //    int pageSize = 20;
        //    int pageNumber = (page ?? 1);



        //    //students = students.Where(a => a.IsApproved == true && a.UserID == theUserID);

        //    if (User.IsInRole("Parent"))
        //    {
        //        Int32 theUser = Convert.ToInt32(User.Identity.Name);
        //        Parent theP = work.ParentRepository.Get(a => a.UserID == theUser).First();
        //        if (studentID != null)
        //        {
        //            int id = Convert.ToInt32(studentID);
        //            students = students.Where(a => a.PersonID == id);
        //        }
        //        else
        //        {
        //            students = students.Where(a => a.PersonID == 0);
        //        }
        //    }
        //    else
        //    {
        //        if (User.IsInRole("Student"))
        //        {
        //            int theUserID = Convert.ToInt32(User.Identity.Name);
        //            students = students.Where(a => a.IsApproved == true && a.UserID == theUserID);
        //        }

        //    }
        //    if (User.IsInRole("Parent"))
        //    {
        //        Int32 theUser = Convert.ToInt32(User.Identity.Name);
        //        List<PrimarySchoolStudent> thSchoolStudents;
        //        //  string userid = User.Identity.Name;
        //        List<Parent> theP = work.ParentRepository.Get(a => a.UserID == theUser).ToList();
        //        Parent theRealParent = theP[0];
        //        int idParent = theRealParent.ParentID;
        //        thSchoolStudents = work.PrimarySchoolStudentRepository.Get(a => a.ParentID == idParent).ToList();
        //        List<SelectListItem> theItem = new List<SelectListItem>();
        //        // theItem.Add(new SelectListItem() { Text = "None", Value = "" });

        //        foreach (var s in thSchoolStudents)
        //        {
        //            theItem.Add(new SelectListItem() { Text = s.LastName + " " + s.FirstName, Value = s.PersonID.ToString() });
        //            ViewData["Students"] = theItem;
        //        }
        //    }
        //    return View("ViewYourFees", students.ToPagedList(pageNumber, pageSize));

        //}


        [Authorize(Roles = "Student")]
        public ViewResult ViewYourStoreItems(string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page
            , string paidschoolfess, string term)
        {



            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            ViewBag.ClassSortParm = sortOrder == "class" ? "class desc" : "class";
            ViewBag.GenderSortParm = sortOrder == "gender" ? "gender desc" : "gender";

            if (Request.HttpMethod == "GET")
            {
                // searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            var students = from s in work.PrimarySchoolStudentRepository.Get()
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstName.ToUpper().Contains(searchString.ToUpper()) || s.Middle.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!String.IsNullOrEmpty(SexString))
            {
                students = students.Where(s => s.Sex.Equals(SexString));
            }

            if (!String.IsNullOrEmpty(LevelString))
            {
                students = students.Where(s => s.PresentLevel.Equals(LevelString));
            }

            if (!String.IsNullOrEmpty(StudentIDString))
            {
                int theID = Convert.ToInt32(StudentIDString);
                students = students.Where(s => s.UserID == theID);
            }

            if (!String.IsNullOrEmpty(SexString))
            {
                students = students.Where(s => s.Sex.Equals(SexString));
            }



            if (!String.IsNullOrEmpty(term) && !String.IsNullOrEmpty(LevelString) && !String.IsNullOrEmpty(paidschoolfess))
            {

                bool paidschoolfess1 = Convert.ToBoolean(paidschoolfess);
                if (paidschoolfess1)
                {
                    List<PrimarySchoolStudent> theStudent = new List<PrimarySchoolStudent>();
                    List<SchoolFeePayment> theStudentPayment = work.SchoolFeePaymentRepository.Get(a => a.Term.Equals(term) && a.Level.Equals(LevelString)).ToList();

                    foreach (var payment in theStudentPayment)
                    {
                        List<PrimarySchoolStudent> thestudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID.Equals(payment.StudentID)).ToList();
                        PrimarySchoolStudent theRealStudent = thestudent[0];
                        if (!(theStudent.Contains(theRealStudent)))
                            theStudent.Add(theRealStudent);
                    }
                    // bool theValue = Convert.ToBoolean(ApprovedString);
                    students = theStudent; //students.Where(s => s.IsApproved == theValue);
                }
                else
                {
                    //student that have paid
                    List<PrimarySchoolStudent> theStudentThatHavePaid = new List<PrimarySchoolStudent>();

                    List<PrimarySchoolStudent> theStudentThatHaveNotPaid = new List<PrimarySchoolStudent>();
                    List<SchoolFeePayment> theStudentPayment = work.SchoolFeePaymentRepository.Get(a => a.Term.Equals(term) && a.Level.Equals(LevelString)).ToList();
                    //student in a given class
                    List<PrimarySchoolStudent> allStudentes = work.PrimarySchoolStudentRepository.Get(a => a.PresentLevel.Equals(LevelString)).ToList();
                    foreach (var payment in theStudentPayment)
                    {
                        List<PrimarySchoolStudent> thestudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID.Equals(payment.StudentID)).ToList();
                        PrimarySchoolStudent theRealStudent = thestudent[0];
                        if (!(theStudentThatHavePaid.Contains(theRealStudent)))
                            theStudentThatHavePaid.Add(theRealStudent);
                    }
                    // bool theValue = Convert.ToBoolean(ApprovedString);
                    foreach (var student in allStudentes)
                    {
                        if (theStudentThatHavePaid.Contains(student))
                        {

                        }
                        else
                        {
                            theStudentThatHaveNotPaid.Add(student);
                        }
                    }
                    //  theStudent.;
                    students = theStudentThatHaveNotPaid; //students.Where(s => s.IsApproved == theValue);
                }
            }

            //if (!String.IsNullOrEmpty(term) || !String.IsNullOrEmpty(LevelString) || !String.IsNullOrEmpty(paidschoolfess))
            //{

            //}

            //  ,bool paidschoolfess, string term)
            switch (sortOrder)
            {
                case "Name desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;

                case "class":
                    students = students.OrderBy(s => s.PresentLevel);
                    break;
                case "class desc":
                    students = students.OrderByDescending(s => s.PresentLevel);
                    break;

                case "gender":
                    students = students.OrderBy(s => s.Sex);
                    break;
                case "gender desc":
                    students = students.OrderByDescending(s => s.Sex);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            int theUserID = Convert.ToInt32(User.Identity.Name);
            students = students.Where(a => a.IsApproved == true && a.UserID == theUserID);
            return View("ViewYourFees", students.ToPagedList(pageNumber, pageSize));

        }

        //
        // GET: /PrimarySchoolStudent/Details/5
        // [DynamicAuthorize]
        [Authorize]
        public ActionResult Details(string id, int tracker = 0)
        {
          //  Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            if (tracker == 1)
            {
                //  ViewData["Success"] = "Show the Dialog";
                 ViewBag.Success = "Show the Dialog";
            }
            PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(theId);
            return View("Details2", theStudent);
        }

        //  [Authorize]
        [AcceptVerbs(HttpVerbs.Get)]
        public FileContentResult GetImage(int id)
        {
            try
            {

                //PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(id);
                List<Photo> thePhoto = work.PhotoRepository.Get(a => a.PersonID == id).ToList();
                Photo myPhoto = thePhoto[0];

                //string imageFile = Path.Combine(Server.MapPath("~/App_Data"), "foo.png");
                //byte[] buffer = File.ReadAllBytes(imageFile);




                return File(myPhoto.PhotoImage, "image/png");
            }
            catch (Exception e)
            {
                byte[] image = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/avater1.jpg"));
                return File(image, "image/png");
            }

            // Server.MapPath("~/Images/avater1.jpg")



        }





        //
        // GET: /PrimarySchoolStudent/Create
        [DynamicAuthorize]
        public ActionResult Create()
        {

            return View();
        }

        public ActionResult Create2()
        {


            //  ViewData["Success"] = "tytyu";

            return View("Create2");
        }

        [HttpPost]
        public ActionResult Create2(PrimarySchoolStudent model)
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

                work.PrimarySchoolStudentRepository.Insert(model);
                work.Save();
                //  }
                // TODO: Add insert logic here

                ViewData["Success"] = "Dear " + model.FirstName + "Your data has been saved successfully, we shall get back at you soon";
                // return Content("Data added successfully");
              //  Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
                //  return RedirectToAction("Details", "SecondarySchoolStudent", new { id = SchoolManagement.Models.Encript.EncryptString( model.PersonID .ToString(),true), tracker = 1});
                return RedirectToAction("Create", "Photo", new { id = SchoolManagement.Models.Encript.EncryptString( model.PersonID .ToString(),true)});
            }
            catch
            {
                return View();
            }
        }


        //
        // POST: /PrimarySchoolStudent/Create
        [DynamicAuthorize]
        [HttpPost]
        public ActionResult Create(PrimarySchoolStudent model)
        {
            model.EnrollmentDate = DateTime.Now;
            try
            {
                if (!(ModelState.IsValid))
                {
                    return View(model);
                }

                model.IsApproved = false;
                // model.DateEntered = DateTime.Today;

                //  model.DateApproved = DateTime.Now;

                model.EnrollmentDate = DateTime.Now;

                work.PrimarySchoolStudentRepository.Insert(model);
                work.Save();
                //  }
                // TODO: Add insert logic here

                ViewData["Success"] = "Dear " + model.FirstName + "Your data has been saved successfully, we shall get back at you soon";
                // return Content("Data added successfully");
                return RedirectToAction("Create", "Photo", new { id = model.PersonID });
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /PrimarySchoolStudent/Edit/5
        [DynamicAuthorize]
        public ActionResult Edit(string id)
        {
          //  Convert.
          Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));// Convert.ToInt32( EncodeDecode.Decode(id));

             //Convert.ToInt64(Security.URLDecrypt(id));
            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();

            theItem.Add(new SelectListItem() { Text = "None", Value = "" });
            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
            theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });            theItem.Add(new SelectListItem() { Text = "Expelled", Value = "Expelled" });
            theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });
            ViewData["Level"] = theItem;
            PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(theId);
            ViewBag.StudentID = theStudent.UserID;
            if (((!(string.IsNullOrEmpty(theStudent.InitialLevel))) && (theStudent.IsApproved == false)))
            {
                if (theStudent.InitialLevel.Contains("KG") || theStudent.InitialLevel.StartsWith("NURS") || theStudent.InitialLevel.StartsWith("PRIMARY"))
                {
                    ViewBag.StudentID = 20000000 + theStudent.PersonID;
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
        // POST: /PrimarySchoolStudent/Edit/5
        [DynamicAuthorize]
        [HttpPost]
        public ActionResult Edit(PrimarySchoolStudent model)
        {
            try
            {
                // TODO: Add update logic here

                if (ModelState.IsValid)
                {
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
                        if (model.PresentLevel.Contains("KG") || model.PresentLevel.StartsWith("NURS") || model.PresentLevel.StartsWith("PRIMARY"))
                        {
                            // model.p
                            MembershipUser user = Membership.GetUser(Convert.ToString(20000000 + model.PersonID), false);
                            if (user == null)
                            {
                                model.RepeatTimes = 0;
                                model.UserID = 20000000 + model.PersonID;
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
                            //  model.PresentLevel = theLevel[0];
                        }

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
                        work.PrimarySchoolStudentRepository.Update(model);
                    }
                    if (User.Identity.Name != "5000001")
                    {
                        AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Student Information Updated,  Student ID:-" + model.UserID, UserID = User.Identity.Name };
                        work.AuditTrailRepository.Insert(audit);
                        work.Save();
                    }

                    work.Save();

                }
                return RedirectToAction("Index");
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

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult EditSuperAdmin(string arm)
        {
            PrimarySchoolStudentViewModel theStu = new PrimarySchoolStudentViewModel();
            List<PrimarySchoolStudent> theStudent = new List<PrimarySchoolStudent>();
            List<PrimarySchoolStudent> theStudent1 = new List<PrimarySchoolStudent>();
            List<PrimarySchoolStudent> st = new List<PrimarySchoolStudent>();
            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();
            theItem.Add(new SelectListItem() { Text = "None", Value = "" });

            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            //{"Graduated","Graduated"},
            //   {"Withdraw","Withdraw"},
            //     {"Left","Left"},
            theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
            theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
            theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });
            if (!(string.IsNullOrEmpty(arm)))
            {

                theStudent = work.PrimarySchoolStudentRepository.Get(a => a.LevelType == arm).ToList();
                theStudent1 = work.PrimarySchoolStudentRepository.Get(a => a.PresentLevel == arm).ToList();
                foreach (var s in theStudent)
                {
                    st.Add(s);
                }

                foreach (var s in theStudent1)
                {
                    if (!(st.Contains(s)))
                    {
                        st.Add(s);
                    }
                }
                theStu.Item = st;
            }
            ViewBag.Student = theStudent.Count();
            ViewData["arm"] = theItem;
            return View("EditSuperAdmin", theStu);

        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult EditSuperAdmin1(PrimarySchoolStudentViewModel model)
        {

            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();
            theItem.Add(new SelectListItem() { Text = "None", Value = "" });

            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
            theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
            theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });
            ViewBag.Student = 0;// theStudent.Count();
            ViewData["arm"] = theItem;

            if (model != null)
            {
                if (model.Item != null)
                {
                    foreach (PrimarySchoolStudent m in model.Item)
                    {
                        PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(m.PersonID);
                        theStudent.LevelType = m.LevelType;
                        string[] theSplitedItem = m.LevelType.Split(':');
                        if (theSplitedItem.Count() == 2)
                        {
                            theStudent.PresentLevel = theSplitedItem[0];
                        }
                        else
                        {
                            //theStudent.PresentLevel = m.LevelType;
                            theStudent.LevelType = m.LevelType;
                        }
                        work.PrimarySchoolStudentRepository.Update(theStudent);

                    }
                    work.Save();
                }



                return View("EditSuperAdmin");
            }
            else
            {
                //  PrimarySchoolStudentViewModel theStu = new PrimarySchoolStudentViewModel();
                // List<PrimarySchoolStudent> theStudent = new List<PrimarySchoolStudent>();
                // List<PrimarySchoolStudent> st = new List<PrimarySchoolStudent>();

                return View("EditSuperAdmin");
                // return View("EditSuperAdmin");
            }

        }

        //
        // GET: /PrimarySchoolStudent/Delete/5
        [DynamicAuthorize]
        public ActionResult Delete(string id)
        {
            Int32 theId = Convert.ToInt32(EncodeDecode.Decode(id));
            PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(theId);
            return View(theStudent);
        }

        //
        // POST: /PrimarySchoolStudent/Delete/5
        [DynamicAuthorize]
        [HttpPost]
        public ActionResult Delete(PrimarySchoolStudent model)
        {
            try
            {
                // TODO: Add delete logic here
                string firstName = model.FirstName;
                string lastName = model.LastName;
                PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(model.PersonID);
                work.PrimarySchoolStudentRepository.Delete(theStudent);
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
        // public dynamic ValidateCode(string code)
        //public ViewResult ValidateCode(string code)
        public ActionResult ValidateCode(string code)
        {

            if (code == "555")
            {
                RedirectToAction("Create2", "PrimarySchoolStudent");
                return View("Create2");

            }
            else
            {
                ViewBag.ID = 1;
                RedirectToAction("Login", "Account", new { returnUrl = "/" });
                return View("Login");

            }
            return View();
            //   RedirectToAction("Login", "Account", new { returnUrl = "/" });
            //  }



        }

    }
}
