using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.DAL;
using SchoolManagement.Model;
using System.Web.Configuration;
using JobHustler.DAL;
using JobHustler.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class ReadContentController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        Dictionary<string, Dictionary<string, string>> ChapterSubChap = new Dictionary<string, Dictionary<string, string>>();
        Dictionary<string, string> theInner = new Dictionary<string, string>();
        List<SelectListItem> cours = new List<SelectListItem>();

        public ActionResult LoadCourse(string level)
        {

            char theLeve = level.Last();
            string theNew = level.Replace(theLeve, ' ');
            level = theNew + theLeve;

            List<Level> theLevels = work.LevelRepository.Get(a => a.LevelName.Equals(level)).ToList();
            Level theLe = theLevels[0];
            //  List<Chapter> theChapters = work.ChapterRepository.Get(a => a.Course.Name.Equals(theCourse) && theLe.LevelName.Equals(theLevel)).ToList();
            List<Course> theCourses = work.CourseRepository.Get(a => a.LevelID == theLe.LevelID).ToList();
            //  List<Course> theCourses = work.CourseRepository.Get(a => a.Level.LevelName.Equals(level)).ToList();
            List<SelectListItem> cours = new List<SelectListItem>();



            foreach (var co in theCourses)
            {
                string physicalPathPdf = HttpContext.Server.MapPath("../") + "UploadPdf" + "\\";
                List<Chapter> theChapters = work.ChapterRepository.Get(a => a.Course.CourseID == co.CourseID).OrderBy(a => a.Number).ToList();
                //List<TextBook> theTextBooks =  work.TextBookRepository.Get(a => a.CourseID == co.CourseID).ToList();

                foreach (var chap in theChapters)
                {
                    theInner.Add(co.Name + ":" + chap.Name + " Chapter " + chap.Number, chap.ParentName);
                    List<AdditionalChapterText> theAdditionalChapters = work.AdditionalChapterTextRepository.Get(a => a.ChapterID == chap.ChapterID).ToList();
                    foreach (var addChap in theAdditionalChapters)
                    {
                        theInner.Add(co.Name + ":" + addChap.Name + " Additional Material", addChap.ParentName);
                    }

                }
                List<TextBook> theTextBooks = work.TextBookRepository.Get(a => a.CourseID == co.CourseID).ToList();
                foreach (var textBook in theTextBooks)
                {
                    theInner.Add(co.Name + ":" + textBook.Name + " TextBook", textBook.ParentName);
                }
                // ChapterSubChap.Add(co.Name, theInner);
                // cours.Add(new SelectListItem { Text = co.Name, Value = co.Name });

            }
            foreach (var t in theInner)
            {

                cours.Add(new SelectListItem { Text = t.Key, Value = t.Value });
            }
            // return Json(ChapterSubChap, JsonRequestBehavior.AllowGet);
            return Json(cours, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Report(string stringName)
        {
            // stringName =  stringName.Remove(' ');

            List<Chapter> theChapter = work.ChapterRepository.Get(a => a.ParentName.Equals(stringName)).ToList();
            List<TextBook> theTextBook = work.TextBookRepository.Get(a => a.ParentName.Equals(stringName)).ToList();
            List<AdditionalChapterText> addtionalChapter = work.AdditionalChapterTextRepository.Get(a => a.ParentName.Equals(stringName)).ToList();
            byte[] _Buffer = null;
            byte[] fileData = null;


            string Usepath = WebConfigurationManager.AppSettings["usehardisk"];

            if (Usepath != "YES")
            {
                // string physicalPathPdf = HttpContext.Server.MapPath("../") + "UploadPdf/";// +"\\";physicalPathPdf + stringName

                string physicalPathPdf = WebConfigurationManager.AppSettings["harddisk"] + ":\\upload";

                //  string SaveLocationPdf = string.Format("{0}\\{1}", path, model.Level + "-" + model.Subject + "-" + "Chapter" + model.Chapter + "-" + model.TopicTitle + ".pdf");
                // Request.Files[0].SaveAs(SaveLocationPdf);//.SaveAs(SaveLocation);

                try
                {
                    if (theChapter.Count > 0)
                    {
                        fileData = (byte[])theChapter.First().FileData.ToArray();
                    }

                    if (theTextBook.Count > 0)
                    {
                        fileData = (byte[])theTextBook.First().FileData.ToArray();
                    }

                    if (addtionalChapter.Count > 0)
                    {
                        fileData = (byte[])addtionalChapter.First().FileData.ToArray();
                    }


                }

                catch (Exception _Exception)
                {

                    // Error

                    Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());

                }


                return File(fileData, "application/pdf");
            }

            else
            {

                string physicalPathPdf = WebConfigurationManager.AppSettings["harddisk"] + ":\\upload";


                try
                {

                }

                catch (Exception _Exception)
                {

                    // Error

                    Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());

                }


                string fileNamePath = physicalPathPdf + "/" + stringName;
                return File(fileNamePath, "application/pdf");

            }



        }
        //
        // GET: /ReadContent/
        [DynamicAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /ReadContent/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ReadContent/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ReadContent/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ReadContent/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ReadContent/Edit/5

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
        // GET: /ReadContent/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ReadContent/Delete/5

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
