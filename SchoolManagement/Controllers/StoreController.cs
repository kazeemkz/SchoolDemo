using System;
using System.Collections.Generic;
using System.Data;
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
    [DynamicAuthorize]
    public class StoreController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        //
        // GET: /Store/

        //   public ActionResult LoadExamCodes(string sortOrder, string currentFilter, string ExamCode, string LevelString, string Visible, int? page)
        public ActionResult Index(string sortOrder, string currentFilter, string ItemClass, string ItemName, int? page)
        {



            int pageSize = 50;
            int pageNumber = (page ?? 1);
            List<Store> theItemStore = work.StoreRepository.Get().OrderByDescending(a => a.ItemName).ToList();


            if (!String.IsNullOrEmpty(ItemName))
            {
                theItemStore = theItemStore.Where(s => s.ItemName.ToUpper().Contains(ItemName.ToUpper())).ToList();

            }


            if (!String.IsNullOrEmpty(ItemClass))
            {
                theItemStore = theItemStore.Where(s => s.Level.Equals(ItemClass)).ToList();
            }





            ViewBag.Count = theItemStore.Count();
            //  return View("Index2", theExamCodes.ToPagedList(pageNumber, pageSize));
            return View(theItemStore.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /Store/Details/5

        public ActionResult Details(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            Store storeItem = work.StoreRepository.GetByID(theId);
            return View(storeItem);
        }

        //
        // GET: /Store/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Store/Create

        [HttpPost]
        public ActionResult Create(Store model)
        {
            if (String.IsNullOrEmpty(model.Level))
            {
                model.Level = "";
            }
            try
            {
                if (!(ModelState.IsValid))
                {
                    return View(model);
                }

                model.DateEntered = DateTime.Now;
                work.StoreRepository.Insert(model);
                work.Save();
                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Added an Item in the Store ,Item Name :-" + model.ItemName, UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
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
        // GET: /Store/Edit/5

        public ActionResult Edit(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            Store theStore = work.StoreRepository.GetByID(theId);
            return View(theStore);
        }

        //
        // POST: /Store/Edit/5

        [HttpPost]
        public ActionResult Edit(Store model)
        {

            if (String.IsNullOrEmpty(model.Level))
            {
                model.Level = "";
            }
            try
            {
                if (ModelState.IsValid)
                {
                    work.StoreRepository.Update(model);
                    work.Save();
                    // TODO: Add update logic here
                    if (User.Identity.Name != "5000001")
                    {
                        AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Edited an Item in the Store ,Item Name :-" + model.ItemName, UserID = User.Identity.Name };
                        work.AuditTrailRepository.Insert(audit);
                        work.Save();
                    }

                }
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");

            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Store/Delete/5

        public ActionResult Delete(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            Store theItem = work.StoreRepository.GetByID(theId);
            return View(theItem);
        }

        //
        // POST: /Store/Delete/5

        [HttpPost]
        public ActionResult Delete(Store model)
        {
            try
            {
                // TODO: Add delete logic here
              string name =  model.ItemName;
                Store theStoreItem = work.StoreRepository.GetByID(model.StoreID);

                work.StoreRepository.Delete(theStoreItem);
                work.Save();
                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Deleted an Item in the Store ,Item Name :-" + name, UserID = User.Identity.Name };
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
