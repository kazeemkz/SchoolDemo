using JobHustler.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SchoolManagement.Model;

namespace SchoolManagement.Controllers
{
    public class DeductionHistoryController : Controller
    {
        //
        // GET: /DeductionHistory/

        UnitOfWork work = new UnitOfWork();
       // public ActionResult Index()
        public ActionResult Index(int? page, string datefrom, string dateto, string staffid)
        {

            List<DeductionHistory> theDeductionHistory = work.DeductionHistoryRepository.Get().OrderByDescending(a => a.DatePaid).ToList();

            
            // List<AttendanceStaff> theAtten = work.AttendanceStaffRepository.Get().OrderByDescending(a => a.DateTaken).ToList();

            if (!String.IsNullOrEmpty(datefrom) && !(String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(datefrom);
                DateTime to = Convert.ToDateTime(dateto);
                theDeductionHistory = theDeductionHistory.Where(a => a.DatePaid.Date >= from.Date && a.DatePaid.Date <= to.Date).ToList();
            }

            if (!String.IsNullOrEmpty(datefrom) && (String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(datefrom);
                // DateTime to = Convert.ToDateTime(dateto);
                theDeductionHistory = theDeductionHistory.Where(a => a.DatePaid.Date == from.Date).ToList();
            }

            if (!String.IsNullOrEmpty(staffid))
            {
               // int theStaffID = Convert.ToInt16(staffid);
                theDeductionHistory = theDeductionHistory.Where(a => a.StaffID == staffid).ToList();
            }



          

               int pageSize = 100;
            int pageNumber = (page ?? 1);

            ViewBag.Count = theDeductionHistory.Count();
            return View(theDeductionHistory.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /DeductionHistory/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /DeductionHistory/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DeductionHistory/Create

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
        // GET: /DeductionHistory/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /DeductionHistory/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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
        // GET: /DeductionHistory/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /DeductionHistory/Delete/5

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
