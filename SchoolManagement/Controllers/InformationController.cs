using JobHustler.DAL;
using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class InformationController : Controller
    {

        UnitOfWork work = new UnitOfWork();
        //
        // GET: /Information/

        public ActionResult Index()
        {
            if (User.IsInRole("Student"))
            {
                Membership.GetUser(true);
                string theUser = Convert.ToString(User.Identity.Name);
                Int32 theUser1 = Convert.ToInt32(User.Identity.Name);
                PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID == theUser1).First();
                List<Information> PersonalInfo = work.InformationRepository.Get(a => a.StudentID.Equals(theUser)).ToList();
                List<Information> theInfo = work.InformationRepository.Get(a => a.To.Equals(theStudent.PresentLevel)).ToList();

                PersonalInfo.AddRange(theInfo);

                return View(PersonalInfo.OrderByDescending(a => a.DateSent));
            }
            else
            {
                Membership.GetUser(true);
                string theUser = Convert.ToString(User.Identity.Name);
                List<Information> theInfo = work.InformationRepository.Get(a => a.SentBy.Equals(theUser)).ToList();
                return View(theInfo.OrderByDescending(a => a.DateSent));
            }
            return View();
        }

        //
        // GET: /Information/Details/5

        public ActionResult Details(int id)
        {
            Membership.GetUser(true);
            Information theInfo = work.InformationRepository.GetByID(id);

            Int32 theID = Convert.ToInt32(theInfo.SentBy);

           List<Person> thePersons = work.PersonRepository.Get(a => a.UserID == theID).ToList();

           if (thePersons.Count() > 0)
           {
               Person theRealPerson = thePersons[0];
               ViewBag.Name = " " + theRealPerson.FirstName + " " + theRealPerson.LastName;
           }
            return View(theInfo);
        }

        //
        // GET: /Information/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Information/Create

        [HttpPost]
        public ActionResult Create(Information model)
        {
            try
            {
                model.SentBy = Convert.ToString(User.Identity.Name);
                model.DateSent = DateTime.Now;
                if (ModelState.IsValid)
                {
                    work.InformationRepository.Insert(model);
                    work.Save();
                }
                // TODO: Add insert logic here
                Membership.GetUser(true);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Information/Edit/5

        public ActionResult Edit(int id)
        {
            Information theInfo = work.InformationRepository.GetByID(id);

            return View(theInfo);
        }

        //
        // POST: /Information/Edit/5

        [HttpPost]
        public ActionResult Edit(Information model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    work.InformationRepository.Update(model);
                    work.Save();
                    Membership.GetUser(true);
                }
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Information/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Information/Delete/5

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
