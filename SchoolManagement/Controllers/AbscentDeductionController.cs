using JobHustler.DAL;
using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Controllers
{
     [Authorize]
    public class AbscentDeductionController : Controller
    {
        //
        // GET: /AbscentDeduction/
        UnitOfWork work = new UnitOfWork();
        public ActionResult Index()
        {
            List<AbscentDeduction> theAbscentDeduction = work.AbscentDeductionRepository.Get().ToList();
            if (theAbscentDeduction.Count() > 0)
            {
                ViewBag.Count = 1;
                //return View();
            }
            return View(theAbscentDeduction);
        }

        //
        // GET: /AbscentDeduction/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /AbscentDeduction/Create

        public ActionResult Create()
        {

            List<AbscentDeduction> theAbscentDeduction = work.AbscentDeductionRepository.Get().ToList();
            if (theAbscentDeduction.Count() > 0)
            {
                return RedirectToAction("Index");
                //return View();
            }
            return View();
        }

        //
        // POST: /AbscentDeduction/Create

        [HttpPost]
        public ActionResult Create(AbscentDeduction model)
        {
            try
            {
                // TODO: Add insert logic here

                if (ModelState.IsValid)
                {

                    work.AbscentDeductionRepository.Insert(model);
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
        // GET: /AbscentDeduction/Edit/5

        public ActionResult Edit(int id)
        {
            AbscentDeduction theDeduction = work.AbscentDeductionRepository.GetByID(id);
            return View(theDeduction);
        }

        //
        // POST: /AbscentDeduction/Edit/5

        [HttpPost]
        public ActionResult Edit(AbscentDeduction model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {

                    work.AbscentDeductionRepository.Update(model);
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
        // GET: /AbscentDeduction/Delete/5

        public ActionResult Delete(int id)
        {
            AbscentDeduction theDeduction = work.AbscentDeductionRepository.GetByID(id);
            return View(theDeduction);
        }

        //
        // POST: /AbscentDeduction/Delete/5

        [HttpPost]
        public ActionResult Delete(AbscentDeduction model)
        {
            try
            {
                // TODO: Add delete logic here
                work.AbscentDeductionRepository.Delete(model);
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
