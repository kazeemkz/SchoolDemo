using JobHustler.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SchoolManagement.Model;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class ApproveSchoolFeePaymentController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        //
        // GET: /ApproveSchoolFeePayment/

        public ActionResult Index(string dateFrom, string dateTo, string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page
        , string arm, string term)
        {
            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();
            theItem.Add(new SelectListItem() { Text = "None", Value = "" });

            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            ViewData["arm"] = theItem;

            var students = from s in work.SchoolFeePaymentRepository.Get()
                           select s;



            if (!String.IsNullOrEmpty(ApprovedString))
            {
                bool theValue = Convert.ToBoolean(ApprovedString);
                students = students.Where(s => s.Approved == theValue);
            }



            if (!String.IsNullOrEmpty(LevelString))
            {
                //students = students.Where(s => s.PresentLevel.Equals(LevelString) && s.PresentLevel !=null);
                students = students.Where(s => s.Level == LevelString);
            }


            if (!String.IsNullOrEmpty(StudentIDString))
            {
                int theID = Convert.ToInt32(StudentIDString);
                students = students.Where(s => s.StudentID == theID);
            }

            if (!String.IsNullOrEmpty(dateFrom) && !String.IsNullOrEmpty(dateTo))
            {
                DateTime dF = Convert.ToDateTime(dateFrom);
                DateTime dT = Convert.ToDateTime(dateTo);
                students = students.Where(s => s.DatePaid >= dF && s.DatePaid <= s.DatePaid).ToList();
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);

            return View(students.ToPagedList(pageNumber, pageSize));
            // return View();
        }

        //
        // GET: /ApproveSchoolFeePayment/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ApproveSchoolFeePayment/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ApproveSchoolFeePayment/Create

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
        // GET: /ApproveSchoolFeePayment/Edit/5

        public ActionResult Edit(int id)
        {
            SchoolFeePayment thePayment = work.SchoolFeePaymentRepository.GetByID(id);
            return View(thePayment);
        }

        //
        // POST: /ApproveSchoolFeePayment/Edit/5

        [HttpPost]
        public ActionResult Edit2(int id)
        {
            try
            {
                SchoolFeePayment theFeetoBeApproved = work.SchoolFeePaymentRepository.GetByID(id);
                theFeetoBeApproved.Approved = true;
                work.SchoolFeePaymentRepository.Update(theFeetoBeApproved);

                SchoolFeesAccount thePayment = new SchoolFeesAccount();

                thePayment.Amount = theFeetoBeApproved.Paid;
                thePayment.ApprovedBy = User.Identity.Name;
                thePayment.BankDraftNumber = theFeetoBeApproved.BankDraftNumber;
                thePayment.ChequeNumber = theFeetoBeApproved.ChequeNumber;
                thePayment.DateApproved = DateTime.Now;
                thePayment.EnteredBy = theFeetoBeApproved.EnteredBy;
                thePayment.PaidDate = theFeetoBeApproved.DatePaid;
                thePayment.POSTransactionNumber = theFeetoBeApproved.POSTransactionNumber;
                thePayment.StudentID = theFeetoBeApproved.StudentID;
                thePayment.TellerNumber = theFeetoBeApproved.TellerNumber;
                thePayment.TransactionMethod = theFeetoBeApproved.PaymentMethod;
                thePayment.TransactionType = "Credit";
                thePayment.Session = theFeetoBeApproved.Session;
                thePayment.Level = theFeetoBeApproved.Level;
                work.SchoolFeesAccountRepository.Insert(thePayment);





                SchoolAccount thePayment1 = new SchoolAccount();

                thePayment1.Amount = theFeetoBeApproved.Paid;
                thePayment1.ApprovedBy = User.Identity.Name;
                thePayment1.BankDraftNumber = theFeetoBeApproved.BankDraftNumber;
                thePayment1.ChequeNumber = theFeetoBeApproved.ChequeNumber;
                thePayment1.DateApproved = DateTime.Now;
                thePayment1.EnteredBy = theFeetoBeApproved.EnteredBy;
                thePayment1.PaidDate = theFeetoBeApproved.DatePaid;
                thePayment1.POSTransactionNumber = theFeetoBeApproved.POSTransactionNumber;
                thePayment1.StudentID = theFeetoBeApproved.StudentID;
                thePayment1.TellerNumber = theFeetoBeApproved.TellerNumber;
                thePayment1.TransactionMethod = theFeetoBeApproved.PaymentMethod;
                thePayment1.TransactionType = "Credit";
                thePayment1.Session = theFeetoBeApproved.Session;
                thePayment1.Level = theFeetoBeApproved.Level;
                thePayment1.Balance = new SchoolAccountBalanceHelper().getSchoolAccountBalance(thePayment1.Amount);
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

        [HttpPost]
        //Edit Entry
        public ActionResult Edit(SchoolFeePayment model)
        {

            try
            {
                //  model.Owing = 
                int paymentID = model.SchoolFeePaymentID;

                UnitOfWork work2 = new UnitOfWork();
                SchoolFeePayment yetToBeUpdatedFee = work2.SchoolFeePaymentRepository.GetByID(paymentID);
                decimal theDifferenceinPayment = (model.Paid - yetToBeUpdatedFee.Paid);

                //update the owing
                // model.Owing = model.Owing - theDifferenceinPayment;

                model.Owing = (yetToBeUpdatedFee.Paid - model.Paid) + model.Owing;
                work.SchoolFeePaymentRepository.Update(model);
                List<SchoolFeePayment> thePayment = work.SchoolFeePaymentRepository.Get(a => a.DatePaid > model.DatePaid && a.StudentID == model.StudentID).ToList();

                decimal theAmount = model.Owing;
                if (thePayment.Count > 0)
                {
                    foreach (var p in thePayment)
                    {
                        if (!(p == model))
                        {
                            // p.Owing = model.Owing - p.Paid;
                            p.Owing = theAmount - p.Paid;
                            theAmount = p.Owing;
                            work.SchoolFeePaymentRepository.Update(p);
                        }
                    }
                }
                work2.Save();
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
        // GET: /ApproveSchoolFeePayment/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ApproveSchoolFeePayment/Delete/5

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
