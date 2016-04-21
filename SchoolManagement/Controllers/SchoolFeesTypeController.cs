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
    public class SchoolFeesTypeController : Controller
    {
        //
        // GET: /SchoolFeesType/
        UnitOfWork work = new UnitOfWork();
        public ActionResult Index()
        {
            List<SchoolFeesType> theFeeType = work.SchoolFeesTypeRepository.Get().ToList();
            List<SelectListItem> theItemFeeType = new List<SelectListItem>();
            foreach (var t in theFeeType)
            {
                theItemFeeType.Add(new SelectListItem() { Text = t.SchoolFeesKind, Value = t.SchoolFeesKind });
            }
            ViewData["theFeeType"] = theFeeType;
            List<SchoolFeesType> theSchoolFeesType = work.SchoolFeesTypeRepository.Get().ToList();
            return View(theSchoolFeesType);
        }

        //
        // GET: /SchoolFeesType/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SchoolFeesType/Create

        public ActionResult Create()
        {
          

            return View();
        }

        //
        // POST: /SchoolFeesType/Create

        [HttpPost]
        public ActionResult Create(SchoolFeesType model)
        {
            model.SchoolFeesKind = model.SchoolFeesKind.TrimEnd().TrimStart();
            List<SchoolFeesType> sct = new List<SchoolFeesType>();
            sct = work.SchoolFeesTypeRepository.Get(a => a.SchoolFeesKind.ToLower() == model.SchoolFeesKind.TrimEnd().TrimStart().ToLower()).ToList();
            try
            {
                if (sct.Count() > 0)
                {
                    ModelState.AddModelError("", "A School Fee Type has already been created!");
                    return View();
                }
                if (TryUpdateModel(model))
                {
                    work.SchoolFeesTypeRepository.Insert(model);
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
        // GET: /SchoolFeesType/Edit/5

        public ActionResult Edit(int id)
        {
            SchoolFeesType theType = work.SchoolFeesTypeRepository.GetByID(id);
            return View(theType);
        }

        //
        // POST: /SchoolFeesType/Edit/5

        [HttpPost]
        public ActionResult Edit(SchoolFeesType model)
        {
            string schoolFeeTy = model.SchoolFeesKind;
            model.SchoolFeesKind = model.SchoolFeesKind.TrimEnd().TrimStart();
            List<SchoolFeesType> sct = new List<SchoolFeesType>();
            sct = work.SchoolFeesTypeRepository.Get(a => a.SchoolFeesKind.ToLower() == schoolFeeTy.TrimEnd().TrimStart().ToLower()).ToList();

            try
            {
                if (sct.Count() > 0)
                {
                    SchoolFeesType scr = sct[0];
                    if (scr.Amount == model.Amount)
                    {
                        ModelState.AddModelError("", "A School Fee Type with same amount has already been created!");
                        return View();
                    }
                }
                if (ModelState.IsValid)
                {
                    UnitOfWork work2 = new UnitOfWork();
                    work2.SchoolFeesTypeRepository.Update(model);
                    work2.Save();
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
        // GET: /SchoolFeesType/Delete/5

        public ActionResult Delete(int id)
        {
            SchoolFeesType theType = work.SchoolFeesTypeRepository.GetByID(id);
            return View(theType);
        }

        //
        // POST: /SchoolFeesType/Delete/5

        [HttpPost]
        public ActionResult Delete(SchoolFeesType model)
        {
            try
            {
                // TODO: Add delete logic here
                work.SchoolFeesTypeRepository.Delete(model);
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
