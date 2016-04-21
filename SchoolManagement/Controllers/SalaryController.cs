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
      [Authorize]
    public class SalaryController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        //

        // GET: /Salary/

        public ActionResult Index(int? page)
        {
            List<Salary> theSalaries = work.SalaryRepository.Get().ToList();
            int pageSize = 50;
            int pageNumber = (page ?? 1);

            ViewBag.Count = theSalaries.Count();
            return View(theSalaries.ToPagedList(pageNumber, pageSize));
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
        public ActionResult Create(Salary model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    work.SalaryRepository.Insert(model);
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
            Salary theSalary = work.SalaryRepository.GetByID(id);
            return View(theSalary);
        }

        //
        // POST: /Salary/Edit/5

        [HttpPost]
        public ActionResult Edit(Salary model)
        {
            try
            {
                // TODO: Add update logic here
                if (TryUpdateModel(model))
                {

                    work.SalaryRepository.Update(model);
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
            Salary theSalary = work.SalaryRepository.GetByID(id);

            return View(theSalary);
        }

        //
        // POST: /Salary/Delete/5

        [HttpPost]
        public ActionResult Delete(Salary model)
        {
            try
            {
                // TODO: Add delete logic here
                work.SalaryRepository.Delete(model.SalaryID);
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
