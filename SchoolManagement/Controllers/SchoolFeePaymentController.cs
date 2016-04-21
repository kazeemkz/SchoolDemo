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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class SchoolFeePaymentController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        UnitOfWork work2 = new UnitOfWork();
        //
        // GET: /SchoolFeePayment/

        [DynamicAuthorize]
        //public ActionResult Index(string levelString, string session, string arm, string DateFrom, string DateTo, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page
        //    , string paidschoolfess, string term, bool? printQuery)
        //{
        public ActionResult Index(string levelString, string session, string arm, string DateFrom, string DateTo, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page
          , string paidschoolfess, string term)
        {
            bool? thePrintQuery = null;
          //  thePrintQuery = printQuery;
            if (User.IsInRole("Parent"))
            {
                List<TermRegistration> studentList = new List<TermRegistration>();
                int theParentUserName = Convert.ToInt32(User.Identity.Name);
                Parent theParent = work.ParentRepository.Get(a => a.UserID == theParentUserName).First();


                List<PrimarySchoolStudent> theStudents = work.PrimarySchoolStudentRepository.Get(a => a.ParentID == theParent.ParentID).ToList();

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





                if (!(string.IsNullOrEmpty(theParent.StudentIDOne)))
                {
                    int theStudentID = Convert.ToInt32(theParent.StudentIDOne);
                    //List<TermRegistration> theList = work.TermRegistrationRepository.Get(a => a.StudentID == theStudentID && a.Term == term && a.Session == session).ToList();
                    List<TermRegistration> theList = work.TermRegistrationRepository.Get(a => a.StudentID == theStudentID).ToList();
                    if (theList.Count > 0)
                    {
                        studentList.Add(theList[0]);
                    }

                }
                if (!(string.IsNullOrEmpty(theParent.StudentIDTwo)))
                {
                    int theStudentID = Convert.ToInt32(theParent.StudentIDTwo);
                    List<TermRegistration> theList = work.TermRegistrationRepository.Get(a => a.StudentID == theStudentID ).ToList();
                    if (theList.Count > 0)
                    {
                        studentList.Add(theList[0]);
                    }

                }
                if (!(string.IsNullOrEmpty(theParent.StudentIDThree)))
                {
                    int theStudentID = Convert.ToInt32(theParent.StudentIDThree);
                    List<TermRegistration> theList = work.TermRegistrationRepository.Get(a => a.StudentID == theStudentID ).ToList();
                    if (theList.Count > 0)
                    {
                        studentList.Add(theList[0]);
                    }

                }

                if (!(string.IsNullOrEmpty(theParent.StudentIDFour)))
                {
                    int theStudentID = Convert.ToInt32(theParent.StudentIDFour);
                    List<TermRegistration> theList = work.TermRegistrationRepository.Get(a => a.StudentID == theStudentID ).ToList();
                    if (theList.Count > 0)
                    {
                        studentList.Add(theList[0]);
                    }

                }

                if (!(string.IsNullOrEmpty(theParent.StudentIDFive)))
                {
                    int theStudentID = Convert.ToInt32(theParent.StudentIDFive);
                    List<TermRegistration> theList = work.TermRegistrationRepository.Get(a => a.StudentID == theStudentID ).ToList();
                    if (theList.Count > 0)
                    {
                        studentList.Add(theList[0]);
                    }

                }

                List<SelectListItem> theItem = new List<SelectListItem>();
                theItem.Add(new SelectListItem() { Text = "None", Value = "" });

                foreach (var level in studentList)
                {
                    theItem.Add(new SelectListItem() { Text = level.StudentID.ToString(), Value = level.StudentID.ToString() });
                }

                ViewData["studentIds"] = theItem;
                //  students = studentList;

            }





            if (term == "First")
            {
                ViewBag.Term = "1";
            }
            if (term == "Second")
            {
                ViewBag.Term = "2";
            }

            if (term == "Third")
            {
                ViewBag.Term = "3";
            }


            decimal totalAmount = 0;

            if (Request.HttpMethod == "GET")
            {
                // searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            var students = from s in work.TermRegistrationRepository.Get(a => a.Term == term)
                           select s;

            // var students = from s in work.TermRegistrationRepository.Get()
            //               select s;

            if (!String.IsNullOrEmpty(term))
            {
                // students = from s in work.TermRegistrationRepository.Get()
                // select s;
                students = students.Where(s => s.Term.Equals(term));
            }

            if (!String.IsNullOrEmpty(session))
            {
                students = students.Where(s => s.Session.Equals(session));
            }

            if (!String.IsNullOrEmpty(LevelString))
            {
                students = students.Where(s => s.Level == LevelString);
            }


            if (!String.IsNullOrEmpty(StudentIDString))
            {
                int theID = Convert.ToInt32(StudentIDString);
                students = students.Where(s => s.StudentID == theID);
            }



            int pageSize = 50;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(paidschoolfess))
            {
                if (paidschoolfess == "Owing")
                {
                    var payments = from s in work.SchoolFeePaymentRepository.Get()
                                   select s;
                    if (!String.IsNullOrEmpty(term))
                    {
                        payments = payments.Where(s => s.Term.Equals(term));
                    }

                    if (!String.IsNullOrEmpty(session))
                    {
                        payments = payments.Where(s => s.Session.Equals(session));
                    }

                    if (!String.IsNullOrEmpty(LevelString))
                    {
                        payments = payments.Where(s => s.Level == LevelString);
                    }


                    if (!String.IsNullOrEmpty(StudentIDString))
                    {
                        int theID = Convert.ToInt32(StudentIDString);
                        payments = payments.Where(s => s.StudentID == theID);
                    }

                    // payments = payments.Where(a => a.Owing > 0).OrderBy(a => a.StudentID);//.OrderBy(a => a.DatePaid);
                    TermRegistration theStudentWhoRegistered = new TermRegistration();

                    List<TermRegistration> theOwingStudents = new List<TermRegistration>();
                    List<SchoolFeePayment> theRegisteredSchoolFeePyament = new List<SchoolFeePayment>();

                    SchoolFeePayment thePayment = new SchoolFeePayment();
                    TermRegistration tR = new TermRegistration();
                    SchoolFeePayment sF = new SchoolFeePayment();

                    List<TermRegistration> theRegisteredStudents = new List<TermRegistration>();

                    theRegisteredStudents = students.ToList();



                    foreach (TermRegistration t in theRegisteredStudents)
                    {
                        List<SchoolFeePayment> thisGuyPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == t.StudentID && a.Term == term).OrderByDescending(a => a.DatePaid).ToList();
                        if (thisGuyPayment.Count() > 0)
                        {
                            SchoolFeePayment theCurrentPayment = thisGuyPayment[0];
                            if (theCurrentPayment.Owing > 0)
                            {
                                totalAmount += theCurrentPayment.Owing;
                                theOwingStudents.Add(new TermRegistration { DateRegistered = t.DateRegistered, Owing = t.Owing, TermRegistrationID = t.TermRegistrationID, Register = t.Register, Sex = t.Sex, LastName = t.LastName, FirstName = t.FirstName, SchoolFeesKind = theCurrentPayment.TheSchoolFeesType, Cost = theCurrentPayment.Cost, Level = theCurrentPayment.Level, Session = theCurrentPayment.Session, Term = theCurrentPayment.Term, StudentID = theCurrentPayment.StudentID, });
                            }
                        }

                    }


                    students = theOwingStudents;
                }
                else
                {
                    if (Convert.ToBoolean(paidschoolfess) == true)
                    {
                        List<SchoolFeePayment> listOfPayments = work.SchoolFeePaymentRepository.Get().Where(a => a.Owing <= 0).OrderBy(a => a.DatePaid).ToList();
                        List<TermRegistration> theRegisteredStuentWhoHavePaid = new List<TermRegistration>();
                        foreach (TermRegistration r in students)
                        {
                            foreach (SchoolFeePayment l in listOfPayments)
                            {
                                if (r.StudentID == l.StudentID)
                                {
                                    totalAmount += l.Cost;
                                    theRegisteredStuentWhoHavePaid.Add(r);
                                    break;
                                }
                            }
                            // if(s.StudentID ==)theRegisteredStuentWhoHavePaid
                        }
                        students = theRegisteredStuentWhoHavePaid;
                    }
                    else
                    {
                        List<TermRegistration> theNeverPaidStuddentStudents = new List<TermRegistration>();
                        List<TermRegistration> theRegisteredStudents = new List<TermRegistration>();

                        theRegisteredStudents = students.ToList();
                        theRegisteredStudents = students.ToList();



                        foreach (TermRegistration t in theRegisteredStudents)
                        {
                            List<SchoolFeePayment> thisGuyPayment = new List<SchoolFeePayment>();
                            //   List<SchoolFeePayment> thisGuyPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == t.StudentID && a.Term == term).OrderByDescending(a => a.DatePaid).ToList();
                            if (string.IsNullOrEmpty(session))
                            {
                                thisGuyPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == t.StudentID && a.Term == term).OrderByDescending(a => a.DatePaid).ToList();
                            }

                            if (!(string.IsNullOrEmpty(session)))
                            {
                                thisGuyPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == t.StudentID && a.Session == session && a.Term == term).OrderByDescending(a => a.DatePaid).ToList();
                            }

                            if (thisGuyPayment.Count() == 0)
                            {

                                totalAmount += t.Cost;
                                theNeverPaidStuddentStudents.Add(new TermRegistration { LastName = t.LastName, FirstName = t.FirstName, SchoolFeesKind = t.SchoolFeesKind, Cost = t.Cost, Level = t.Level, Session = t.Session, Term = t.Term, StudentID = t.StudentID });

                            }

                        }
                        students = theNeverPaidStuddentStudents;
                    }

                }
            }

            if (User.IsInRole("Parent"))
            {
                //ViewBag.Term = "1";
                int theParentUserName = Convert.ToInt32(User.Identity.Name);
                Parent theParent = work.ParentRepository.Get(a => a.UserID == theParentUserName).First();

                List<TermRegistration> studentList = new List<TermRegistration>();

                int theStudentID = 0;
                if (!(string.IsNullOrEmpty(StudentIDString)))
                {
                    theStudentID = Convert.ToInt32(StudentIDString);
                }
                List<TermRegistration> theList = new List<TermRegistration>();
                theList = work.TermRegistrationRepository.Get(a => a.StudentID == theStudentID && a.Term == term && a.Session == session && a.Level == LevelString).ToList();
                students = theList;


            }

            if (User.IsInRole("Student"))
            {
                //  ViewBag.Term = "1";
                int theStudentUserName = Convert.ToInt32(User.Identity.Name);
                List<PrimarySchoolStudent> theStudent = new List<PrimarySchoolStudent>();
                List<TermRegistration> theList = new List<TermRegistration>();
                theStudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID == theStudentUserName).ToList();

                //  List<TermRegistration> studentList = new List<TermRegistration>();
                if ((theStudent.Count > 0))
                {
                    int theStudentID = Convert.ToInt32(theStudent[0].UserID);
                    theList = work.TermRegistrationRepository.Get(a => a.StudentID == theStudentID && a.Level == LevelString && a.Term == term && a.Session == session).ToList();
                    //if (theList.Count > 0)
                    //{
                    //   theList.Add(theList[0]);
                    //  }

                }
                students = theList;
            }

            if (thePrintQuery != null && students.Count() > 0)
            {
                thePrintQuery = null;
                StringWriter oStringWriter1 = new StringWriter();
                Document itextDoc = new Document(PageSize.LETTER);
                itextDoc.Open();
                Response.ContentType = "application/pdf";
                PrintResult print = new PrintResult();
                // Set up the document and the MS to write it to and create the PDF writer instance
                MemoryStream ms = new MemoryStream();
                //Document document = new Document(PageSize.A3.Rotate());
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                // Open the PDF document
                document.Open();

                Document thedoc = new SchoolFeesPrinting().FeesPrinting(students.ToList(), ref oStringWriter1, ref document);
                iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
                seperator.Offset = -6f;

                document.Close();

                // Hat tip to David for his code on stackoverflow for this bit
                // http://stackoverflow.com/questions/779430/asp-net-mvc-how-to-get-view-to-generate-pdf
                byte[] file = ms.ToArray();
                MemoryStream output = new MemoryStream();
                output.Write(file, 0, file.Length);
                output.Position = 0;

                HttpContext.Response.AddHeader("content-disposition", "attachment; filename=SchoolFees.pdf");
                return new FileStreamResult(output, "application/pdf"); //new FileStreamResult(output, "application/pdf");

               // return View(new FileStreamResult(output, "application/pdf"));
            }

         
            ViewBag.TotalAmount = totalAmount;
            ViewBag.Count = students.Count();
            return View(students.ToPagedList(pageNumber, pageSize));
            // }
            // else
            // {
            //   return View("Index3", students.ToPagedList(pageNumber, pageSize));
            // }
        }



        //
        // GET: /SchoolFeePayment/Details/5
        [DynamicAuthorize]
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SchoolFeePayment/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SchoolFeePayment/Create
        [DynamicAuthorize]
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
        // GET: /SchoolFeePayment/Edit/5
        // [Authorize(Roles = "Student,Parent")] 
        [Authorize]
        [DynamicAuthorize]
        public ActionResult ViewFeeForStudent(int id, string term)
        {

            PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(id);

            List<SchoolFeePayment> theStudentFeesPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == theStudent.UserID && a.Level.Equals(theStudent.PresentLevel) && a.Term.Equals(term)).ToList();

            // PopulateActivity(theStudent, theStudentFeesPayment, term);
            if (theStudentFeesPayment.Count == 0)
            {

                return View("ViewSchoolFee", new SchoolFeePayment() { Level = theStudent.PresentLevel, StudentID = theStudent.UserID });
            }
            else
            {
                return View("ViewSchoolFee", theStudentFeesPayment[0]);
            }

        }
        [DynamicAuthorize]
        public ActionResult Edit(string id, string term, string TheSchoolFeesType, string session)
        {
            string theTerm = "";
            if (term == "1")
            {
                theTerm = "First";
                ViewBag.Term = "First";
            }
            if (term == "2")
            {
                theTerm = "Second";
                ViewBag.Term = "Second";
            }

            if (term == "3")
            {
                theTerm = "Third";
                ViewBag.Term = "Third";
            }

            ViewBag.session = session;

            Int32 stuentID = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            TermRegistration theTermRegistration = new TermRegistration();
            List<TermRegistration> theTermRegistrations = work.TermRegistrationRepository.Get(a => a.Session == session && a.Term == theTerm && a.StudentID == stuentID).ToList();

            //  List<TermRegistration> theTermRegistrations = work.TermRegistrationRepository.Get(a => a.Session == session && a.Term == theTerm && a.StudentID == stuentID).ToList();
            List<SchoolFeePayment> theStudentFeesPaymentLast10Transactions = work.SchoolFeePaymentRepository.Get(a => a.StudentID == stuentID).OrderByDescending(a => a.DatePaid).Take(10).OrderBy(a => a.DatePaid).ToList();

            ViewBag.Last10Transactions = theStudentFeesPaymentLast10Transactions;
            if (theTermRegistrations.Count() > 0)
            {
                theTermRegistration = theTermRegistrations[0];
                ViewBag.TermRegistration = theTermRegistration;

                ViewBag.TheSchoolFeesType = theTermRegistration.SchoolFeesKind;
                ViewBag.session = theTermRegistration.Session;
                //theTermRegistration.Cost =

                PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID == stuentID).First();
                ViewBag.Class = theStudent.LevelType;
                ViewBag.Name = theStudent.LastName.ToUpper();
                //List<SchoolFeePayment> theStudentFeesPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == theStudent.UserID && a.Level.Equals(theStudent.PresentLevel) && a.Term.Equals(term)).ToList();

                List<SchoolFeePayment> theStudentFeesPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == theStudent.UserID).OrderBy(a => a.DatePaid).Take(20).ToList();

                List<SchoolFeePayment> theStudentFeesForThisStudentPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == theStudent.UserID && a.Session == session && a.Term == theTerm).OrderBy(a => a.DatePaid).Take(10).ToList();

                //check if the student is owing for current term
                List<SchoolFeePayment> theStudentFeesForStudentPaymentForThisTerm = work.SchoolFeePaymentRepository.Get(a => a.StudentID == theStudent.UserID && a.Session == session && a.Term == theTerm).OrderByDescending(a => a.DatePaid).ToList();

                // List<TermRegistration> theLastStudentRegistration = work.TermRegistrationRepository.Get(a=>a.)

                //ViewData["SchoolFeesKind"] = theItem;
                List<SchoolFeePayment> owingPayment = new List<SchoolFeePayment>();

                decimal totalOwing = 0;
                // foreach (SchoolFeePayment s in theStudentFeesPayment)
                // if (theStudentFeesPayment.Count > 0)
                if (theStudentFeesForStudentPaymentForThisTerm.Count > 0)
                {
                    //  SchoolFeePayment p = theStudentFeesPayment.OrderByDescending(a => a.DatePaid).First();
                    SchoolFeePayment p = theStudentFeesForStudentPaymentForThisTerm.OrderByDescending(a => a.DatePaid).First();
                    totalOwing = p.Owing;
                    if (totalOwing > 0)
                    {
                        owingPayment.Add(p);

                    }
                }


                ViewBag.totalOwing = totalOwing;
                ViewBag.Last10Transaction = theStudentFeesForThisStudentPayment;
                ViewBag.theCurrentTermPayment = theStudentFeesForThisStudentPayment.Count();
                ViewBag.Cost = theTermRegistration.Cost;// theType.Amount;
                ViewBag.Owing = owingPayment;
                return View(new SchoolFeePayment() { Level = theStudent.PresentLevel, StudentID = theStudent.UserID, Term = term });
            }
            else
            {
                return View();
            }


        }


        //for viewing parent and student payment history
        [Authorize(Roles = "Student,Parent")]
        public ActionResult ViewYourFees(string id, string term, string TheSchoolFeesType, string session)
        {
            string theTerm = "";
            if (term == "1")
            {
                theTerm = "First";
                ViewBag.Term = "First";
            }
            if (term == "2")
            {
                theTerm = "Second";
                ViewBag.Term = "Second";
            }

            if (term == "3")
            {
                theTerm = "Third";
                ViewBag.Term = "Third";
            }

            ViewBag.session = session;

            ViewBag.ParentStudent = "ParentStudent";

            Int32 stuentID = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            TermRegistration theTermRegistration = new TermRegistration();
            List<TermRegistration> theTermRegistrations = work.TermRegistrationRepository.Get(a => a.Session == session && a.Term == theTerm && a.StudentID == stuentID).ToList();

            //  List<TermRegistration> theTermRegistrations = work.TermRegistrationRepository.Get(a => a.Session == session && a.Term == theTerm && a.StudentID == stuentID).ToList();
            List<SchoolFeePayment> theStudentFeesPaymentLast10Transactions = work.SchoolFeePaymentRepository.Get(a => a.StudentID == stuentID).OrderByDescending(a => a.DatePaid).Take(10).OrderBy(a => a.DatePaid).ToList();

            ViewBag.Last10Transactions = theStudentFeesPaymentLast10Transactions;
            if (theTermRegistrations.Count() > 0)
            {
                theTermRegistration = theTermRegistrations[0];
                ViewBag.TermRegistration = theTermRegistration;

                ViewBag.TheSchoolFeesType = theTermRegistration.SchoolFeesKind;
                ViewBag.session = theTermRegistration.Session;
                //theTermRegistration.Cost =

                PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID == stuentID).First();
                ViewBag.Class = theStudent.LevelType;
                ViewBag.Name = theStudent.LastName.ToUpper();
                //List<SchoolFeePayment> theStudentFeesPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == theStudent.UserID && a.Level.Equals(theStudent.PresentLevel) && a.Term.Equals(term)).ToList();

                List<SchoolFeePayment> theStudentFeesPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == theStudent.UserID).OrderBy(a => a.DatePaid).Take(20).ToList();

                List<SchoolFeePayment> theStudentFeesForThisStudentPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == theStudent.UserID && a.Session == session && a.Term == theTerm).OrderBy(a => a.DatePaid).Take(10).ToList();

                //check if the student is owing for current term
                List<SchoolFeePayment> theStudentFeesForStudentPaymentForThisTerm = work.SchoolFeePaymentRepository.Get(a => a.StudentID == theStudent.UserID && a.Session == session && a.Term == theTerm).OrderByDescending(a => a.DatePaid).ToList();

                // List<TermRegistration> theLastStudentRegistration = work.TermRegistrationRepository.Get(a=>a.)

                //ViewData["SchoolFeesKind"] = theItem;
                List<SchoolFeePayment> owingPayment = new List<SchoolFeePayment>();

                decimal totalOwing = 0;
                // foreach (SchoolFeePayment s in theStudentFeesPayment)
                // if (theStudentFeesPayment.Count > 0)
                if (theStudentFeesForStudentPaymentForThisTerm.Count > 0)
                {
                    //  SchoolFeePayment p = theStudentFeesPayment.OrderByDescending(a => a.DatePaid).First();
                    SchoolFeePayment p = theStudentFeesForStudentPaymentForThisTerm.OrderByDescending(a => a.DatePaid).First();
                    totalOwing = p.Owing;
                    if (totalOwing > 0)
                    {
                        owingPayment.Add(p);

                    }
                }


                ViewBag.totalOwing = totalOwing;
                ViewBag.Last10Transaction = theStudentFeesForThisStudentPayment;
                ViewBag.theCurrentTermPayment = theStudentFeesForThisStudentPayment.Count();
                ViewBag.Cost = theTermRegistration.Cost;// theType.Amount;
                ViewBag.Owing = owingPayment;
                return View("Edit", new SchoolFeePayment() { Level = theStudent.PresentLevel, StudentID = theStudent.UserID, Term = term });
            }
            else
            {
                return View("Edit");
            }


        }

        // POST: /SchoolFeePayment/Edit/5
        //[DynamicAuthorize]
        [HttpPost]
        public ActionResult Edit(SchoolFeePayment model, decimal cost, string TheSchoolFeesType, string term, string session)
        {
            try
            {
                //  Int32 stuentID = Convert.ToInt32(Models.Encript.DecryptString(id, true));
                string theTerm = "";
                model.DatePaid = DateTime.Now;
                model.TheSchoolFeesType = TheSchoolFeesType;
                model.EnteredBy = User.Identity.Name;
                if (term == "1")
                {
                    theTerm = "First";
                    model.Term = "First";
                }
                if (term == "2")
                {
                    theTerm = "Second";
                    model.Term = "Second";
                }

                if (term == "3")
                {
                    theTerm = "Second";
                    model.Term = "Third";
                }
                TermRegistration theTermRegistration = work2.TermRegistrationRepository.Get(a => a.Session == session && a.Term == theTerm && a.StudentID == model.StudentID).First();
                model.TheSchoolFeesType = theTermRegistration.SchoolFeesKind;

                if (ModelState.IsValid)
                {
                    int theStudentID = model.StudentID;
                    List<SchoolFeePayment> theStudentFeesPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == theStudentID).OrderByDescending(a => a.DatePaid).ToList();

                    if (theStudentFeesPayment.Count > 0)
                    {
                        //take out the very last payment
                        SchoolFeePayment p = theStudentFeesPayment.First();

                        if (p.Owing == 0)
                        {
                            model.Owing = (model.Cost - model.Paid);
                        }
                        if (p.Owing < 0)
                        {
                            model.Owing = (p.Owing + model.Cost) - (model.Paid);
                        }
                        if (p.Owing > 0)
                        {
                            model.Owing = (p.Owing) - (model.Paid);
                        }
                        //totalOwing = p.Owing;
                    }
                    else
                    {
                        model.Owing = model.Cost - model.Paid;

                    }

                    work.SchoolFeePaymentRepository.Insert(model);
                    work.Save();
                }
                // TODO: Add update logic here
                //  model.

                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Updated school Fees for User ID -:" + model.StudentID, UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
                    work.Save();
                }

                return RedirectToAction("Index", "SchoolFeePayment");
            }
            catch
            {
                return View("Edit");
            }
        }

        //
        // GET: /SchoolFeePayment/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SchoolFeePayment/Delete/5

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
