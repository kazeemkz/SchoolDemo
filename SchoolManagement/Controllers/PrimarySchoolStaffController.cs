using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using JobHustler.DAL;
using MvcMembership;
using MvcMembership.Settings;
using SchoolManagement.Model;
using SchoolManagement.Models;
using PagedList;
using JobHustler.Models;
using SchoolManagement.DAL;
using System.Text;

namespace SchoolManagement.Controllers
{
    // [DynamicAuthorize]
    public class PrimarySchoolStaffController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        UnitOfWork work2 = new UnitOfWork();

        smContext db = new smContext();
        //private readonly IRolesService _rolesService;
        //private readonly ISmtpClient _smtpClient;
        //private readonly IMembershipSettings _membershipSettings;
        //private readonly IUserService _userService;
        //private readonly IPasswordService _passwordService;
        //
        // GET: /PrimarySchoolStaff/

        //public ActionResult Index()
        //{
        //    List<PrimarySchoolStaff> theStaff = work.PrimarySchoolStaffRepository.Get().ToList();
        //    return View(theStaff);
        //}
        [DynamicAuthorize]
        public ViewResult Index(string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string role, string StudentIDString, int? page)
        {
            //get the list of class types
            Dictionary<string, string> theDic = new Dictionary<string, string>();
            List<SelectListItem> theItem = new List<SelectListItem>();
            //  List<Level> theLevels = work.LevelRepository.Get().ToList();
            //   List<SelectListItem> theItem3 = new List<SelectListItem>();
            List<MyRole> theRoles = work.MyRoleRepository.Get().ToList();
            theItem.Add(new SelectListItem() { Text = "Choose...", Value = "" });

            foreach (var role1 in theRoles)
            {
                if (!(string.IsNullOrEmpty(role1.RoleName)) && role1.RoleName != "Student")
                {
                    theItem.Add(new SelectListItem() { Text = role1.RoleName, Value = role1.RoleName });
                }
            }

            // theItem.Add(new SelectListItem() { Text = "PRIMART 1A", Value = "PRIMART 1A" });
            ViewData["Level"] = theItem;


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

            var students = from s in work.PrimarySchoolStaffRepository.Get()
                           select s;
            students = students.Where(s => s.UserID != 5000001);
            students = students.Where(s => s.UserID != 5000052);
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstName.ToUpper().Contains(searchString.ToUpper()) || s.Middle.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!String.IsNullOrEmpty(SexString))
            {
                students = students.Where(s => s.Sex.Equals(SexString));
            }

