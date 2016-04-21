using JobHustler.DAL;
using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TermController : Controller
    {
        //
        // GET: /Term/
        UnitOfWork work = new UnitOfWork();
        public ActionResult Index()
        {
            List<Term> theTerms = work.TermRepository.Get().ToList();
            if (theTerms.Count() > 0)
            {
                ViewBag.Count = 1;
                //return View();
            }
            return View(theTerms);
            //}

        }

        //
        // GET: /Term/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Term/Create

        public ActionResult Create()
        {
            List<Term> theTerms = work.TermRepository.Get().ToList();
            if (theTerms.Count() > 0)
            {
                return RedirectToAction("Index");
                //return View();
            }
            return View();
        }

        //
        // POST: /Term/Create

        [HttpPost]
        public ActionResult Create(Term model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    work.TermRepository.Insert(model);
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
        // GET: /Term/Edit/5

        public ActionResult Edit(int id)
        {
            Term theTerm = work.TermRepository.GetByID(id);

            return View(theTerm);
        }

        //
        // POST: /Term/Edit/5

        [HttpPost]
        public ActionResult Edit(Term model)
        {
            try
            {
                // TODO: Add update logic here
                if (ModelState.IsValid)
                {
                    work.TermRepository.Update(model);
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
        // GET: /Term/Delete/5

        public ActionResult Delete(int id)
        {
            Term theTerm = work.TermRepository.GetByID(id);
            return View(theTerm);
        }

        //
        // POST: /Term/Delete/5

        [HttpPost]
        public ActionResult Delete(Term model)
        {
            try
            {
                // TODO: Add delete logic here

             //   Term theTerm = work.TermRepository.GetByID(model.TermID);

                work.TermRepository.Delete(model);
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
