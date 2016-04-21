using JobHustler.DAL;
using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using JobHustler.Models;
//using PagedList.Mvc;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        //
        // GET: /Post/
        UnitOfWork work = new UnitOfWork();
        [DynamicAuthorize]
        public ActionResult Index(int? page, string sortOrder, string currentFilter)
        {
            int pageSize = 40;
            int pageNumber = (page ?? 1);

            Int32 theUserName = Convert.ToInt32(User.Identity.Name);

            Person theStudent = work.PersonRepository.Get(a => a.UserID == theUserName).First();

            if (theStudent is PrimarySchoolStudent)
            {

                PrimarySchoolStudent theStudent1 = work.PrimarySchoolStudentRepository.Get(a => a.UserID == theUserName).First();
                List<Post> thePost = work.PostRepository.Get(a => a.Class == theStudent1.LevelType).ToList();
                //List<Post> thePost = work.PostRepository.Get(a => a.Level.Equals(theStudent1.PresentLevel) || a.Class == theStudent1.PresentLevel).ToList();
                //  return View();
                //  List<Post> thePost = work.PostRepository.Get(a =>a.Class.Equals(theStudent1.PresentLevel)).ToList();

                return View(thePost.OrderByDescending(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
                //  return View(thePost.OrderByDescending(a => a.DateCreated));

            }
            if (theStudent is PrimarySchoolStaff)
            {
                PrimarySchoolStaff staff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == theUserName).First();
                // List<Post> thePostStudent = new List<Post>();


                // staff.
                List<Post> thePost = work.PostRepository.Get(a => a.Level.Equals("Non-Student")).ToList();

                //if (staff.ClassTeacherOf != null)
                //{

                //    string[] theClass = staff.ClassTeacherOf.Split(':');
                //    if (theClass[0] != null)
                //    {
                //        string teacherClass = theClass[0];
                //        List<Post> thePostStudent = work.PostRepository.Get(a => a.Level.Equals(teacherClass)).ToList();
                //        thePost.AddRange(thePostStudent);
                //        //    ViewBag.Delete = "True";
                //    }


                //}
                //  List<Post> thePostStudent = work.PostRepository.Get(a => a.Level).ToList();
                //  return View();
                return View(thePost.OrderByDescending(a => a.DateCreated).ToPagedList(pageNumber, pageSize));
                // return View(thePost.OrderByDescending(a => a.DateCreated));
            }

            return View();
        }

        //
        // GET: /Post/Details/5
        [DynamicAuthorize]
        public ActionResult Details(string id)
        {
            Int32 theId = 0;
            try
            {
                 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            }
            catch (Exception e) {
              string theNewId =  Models.Encript.EncryptString(id, true);
              theId = Convert.ToInt32(Models.Encript.DecryptString(theNewId, true));
            }
            Post thePost = work.PostRepository.GetByID(theId);
            return View(thePost);
        }

        //
        // GET: /Post/Create
        [DynamicAuthorize]
        public ActionResult Create()
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

        //
        // POST: /Post/Create

        [HttpPost]
        [DynamicAuthorize]
        public ActionResult Create(Post model)
        {
            if (User.IsInRole("Student"))
            {
                Int32 userId = Convert.ToInt32(User.Identity.Name);
                PrimarySchoolStudent theUser = work.PrimarySchoolStudentRepository.Get(a => a.UserID == userId).First();

                model.Class = theUser.LevelType;
            }
            if (string.IsNullOrEmpty(model.Class))
            {
                List<Level> theLevels = work.LevelRepository.Get().ToList();
                List<SelectListItem> theItem = new List<SelectListItem>();
                theItem.Add(new SelectListItem() { Text = "None", Value = "" });

                foreach (var level in theLevels)
                {
                    theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
                }

                ViewData["arm"] = theItem;
                ModelState.AddModelError("", " Please Choose a Class Arm!");
                return View();
            }

            Int32 theUserName = Convert.ToInt32(User.Identity.Name);
            Person thePerson = work.PersonRepository.Get(a => a.UserID == theUserName).First();
            model.DateCreated = DateTime.Now;
            if (thePerson is PrimarySchoolStudent)
            {
                PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.Get(a => a.UserID == theUserName).First();
                // PrimarySchoolStudent new 
                model.Level = theStudent.PresentLevel;
            }
            else
            {
                model.Level = "Non-Student";
            }
            model.PosterName = thePerson.UserID;
            model.RealName = thePerson.FirstName + " " + thePerson.LastName;
            try
            {
                if (ModelState.IsValid)
                {
                    work.PostRepository.Insert(model);
                    work.Save();
                }
                // TODO: Add insert logic here

                return RedirectToAction("Index");
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
        // GET: /Post/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Post/Edit/5

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
        // GET: /Post/Delete/5

        public ActionResult Delete(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            Post thePost = work.PostRepository.GetByID(theId);

            work.PostRepository.Delete(thePost);
            work.Save();
            return RedirectToAction("Index");
            //  return View();
        }

        //
        // POST: /Post/Delete/5

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
