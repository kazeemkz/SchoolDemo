using JobHustler.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace SchoolManagement.Controllers
{
    public class AuditTrailController : Controller
    {
        //
        // GET: /AuditTrail/
        UnitOfWork work = new UnitOfWork();
        public ActionResult Index(string date, string dateto, string stafftid, int? page)
        {
            var audit = from s in work.AuditTrailRepository.Get().OrderByDescending(a => a.Date)
                        select s;
            if (!String.IsNullOrEmpty(stafftid))
            {
                audit = audit.Where(a => a.UserID == stafftid);


            }
            if (!String.IsNullOrEmpty(date) && !(String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(date);
                DateTime to = Convert.ToDateTime(dateto);
                audit = audit.Where(a => a.Date >= from && a.Date <= to).ToList();
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);

            ViewBag.Count = audit.Count();
           
            return View(audit.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /AuditTrail/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AuditTrail/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AuditTrail/Create

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
        // GET: /AuditTrail/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /AuditTrail/Edit/5

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
        // GET: /AuditTrail/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /AuditTrail/Delete/5

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
