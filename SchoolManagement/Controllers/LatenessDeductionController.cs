using JobHustler.DAL;
using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Controllers
{
    public class LatenessDeductionController : Controller
    {
        //
        // GET: /LatenessDeduction/
        UnitOfWork work = new UnitOfWork();
        public ActionResult Index()
        {
            List<LatenessDeduction> theLatenessDeduction = work.LatenessDeductionRepository.Get().ToList();
            if (theLatenessDeduction.Count() > 0)
            {
                ViewBag.Count = 1;
                //return View();
            }
            return View(theLatenessDeduction);
        }

        //
        // GET: /LatenessDeduction/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /LatenessDeduction/Create

        public ActionResult Create()
        {
            List<LatenessDeduction> theLatenessDeduction = work.LatenessDeductionRepository.Get().ToList();
            if (theLatenessDeduction.Count() > 0)
            {
                return RedirectToAction("Index");
                //return View();
            }
            return View();
        }

        //
        // POST: /LatenessDeduction/Create

        [HttpPost]
        public ActionResult Create(LatenessDeduction model)
        {
            try
            {
                // TODO: Add insert logic here

                if (ModelState.IsValid)
                {

                    work.LatenessDeductionRepository.Insert(model);
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
        // GET: /LatenessDeduction/Edit/5

        public ActionResult Edit(int id)
        {
            LatenessDeduction theDeduction = work.LatenessDeductionRepository.GetByID(id);
            return View(theDeduction);
        }

        //
        // POST: /LatenessDeduction/Edit/5

        [HttpPost]
        public ActionResult Edit(LatenessDeduction model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {

                    work.LatenessDeductionRepository.Update(model);
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
        // GET: /LatenessDeduction/Delete/5

        public ActionResult Delete(int id)
        {
            LatenessDeduction theDeduction = work.LatenessDeductionRepository.GetByID(id);
            return View(theDeduction);
        }

        //
        // POST: /LatenessDeduction/Delete/5

        [HttpPost]
        public ActionResult Delete(LatenessDeduction model)
        {
            try
            {
                // TODO: Add delete logic here
                work.LatenessDeductionRepository.Delete(model);
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
