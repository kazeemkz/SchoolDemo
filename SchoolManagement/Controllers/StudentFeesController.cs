using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobHustler.DAL;
using JobHustler.Models;
using SchoolManagement.Model;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [DynamicAuthorize]
    public class StudentFeesController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        // StudentFeeViewModel feeModel = new StudentFeeViewModel();
        //
        // GET: /StudentFees/
        // [DynamicAuthorize]
        //  public ActionResult Index(string name = "NURSERY 1", string term = "1")
        public ActionResult Index(string theFeeTypes, string term)
        {
            //List<Level> theLevels = work.LevelRepository.Get().ToList();
            //List<SelectListItem> theItem = new List<SelectListItem>();


            List<SchoolFeesType> theFeeType = work.SchoolFeesTypeRepository.Get().ToList();
            List<SelectListItem> theItemFeeType = new List<SelectListItem>();
            foreach (var t in theFeeType)
            {
                theItemFeeType.Add(new SelectListItem() { Text = t.SchoolFeesKind, Value = t.SchoolFeesKind });
            }
            ViewData["theFeeType"] = theFeeType;


            var fees = work.StudentFeesRepository.Get();
            //select s;
            if (String.IsNullOrEmpty(theFeeTypes) && String.IsNullOrEmpty(term))
            {
                //SchoolFeesType sft =  work.SchoolFeesTypeRepository.Get(a => a.SchoolFeesKind == "theFeeTypes").First();
                fees = fees.Where(s => s.SchoolFeesTypeID == 0);
            }

            if (!String.IsNullOrEmpty(theFeeTypes))
            {
                SchoolFeesType sft = work.SchoolFeesTypeRepository.Get(a => a.SchoolFeesKind == "theFeeTypes").First();
                fees = fees.Where(s => s.SchoolFeesTypeID == sft.SchoolFeesTypeID);
            }
            if (!String.IsNullOrEmpty(term))
            {
                fees = fees.Where(s => s.Term == term);
            }

            var viewModel = new StudentFeeViewModel();
            //  List<StudentFees>  theFees =  work.StudentFeesRepository.Get(a=>a.Level.Equals(name)).OrderBy(a=>a.Level).ToList();

            //if (name != "")
            //{
            // viewModel.Level = name;
            viewModel.StudentFees = fees.ToList();
            // }

            //foreach(var fee in theFees)
            //{
            //    viewModel.
            //}
            return View(viewModel);
        }

        //
        // GET: /StudentFees/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /StudentFees/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /StudentFees/Create

        [HttpPost]
        public ActionResult Create(StudentFees model)
        {
            try
            {
                if (!(ModelState.IsValid))
                {
                    return View(model);
                }

                work.StudentFeesRepository.Insert(model);
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
        // GET: /StudentFees/Edit/5

        public ActionResult Edit(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            StudentFees feeItem = work.StudentFeesRepository.GetByID(theId);

            return View(feeItem);
        }

        //
        // POST: /StudentFees/Edit/5

        [HttpPost]
        public ActionResult Edit(StudentFees model)
        {
            try
            {
                work.StudentFeesRepository.Update(model);
                work.Save();
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /StudentFees/Delete/5

        public ActionResult Delete(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            StudentFees feeItem = work.StudentFeesRepository.GetByID(theId);
            return View(feeItem);
        }

        //
        // POST: /StudentFees/Delete/5

        [HttpPost]
        public ActionResult Delete(StudentFees model)
        {
            try
            {
                // TODO: Add delete logic here
                work.StudentFeesRepository.Delete(model);
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
