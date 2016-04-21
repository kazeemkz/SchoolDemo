using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobHustler.DAL;
using SchoolManagement.Model;
using PagedList;
using SchoolManagement.Models;
using JobHustler.Models;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.UI;

namespace SchoolManagement.Controllers
{
    //  [DynamicAuthorize]
    [Authorize]
    public class ExamController : Controller
    {
        UnitOfWork work = new UnitOfWork();

        [DynamicAuthorize]
        public ViewResult Index(string studentID, string arm, string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page)
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
                //theItem.Add(new SelectListItem() { Text = "Repeat Class", Value = "Repeat Class" });

                theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
                theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
                theItem.Add(new SelectListItem() { Text = "Expelled", Value = "Expelled" });
                theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });
                ViewData["Level"] = theItem;

                ViewData["arm"] = theItem;
            }
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

            // var Students1;
            var students = from s in work.PrimarySchoolStudentRepository.Get()
                           select s;


            students = students.Where(s => s.IsApproved == true);
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstName.ToUpper().Contains(searchString.ToUpper()) || s.Middle.ToUpper().Contains(searchString.ToUpper()));
            }


            if (!String.IsNullOrEmpty(LevelString))
            {
                students = students.Where(s => s.PresentLevel == LevelString);
                //students = students.Where(s => s.PresentLevel.Equals(LevelString));
            }

            if (!String.IsNullOrEmpty(arm))
            {
                students = students.Where(s => s.LevelType == arm);
                //students = students.Where(s => s.PresentLevel.Equals(LevelString));
            }


            if (!String.IsNullOrEmpty(SexString))
            {
                students = students.Where(s => s.Sex.Equals(SexString));
            }

            if (!String.IsNullOrEmpty(StudentIDString))
            {
                int theID = Convert.ToInt32(StudentIDString);
                students = students.Where(s => s.UserID == theID);
            }



            if (!String.IsNullOrEmpty(ApprovedString))
            {
                bool theValue = Convert.ToBoolean(ApprovedString);
                students = students.Where(s => s.IsApproved == theValue);
            }

            int pageSize = 40;
            int pageNumber = (page ?? 1);

            //if (User.IsInRole("Parent"))
            //{
            //    Int32 theUser = Convert.ToInt32(User.Identity.Name);
            //    List<PrimarySchoolStudent> thSchoolStudents;
            //    //  string userid = User.Identity.Name;
            //    List<Parent> theP = work.ParentRepository.Get(a => a.UserID == theUser).ToList();
            //    Parent theRealParent = theP[0];
            //    int idParent = theRealParent.ParentID;
            //    thSchoolStudents = work.PrimarySchoolStudentRepository.Get(a => a.ParentID == idParent).ToList();
            //    List<SelectListItem> theItem = new List<SelectListItem>();
            //    foreach (var s in thSchoolStudents)
            //    {
            //        theItem.Add(new SelectListItem() { Text = s.LastName + " " + s.FirstName, Value = s.PersonID.ToString() });
            //        ViewData["Students"] = theItem;
            //    }
            //}



            if (!(User.IsInRole("SuperAdmin")))
            {


                //  string arm, string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page)
                if (string.IsNullOrEmpty(arm))
                {
                    students = students.Where(a => a.LevelType == "kazoo");
                    ViewBag.Count = students.Count();
                    return View(students.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    ViewBag.Count = students.Count();
                    return View(students.ToPagedList(pageNumber, pageSize));
                }
                //return View(students.ToPagedList(pageNumber, pageSize));
                //  }

            }
            if (User.IsInRole("SuperAdmin"))
            {
                ViewBag.Count = students.Count();
                return View(students.ToPagedList(pageNumber, pageSize));
            }
            return View();
        }


        // GET: /Exam/Details/5

        //  { studentID =Model.StudentID, Term = Model.Term, studentLevel = Model.StudentIDInt, studentName = Model.StudentName })


        public FileStreamResult PrintResult(string studentName, string Term, string studentLevel)
        {
            StringWriter oStringWriter1 = new StringWriter();

            Document itextDoc = new Document(PageSize.LETTER);
            itextDoc.Open();

            // oStringWriter.Write("This is the content");
            //  Response.ContentType = "text/plain";
            Response.ContentType = "application/pdf";


            Int32 studentUserID = Convert.ToInt32(studentName);

            Person thePerson = work.PersonRepository.Get(a => a.UserID == studentUserID).First();

            if (thePerson is SecondarySchoolStudent)
            {
                //http://www.bluelemoncode.com/post/2011/12/06/Using-iTextSharp-with-aspnet-to-add-header-in-pdf-file.aspx
                PrintResult print = new PrintResult();
                // Set up the document and the MS to write it to and create the PDF writer instance
                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.A3.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                // Open the PDF document
                document.Open();


                // Set up fonts used in the document
                Font font_heading_1 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 19, Font.BOLD);
                Font font_body = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9);

                // Create the heading paragraph with the headig font
                // Paragraph paragraph;
                // paragraph = new Paragraph("Hello world!", font_heading_1);
                Document thedoc = print.PrinttheResult(studentName, Term, studentLevel, ref oStringWriter1, ref document);
                // Add a horizontal line below the headig text and add it to the paragraph
                iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
                seperator.Offset = -6f;

                document.Close();

                // Hat tip to David for his code on stackoverflow for this bit
                // http://stackoverflow.com/questions/779430/asp-net-mvc-how-to-get-view-to-generate-pdf
                byte[] file = ms.ToArray();
                MemoryStream output = new MemoryStream();
                output.Write(file, 0, file.Length);
                output.Position = 0;

                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=form.pdf");
                return new FileStreamResult(output, "application/pdf"); //new FileStreamResult(output, "application/pdf");
            }
            else
            {
                //http://www.bluelemoncode.com/post/2011/12/06/Using-iTextSharp-with-aspnet-to-add-header-in-pdf-file.aspx
                PrintResult print = new PrintResult();
                // Set up the document and the MS to write it to and create the PDF writer instance
                MemoryStream ms = new MemoryStream();
                Document document = new Document(PageSize.A3.Rotate());
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                // Open the PDF document
                document.Open();


                // Set up fonts used in the document
                Font font_heading_1 = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 19, Font.BOLD);
                Font font_body = FontFactory.GetFont(FontFactory.TIMES_ROMAN, 9);
                Document thedoc = print.PrinttheResultPrimary(studentName, Term, studentLevel, ref oStringWriter1, ref document);
                // Add a horizontal line below the headig text and add it to the paragraph
                iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
                seperator.Offset = -6f;

                document.Close();

                // Hat tip to David for his code on stackoverflow for this bit
                // http://stackoverflow.com/questions/779430/asp-net-mvc-how-to-get-view-to-generate-pdf
                byte[] file = ms.ToArray();
                MemoryStream output = new MemoryStream();
                output.Write(file, 0, file.Length);
                output.Position = 0;

                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=form.pdf");
                return new FileStreamResult(output, "application/pdf"); //new FileStreamResult(output, "application/pdf");

            }

            //  return View();

        }


        [DynamicAuthorize]
        public ActionResult Details(string ids, string term, string level)
        {
            Int32 id = Convert.ToInt32(Models.Encript.DecryptString(ids, true));
            PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(id);
            //string theLevel1 = theStudent.PresentLevel;
            string theLevel1 = level;
            //  List<SubjectRegistration> theAssignedSubjects1 = work.SubjectRegistrationRepository.Get(a => a.Level.Equals(theLevel1)).OrderBy(a => a.Level).ToList();
            // if (theAssignedSubjects1.Count == 0)
            // {
            ExamSubjectReg theReg = new ExamSubjectReg();
            theReg.StudentName = theStudent.UserID;
            theReg.Term = term;
            theReg.StudentIDInt = id;
            //  theReg
          //  ViewBag.ID = Models.Encript.DecryptString(Convert.ToString( id),true);
            ViewBag.ID = id;
            //  theReg.
            // ModelState.AddModelError("", "Create a Class Subjects First for " + theStudent.PresentLevel);
            return View("Details1", theReg);
            // }
            //SubjectRegistration theRealSub = theAssignedSubjects1[0];

        }

        //
        // GET: /Exam/Create
        // [Authorize(Role="Student")]
        [DynamicAuthorize]
        public ActionResult Create(string studentID, string level, string term)
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
            int userID = 0; //=// Convert.ToInt32(User.Identity.Name);
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
                        userID = theStudents.UserID;
                        // work.PrimarySchoolStudentRepository.Get(a=>a.UserID == ids).First();
                    }
                    else
                    {
                        userID = 0;
                    }

                }
                else
                {
                    userID = 0;
                }
            }
            else
            {
                userID = Convert.ToInt32(User.Identity.Name);

            }

            //userID = Convert.ToInt32(User.Identity.Name);
            List<PrimarySchoolStudent> theStudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID == userID).ToList();
            List<Exam> theExam = null;
            if (level == null || term == null)
            {

            }
            else
            {

                List<Exam2> theExams2 = work.Exam2Repository.Get(a => a.StudentCode == userID && a.Term.Equals(term) && a.Level.Equals(level)).ToList();
                if (theExams2.Count() > 0)
                {



                    List<Exam> theExamses = work.ExamRepository.Get(a => a.StudentCode == userID && a.Term.Equals(term) && a.Level.Equals(level)).ToList();
                    List<Exam1> theExams1 = work.Exam1Repository.Get(a => a.StudentCode == userID && a.Term.Equals(term) && a.Level.Equals(level)).ToList();

                    // List<Exam2> theExams2 = work.Exam2Repository.Get(a => a.StudentCode == 2000001 && a.Term.Equals(term) && a.Level.Equals(level)).ToList();

                    List<Subject> FailedSubjest2 = new List<Subject>();


                    List<Subject> FailedSubjest1 = new List<Subject>();

                    List<Subject> theList = new List<Subject>();
                    foreach (var sun in theExamses)
                    {
                        //work
                        theList.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                    }


                    ExamSubjectReg ExamSubReg1 = new ExamSubjectReg();



                    foreach (var sun in theExams2)
                    {
                        //work
                        FailedSubjest2.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                    }


                    foreach (var sun in theExams1)
                    {
                        //work
                        FailedSubjest1.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                    }

                    // theList.co;
                    ExamSubReg1.StudentID = theStudent[0].LastName;
                    ExamSubReg1.FailedSubjects = FailedSubjest1;
                    ExamSubReg1.FailedSubjects2 = FailedSubjest2;
                    // ExamSubReg.TheExam = theExam;
                    ExamSubReg1.TheSubjects = theList;
                    ExamSubReg1.Level = level; //theStudent.PresentLevel;
                    ExamSubReg1.StudentName = theStudent[0].UserID;
                    ExamSubReg1.StudentClassType = theStudent[0].LevelType;
                    ExamSubReg1.Term = term;
                    return View(ExamSubReg1);
                }

                List<Exam1> theExams1For1 = work.Exam1Repository.Get(a => a.StudentCode == userID && a.Term.Equals(term) && a.Level.Equals(level)).ToList();
                if (theExams1For1.Count() > 0)
                {
                    List<Exam> theExamses = work.ExamRepository.Get(a => a.StudentCode == userID && a.Term.Equals(term) && a.Level.Equals(level)).ToList();
                    List<Exam1> theExams1 = work.Exam1Repository.Get(a => a.StudentCode == userID && a.Term.Equals(term) && a.Level.Equals(level)).ToList();

                    // List<Exam2> theExams2 = work.Exam2Repository.Get(a => a.StudentCode == 2000001 && a.Term.Equals(term) && a.Level.Equals(level)).ToList();

                    //  List<Subject> FailedSubjest2 = new List<Subject>();


                    List<Subject> FailedSubjest1 = new List<Subject>();

                    List<Subject> theList = new List<Subject>();
                    foreach (var sun in theExamses)
                    {
                        //work
                        theList.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                    }


                    ExamSubjectReg ExamSubReg1 = new ExamSubjectReg();
                    foreach (var sun in theExams1)
                    {
                        //work
                        FailedSubjest1.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                    }

                    // theList.co;
                    ExamSubReg1.StudentID = theStudent[0].LastName;
                    ExamSubReg1.FailedSubjects = FailedSubjest1;
                    // ExamSubReg1.FailedSubjects2 = FailedSubjest1;
                    // ExamSubReg.TheExam = theExam;
                    ExamSubReg1.TheSubjects = theList;
                    ExamSubReg1.Level = level; //theStudent.PresentLevel;
                    ExamSubReg1.StudentName = theStudent[0].UserID;
                    ExamSubReg1.StudentClassType = theStudent[0].LevelType;
                    ExamSubReg1.Term = term;
                    // ExamSubReg1.Repeat = theStudent.RepeatTimes;
                    // ViewBag.tracker = tracker;
                    // ExamSubReg.TheSubjectRegistration = SubjectRegistrationToUpdate;


                    return View(ExamSubReg1);



                }
                else
                {
                    List<Exam> theExamses = work.ExamRepository.Get(a => a.StudentCode == userID && a.Term.Equals(term) && a.Level.Equals(level)).ToList();

                    List<Subject> theList = new List<Subject>();
                    foreach (var sun in theExamses)
                    {
                        //work
                        theList.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                    }


                    ExamSubjectReg ExamSubReg1 = new ExamSubjectReg();


                    // theList.co;
                    ExamSubReg1.StudentID = theStudent[0].LastName;
                    // ExamSubReg1.FailedSubjects = FailedSubjest2;
                    //  ExamSubReg1.FailedSubjects2 = FailedSubjest1;
                    // ExamSubReg.TheExam = theExam;
                    ExamSubReg1.TheSubjects = theList;
                    ExamSubReg1.Level = level; //theStudent.PresentLevel;
                    ExamSubReg1.StudentName = theStudent[0].UserID;
                    ExamSubReg1.StudentClassType = theStudent[0].LevelType;
                    ExamSubReg1.Term = term;
                    // ExamSubReg1.Repeat = theStudent.RepeatTimes;
                    // ViewBag.tracker = tracker;
                    // ExamSubReg.TheSubjectRegistration = SubjectRegistrationToUpdate;


                    return View(ExamSubReg1);






                    // theExam = work.ExamRepository.Get(a => a.Level.Equals(level) && a.Term.Equals(term) && a.StudentCode == 2000001).ToList();
                    // return View(theExam);
                }


            }
            return View();

        }

        //
        // POST: /Exam/Create

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
        [HttpPost]
        [Authorize]
        public ActionResult PromotEditStudent(string level, Int32 StudentName)
        {
            try
            {
                if (!(string.IsNullOrEmpty(level)))// != "")
                {
                    if (level == "Repeat Class")
                    {
                        List<PrimarySchoolStudent> theStudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID == StudentName).ToList();
                        PrimarySchoolStudent theMainStudent = theStudent[0];
                        // if (theMainStudent.RepeatTimes == null)
                        //  theMainStudent.RepeatTimes = 0;
                        theMainStudent.RepeatTimes = theMainStudent.RepeatTimes + 1;
                        // theMainStudent.LevelType = level;
                        work.PrimarySchoolStudentRepository.Update(theMainStudent);
                        UnitOfWork work2 = new UnitOfWork();
                      //  List<SchoolFeePayment> theFee1 = work.SchoolFeePaymentRepository.Get(a => a.Level.Equals(theMainStudent.PresentLevel)).ToList();
                      //  foreach (var fee in theFee1)
                      //  {
                         //   work2.SchoolFeePaymentRepository.Delete(fee);
                       // }
                      //  work2.Save();
                    }
                    else
                    {
                        string[] spltedStuff = level.Split(':');
                        List<PrimarySchoolStudent> theStudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID == StudentName).ToList();
                        PrimarySchoolStudent theMainStudent = theStudent[0];
                        if (spltedStuff.Count() == 2)
                        {
                            theMainStudent.PresentLevel = spltedStuff[0];
                            theMainStudent.LevelType = level;
                        }
                        else
                        {

                        }

                        theMainStudent.RepeatTimes = 0;
                        /// theMainStudent.LevelType = level;
                        work.PrimarySchoolStudentRepository.Update(theMainStudent);
                    }
                    work.Save();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch
            {

                return View();
            }

            //   retu
        }
        //
        // GET: /Exam/Edit/5
        [DynamicAuthorize]
        public ActionResult Edit(string ids, string term, string Level, ExamSubjectReg Model)
        {
            Int32 id = 0;
            if (!(string.IsNullOrEmpty(ids)))
            {

                 id = Convert.ToInt32(Models.Encript.DecryptString(ids, true));
            }
            ViewBag.Session = null;
            //disallow class teachers from viewing other classes
            if (User.IsInRole("Subject Teacher") || User.IsInRole("Class Teacher"))
            {
                PrimarySchoolStudent thStudent;
                string classType = "ka";

                if (ids != null)
                {
                    thStudent = work.PrimarySchoolStudentRepository.GetByID(id);
                    if (thStudent == null)
                    {
                        classType = "ka";
                    }
                    else
                    {
                        classType = thStudent.LevelType;
                    }

                }
                string theUserName = User.Identity.Name;
                Int32 theUserName1 = Convert.ToInt32(theUserName);
                PrimarySchoolStaff theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == theUserName1).First();
                string theTeachersClass = theStaff.ClassTeacherOf;
                string[] splitClasses = theTeachersClass.Split('-');
                List<string> listOfClasses = new List<string>();
                listOfClasses = splitClasses.ToList();
                bool itThere = false;
                foreach (var c in listOfClasses)
                {
                    if (c == classType)
                    {
                        itThere = true;
                    }
                }
                if (itThere == false)
                {
                    return View("Index");
                }

                //  ViewData["arm"] = theItemClasses;
            }


            if (string.IsNullOrEmpty(term))
            {
                term = Model.Term;
            }

            //promotion section
            List<Level> theLevels = work.LevelRepository.Get().OrderBy(a => a.LevelName).ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();

            theItem.Add(new SelectListItem() { Text = "Choose..", Value = "" });



            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            theItem.Add(new SelectListItem() { Text = "Repeat Class", Value = "Repeat Class" });

            theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
            theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
            theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });
            ViewData["Level"] = theItem;





            string Theterm = term;
            PrimarySchoolStudent theStudent = new PrimarySchoolStudent();
            string theLevel1 = "";
            if (Model.StudentIDInt != 0)
            {
                //if we are looking at repeat records and we find a student record, then display the repeat record
                theStudent = work.PrimarySchoolStudentRepository.GetByID(Model.StudentIDInt);

                theLevel1 = Model.Level;
                List<Exam1> theExams1 = work.Exam1Repository.Get(a => a.StudentCode == theStudent.UserID && a.Term.Equals(Theterm) && a.Level.Equals(theLevel1)).ToList();
                List<Exam> theExamses = work.ExamRepository.Get(a => a.StudentCode == theStudent.UserID && a.Term.Equals(Theterm) && a.Level.Equals(theLevel1)).ToList();
                if (theExams1.Count() > 0)
                {
                    theStudent.RepeatTimes = 1;
                }
                List<Exam2> theExams2 = work.Exam2Repository.Get(a => a.StudentCode == theStudent.UserID && a.Term.Equals(Theterm) && a.Level.Equals(theLevel1)).ToList();
                if (theExams2.Count() > 0)
                {
                    theStudent.RepeatTimes = 2;
                }
            }
            else
            {
                theStudent = work.PrimarySchoolStudentRepository.GetByID(id);

                theLevel1 = theStudent.PresentLevel;
            }
            //string term = "";
            // try
            // {
            List<SubjectRegistration> theAssignedSubjects1 = work.SubjectRegistrationRepository.Get(a => a.Level.Equals(theLevel1)).OrderBy(a => a.Level).ToList();
            //}
            // catch()

            if (theAssignedSubjects1.Count == 0)
            {
                ExamSubjectReg theReg = new ExamSubjectReg();
                theReg.StudentName = theStudent.UserID;
                theReg.Term = Theterm;
                //  theReg.
                ModelState.AddModelError("", "Create a Class Subjects First for Class " + theStudent.PresentLevel);
                return View(theReg);
            }
            SubjectRegistration theRealSub = theAssignedSubjects1[0];

            //get the registered classes for this guy
            SubjectRegistration SubjectRegistrationToUpdate1 = work.SubjectRegistrationRepository.GetByID(theRealSub.SubjectRegistrationID);

            List<Subject> theList = new List<Subject>();

            List<Exam> theExams = work.ExamRepository.Get(a => a.StudentCode == theStudent.UserID && a.Term.Equals(Theterm) && a.Level.Equals(theLevel1)).ToList();
            string theSession = null;
            if (theExams.Count != 0 && theStudent.RepeatTimes == 0)
            {
                int counter = 1;
                foreach (var sun in theExams)
                {
                    if (counter == 1)
                    {
                        if ((String.IsNullOrEmpty(sun.Session)))
                        {
                            List<Term> theTerm = work.TermRepository.Get().ToList();
                            if (theTerm.Count() == 1)
                            {
                                theSession = theTerm[0].SessionStartYear + "/" + theTerm[0].SessionEndYear;
                            }
                        }
                        else
                        {
                            theSession = sun.Session;
                        }
                        counter = 2;
                    }
                    //work
                    theList.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                }
                ExamSubjectReg ExamSubReg1 = new ExamSubjectReg();

                foreach (var t in SubjectRegistrationToUpdate1.Subjects)
                {
                    theList.Add(t);
                }

                var List = theList.GroupBy(i => i.Name).Select(q => q.First()).ToList();

                // theList.co;
                ExamSubReg1.StudentID = theStudent.LastName;
                // ExamSubReg.TheExam = theExam;
                ExamSubReg1.TheSubjects = List;
                ExamSubReg1.Level = theLevel1; //theStudent.PresentLevel;
                ExamSubReg1.StudentName = theStudent.UserID;
                ExamSubReg1.StudentClassType = theStudent.LevelType;
                ExamSubReg1.Term = Theterm;
                ExamSubReg1.Repeat = theStudent.RepeatTimes;
                ViewBag.Session = theSession;
                // ViewBag.tracker = tracker;
                // ExamSubReg.TheSubjectRegistration = SubjectRegistrationToUpdate;

                return View(ExamSubReg1);
            }
            if (theExams.Count == 0 && theStudent.RepeatTimes == 0)
            //  else
            {

                string theLevel = theLevel1;// theStudent.PresentLevel;
                List<SubjectRegistration> theAssignedSubjects = work.SubjectRegistrationRepository.Get(a => a.Level.Equals(theLevel)).OrderBy(a => a.Level).ToList();
                SubjectRegistration thAS = theAssignedSubjects[0];
                SubjectRegistration SubjectRegistrationToUpdate = work.SubjectRegistrationRepository.GetByID(thAS.SubjectRegistrationID);
                //   List<Exam> theExam = work.ExamRepository.Get(a => a.Level.Equals(theLevel) || a.Term == term).OrderBy(a => a.SubjectName).ToList();
                Exam theExam = new Exam();
                foreach (var sun in SubjectRegistrationToUpdate.Subjects)
                {
                    // theSession = sun.Session;
                    theList.Add(sun);
                }
                ExamSubjectReg ExamSubReg = new ExamSubjectReg();
                ExamSubReg.StudentID = theStudent.LastName;
                // ExamSubReg.TheExam = theExam;
                ExamSubReg.TheSubjects = theList;
                ExamSubReg.Level = theLevel1;//theStudent.PresentLevel;
                ExamSubReg.StudentName = theStudent.UserID;
                ExamSubReg.StudentClassType = theStudent.LevelType;
                ExamSubReg.Term = Theterm;
                ExamSubReg.Repeat = theStudent.RepeatTimes;
                // ViewBag.tracker = tracker;
                ExamSubReg.TheSubjectRegistration = SubjectRegistrationToUpdate;

                List<Term> theTerm = work.TermRepository.Get().ToList();
                if (theTerm.Count() == 1)
                {
                    ViewBag.Session = theTerm[0].SessionStartYear + "/" + theTerm[0].SessionEndYear;
                }
                //  .Session = 



                return View(ExamSubReg);
            }

            if (theExams.Count >= 0 && theStudent.RepeatTimes == 1)
            {
                string theSessionRepeatOne = null;
                List<Exam> theExamses = work.ExamRepository.Get(a => a.StudentCode == theStudent.UserID && a.Term.Equals(Theterm) && a.Level.Equals(theLevel1)).ToList();
                List<Exam1> theExams1 = work.Exam1Repository.Get(a => a.StudentCode == theStudent.UserID && a.Term.Equals(Theterm) && a.Level.Equals(theLevel1)).ToList();

                foreach (var sun in theExamses)
                {
                    theSessionRepeatOne = sun.Session;
                    //work
                    theList.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                }
                ExamSubjectReg ExamSubReg1 = new ExamSubjectReg();

                //foreach (var t in SubjectRegistrationToUpdate1.Subjects)
                //{
                //    theList.Add(t);
                //}

                //var List = theList.GroupBy(i => i.Name).Select(q => q.First()).ToList();
                List<Subject> FailedSubjects = new List<Subject>();
                string terms = null;
                foreach (var t in theExams1)
                {
                    terms  = t.Session;
                    ViewBag.SessionFailed1 = t.Session;
                    FailedSubjects.Add(new Subject { FirstCA = t.FirstCA, SecondCA = t.SecondCA, SubjectExam = t.SubjectExam, Name = t.SubjectName, Total = t.FirstCA + t.SecondCA + t.SubjectExam });
                }

                List<Term> theTerm = work.TermRepository.Get().ToList();
                if (theTerm.Count() == 1 && (String.IsNullOrEmpty(terms)))
                {
                    ViewBag.SessionFailed1 = theTerm[0].SessionStartYear + "/" + theTerm[0].SessionEndYear;
                }

                foreach (var t in SubjectRegistrationToUpdate1.Subjects)
                {
                    FailedSubjects.Add(t);
                }
                var FailedSubjectss = FailedSubjects.GroupBy(i => i.Name).Select(q => q.First()).ToList();
                // theList.co;
                ExamSubReg1.StudentID = theStudent.LastName;
                // ExamSubReg.TheExam = theExam;
                ExamSubReg1.TheSubjects = theList;
                ExamSubReg1.Level = theLevel1; //theStudent.PresentLevel;
                ExamSubReg1.StudentName = theStudent.UserID;
                ExamSubReg1.StudentClassType = theStudent.LevelType;
                ExamSubReg1.Term = Theterm;
                ExamSubReg1.Repeat = theStudent.RepeatTimes;
                ExamSubReg1.FailedSubjects = FailedSubjectss;
                // ViewBag.tracker = tracker;
                // ExamSubReg.TheSubjectRegistration = SubjectRegistrationToUpdate;
                ViewBag.Session = theSessionRepeatOne;
                return View("Edit1", ExamSubReg1);

            }

            if (theExams.Count >= 0 && theStudent.RepeatTimes == 2)
            {
                List<Exam> theExamses = work.ExamRepository.Get(a => a.StudentCode == theStudent.UserID && a.Term.Equals(Theterm) && a.Level.Equals(theLevel1)).ToList();
                List<Exam1> theExams1 = work.Exam1Repository.Get(a => a.StudentCode == theStudent.UserID && a.Term.Equals(Theterm) && a.Level.Equals(theLevel1)).ToList();

                List<Exam2> theExams2 = work.Exam2Repository.Get(a => a.StudentCode == theStudent.UserID && a.Term.Equals(Theterm) && a.Level.Equals(theLevel1)).ToList();

                List<Subject> FailedSubjest = new List<Subject>();


                List<Subject> FailedSubjest1 = new List<Subject>();

                foreach (var sun in theExamses)
                {
                    //work
                    ViewBag.Session = sun.Session;
                    theList.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                }
                ExamSubjectReg ExamSubReg1 = new ExamSubjectReg();

                //foreach (var t in SubjectRegistrationToUpdate1.Subjects)
                //{
                //    theList.Add(t);
                //}

                //var List = theList.GroupBy(i => i.Name).Select(q => q.First()).ToList();

                foreach (var sun in theExams1)
                {
                    //work
                    ViewBag.SessionFailed1 = sun.Session;
                    FailedSubjest.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                }

                string termss = null;
                foreach (var sun in theExams2)
                {
                    termss = sun.Session;
                    //work
                    ViewBag.SessionFailed2 = sun.Session;
                    FailedSubjest1.Add(new Subject { FirstCA = sun.FirstCA, SecondCA = sun.SecondCA, SubjectExam = sun.SubjectExam, Name = sun.SubjectName, Total = sun.FirstCA + sun.SecondCA + sun.SubjectExam });

                }
                List<Term> theTerm = work.TermRepository.Get().ToList();
                if (theTerm.Count() == 1 && (String.IsNullOrEmpty(termss)))
                {
                    ViewBag.SessionFailed2 = theTerm[0].SessionStartYear + "/" + theTerm[0].SessionEndYear;
                }

                foreach (var t in SubjectRegistrationToUpdate1.Subjects)
                {
                    FailedSubjest1.Add(t);
                }
                var FailedSubjectss = FailedSubjest1.GroupBy(i => i.Name).Select(q => q.First()).ToList();

                // theList.co;
                ExamSubReg1.StudentID = theStudent.LastName;
                ExamSubReg1.FailedSubjects = FailedSubjest;
                ExamSubReg1.FailedSubjects2 = FailedSubjectss;
                // ExamSubReg.TheExam = theExam;
                ExamSubReg1.TheSubjects = theList;
                ExamSubReg1.Level = theLevel1; //theStudent.PresentLevel;
                ExamSubReg1.StudentName = theStudent.UserID;
                ExamSubReg1.StudentClassType = theStudent.LevelType;
                ExamSubReg1.Term = Theterm;
                ExamSubReg1.Repeat = theStudent.RepeatTimes;
                // ViewBag.tracker = tracker;
                // ExamSubReg.TheSubjectRegistration = SubjectRegistrationToUpdate;

                return View("Edit2", ExamSubReg1);


            }
            return View();
            //   }
        }

        //
        // POST: /Exam/Edit/5

        [HttpPost, ValidateInput(true)]
        [DynamicAuthorize]
        public ActionResult Edit(ExamSubjectReg model, string term)
        {
            ExamSubjectReg themodel = model;
            if (string.IsNullOrEmpty(term))
            {
                term = model.Term;
            }

            List<Level> theLevels = work.LevelRepository.Get().OrderBy(a => a.LevelName).ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();

            theItem.Add(new SelectListItem() { Text = "Choose..", Value = "" });



            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            theItem.Add(new SelectListItem() { Text = "Repeat Class", Value = "Repeat Class" });

            theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
            theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
            theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });
            ViewData["Level"] = theItem;
            List<PrimarySchoolStudent> Student = work.PrimarySchoolStudentRepository.Get(a => a.UserID == model.StudentName).ToList();
            PrimarySchoolStudent theStudent = Student[0];

            try
            {
                if (ModelState.IsValid)
                {
                    //  model.TheSubjects


                    //for each subjects, create an exam upject
                    // foreach (var subExam in model.TheSubjects)
                    //{

                    if (theStudent.RepeatTimes == 0 && model.TheSubjects.Count > 0)
                    {
                        foreach (var subExam in model.TheSubjects)
                        {
                            Exam theExams = new Exam();
                            theExams.Term = term;
                            theExams.SubjectName = subExam.Name;
                            theExams.StudentCode = model.StudentName;
                            theExams.Level = model.Level;
                            List<Term> theTerm = work.TermRepository.Get().ToList();
                            if (theTerm.Count() == 1)
                            {
                                theExams.Session = theTerm[0].SessionStartYear + "/" + theTerm[0].SessionEndYear;
                            }


                            //  theExams.Leve
                            if (subExam.SecondCA != null)
                            {
                                theExams.SecondCA = Convert.ToDecimal(subExam.SecondCA);
                            }
                            if (subExam.FirstCA != null)
                            {
                                theExams.FirstCA = Convert.ToDecimal(subExam.FirstCA);
                            }

                            if (subExam.SubjectExam != null)
                            {
                                theExams.SubjectExam = Convert.ToDecimal(subExam.SubjectExam);
                            }

                            List<Exam> theDatabaseExam = work.ExamRepository.Get(a => a.SubjectName.Equals(theExams.SubjectName) && a.Term.Equals(theExams.Term) &&
                                a.StudentCode == theExams.StudentCode && a.Level.Equals(model.Level)).ToList();

                            //if we dont av an exam record, then insert to database
                            if (theDatabaseExam.Count == 0)
                            {
                                // Exam theE = theExams;
                                work.ExamRepository.Insert(theExams);
                                //  work.Save();
                            }
                            else
                            {
                                Exam exam = theDatabaseExam[0];
                                exam.Level = model.Level;
                                exam.FirstCA = theExams.FirstCA;
                                exam.SecondCA = theExams.SecondCA;
                                exam.SubjectExam = theExams.SubjectExam;
                                exam.StudentCode = theExams.StudentCode;
                                List<Term> theTerm1 = work.TermRepository.Get().ToList();
                                if (theTerm1.Count() == 1)
                                {
                                    exam.Session = theTerm1[0].SessionStartYear + "/" + theTerm[0].SessionEndYear;
                                }
                                work.ExamRepository.Update(exam);
                                //work.Save();
                            }
                        }
                        work.Save();
                        // public ActionResult Edit(int? id, string term, string Level, ExamSubjectReg Model)
                        // return RedirectToAction("Edit", new { id = theStudent.UserID, term = model.Term, Level = model.Level, Model = themodel });
                        return RedirectToAction("Edit", "Exam", new { ids = SchoolManagement.Models.Encript.EncryptString(theStudent.PersonID.ToString(), true), term = model.Term, Level = model.Level });
                        // return View("Edit", model);
                    }


                    if (theStudent.RepeatTimes == 1 && model.FailedSubjects.Count > 0)
                    {
                        foreach (var subExam in model.FailedSubjects)
                        {
                            Exam1 theExams = new Exam1();
                            theExams.Term = term;
                            theExams.SubjectName = subExam.Name;
                            theExams.StudentCode = model.StudentName;
                            theExams.Level = model.Level;

                            //  theExams.Leve
                            if (subExam.SecondCA != null)
                            {
                                theExams.SecondCA = Convert.ToDecimal(subExam.SecondCA);
                            }
                            if (subExam.FirstCA != null)
                            {
                                theExams.FirstCA = Convert.ToDecimal(subExam.FirstCA);
                            }

                            if (subExam.SubjectExam != null)
                            {
                                theExams.SubjectExam = Convert.ToDecimal(subExam.SubjectExam);
                            }
                            List<Exam1> theDatabaseExam = work.Exam1Repository.Get(a => a.SubjectName.Equals(theExams.SubjectName) && a.Term.Equals(theExams.Term) &&
                                a.StudentCode == theExams.StudentCode && a.Level.Equals(model.Level)).ToList();

                            //if we dont av an exam record, then insert to database
                            if (theDatabaseExam.Count == 0)
                            {
                                List<Term> theTerm = work.TermRepository.Get().ToList();
                                if (theTerm.Count() == 1)
                                {
                                    theExams.Session = theTerm[0].SessionStartYear + "/" + theTerm[0].SessionEndYear;
                                }

                                work.Exam1Repository.Insert(theExams);
                            }
                            else
                            {
                                Exam1 exam = theDatabaseExam[0];
                                exam.Level = model.Level;
                                exam.FirstCA = theExams.FirstCA;
                                exam.SecondCA = theExams.SecondCA;
                                exam.SubjectExam = theExams.SubjectExam;
                                exam.StudentCode = theExams.StudentCode;

                                List<Term> theTerm = work.TermRepository.Get().ToList();
                                if (theTerm.Count() == 1)
                                {
                                    exam.Session = theTerm[0].SessionStartYear + "/" + theTerm[0].SessionEndYear;
                                }
                                work.Exam1Repository.Update(exam);

                            }
                        }
                        work.Save();
                        return RedirectToAction("Edit", "Exam", new { ids = SchoolManagement.Models.Encript.EncryptString(theStudent.PersonID.ToString(), true), term = model.Term, Level = model.Level });
                        // return View("Edit1", model);

                    }

                    if (theStudent.RepeatTimes == 2 && model.FailedSubjects2.Count > 0)
                    {
                        foreach (var subExam in model.FailedSubjects2)
                        {
                            Exam2 theExams = new Exam2();
                            theExams.Term = term;
                            theExams.SubjectName = subExam.Name;
                            theExams.StudentCode = model.StudentName;
                            theExams.Level = model.Level;

                            //  theExams.Leve
                            if (subExam.SecondCA != null)
                            {
                                theExams.SecondCA = Convert.ToDecimal(subExam.SecondCA);
                            }
                            if (subExam.FirstCA != null)
                            {
                                theExams.FirstCA = Convert.ToDecimal(subExam.FirstCA);
                            }

                            if (subExam.SubjectExam != null)
                            {
                                theExams.SubjectExam = Convert.ToDecimal(subExam.SubjectExam);
                            }
                            List<Exam2> theDatabaseExam = work.Exam2Repository.Get(a => a.SubjectName.Equals(theExams.SubjectName) && a.Term.Equals(theExams.Term) &&
                                a.StudentCode == theExams.StudentCode && a.Level.Equals(model.Level)).ToList();

                            //if we dont av an exam record, then insert to database
                            if (theDatabaseExam.Count == 0)
                            {
                                List<Term> theTerm = work.TermRepository.Get().ToList();
                                if (theTerm.Count() == 1)
                                {
                                    theExams.Session = theTerm[0].SessionStartYear + "/" + theTerm[0].SessionEndYear;
                                }

                                work.Exam2Repository.Insert(theExams);
                            }
                            else
                            {
                                Exam2 exam = theDatabaseExam[0];
                                exam.Level = model.Level;
                                exam.FirstCA = theExams.FirstCA;
                                exam.SecondCA = theExams.SecondCA;
                                exam.SubjectExam = theExams.SubjectExam;
                                exam.StudentCode = theExams.StudentCode;
                                List<Term> theTerm = work.TermRepository.Get().ToList();
                                if (theTerm.Count() == 1)
                                {
                                    exam.Session = theTerm[0].SessionStartYear + "/" + theTerm[0].SessionEndYear;
                                }
                                work.Exam2Repository.Update(exam);

                            }
                        }
                        work.Save();
                        return RedirectToAction("Edit", "Exam", new { ids = SchoolManagement.Models.Encript.EncryptString(theStudent.PersonID.ToString(), true), term = model.Term, Level = model.Level });
                        //  return View("Edit2", model);
                    }
                    //work.Save();
                    // }



                    // TODO: Add update logic here

                    return RedirectToAction("Index");
                }
                return RedirectToAction("Edit", "Exam", new { ids = SchoolManagement.Models.Encript.EncryptString(theStudent.PersonID.ToString(), true), term = model.Term, Level = model.Level });
                // return View("Edit", model);  ////  
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Exam/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Exam/Delete/5

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
