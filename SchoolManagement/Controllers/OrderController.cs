using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobHustler.DAL;
using JobHustler.Models;
using SchoolManagement.Model;
using PagedList;
using SchoolManagement.Models;
//using PagedList.Mvc;

namespace SchoolManagement.Controllers
{
    // [DynamicAuthorize]
    [Authorize]
    public class OrderController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        //
        // GET: /Order/
        [DynamicAuthorize]
        public ViewResult Index(string sortOrder, string currentFilter, string Date, string searchString, string SexString, string LevelString, string StudentIDString, int? page)
        //  public ActionResult Index(string sortOrder, string currentFilter, int? page)
        //public ActionResult Index()
        {
            //var theOrders = from s in  work.OrderRepository.Get().OrderByDescending(a => a.OrderDate)
            //             select s;

            List<Order> theOrders = work.OrderRepository.Get().OrderByDescending(a => a.OrderDate).ToList();


            decimal theAmount = 0;

            //  var theOrders = work.OrderRepository.Get().OrderByDescending(a => a.OrderDate);


            if (!String.IsNullOrEmpty(searchString))
            {
                theOrders = theOrders.Where(s => s.StudentLastName.ToUpper().Contains(searchString.ToUpper())).ToList();

            }


            if (!String.IsNullOrEmpty(LevelString))
            {
                theOrders = theOrders.Where(s => s.Level.Equals(LevelString)).ToList();
            }

            if (!String.IsNullOrEmpty(Date))
            {
                DateTime d = Convert.ToDateTime(Date);
                theOrders = theOrders.Where(s => s.OrderDate.Value.Date == d.Date).ToList();
            }


            if (!String.IsNullOrEmpty(StudentIDString))
            {
                int theID = Convert.ToInt32(StudentIDString);
                theOrders = theOrders.Where(s => s.studentID == theID).ToList();
            }


            if (!String.IsNullOrEmpty(Date) && String.IsNullOrEmpty(StudentIDString) && String.IsNullOrEmpty(LevelString) && String.IsNullOrEmpty(searchString))
            {

                foreach (var oder in theOrders)
                {
                    theAmount = theAmount + oder.TotalAmount;
                }

                ViewBag.TotalSold = "( On this Date #" + theAmount + " was realised ! )";
            }


            if (!String.IsNullOrEmpty(Date) && String.IsNullOrEmpty(StudentIDString) && !String.IsNullOrEmpty(LevelString) && String.IsNullOrEmpty(searchString))
            {

                foreach (var oder in theOrders)
                {
                    theAmount = theAmount + oder.TotalAmount;
                }

                ViewBag.TotalSold = "( On this Date #" + theAmount + " was realised from this Class! )";
            }






            int pageSize = 50;
            int pageNumber = (page ?? 1);
            //   List<Store> theItemStore = work.StoreRepository.Get().OrderBy(a => a.ItemName).ToList();



            ViewBag.Count = theOrders.Count();

            return View(theOrders.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Student,Parent")]
        public ViewResult Index2(string sortOrder, string currentFilter, string Date, int? page, string studentID)
        //  public ActionResult Index2()
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
                // theItem.Add(new SelectListItem() { Text = "None", Value = "" });

                foreach (var s in thSchoolStudents)
                {
                    theItem.Add(new SelectListItem() { Text = s.LastName + " " + s.FirstName, Value = s.PersonID.ToString() });
                    ViewData["Students"] = theItem;
                }
            }

            List<Order> theOrders = new List<Order>();

            if (User.IsInRole("Parent"))
            {
                Int32 theUser = Convert.ToInt32(User.Identity.Name);
                Parent theP = work.ParentRepository.Get(a => a.UserID == theUser).First();
                if (studentID != null)
                {
                    int id = Convert.ToInt32(studentID);
                    PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(id);
                    if (theStudent != null)
                    {
                        theOrders = work.OrderRepository.Get().OrderByDescending(a => a.OrderDate).ToList();
                        theOrders = theOrders.Where(a => a.studentID == theStudent.UserID).ToList();
                    }
                    else
                    {
                        theOrders = theOrders.Where(a => a.studentID == 0).ToList();
                    }

                }
                else
                {
                    theOrders = theOrders.Where(a => a.studentID == 0).ToList();
                }
            }
            else
            {
                int student = Convert.ToInt32(User.Identity.Name);
                theOrders = work.OrderRepository.Get(a => a.studentID == student).OrderByDescending(a => a.OrderDate).ToList();


            }
            if (!String.IsNullOrEmpty(Date))
            {
                DateTime d = Convert.ToDateTime(Date);
                theOrders = theOrders.Where(s => s.OrderDate.Value.Date == d.Date).ToList();
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);


            return View(theOrders.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Order/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult ReverseOrder(int id)
        {
            return View();
        }

        //
        // GET: /Order/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Order/Create

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
        // GET: /Order/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Order/Edit/5

        [HttpPost]
        public ActionResult Edit(string id)
        {
            try
            {
                Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));

                Order theOrder = work.OrderRepository.GetByID(theId);

                theOrder.Approved = true;


                SchoolAccount thePayment1 = new SchoolAccount();

                thePayment1.Amount = theOrder.TotalAmount;
                thePayment1.ApprovedBy = User.Identity.Name;
                //thePayment1.BankDraftNumber = ;
                // thePayment1.ChequeNumber = theFeetoBeApproved.ChequeNumber;
                thePayment1.DateApproved = DateTime.Now;
                // thePayment1.EnteredBy = theFeetoBeApproved.EnteredBy;
                thePayment1.PaidDate = Convert.ToDateTime(theOrder.OrderDate);
                // thePayment1.POSTransactionNumber = theFeetoBeApproved.POSTransactionNumber;
                thePayment1.StudentID = theOrder.studentID;
                //  thePayment1.TellerNumber = theFeetoBeApproved.TellerNumber;
                thePayment1.TransactionMethod = Convert.ToString(theOrder.OrderNumber);
                thePayment1.TransactionType = "Credit, Stored Item Sold";
                thePayment1.Balance = new SchoolAccountBalanceHelper().getSchoolAccountBalance(thePayment1.Amount);
                List<TermRegistration> theTermReg = new List<TermRegistration>();
                theTermReg = work.TermRegistrationRepository.Get(a => a.StudentID == theOrder.studentID).OrderByDescending(a => a.DateRegistered).ToList();

                if (theTermReg.Count() > 0)
                {
                    thePayment1.Session = theTermReg[0].Session;
                }

                thePayment1.Level = theOrder.Level;// theFeetoBeApproved.Level;
                work.SchoolAccountRepository.Insert(thePayment1);
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
        // GET: /Order/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Order/Delete/5

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