            if (!String.IsNullOrEmpty(role))
            {
                students = students.Where(s => s.Role.Equals(role));
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

            if (!String.IsNullOrEmpty(ApprovedString))
            {
                bool theValue = Convert.ToBoolean(ApprovedString);
                students = students.Where(s => s.IsApproved == theValue);
            }
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
                    students = students.OrderBy(s => s.ClassTeacherOf);
                    break;
                case "class desc":
                    students = students.OrderByDescending(s => s.ClassTeacherOf);
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

            int pageSize = 40;
            int pageNumber = (page ?? 1);
            ViewBag.Count = students.Count();

            if (!(User.IsInRole("SuperAdmin")))
            {

                int UserName = Convert.ToInt32(User.Identity.Name);
                List<PrimarySchoolStaff> theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == UserName).ToList();
                PrimarySchoolStaff theStaf = theStaff[0];
                students = students.Where(a => a.UserID == UserName && a.IsApproved == true);
                ViewBag.Count = students.Count();
                return View(students.ToPagedList(pageNumber, pageSize));

            }
            else
            {
                return View(students.ToPagedList(pageNumber, pageSize));
            }
        }



        //
        // GET: /PrimarySchoolStaff/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        public ActionResult Details(string id, int tracker = 0)
        {
            //if (tracker == 419)
            //{
            //    ViewData["Success"] = "Show the Dialog";
            //}
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));

            PrimarySchoolStaff theStaff = work.PrimarySchoolStaffRepository.GetByID(theId);
            //  PrimarySchoolStaff theStaff = db.PrimarySchoolStaff.Include("TheDeduction").Where(a => a.PersonID == theId).First();

            Salary theSalary = work.SalaryRepository.GetByID(theStaff.SalaryID);
            // PrimarySchoolStaff theStaff = work.PrimarySchoolStaffRepository.GetByID(theId);
            if (theSalary != null)
            {
                ViewBag.Salary = theSalary.SalaryDescription + " Amount per Month NGN: " + theSalary.Amount;
            }

            //get deductions out
            String[] theContributionId;// = new StringBuilder();
            List<Deduction> theDeductiong = new List<Deduction>();
            if (!(string.IsNullOrEmpty(theStaff.ContributionIDs)))
            {

                theContributionId = theStaff.ContributionIDs.Split(' ');

                foreach (var c in theContributionId)
                {
                    if (!(string.IsNullOrEmpty(c)))
                    {
                        int theContributionID = Convert.ToInt16(c);

                        Deduction thed = work.DeductionRepository.GetByID(theContributionID);
                        if (thed != null)
                        {
                            theDeductiong.Add(thed);
                        }
                    }

                }
            }
            theStaff.TheDeduction = theDeductiong;
            return View("Details", theStaff);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public FileContentResult GetImage(int id)
        {
            //PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(id);
            List<Photo> thePhoto = work.PhotoRepository.Get(a => a.PersonID == id).ToList();

            if (thePhoto.Count() > 0)
            {
                Photo myPhoto = thePhoto[0];
                return File(myPhoto.PhotoImage, "image/png");
            }
            else
            {
                byte[] image = System.IO.File.ReadAllBytes(Server.MapPath("~/Images/avater1.jpg"));
                return File(image, "image/png");
            }

        }

        //
        // GET: /PrimarySchoolStaff/Create
        [DynamicAuthorize]
        public ActionResult Create()
        {


            List<SelectListItem> theItem3 = new List<SelectListItem>();
            List<MyRole> theRoles = work.MyRoleRepository.Get().ToList();


            foreach (var role in theRoles)
            {
                if (!(string.IsNullOrEmpty(role.RoleName)) && role.RoleName != "Student")
                {
                    theItem3.Add(new SelectListItem() { Text = role.RoleName, Value = role.RoleName });
                }
            }
            ViewData["Role"] = theItem3;




            //  List<Level> theLevel = work.LevelRepository.Get().ToList();
            Dictionary<string, string> theDic = new Dictionary<string, string>();
            List<SelectListItem> theItem = new List<SelectListItem>();

            List<Level> theLevels = work.LevelRepository.Get().ToList();
            theItem.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });
            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }

            // theItem.Add(new SelectListItem() { Text = "PRIMART 1A", Value = "PRIMART 1A" });
            ViewData["Level"] = theItem;




            List<SelectListItem> theSubjectList = new List<SelectListItem>();

            List<Subject> theSubject = work.SubjectRepository.Get().ToList();
            theSubjectList.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });
            foreach (var subject in theSubject)
            {
                theSubjectList.Add(new SelectListItem() { Text = subject.Name, Value = subject.Name });
            }

            // theItem.Add(new SelectListItem() { Text = "PRIMART 1A", Value = "PRIMART 1A" });
            ViewData["Subject"] = theSubjectList;


            //List<LatenessDeduction> theLatenessDeduction = work.LatenessDeductionRepository.Get().ToList();
            List<SelectListItem> theLatenessDeductionListItem = new List<SelectListItem>();

            theLatenessDeductionListItem.Add(new SelectListItem() { Text = "None", Value = "" });
            theLatenessDeductionListItem.Add(new SelectListItem() { Text = "YES", Value = "true" });
            theLatenessDeductionListItem.Add(new SelectListItem() { Text = "NO", Value = "false" });

            ViewData["LatenessDeduction"] = theLatenessDeductionListItem;

            List<SelectListItem> theAbscentDeductionListItem = new List<SelectListItem>();

            theAbscentDeductionListItem.Add(new SelectListItem() { Text = "None", Value = "" });
            theAbscentDeductionListItem.Add(new SelectListItem() { Text = "YES", Value = "true" });
            theAbscentDeductionListItem.Add(new SelectListItem() { Text = "NO", Value = "false" });

            ViewData["AbscentDeduction"] = theAbscentDeductionListItem;


            //salary
            List<SelectListItem> theSalaryList = new List<SelectListItem>();

            List<Salary> theSalary = work.SalaryRepository.Get().ToList();
            theSalaryList.Add(new SelectListItem() { Text = "Select", Value = "" });
            foreach (var s in theSalary)
            {
                theSalaryList.Add(new SelectListItem() { Text = s.SalaryDescription + "  Amount(NGN) " + s.Amount, Value = Convert.ToString(s.SalaryID) });
            }
            ViewData["salary"] = theSalaryList;
            StaffViewModel theStaff = new StaffViewModel();

            List<Deduction> theDeductions = work.DeductionRepository.Get().ToList();
            theStaff.Deductions = theDeductions;
            return View(theStaff);
        }

        //
        // POST: /PrimarySchoolStaff/Create
        [DynamicAuthorize]
        [HttpPost]
        //public ActionResult Create(PrimarySchoolStaff model, string classteacher1, string classteacher2, string classteacher3, string classteacher4)
        public ActionResult Create(StaffViewModel viewmodel, string[] PrimarySchoolStaff, string classteacher1, string classteacher2, string classteacher3, string classteacher4)
        {
            try
            {
                // viewmodel.Deductions
                PrimarySchoolStaff model = viewmodel.PrimarySchoolStaff;
                model.EnrollmentDate = DateTime.Now;
                StringBuilder theContributionId = new StringBuilder();
                if (viewmodel.Deductions.Count > 0)
                {
                    List<Deduction> theDec = new List<Deduction>();
                    foreach (Deduction d in viewmodel.Deductions)
                    {
                        if (d.Selected == true)
                        {
                            // List<PrimarySchoolStaff> theS = new List<PrimarySchoolStaff>();
                            // theS.Add(model);
                            // d.ThePrimarySchoolStaff = theS;
                            theDec.Add(d);
                            theContributionId.Append(d.DeductionID);
                            theContributionId.Append(' ');
                        }
                    }
                    model.TheDeduction = theDec;
                    model.ContributionIDs = theContributionId.ToString();
                }
                if (!(ModelState.IsValid))
                {
                    List<SelectListItem> theItem3 = new List<SelectListItem>();
                    List<MyRole> theRoles = work.MyRoleRepository.Get().ToList();


                    foreach (var role in theRoles)
                    {
                        if (!(string.IsNullOrEmpty(role.RoleName)) && role.RoleName != "Student")
                        {
                            theItem3.Add(new SelectListItem() { Text = role.RoleName, Value = role.RoleName });
                        }
                    }
                    ViewData["Role"] = theItem3;
                    Dictionary<string, string> theDic = new Dictionary<string, string>();
                    List<SelectListItem> theItem = new List<SelectListItem>();

                    List<Level> theLevels = work.LevelRepository.Get().ToList();
                    theItem.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });
                    foreach (var level in theLevels)
                    {
                        theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
                    }

                    // theItem.Add(new SelectListItem() { Text = "PRIMART 1A", Value = "PRIMART 1A" });
                    ViewData["Level"] = theItem;

                    List<SelectListItem> theSubjectList = new List<SelectListItem>();

                    List<Subject> theSubject = work.SubjectRepository.Get().ToList();
                    theSubjectList.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });
                    foreach (var subject in theSubject)
                    {
                        theSubjectList.Add(new SelectListItem() { Text = subject.Name, Value = subject.Name });
                    }

                    // theItem.Add(new SelectListItem() { Text = "PRIMART 1A", Value = "PRIMART 1A" });
                    ViewData["Subject"] = theSubjectList;

                    //List<LatenessDeduction> theLatenessDeduction = work.LatenessDeductionRepository.Get().ToList();
                    List<SelectListItem> theLatenessDeductionListItem = new List<SelectListItem>();

                    theLatenessDeductionListItem.Add(new SelectListItem() { Text = "None", Value = "" });
                    theLatenessDeductionListItem.Add(new SelectListItem() { Text = "YES", Value = "true" });
                    theLatenessDeductionListItem.Add(new SelectListItem() { Text = "NO", Value = "false" });

                    ViewData["LatenessDeduction"] = theLatenessDeductionListItem;


                    List<SelectListItem> theAbscentDeductionListItem = new List<SelectListItem>();

                    theAbscentDeductionListItem.Add(new SelectListItem() { Text = "None", Value = "" });
                    theAbscentDeductionListItem.Add(new SelectListItem() { Text = "YES", Value = "true" });
                    theAbscentDeductionListItem.Add(new SelectListItem() { Text = "NO", Value = "false" });

                    ViewData["AbscentDeduction"] = theAbscentDeductionListItem;


                    //salary
                    List<SelectListItem> theSalaryList = new List<SelectListItem>();

                    List<Salary> theSalary = work.SalaryRepository.Get().ToList();
                    theSalaryList.Add(new SelectListItem() { Text = "Select", Value = "" });
                    foreach (var s in theSalary)
                    {
                        theSalaryList.Add(new SelectListItem() { Text = s.SalaryDescription + "  Amount " + s.Amount, Value = Convert.ToString(s.Amount) });
                    }
                    ViewData["salary"] = theSalaryList;

                    StaffViewModel theStaff = new StaffViewModel();

                    List<Deduction> theDeductions = work.DeductionRepository.Get().ToList();
                    theStaff.Deductions = theDeductions;
                    theStaff.PrimarySchoolStaff = model;

                    return View(theStaff);
                    // return View(model);
                }

                model.ClassTeacherOf = model.ClassTeacherOf + "-" + classteacher1 + "-" + classteacher2 + "-" + classteacher3 + "-" + classteacher4;
                // TODO: Add insert logic here
                model.IsApproved = false;
                model.EnrollmentDate = DateTime.Now;



                // model .da.DateEntered = DateTime.Today;
                //  model.DateApproved = DateTime.Now;
                work.PrimarySchoolStaffRepository.Insert(model);

                work.Save();
                return RedirectToAction("Create", "Photo", new { id = SchoolManagement.Models.Encript.EncryptString(model.PersonID.ToString(), true) });
                //   return RedirectToAction("Create", "Photo", new { id = model.PersonID });

            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /PrimarySchoolStaff/Edit/5


        [DynamicAuthorize]
        public ActionResult Edit(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            PrimarySchoolStaff theStudent = work.PrimarySchoolStaffRepository.GetByID(theId);
            string thKeyRole = theStudent.Role;
            string theClasses = theStudent.ClassTeacherOf;

            //StringBuilder theContributionId = new StringBuilder();
            // work.PrimarySchoolStaffRepository.GetByID


            string[] teacherClasses = new string[4];

            teacherClasses = theClasses.Split('-');

            List<string> theteacherClasses = teacherClasses.ToList();

            int counter = theteacherClasses.Count();

            if (counter == 1)
            {
                theteacherClasses.Add("Not Applicable");
                theteacherClasses.Add("Not Applicable");
                theteacherClasses.Add("Not Applicable");
                theteacherClasses.Add("Not Applicable");

            }

            if (counter == 2)
            {

                theteacherClasses.Add("Not Applicable");
                theteacherClasses.Add("Not Applicable");
                theteacherClasses.Add("Not Applicable");
            }



            if (counter == 3)
            {


                theteacherClasses.Add("Not Applicable");
                theteacherClasses.Add("Not Applicable");
            }


            if (counter == 4)
            {


                theteacherClasses.Add("Not Applicable");
            }

            teacherClasses = theteacherClasses.ToArray();


            List<SelectListItem> theItem3 = new List<SelectListItem>();
            List<MyRole> theRoles = work.MyRoleRepository.Get().ToList();

            theItem3.Add(new SelectListItem() { Text = thKeyRole, Value = thKeyRole });


            foreach (var role in theRoles)
            {

                if (role.RoleName != null && role.RoleName != "Student")
                {
                    string thRole = role.RoleName.Trim();
                    //theItem3.Add(new SelectListItem() { Text = role.RoleName, Value = role.RoleName });
                    theItem3.Add(new SelectListItem() { Text = thRole, Value = thRole });
                    // }
                }

            }
            ViewBag.Role = theItem3;
            // ViewData["Role"] = theItem3;

            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();

            List<SelectListItem> class1 = new List<SelectListItem>();
            List<SelectListItem> class2 = new List<SelectListItem>();
            List<SelectListItem> class3 = new List<SelectListItem>();
            List<SelectListItem> class4 = new List<SelectListItem>();

            theItem.Add(new SelectListItem() { Text = teacherClasses[0], Value = teacherClasses[0] });
            theItem.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });

            class1.Add(new SelectListItem() { Text = teacherClasses[1], Value = teacherClasses[1] });
            class1.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });

            class2.Add(new SelectListItem() { Text = teacherClasses[2], Value = teacherClasses[1] });
            class2.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });

            class3.Add(new SelectListItem() { Text = teacherClasses[3], Value = teacherClasses[3] });
            class3.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });

            class4.Add(new SelectListItem() { Text = teacherClasses[4], Value = teacherClasses[4] });
            class4.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });


            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }

            ViewData["Level"] = theItem;

            foreach (var level in theLevels)
            {
                class1.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            ViewData["Level1"] = class1;
            foreach (var level in theLevels)
            {
                class2.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            ViewData["Level2"] = class2;
            foreach (var level in theLevels)
            {
                class3.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            ViewData["Level3"] = class3;
            foreach (var level in theLevels)
            {
                class4.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            ViewData["Level4"] = class4;

            List<SelectListItem> theSubjectList = new List<SelectListItem>();

            List<Subject> theSubject = work.SubjectRepository.Get().ToList();
            theSubjectList.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });
            foreach (var subject in theSubject)
            {
                theSubjectList.Add(new SelectListItem() { Text = subject.Name, Value = subject.Name });
            }

            ViewData["Subject"] = theSubjectList;



            //List<LatenessDeduction> theLatenessDeduction = work.LatenessDeductionRepository.Get().ToList();
            List<SelectListItem> theLatenessDeductionListItem = new List<SelectListItem>();

            theLatenessDeductionListItem.Add(new SelectListItem() { Text = "None", Value = "" });
            theLatenessDeductionListItem.Add(new SelectListItem() { Text = "YES", Value = "true" });
            theLatenessDeductionListItem.Add(new SelectListItem() { Text = "NO", Value = "false" });

            ViewData["LatenessDeduction"] = theLatenessDeductionListItem;

            List<SelectListItem> theAbscentDeductionListItem = new List<SelectListItem>();

            theAbscentDeductionListItem.Add(new SelectListItem() { Text = "None", Value = "" });
            theAbscentDeductionListItem.Add(new SelectListItem() { Text = "YES", Value = "true" });
            theAbscentDeductionListItem.Add(new SelectListItem() { Text = "NO", Value = "false" });

            ViewData["AbscentDeduction"] = theAbscentDeductionListItem;


            //salary
            List<SelectListItem> theSalaryList = new List<SelectListItem>();

            List<Salary> theSalary = work.SalaryRepository.Get().ToList();
            theSalaryList.Add(new SelectListItem() { Text = "Select", Value = "" });
            foreach (var s in theSalary)
            {
                theSalaryList.Add(new SelectListItem() { Text = s.SalaryDescription + "  Amount(NGN) " + s.Amount, Value = Convert.ToString(s.SalaryID) });
            }
            ViewData["salary"] = theSalaryList;
            StaffViewModel theStaff = new StaffViewModel();

            List<Deduction> theDeductions = work.DeductionRepository.Get().ToList();
            theStaff.Deductions = theDeductions;
            theStaff.PrimarySchoolStaff = theStudent;

            string[] theContributionId; //= theStudent.ContributionIDs;
            List<Deduction> theDeductiong = new List<Deduction>();
            if (!(string.IsNullOrEmpty(theStudent.ContributionIDs)))
            {

                theContributionId = theStudent.ContributionIDs.Split(' ');

                foreach (var c in theContributionId)
                {
                    if (!(string.IsNullOrEmpty(c)))
                    {
                        int theContributionID = Convert.ToInt16(c);

                        Deduction thed = work.DeductionRepository.GetByID(theContributionID);
                        if (thed != null)
                        {
                            thed.Selected = true;
                            theDeductiong.Add(thed);
                        }
                    }

                }
            }
            List<Deduction> allSubscribedDeductions = new List<Deduction>();
            List<Deduction> allNonSubscribedandSubDeductions = new List<Deduction>();
            List<Deduction> allDeductions = work.DeductionRepository.Get().ToList();

            List<Deduction> allNonSubscribedDeductions = work.DeductionRepository.Get().ToList();

            foreach (var d in theDeductiong)
            {
                allNonSubscribedDeductions = allNonSubscribedDeductions.Where(a => a.DeductionID != d.DeductionID).ToList();

            }
            allNonSubscribedandSubDeductions.AddRange(theDeductiong);
            allNonSubscribedandSubDeductions.AddRange(allNonSubscribedDeductions);

            theStaff.Deductions = allNonSubscribedandSubDeductions;
            ViewBag.Staff = 5000000 + theStudent.PersonID;


            return View(theStaff);
            // return View();
        }

        //
        // POST: /PrimarySchoolStaff/Edit/5

        [HttpPost]
        public ActionResult Edit(StaffViewModel viewmodel, string classteacher1, string classteacher2, string classteacher3, string classteacher4)
        {
            PrimarySchoolStaff model = viewmodel.PrimarySchoolStaff;
            try
            {
                // TODO: Add update logic here


               // model.EnrollmentDate = DateTime.Now;
                StringBuilder theContributionId = new StringBuilder();
                if (viewmodel.Deductions.Count > 0)
                {
                    List<Deduction> theDec = new List<Deduction>();
                    foreach (Deduction d in viewmodel.Deductions)
                    {
                        if (d.Selected == true)
                        {
                            // List<PrimarySchoolStaff> theS = new List<PrimarySchoolStaff>();
                            // theS.Add(model);
                            //  d.ThePrimarySchoolStaff = theS;
                            theContributionId.Append(d.DeductionID);
                            theContributionId.Append(' ');
                            theDec.Add(d);
                        }
                    }
                    model.ContributionIDs = theContributionId.ToString();
                    model.TheDeduction = theDec;
                    // theDec[0].
                    // TheDeduction[0].
                    //  model.TheDeduction
                }

                //  PrimarySchoolStaff model = viewmodel.PrimarySchoolStaff;
                if (ModelState.IsValid)
                {
                    model.ClassTeacherOf = model.ClassTeacherOf + "-" + classteacher1 + "-" + classteacher2 + "-" + classteacher3 + "-" + classteacher4;
                    // PrimarySchoolStudent theStudent =   work.PrimarySchoolStudentRepository.GetByID(model.PersonID);

                    if (model.IsApproved == true)
                    {
                        MembershipUser user = Membership.GetUser(Convert.ToString(5000000 + model.PersonID), false); ;
                        if (user == null)
                        {
                            //if (Membership.FindUsersByName(Convert.ToString(5000000 + model.PersonID)) == null)
                            //{
                            model.UserID = 5000000 + model.PersonID;
                            // model.level
                            model.DateApproved = DateTime.Now;
                            model.IsApproved = true;

                            string password = PaddPassword.Padd(model.FirstName.ToLower() + model.Middle.ToLower() + model.LastName.ToLower());
                            Membership.CreateUser(model.UserID.ToString(), password, model.Email);
                            // var user = _userService.Get(id);
                            Tweaker.AdjustTimer(model.UserID.ToString());
                            Roles.AddUserToRole(model.UserID.ToString(), model.Role);
                        }
                        else
                        {
                            PrimarySchoolStaff staff = work2.PrimarySchoolStaffRepository.GetByID(model.PersonID);
                            string[] RoleList = Roles.GetAllRoles();
                            //  Roles.RemoveUserFromRoles(model.UserID.ToString(), RoleList);
                            foreach (var role in RoleList)
                            {
                                if (Roles.IsUserInRole(model.UserID.ToString(), role))
                                {
                                    Roles.RemoveUserFromRole(model.UserID.ToString(), role);
                                }
                            }
                            Roles.AddUserToRole(model.UserID.ToString(), model.Role);
                            work.PrimarySchoolStaffRepository.Update(model);

                            Tweaker.AdjustTimer(model.UserID.ToString());
                        }
                    }
                    //foreach (Deduction d in model.TheDeduction)
                    //{
                    //    work.DeductionRepository.Update(d);
                    //}

                    // work.DeductionRepository.
                    work.PrimarySchoolStaffRepository.Update(model);

                    work.Save();
                    if (User.Identity.Name != "5000001")
                    {
                        AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Staff Information Updated,  Staff ID:-" + model.UserID, UserID = User.Identity.Name };
                        work.AuditTrailRepository.Insert(audit);
                        work.Save();
                    }

                }

                return RedirectToAction("Index");
            }
            catch
            {
                PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(model.PersonID);
                string thKeyRole = theStudent.Role;



                List<SelectListItem> theItem3 = new List<SelectListItem>();
                List<MyRole> theRoles = work.MyRoleRepository.Get().ToList();

                theItem3.Add(new SelectListItem() { Text = thKeyRole, Value = thKeyRole });


                foreach (var role in theRoles)
                {
                    //  if (theStudent.Role.Equals(role.RoleName))
                    //  {
                    //     theItem3.Add(new SelectListItem() { Selected = true, Text = role.RoleName, Value = role.RoleName });
                    // }
                    // else
                    // {

                    if (role.RoleName != null && role.RoleName != "Student")
                    {
                        string thRole = role.RoleName.Trim();
                        //theItem3.Add(new SelectListItem() { Text = role.RoleName, Value = role.RoleName });
                        theItem3.Add(new SelectListItem() { Text = thRole, Value = thRole });
                        // }
                    }

                }
                ViewBag.Role = theItem3;
                // ViewData["Role"] = theItem3;

                List<Level> theLevels = work.LevelRepository.Get().ToList();
                List<SelectListItem> theItem = new List<SelectListItem>();
                theItem.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });
                theItem.Add(new SelectListItem() { Text = "Proprietor", Value = "Proprietor" });
                theItem.Add(new SelectListItem() { Text = "Principal", Value = "Principal" });
                theItem.Add(new SelectListItem() { Text = "Bursar", Value = "Bursar" });
                theItem.Add(new SelectListItem() { Text = "Admin Secretary", Value = "Admin Secretary" });
                theItem.Add(new SelectListItem() { Text = "Supervisor", Value = "Supervisor" });
                theItem.Add(new SelectListItem() { Text = "Vice Principal", Value = "Vice Principal" });
                theItem.Add(new SelectListItem() { Text = "Head Teacher", Value = "Head Teache" });
                theItem.Add(new SelectListItem() { Text = "Administrator", Value = "Administrator" });
                theItem.Add(new SelectListItem() { Text = "Store Man", Value = "Store Man" });
                theItem.Add(new SelectListItem() { Text = "Librarian", Value = "Librarian" });
                theItem.Add(new SelectListItem() { Text = "Lab Attendant", Value = "Lab Attendant" });
                theItem.Add(new SelectListItem() { Text = "Subject Teacher", Value = "Subject Teacher" });
                theItem.Add(new SelectListItem() { Text = "Clerk", Value = "Clerk" });
                theItem.Add(new SelectListItem() { Text = "Receptionist", Value = "Receptionist" });
                theItem.Add(new SelectListItem() { Text = "Caregiver", Value = "Caregiver" });
                theItem.Add(new SelectListItem() { Text = "Minder", Value = "Minder" });
                theItem.Add(new SelectListItem() { Text = "Cleaner", Value = "Cleaner" });
                theItem.Add(new SelectListItem() { Text = "Class Assistant", Value = "Class Assistant" });

                foreach (var level in theLevels)
                {
                    theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
                }
                ViewData["Level"] = theItem;

                List<SelectListItem> theSubjectList = new List<SelectListItem>();

                List<Subject> theSubject = work.SubjectRepository.Get().ToList();
                theSubjectList.Add(new SelectListItem() { Text = "Not Applicable", Value = "Not Applicable" });
                foreach (var subject in theSubject)
                {
                    theSubjectList.Add(new SelectListItem() { Text = subject.Name, Value = subject.Name });
                }

                ViewData["Subject"] = theSubjectList;



                //List<LatenessDeduction> theLatenessDeduction = work.LatenessDeductionRepository.Get().ToList();
                List<SelectListItem> theLatenessDeductionListItem = new List<SelectListItem>();

                theLatenessDeductionListItem.Add(new SelectListItem() { Text = "None", Value = "" });
                theLatenessDeductionListItem.Add(new SelectListItem() { Text = "YES", Value = "true" });
                theLatenessDeductionListItem.Add(new SelectListItem() { Text = "NO", Value = "false" });

                ViewData["LatenessDeduction"] = theLatenessDeductionListItem;

                List<SelectListItem> theAbscentDeductionListItem = new List<SelectListItem>();

                theAbscentDeductionListItem.Add(new SelectListItem() { Text = "None", Value = "" });
                theAbscentDeductionListItem.Add(new SelectListItem() { Text = "YES", Value = "true" });
                theAbscentDeductionListItem.Add(new SelectListItem() { Text = "NO", Value = "false" });

                ViewData["AbscentDeduction"] = theAbscentDeductionListItem;


                //salary
                List<SelectListItem> theSalaryList = new List<SelectListItem>();

                List<Salary> theSalary = work.SalaryRepository.Get().ToList();
                theSalaryList.Add(new SelectListItem() { Text = "Select", Value = "" });
                foreach (var s in theSalary)
                {
                    theSalaryList.Add(new SelectListItem() { Text = s.SalaryDescription + "  Amount(NGN) " + s.Amount, Value = Convert.ToString(s.SalaryID) });
                }
                ViewData["salary"] = theSalaryList;
                StaffViewModel theStaff = new StaffViewModel();

                List<Deduction> theDeductions = work.DeductionRepository.Get().ToList();
                theStaff.Deductions = theDeductions;
                return View(theStaff);
            }
        }

        //
        // GET: /PrimarySchoolStaff/Delete/5
        [DynamicAuthorize]
        public ActionResult Delete(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            PrimarySchoolStaff theStaff = work.PrimarySchoolStaffRepository.GetByID(theId);
            return View(theStaff);
        }

        //
        // POST: /PrimarySchoolStaff/Delete/5

        [HttpPost]
        public ActionResult Delete(PrimarySchoolStaff model)
        {
            try
            {
                // TODO: Add delete logic here
                string firstName = model.FirstName;
                string lastName = model.LastName;
                PrimarySchoolStaff theStaff = work.PrimarySchoolStaffRepository.GetByID(model.PersonID);
                work.PrimarySchoolStaffRepository.Delete(theStaff);
                Membership.DeleteUser(Convert.ToString(theStaff.UserID));
                work.Save();

                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Staff was Deleted,  First Name:-" + firstName + " Last Name:-" + lastName, UserID = User.Identity.Name };
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
