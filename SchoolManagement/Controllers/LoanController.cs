using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using JobHustler.DAL;
using SchoolManagement.Model;

namespace SchoolManagement.Controllers
{
    public class LoanController : Controller
    {
        //
        // GET: /Loan/
        UnitOfWork work = new UnitOfWork();

        public ActionResult Index(int? page, string datefrom, string dateto, string approved, string staffid)
        {
            List<Loan> theLoan = work.LoanRepository.Get().OrderByDescending(a => a.DateApproved).ToList();






            // List<AttendanceStaff> theAtten = work.AttendanceStaffRepository.Get().OrderByDescending(a => a.DateTaken).ToList();

            if (!String.IsNullOrEmpty(datefrom) && !(String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(datefrom);
                DateTime to = Convert.ToDateTime(dateto);
                theLoan = theLoan.Where(a => a.DateCreated.Date >= from.Date && a.DateCreated.Date <= to.Date).ToList();
            }

            if (!String.IsNullOrEmpty(datefrom) && (String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(datefrom);
                // DateTime to = Convert.ToDateTime(dateto);
                theLoan = theLoan.Where(a => a.DateCreated.Date == from.Date).ToList();
            }

            if (!String.IsNullOrEmpty(staffid))
            {
                int theStaffID = Convert.ToInt16(staffid);
                theLoan = theLoan.Where(a => a.StaffID == theStaffID).ToList();
            }


            if (!String.IsNullOrEmpty(approved))
            {
                bool value = Convert.ToBoolean(approved);
                theLoan = theLoan.Where(a => a.Approved == value).ToList();
            }













            int pageSize = 50;
            int pageNumber = (page ?? 1);

            ViewBag.Count = theLoan.Count();
            return View(theLoan.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Loan/Details/5

        public ActionResult Details(int id)
        {
            Loan theLoan = work.LoanRepository.GetByID(id);
            return View(theLoan);
        }

        //
        // GET: /Loan/Create

        public ActionResult Create()
        {
            List<PrimarySchoolStaff> staff = work.PrimarySchoolStaffRepository.Get(a => a.UserID != 5000001 && a.UserID != 5000022).OrderBy(a => a.LastName).ToList();
            List<SelectListItem> theStaffItem = new List<SelectListItem>();

            theStaffItem.Add(new SelectListItem() { Text = "None", Value = "" });
            foreach (var s in staff)
            {

                theStaffItem.Add(new SelectListItem() { Text = s.LastName + " " + s.Middle + " " + s.FirstName, Value = Convert.ToString(s.UserID) });
            }
            ViewData["Staff"] = theStaffItem;
            return View();
        }

        //
        // POST: /Loan/Create

        [HttpPost]
        public ActionResult Create(Loan model)
        {


            decimal theAmountToBeLoaned = model.LoanAmount;
            decimal paymentModel = 0;

            if (model.FirstAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.FirstAmountPayment);
            }

            if (model.SecondAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.SecondAmountPayment);
            }

            if (model.ThirdAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.ThirdAmountPayment);
            }

            if (model.ForthAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.ForthAmountPayment);
            }

            if (model.FifthAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.FifthAmountPayment);
            }

            if (model.SixthAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.SixthAmountPayment);
            }

            if (model.SeventhAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.SeventhAmountPayment);
            }

            if (model.EightAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.EightAmountPayment);
            }
            if (model.NinthAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.NinthAmountPayment);
            }

            if (model.TenthAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.TenthAmountPayment);
            }
            if (model.EleventhAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.EleventhAmountPayment);
            }

            if (model.TwelfthAmountPayment != null)
            {
                paymentModel += Convert.ToDecimal(model.TwelfthAmountPayment);
            }




            if (paymentModel > theAmountToBeLoaned)
            {

                ModelState.AddModelError("", "Payment Model is greater than Amount to be Loaned!");
                return View();
            }

            if (paymentModel < theAmountToBeLoaned)
            {

                ModelState.AddModelError("", "Payment Model is less than Amount to be Loaned!");
                return View();
            }

            model.DateCreated = Models.DateHelper.NigeriaTime(DateTime.Now);
            model.Approved = false;
            model.CreatedBy = User.Identity.Name;
            try
            {
                if (ModelState.IsValid)
                {
                    work.LoanRepository.Insert(model);
                    work.Save();
                }
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Loan/Edit/5

        public ActionResult Edit(int id)
        {
            List<PrimarySchoolStaff> staff = work.PrimarySchoolStaffRepository.Get(a => a.UserID != 5000001 && a.UserID != 5000022).OrderBy(a => a.LastName).ToList();
            List<SelectListItem> theStaffItem = new List<SelectListItem>();

            theStaffItem.Add(new SelectListItem() { Text = "None", Value = "" });
            foreach (var s in staff)
            {

                theStaffItem.Add(new SelectListItem() { Text = s.LastName + " " + s.Middle + " " + s.FirstName, Value = Convert.ToString(s.UserID) });
            }
            ViewData["Staff"] = theStaffItem;
            Loan theLoan = work.LoanRepository.GetByID(id);
            return View(theLoan);
        }

        //
        // POST: /Loan/Edit/5

        [HttpPost]
        public ActionResult Edit(Loan model)
        {
            try
            {
                if (model.Approved == true)
                {
                    model.DateApproved = Models.DateHelper.NigeriaTime(DateTime.Now);
                    model.ApprovedBy = User.Identity.Name;

                    if (ModelState.IsValid)
                    {

                        work.LoanRepository.Update(model);
                        work.Save();
                    }

                }
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                List<PrimarySchoolStaff> staff = work.PrimarySchoolStaffRepository.Get(a => a.UserID != 5000001 && a.UserID != 5000022).OrderBy(a => a.LastName).ToList();
                List<SelectListItem> theStaffItem = new List<SelectListItem>();

                theStaffItem.Add(new SelectListItem() { Text = "None", Value = "" });
                foreach (var s in staff)
                {

                    theStaffItem.Add(new SelectListItem() { Text = s.LastName + " " + s.Middle + " " + s.FirstName, Value = Convert.ToString(s.UserID) });
                }
                ViewData["Staff"] = theStaffItem;
                return View();
            }
        }

        //
        // GET: /Loan/Delete/5

        public ActionResult Delete(int id)
        {
            Loan theLoan = work.LoanRepository.GetByID(id);
            return View(theLoan);
        }

        //
        // POST: /Loan/Delete/5

        [HttpPost]
        public ActionResult Delete(Loan model)
        {
            try
            {
                // TODO: Add delete logic here
                work.LoanRepository.Delete(model);
                work.Save();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
