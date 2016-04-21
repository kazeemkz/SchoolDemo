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
    public class DeductionController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        //

        // GET: /Salary/

        public ActionResult Index(int? page)
        {
            List<Deduction> theDeduction = work.DeductionRepository.Get().ToList();
            int pageSize = 50;
            int pageNumber = (page ?? 1);

            ViewBag.Count = theDeduction.Count();
            return View(theDeduction.ToPagedList(pageNumber, pageSize));
            // return View();
        }

        //
        // GET: /Salary/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Salary/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Salary/Create

        [HttpPost]
        public ActionResult Create(Deduction model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    work.DeductionRepository.Insert(model);
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
        // GET: /Salary/Edit/5

        public ActionResult Edit(int id)
        {
            Deduction theDeduction = work.DeductionRepository.GetByID(id);
            return View(theDeduction);
        }

        //
        // POST: /Salary/Edit/5

        [HttpPost]
        public ActionResult Edit(Deduction model)
        {
            try
            {
                // TODO: Add update logic here
                if (TryUpdateModel(model))
                {

                    work.DeductionRepository.Update(model);
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
        // GET: /Salary/Delete/5

        public ActionResult Delete(int id)
        {
            Deduction theDeduction = work.DeductionRepository.GetByID(id);

            return View(theDeduction);
        }

        //
        // POST: /Salary/Delete/5

        [HttpPost]
        public ActionResult Delete(Deduction model)
        {
            try
            {
                // TODO: Add delete logic here
                work.DeductionRepository.Delete(model.DeductionID);
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
