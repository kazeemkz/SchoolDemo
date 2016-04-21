using JobHustler.DAL;
using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace SchoolManagement.Controllers
{
      [Authorize]
    public class SchoolFeesAccountController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        //
        // GET: /SchoolFeesAccount/

        public ActionResult Index(string arm, string term,string dateFrom, string dateTo, string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page)
        {
        
            var account = from s in work.SchoolFeesAccountRepository.Get()
                           select s;

            ViewBag.CountControlCreate = account.Count();
            if (!String.IsNullOrEmpty(dateFrom) && !String.IsNullOrEmpty(dateTo))
            {
                DateTime dF = Convert.ToDateTime(dateFrom);
                DateTime dT = Convert.ToDateTime(dateTo);
               // dT.AddHours(24);
                account = account.Where(s => s.DateApproved.Date >= dF.Date && s.DateApproved.Date <= dT.Date).ToList();
            }
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            //   List<Store> theItemStore = work.StoreRepository.Get().OrderBy(a => a.ItemName).ToList();
            decimal totalAmount = 0;
            foreach (SchoolFeesAccount aF in account)
            {
                totalAmount += aF.Amount;
            }
            ViewBag.TotalAmount = totalAmount;
            ViewBag.Count = account.Count();


            return View(account.ToPagedList(pageNumber, pageSize));
            // return View(theAccount);
        }

        //
        // GET: /SchoolFeesAccount/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SchoolFeesAccount/Create

        public ActionResult Create()
        {
            // List<SchoolFeesAccount> theAccount = work.SchoolFeesAccountRepository.Get().ToList();
            return View();
        }

        //
        // POST: /SchoolFeesAccount/Create

        [HttpPost]
        public ActionResult Create(SchoolFeesAccount model)
        {
            try
            {
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
                work.SchoolFeesAccountRepository.Insert(model);
                work.Save();
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SchoolFeesAccount/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /SchoolFeesAccount/Edit/5

        [HttpPost]
        public ActionResult Edit(SchoolFeesAccount model)
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
        // GET: /SchoolFeesAccount/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SchoolFeesAccount/Delete/5

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
