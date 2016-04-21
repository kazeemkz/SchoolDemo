using JobHustler.DAL;
using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class SchoolAccountController : Controller
    {
        //
        // GET: /SchoolAccount/

        UnitOfWork work = new UnitOfWork();
        //
        // GET: /SchoolFeesAccount/

        public ActionResult Index(string arm, string term, string dateFrom, string dateTo, string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page)
        {
            ViewBag.TotalAmount = 0;
            var account = from s in work.SchoolAccountRepository.Get().OrderByDescending(a => a.DateApproved)
                          select s;

            ViewBag.CountControlCreate = account.Count();
            if (!String.IsNullOrEmpty(dateFrom) && !String.IsNullOrEmpty(dateTo))
            {
                DateTime dF = Convert.ToDateTime(dateFrom);
                DateTime dT = Convert.ToDateTime(dateTo);
                // dT.AddHours(24);
                account = account.Where(s => s.DateApproved.Date >= dF.Date && s.DateApproved.Date <= dT.Date).ToList();
                List<SchoolAccount> theLastAccountBalance = new List<SchoolAccount>();
                theLastAccountBalance = account.OrderByDescending(a => a.DateApproved).ToList();

                if (theLastAccountBalance.Count() > 0)
                {
                    ViewBag.TotalAmount = theLastAccountBalance.First().Balance;
                }
            }
            else
            {
                if (account.Count() > 0)
                {
                    ViewBag.TotalAmount = account.First().Balance;
                }
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            //   List<Store> theItemStore = work.StoreRepository.Get().OrderBy(a => a.ItemName).ToList();
            decimal totalAmount = 0;
            //foreach (SchoolAccount aF in account)
            //{
            //    totalAmount += aF.Amount;
            //}
            //ViewBag.TotalAmount = totalAmount;
            //ViewBag.Count = account.Count();


            return View(account.ToPagedList(pageNumber, pageSize));
            // return View(theAccount);
        }


        //
        // GET: /SchoolAccount/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SchoolAccount/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SchoolAccount/Create

        [HttpPost]
        public ActionResult Create(SchoolAccount model)
        {
            try
            {

                // TODO: Add insert logic here
                // model.Amount = theFeetoBeApproved.Paid;
                model.ApprovedBy = User.Identity.Name;
                model.BankDraftNumber = "";
                model.ChequeNumber = "";
                model.DateApproved = DateTime.Now;
                model.EnteredBy = User.Identity.Name;
                model.PaidDate = DateTime.Now;
                model.POSTransactionNumber = "";
                model.StudentID = 0;
                model.TellerNumber = "";
                model.TransactionMethod = "";
                model.TransactionType = "Credit";
                model.Session = "";

                model.Balance = new SchoolAccountBalanceHelper().getSchoolAccountBalance(model.Amount);
                work.SchoolAccountRepository.Insert(model);
                work.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SchoolAccount/Edit/5

        public ActionResult Edit()
        {
            SchoolAccount theAccount = new SchoolAccount();
            return View(theAccount);
        }

        //
        // POST: /SchoolAccount/Edit/5

        [HttpPost]
        public ActionResult Edit(SchoolAccount model)
        {
            try
            {
                // TODO: Add update logic here
                if (model.Amount > 0)
                {
                    model.Amount = -1 * (model.Amount);
                    model.ApprovedBy = User.Identity.Name;
                    model.BankDraftNumber = "";
                    model.ChequeNumber = "";
                    model.DateApproved = DateTime.Now;
                    model.EnteredBy = User.Identity.Name;
                    model.PaidDate = DateTime.Now;
                    model.POSTransactionNumber = "";
                    model.StudentID = 0;
                    model.TellerNumber = "";
                    model.TransactionMethod = "";
                    model.TransactionType = "Debit";
                    model.Session = "";
                    model.Balance = new SchoolAccountBalanceHelper().getSchoolAccountBalance(model.Amount);
                    work.SchoolAccountRepository.Insert(model);
                    work.Save();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SchoolAccount/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SchoolAccount/Delete/5

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
