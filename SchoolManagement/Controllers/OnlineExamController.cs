using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobHustler.DAL;
using JobHustler.Models;
using SchoolManagement.Model;
using SchoolManagement.Models;
using PagedList;
//using PagedList.Mvc;
using System.Web.Security;
using SchoolManagement.DAL;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class OnlineExamController : Controller
    {
        //
        // GET: /Exam/
        UnitOfWork work = new UnitOfWork();
        UnitOfWork work2 = new UnitOfWork();
        OnlineExam Examme = new OnlineExam();
        [System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        // [Authorize(Roles = "Student")]
        [DynamicAuthorize]
        // public ActionResult IndexAsync(string ExamCode = "kababa")
        public ActionResult Index(string ExamCode = "kababa")
        {
            //HttpContext.Server.ScriptTimeout = 888888888;
            List<OnlineExam> theExam = null;
            Int32 examStudentUserName = Convert.ToInt32(User.Identity.Name);
            //List<PrimarySchoolStudent> examStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == examStudentUserName).ToList();
            //PrimarySchoolStudent examStudent = examStudents[0];
            PrimarySchoolStudent examStudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID == examStudentUserName).First();
            // PrimarySchoolStudent examStudent = examStudents[0];
            //Course theCourse = work.CourseRepository.GetByID(id);
            if (ExamCode == "kababa")
            {
                if (examStudent.ExamWritingNow != null)
                {
                    theExam = work.OnlineExamRepository.Get(a => a.Visible == "true" && a.LevelName.Equals(examStudent.PresentLevel) && !(a.ExamCode.Equals(examStudent.ExamWritingNow))).ToList();
                }
                else
                {
                    theExam = work.OnlineExamRepository.Get(a => a.Visible == "true" && a.LevelName.Equals(examStudent.PresentLevel)).ToList();
                }

                //  List<Exam> theExam = work.ExamRepository.Get(a => a.LevelName.Equals(level) && a.Term== term && a.Visible== "true").ToList();// && a.CourseID == courseID
            }
            else
            {
                if (examStudent.ExamWritingNow != null)
                {
                    theExam = work.OnlineExamRepository.Get(a => a.Visible == "true" && a.ExamCode.Equals(ExamCode) && a.LevelName.Equals(examStudent.PresentLevel) && !(a.ExamCode.Equals(examStudent.ExamWritingNow))).ToList();
                }
                else
                {
                    theExam = work.OnlineExamRepository.Get(a => a.Visible == "true" && a.ExamCode.Equals(ExamCode) && a.LevelName.Equals(examStudent.PresentLevel)).ToList();
                }

                if (theExam.Count > 0)
                {

                    Examme = theExam[0];
                    IList<Question> theQs = new ExamService().GetQuestions(theExam[0]);
                    var rnd = new Random();
                    IList<Question> theRamdomExam = theQs.OrderBy(x => rnd.Next()).ToList();
                    IList<Question> theRealRamdomExam = new List<Question>();
                    int counter = 0;
                    foreach(var q in theRamdomExam)
                    {
                        counter = counter + 1;
                        q.FakeNumber = counter;
                        theRealRamdomExam.Add(q);
                    }
                   // Examme.AddQuestion(theQs);
                   // Examme.AddQuestion(theRamdomExam);
                    Examme.AddQuestion(theRealRamdomExam);
                }



            }
            // if its only a practise exam/test dont update anyfin
            if ((Examme.ExamType != "Practise" && Examme.ExamType != "PractiseRecord"))
            {
                if (examStudent.ExamWritingNow != null && theExam.Count > 0 && ExamCode != "kababa")
                {
                    if (examStudent.ExamWritingNow.Equals(ExamCode))
                    {
                        ModelState.AddModelError("", "Becuase you Refreshed(or Pressed the back Button) this page, you are now disqalified from this Exam!");
                        OnlineExam myFakeExam = new OnlineExam();
                        return View(myFakeExam);
                    }
                    else
                    {
                        //examStudent.ExamWritingNow = ExamCode;
                        //work.PrimarySchoolStudentRepository.Update(examStudent);
                        //work.Save();
                        int personID = examStudent.PersonID;
                        PrimarySchoolStudent studentEx = work2.PrimarySchoolStudentRepository.GetByID(personID);
                        studentEx.ExamWritingNow = ExamCode;
                        //examStudent.ExamWritingNow = ExamCode;
                        work2.PrimarySchoolStudentRepository.Update(studentEx);
                        work2.Save();
                    }
                }

                if (examStudent.ExamWritingNow == null && theExam.Count > 0 && ExamCode != "kababa")
                {
                    int personID = examStudent.PersonID;
                    PrimarySchoolStudent studentEx = work2.PrimarySchoolStudentRepository.GetByID(personID);
                    studentEx.ExamWritingNow = ExamCode;
                    //examStudent.ExamWritingNow = ExamCode;
                    work2.PrimarySchoolStudentRepository.Update(studentEx);
                    work2.Save();
                }
            }

            List<SelectListItem> ExamCodes = new List<SelectListItem>();

            foreach (var exam in theExam)
            {
                ExamCodes.Add(new SelectListItem() { Text = exam.ExamCode, Value = exam.ExamCode });
            }


            ViewData["ExamCodes"] = ExamCodes;

            return View(Examme);
        }


        public JsonResult Timer()
        {
            MembershipUser user = Membership.GetUser(User.Identity.Name);
            user.LastActivityDate = DateTime.Now;

            Membership.UpdateUser(user);


            // You can return anything to reset the timer.
            return Json(new { Timer = "reset" }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult LoadNextQuestionNumber(string examCode)
        {
            List<OnlineExam> theExams = work.OnlineExamRepository.Get(a => a.ExamCode.Equals(examCode)).ToList();
            OnlineExam theRealExam = theExams[0];
            List<Question> theQuestions = work.QuestionRepository.Get(a => a.OnlineExamID == theRealExam.OnlineExamID).ToList();
            int theCount = theQuestions.Count() + 1;

            return Json(theCount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadExamDuration(int examID)
        {
            OnlineExam theExam = work.OnlineExamRepository.GetByID(examID);

            return Json(theExam.Duration, JsonRequestBehavior.AllowGet);
        }
        //but this in index in roles and securitues
       // [DynamicAuthorize]
        [Authorize]
        public ActionResult LoadExamCodes(string sortOrder, string currentFilter, string ExamCode, string LevelString, string Visible, int? page)

      // public ActionResult LoadExamCodes()
        {

            var theExamCodes = from s in work.OnlineExamRepository.Get()
                               select s;
            if (!String.IsNullOrEmpty(ExamCode))
            {
                string theCode = ExamCode.ToLower();
                theExamCodes = theExamCodes.Where(s => s.ExamCode.ToLower().Contains(theCode));

            }

            if (!String.IsNullOrEmpty(LevelString))
            {
                //  string theCode = ExamCode.ToLower();
                theExamCodes = theExamCodes.Where(s => s.LevelName.Equals(LevelString));

            }

            if (!String.IsNullOrEmpty(Visible))
            {
                // bool theVal = Convert.ToBoolean(Visible);
                //  string theCode = ExamCode.ToLower();
                theExamCodes = theExamCodes.Where(s => s.Visible.Equals(Visible));

            }

            // List<Exam> theExamCodes;
            //  theExamCodes = work.ExamRepository.Get().ToList();// && a.CourseID == courseID


            //  if (examCode == "kazoo")
            // {

            //// }
            // else
            // {
            //   theExamCodes = work.ExamRepository.Get(a => a.ExamCode.Equals(examCode)).ToList();// && a.CourseID == courseID
            // }
            //    //   @Html.DropDownListFor(model => model.LevelType, new SelectList((System.Collections.IEnumerable)ViewData["Level"], "Value", "Text"))

            // List<SelectListItem> theItem = new List<SelectListItem>();

            int pageSize = 30;
            int pageNumber = (page ?? 1);
            List<OnlineExam> theRealExamCode = new List<OnlineExam>();

            foreach (var examCodes in theExamCodes)
            {
                IList<Question> theQs = new ExamService().GetQuestions(examCodes);
                examCodes.Question = theQs;
                //  examCodes.AddQuestion(theQs);
                theRealExamCode.Add(examCodes);
                //theRealExamCode
            }
            // ViewData["examCode"] = theItem;
            //  return View("Index2", theExamCodes);
            ViewBag.Count = theExamCodes.Count();
            return View("Index2", theRealExamCode.OrderByDescending(a=>a.Date).ToPagedList(pageNumber, pageSize));
            // return View("Index2", theExamCodes.ToPagedList(pageNumber, pageSize));
        }



        public ActionResult LoadExam(string level)
        {
            List<OnlineExam> theExams = work.OnlineExamRepository.Get().ToList();
            List<SelectListItem> exam = new List<SelectListItem>();
            if (string.IsNullOrEmpty(level))
            {

            }
            else
            {
                theExams = work.OnlineExamRepository.Get(a => a.ExamCode != level).ToList();

                exam.Add(new SelectListItem { Text = level, Value = level, Selected = true });
            }
            //  List<Level> theLevel = work1.LevelRepository.Get(a => a.Name.Equals(level)).ToList();



            foreach (var co in theExams)
            {
                //Course theCourse = work.CourseRepository.GetByID(co.);
                exam.Add(new SelectListItem { Text = co.ExamCode, Value = co.ExamCode });
            }
            return Json(exam, JsonRequestBehavior.AllowGet);
        }

        //  [Authorize(Roles = "Student")]
        //[NoAsyncTimeoutAttribute]
        [HttpPost]
        public ActionResult Grade(OnlineExam exam)
        {
            if (exam.Questions.Count == 0)
            {
                // RedirectToAction("Index");
                return View("Error");
            }
            else
            {

                var grade = new ExamService().Grade(exam);
                Question theQuestion = work.QuestionRepository.GetByID(exam.Questions[0].QuestionID);
                OnlineExam theOnlineExam = work.OnlineExamRepository.GetByID(theQuestion.OnlineExamID);

                //get user who has just finished the exam
                Int32 UserName = Convert.ToInt32(User.Identity.Name);
                List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.UserID == UserName).ToList();
                PrimarySchoolStudent theStudent = theStudents[0];

                if (theOnlineExam.ExamType == "PractiseRecord")
                {
                    string theUserID = Convert.ToString(theStudent.UserID);
                    List<PractiseSave> thePractice = work.PractiseSaveRepository.Get(a => a.Term.Equals(theOnlineExam.Term) && a.Code.Equals(theOnlineExam.ExamCode) && a.StudentID.Equals(theUserID)).ToList();
                    //Check if there is not an exam code equal to this one
                    if (thePractice.Count == 0)
                    {
                        PractiseSave theStudentScore = new PractiseSave();
                        double e = ((grade.Score / grade.TotalPoints) * 100);
                        theStudentScore.Score = Convert.ToDecimal(e);
                        theStudentScore.Level = theStudent.PresentLevel;
                        theStudentScore.StudentID = Convert.ToString(theStudent.UserID);
                        theStudentScore.Subject = theOnlineExam.Course;
                        theStudentScore.Term = theOnlineExam.Term;
                        theStudentScore.Code = theOnlineExam.ExamCode;
                        theStudentScore.LevelType = theStudent.LevelType;
                        // theStudentScore.
                        work.PractiseSaveRepository.Insert(theStudentScore);
                        work.Save();
                    }
                }

                if (theOnlineExam.ExamType == "Exam" || theOnlineExam.ExamType == "TEST 1" || theOnlineExam.ExamType == "TEST 2")
                {
                    //if student has no repeat
                    if (theStudent.RepeatTimes == 0)
                    {
                        List<Exam> theExam0 = work.ExamRepository.Get(a => a.StudentCode == theStudent.UserID && a.Level.Equals(theStudent.PresentLevel) &&
                            a.SubjectName.Equals(theOnlineExam.Course) && a.Term.Equals(theOnlineExam.Term)).ToList();
                        // 

                        if (theOnlineExam.ExamType == "Exam")
                        {
                            if (theExam0.Count > 0)
                            {
                                // if(theExam0)
                                Exam theStudentScore = theExam0[0];
                                if (theStudentScore.SubjectExam == 0)
                                {
                                    double e = ((grade.Score / grade.TotalPoints) * 100) * 0.6;
                                    theStudentScore.SubjectExam = Convert.ToDecimal(e);
                                    work2.ExamRepository.Update(theStudentScore);
                                    work2.Save();
                                }
                            }
                            else
                            {

                                Exam theStudentScore = new Exam();
                                theStudentScore.Term = theOnlineExam.Term;
                                theStudentScore.SubjectName = theOnlineExam.Course;
                                theStudentScore.Level = theStudent.PresentLevel;
                                theStudentScore.StudentCode = theStudent.UserID;
                                double e = ((grade.Score / grade.TotalPoints) * 100) * 0.6;
                                theStudentScore.SubjectExam = Convert.ToDecimal(e);
                                work2.ExamRepository.Insert(theStudentScore);
                                work2.Save();
                            }
                        }

                        if (theOnlineExam.ExamType == "TEST 1")
                        {

                            if (theExam0.Count > 0)
                            {
                                Exam theStudentScore = theExam0[0];
                                if (theStudentScore.FirstCA == 0)
                                {
                                    double t2 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                    theStudentScore.FirstCA = Convert.ToDecimal(t2);
                                    work2.ExamRepository.Update(theStudentScore);
                                    work2.Save();
                                }
                            }
                            else
                            {


                                double t1 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                Exam theExam = new Exam();
                                theExam.FirstCA = Convert.ToDecimal(t1);
                                theExam.Term = theOnlineExam.Term;
                                theExam.SubjectName = theOnlineExam.Course;
                                theExam.Level = theStudent.PresentLevel;
                                theExam.StudentCode = theStudent.UserID;
                                // theStudentScore.FirstCA = 
                                work2.ExamRepository.Insert(theExam);
                                work2.Save();
                            }
                        }

                        if (theOnlineExam.ExamType == "TEST 2")
                        {
                            if (theExam0.Count > 0)
                            {
                                Exam theStudentScore = theExam0[0];
                                if (theStudentScore.SecondCA == 0)
                                {
                                    double t2 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                    theStudentScore.SecondCA = Convert.ToDecimal(t2);
                                    work2.ExamRepository.Update(theStudentScore);
                                    work2.Save();
                                }
                            }

                            else
                            {
                                if (theOnlineExam.ExamType == "TEST 2")
                                {
                                    double t1 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                    Exam theExam = new Exam();
                                    theExam.SecondCA = Convert.ToDecimal(t1);
                                    theExam.Term = theOnlineExam.Term;
                                    theExam.SubjectName = theOnlineExam.Course;
                                    theExam.Level = theStudent.PresentLevel;
                                    theExam.StudentCode = theStudent.UserID;
                                    // theStudentScore.FirstCA = 
                                    work2.ExamRepository.Insert(theExam);
                                    work2.Save();



                                    //Exam theStudentScore = theExam0[0];
                                    //double t2 = ((grade.Score / grade.TotalPoints) * 100) * 0.3;
                                    //theStudentScore.SecondCA = Convert.ToDecimal(t2);
                                    //work2.ExamRepository.Update(theStudentScore);
                                    //work2.Save();
                                }

                            }
                        }
                    }
                    //check for repeat 1
                    if (theStudent.RepeatTimes == 1)
                    {
                        List<Exam1> theExam1 = work.Exam1Repository.Get(a => a.StudentCode == theStudent.UserID && a.Level.Equals(theStudent.PresentLevel) &&
                            a.SubjectName.Equals(theOnlineExam.Course) && a.Term.Equals(theOnlineExam.Term)).ToList();
                        // 
                        if (theOnlineExam.ExamType == "Exam")
                        {
                            if (theExam1.Count > 0)
                            {
                                Exam1 theStudentScore = theExam1[0];
                                if (theStudentScore.SubjectExam == 0)
                                {
                                    double e = ((grade.Score / grade.TotalPoints) * 100) * 0.6;
                                    theStudentScore.SubjectExam = Convert.ToDecimal(e);
                                    work2.Exam1Repository.Update(theStudentScore);
                                    work2.Save();
                                }
                            }

                            else
                            {

                                if (theOnlineExam.ExamType == "Exam")
                                {

                                    Exam1 theStudentScore = new Exam1();
                                    theStudentScore.Term = theOnlineExam.Term;
                                    theStudentScore.SubjectName = theOnlineExam.Course;
                                    theStudentScore.Level = theStudent.PresentLevel;
                                    theStudentScore.StudentCode = theStudent.UserID;
                                    double e = ((grade.Score / grade.TotalPoints) * 100) * 0.6;
                                    theStudentScore.SubjectExam = Convert.ToDecimal(e);
                                    work2.Exam1Repository.Insert(theStudentScore);
                                    work2.Save();




                                    //Exam1 theStudentScore = new Exam1();
                                    //theStudentScore.SecondCA = 0.00M;
                                    //double e = ((grade.Score / grade.TotalPoints) * 100) * 0.5;
                                    //// theStudentScore.SubjectExam = e;
                                    //theStudentScore.SubjectExam = Convert.ToDecimal(e);
                                    //work2.Exam1Repository.Insert(theStudentScore);
                                    //work2.Save();
                                }

                            }

                        }
                        if (theOnlineExam.ExamType == "TEST 1")
                        {
                            if (theExam1.Count > 0)
                            {
                                Exam1 theStudentScore = theExam1[0];
                                if (theStudentScore.FirstCA == 0)
                                {
                                    double t2 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                    theStudentScore.FirstCA = Convert.ToDecimal(t2);
                                    work2.Exam1Repository.Update(theStudentScore);
                                    work2.Save();
                                }
                            }
                            else
                            {

                                double t1 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                Exam1 theExamlocal1 = new Exam1();
                                theExamlocal1.FirstCA = Convert.ToDecimal(t1);
                                theExamlocal1.Term = theOnlineExam.Term;
                                theExamlocal1.SubjectName = theOnlineExam.Course;
                                theExamlocal1.Level = theStudent.PresentLevel;
                                theExamlocal1.StudentCode = theStudent.UserID;
                                // theStudentScore.FirstCA = 
                                work2.Exam1Repository.Insert(theExamlocal1);
                                work2.Save();
                            }
                        }

                        if (theExam1.Count > 0)
                        {
                            if (theOnlineExam.ExamType == "TEST 2")
                            {
                                Exam1 theStudentScore = theExam1[0];
                                if (theStudentScore.SecondCA == 0)
                                {
                                    double t2 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                    theStudentScore.SecondCA = Convert.ToDecimal(t2);
                                    work2.Exam1Repository.Update(theStudentScore);
                                    work2.Save();
                                }
                            }

                        }
                        else
                        {
                            if (theOnlineExam.ExamType == "TEST 2")
                            {
                                double t1 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                Exam1 theExam = new Exam1();
                                theExam.SecondCA = Convert.ToDecimal(t1);
                                theExam.Term = theOnlineExam.Term;
                                theExam.SubjectName = theOnlineExam.Course;
                                theExam.Level = theStudent.PresentLevel;
                                theExam.StudentCode = theStudent.UserID;
                                // theStudentScore.FirstCA = 
                                work2.Exam1Repository.Insert(theExam);
                                work2.Save();
                            }

                        }

                    }




                    //check for repeat 2
                    if (theStudent.RepeatTimes == 2)
                    {
                        List<Exam2> theExam2 = work.Exam2Repository.Get(a => a.StudentCode == theStudent.UserID && a.Level.Equals(theStudent.PresentLevel) &&
                            a.SubjectName.Equals(theOnlineExam.Course) && a.Term.Equals(theOnlineExam.Term)).ToList();
                        // 
                        if (theOnlineExam.ExamType == "Exam")
                        {
                            if (theExam2.Count > 0)
                            {
                                Exam2 theStudentScore = theExam2[0];
                                if (theStudentScore.SubjectExam == 0)
                                {
                                    double e = ((grade.Score / grade.TotalPoints) * 100) * 0.6;
                                    theStudentScore.SubjectExam = Convert.ToDecimal(e);
                                    work2.Exam2Repository.Update(theStudentScore);
                                    work2.Save();
                                }
                            }

                            else
                            {

                                if (theOnlineExam.ExamType == "Exam")
                                {
                                    Exam2 theStudentScore = new Exam2();
                                    theStudentScore.Term = theOnlineExam.Term;
                                    theStudentScore.SubjectName = theOnlineExam.Course;
                                    theStudentScore.Level = theStudent.PresentLevel;
                                    theStudentScore.StudentCode = theStudent.UserID;
                                    double e = ((grade.Score / grade.TotalPoints) * 100) * 0.6;
                                    theStudentScore.SubjectExam = Convert.ToDecimal(e);
                                    work2.Exam2Repository.Insert(theStudentScore);
                                    work2.Save();


                                    //Exam2 theStudentScore = new Exam2();
                                    //theStudentScore.SecondCA = 0.00M;
                                    //double e = ((grade.Score / grade.TotalPoints) * 100) * 0.5;
                                    //// theStudentScore.SubjectExam = e;
                                    //theStudentScore.SubjectExam = Convert.ToDecimal(e);
                                    //work2.Exam2Repository.Insert(theStudentScore);
                                    //work2.Save();
                                }

                            }
                        }

                        if (theOnlineExam.ExamType == "TEST 1")
                        {
                            if (theExam2.Count == 0)
                            {
                                double t1 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                Exam2 theExamlocal2 = new Exam2();
                                theExamlocal2.FirstCA = Convert.ToDecimal(t1);
                                theExamlocal2.Term = theOnlineExam.Term;
                                theExamlocal2.SubjectName = theOnlineExam.Course;
                                theExamlocal2.Level = theStudent.PresentLevel;
                                theExamlocal2.StudentCode = theStudent.UserID;
                                // theStudentScore.FirstCA = 
                                work2.Exam2Repository.Insert(theExamlocal2);
                                work2.Save();
                            }
                            else
                            {
                                Exam2 theStudentScore = theExam2[0];
                                if (theStudentScore.FirstCA == 0)
                                {
                                    double t2 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                    theStudentScore.FirstCA = Convert.ToDecimal(t2);
                                    work2.Exam2Repository.Update(theStudentScore);
                                    work2.Save();
                                }
                            }
                        }

                        if (theOnlineExam.ExamType == "TEST 2")
                        {
                            if (theExam2.Count > 0)
                            {
                                Exam2 theStudentScore = theExam2[0];
                                if (theStudentScore.SecondCA == 0)
                                {
                                    double t2 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                    theStudentScore.SecondCA = Convert.ToDecimal(t2);
                                    work2.Exam2Repository.Update(theStudentScore);
                                    work2.Save();
                                }
                            }

                            else
                            {
                                if (theOnlineExam.ExamType == "TEST 2")
                                {
                                    double t1 = ((grade.Score / grade.TotalPoints) * 100) * 0.2;
                                    Exam2 theExam = new Exam2();
                                    theExam.SecondCA = Convert.ToDecimal(t1);
                                    theExam.Term = theOnlineExam.Term;
                                    theExam.SubjectName = theOnlineExam.Course;
                                    theExam.Level = theStudent.PresentLevel;
                                    theExam.StudentCode = theStudent.UserID;
                                    // theStudentScore.FirstCA = 
                                    work2.Exam2Repository.Insert(theExam);
                                    work2.Save();
                                }

                            }
                        }
                    }
                }
                return View(grade);
            }
            //return View();
        }

        //
        // GET: /Exam/Details/5
        [DynamicAuthorize]
        public ActionResult Details(int id)
        {
            OnlineExam theExam = work.OnlineExamRepository.GetByID(id);
            return View(theExam);
        }

        //
        // GET: /Exam/Create

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreatePartial(string levelValue)
        {
            List<SelectListItem> theSubjectList = new List<SelectListItem>();

            List<Subject> theSubject = work.SubjectRepository.Get().ToList();

            foreach (var subject in theSubject)
            {
                theSubjectList.Add(new SelectListItem() { Text = subject.Name, Value = subject.Name });
            }

            // theItem.Add(new SelectListItem() { Text = "PRIMART 1A", Value = "PRIMART 1A" });
            ViewData["Subject"] = theSubjectList;
            return PartialView("_Create");
        }

        //
        // POST: /Exam/Create
        [DynamicAuthorize]
        [HttpPost]
        public ActionResult Create(OnlineExam theExam)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // TODO: Add insert logic here

                    string theExamCode = theExam.ExamCode.Trim().ToLower();
                    List<OnlineExam> theExams = work.OnlineExamRepository.Get(a => a.ExamCode.Equals(theExamCode)).ToList();
                    if (theExams.Count > 0)
                    {
                        return RedirectToAction("Create", "Question");
                    }
                    else
                    {
                        OnlineExam theEx = theExam;
                        theEx.Date = DateTime.Now;

                        // smContext c = new smContext();
                        // c.Entry(theEx).State = System.Data.Entity.EntityState.Added;
                        // UnitOfWork w = new UnitOfWork();
                        work.OnlineExamRepository.Insert(theEx);
                        work.Save();
                        // w.OnlineExamRepository.Insert(theEx);
                        // c.SaveChanges(); ;
                        return RedirectToAction("Create", "Question");
                    }

                    // return RedirectToAction("Index");
                }
                else
                {
                    return View("_Create", theExam);
                }
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Exam/Edit/5
        [DynamicAuthorize]
        public ActionResult Edit(int id)
        {
            OnlineExam theExam = work.OnlineExamRepository.GetByID(id);

            List<SelectListItem> theSubjectList = new List<SelectListItem>();

            List<Subject> theSubject = work.SubjectRepository.Get().ToList();

            foreach (var subject in theSubject)
            {
                theSubjectList.Add(new SelectListItem() { Text = subject.Name, Value = subject.Name });
            }

            // theItem.Add(new SelectListItem() { Text = "PRIMART 1A", Value = "PRIMART 1A" });
            ViewData["Subject"] = theSubjectList;
            return View(theExam);
        }

        //
        // POST: /Exam/Edit/5
        [DynamicAuthorize]
        [HttpPost]
        public ActionResult Edit(OnlineExam theExam)
        {
            try
            {
                // TODO: Add update logic here

                work.OnlineExamRepository.Update(theExam);
                work.Save();
                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Edited an Online Exam of code:-"+ " "+ theExam.ExamCode, UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
                    work.Save();
                }


                return RedirectToAction("LoadExamCodes");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Exam/Delete/5
        [DynamicAuthorize]
        public ActionResult Delete(int id)
        {
            OnlineExam theExam = work.OnlineExamRepository.GetByID(id);

            return View(theExam);
        }

        //
        // POST: /Exam/Delete/5

        [HttpPost]
        public ActionResult Delete(OnlineExam theExam)
        {
            try
            {

                //PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(model.PersonID);
                //work.PrimarySchoolStudentRepository.Delete(theStudent);

                OnlineExam theMainExam = work.OnlineExamRepository.GetByID(theExam.OnlineExamID);

                string examName = theMainExam.ExamCode;
                //  GetQuestions(Exam theMainExam, ref UnitOfWork work)

                IList<Question> theQs = new ExamService().GetQuestions(theMainExam, ref work);

                foreach (var theQ in theQs)
                {
                    // foreach (var choice in theQ.Choices)
                    // {
                    //  work.ChoiceRepository.Delete(choice);
                    // }
                    work.QuestionRepository.Delete(theQ);
                }
               
                // IList<Question> theQs = new ExamService().GetQuestions(theMainExa);
                //  examCodes.AddQuestion(theQs);
                work.OnlineExamRepository.Delete(theMainExam);
                work.Save();

                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Deleted an online exam name:-"+ " "+examName, UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
                    work.Save();
                }

                return RedirectToAction("LoadExamCodes");
            }
            catch
            {
                return View();
            }
        }
    }
}
