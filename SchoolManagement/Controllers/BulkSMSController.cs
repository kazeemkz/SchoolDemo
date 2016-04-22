using JobHustler.DAL;
using JobHustler.Models;
using SchoolManagement.Model;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Controllers
{
     [DynamicAuthorize]
    [Authorize]
    public class BulkSMSController : Controller
    {
        //
        // GET: /BulkSMS/
        UnitOfWork work = new UnitOfWork();

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /BulkSMS/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /BulkSMS/Create
          [DynamicAuthorize]
        public ActionResult Create()
        {
            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();
            theItem.Add(new SelectListItem() { Text = "None", Value = "" });

            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type }); ;
            }

            ViewData["arm"] = theItem;
            return View();
        }

        //
        // POST: /BulkSMS/Create

        [HttpPost]
       // [AsyncTimeout(20000)]
       // public async Task<ActionResult> Create(BulkSMS model)
        public ActionResult Create(BulkSMS model)
        {

            try
            {
                // TODO: Add insert logic here
                //var answer = await SendBulkSMS.SendNow(model);
                 var answer = SendBulkSMS.SendNow(model);
                //  await answer.GetRequestStreamAsync();
                // await Task.WhenAll(SendBulkSMS.SendNow(model));
               // Task<dynamic> task = SendBulkSMS.SendNow(model);
               // dynamic result = await task;
                return RedirectToAction("Create");
            }
            catch
            {
                List<Level> theLevels = work.LevelRepository.Get().ToList();
                List<SelectListItem> theItem = new List<SelectListItem>();
                theItem.Add(new SelectListItem() { Text = "None", Value = "" });

                foreach (var level in theLevels)
                {
                    theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
                }

                ViewData["arm"] = theItem;
                return View();
            }
        }

        //
        // GET: /BulkSMS/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /BulkSMS/Edit/5

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
        // GET: /BulkSMS/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /BulkSMS/Delete/5

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
