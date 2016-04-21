using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobHustler.DAL;
using JobHustler.Models;
using SchoolManagement.Model;
using PagedList;
//using PagedList.Mvc;

namespace SchoolManagement.Controllers
{
    [Authorize]
    [DynamicAuthorize]
    public class LevelController : Controller
    {
        //
        // GET: /Level/
        UnitOfWork work = new UnitOfWork();
        public ActionResult Index(string sortOrder, string currentFilter, string LevelString, int? page)
       // public ActionResult Index()
        {

            var level = from s in work.LevelRepository.Get().OrderBy(a => a.LevelName)
                               select s;
            if (!String.IsNullOrEmpty(LevelString))
            {
              //  string theCode = ExamCode.ToLower();
                level = level.Where(s => s.LevelName.Equals(LevelString));

            }
            int pageSize = 40;
            int pageNumber = (page ?? 1);

            ViewBag.Count = level.Count();
      //   List<Level>  theLevels =   work.LevelRepository.Get().OrderBy(a=>a.LevelName).ToList();
         return View(level.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Level/Details/5

        public ActionResult Details(int id)
        {
        Level theLevel =     work.LevelRepository.GetByID(id);
        return View(theLevel);
        }

        //
        // GET: /Level/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Level/Create

        [HttpPost]
        public ActionResult Create(Level model)
        {
            try
            {
                if (!(ModelState.IsValid))
                {
                    return View(model);
                }
                // TODO: Add insert logic here
              List<Level> theLevel =  work.LevelRepository.Get(a=>a.LevelName.Equals(model.LevelName) && a.Type.Equals(model.Type) ).ToList();
              if (theLevel.Count > 0)
              {
                  ModelState.AddModelError("", "An Arm of this Class has Already been Created");
                  return View(model);
              }
              else
              {

                  work.LevelRepository.Insert(model);
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
        // GET: /Level/Edit/5

        public ActionResult Edit(string id)
        {

            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            Level theLevel = work.LevelRepository.GetByID(theId);
          return View(theLevel);
        }

        //
        // POST: /Level/Edit/5

        [HttpPost]
        public ActionResult Edit(Level model)
        {
            try
            {
                // TODO: Add update logic here

                if (!(ModelState.IsValid))
                {
                    return View(model);
                }

                // TODO: Add insert logic here
                List<Level> theLevel = work.LevelRepository.Get(a => a.LevelName.Equals(model.LevelName) && a.Type.Equals(model.Type)).ToList();
                if (theLevel.Count > 0)
                {
                    ModelState.AddModelError("", "An Arm of this Class has Already been Created");
                    return View(model);
                }


                work.LevelRepository.Update(model);
                work.Save();
                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Edited Class", UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
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
        // GET: /Level/Delete/5

        public ActionResult Delete(int id)
        {
            Level theLevel = work.LevelRepository.GetByID(id);
            return View(theLevel);
           // return View();
        }

        //
        // POST: /Level/Delete/5

        [HttpPost]
        public ActionResult Delete(Level model)
        {
            try
            {
                string theL = model.LevelName;
                // TODO: Add delete logic here
                work.LevelRepository.Delete(model);
                work.Save();
                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Deleted a Class Name:-"+ theL , UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
                    work.Save();
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
