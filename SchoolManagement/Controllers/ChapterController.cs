using SchoolManagement.Model;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.DAL;
using JobHustler.DAL;
using System.Text;
using JobHustler.Models;
using System.IO;

namespace SchoolManagement.Controllers
{
     [Authorize]
    public class ChapterController : Controller
    {
        //

        UnitOfWork work = new UnitOfWork();
        // GET: /Chapter/

        public ActionResult Index(int id = 0)
        {
            ViewBag.Success = id.ToString();
            return View();
        }

        //Delete Chapter
        [DynamicAuthorize]
        public ActionResult DeleteChapter(string stringName)
        {
            //   char theLeve = stringName.Last();
            // string theNew = stringName.Replace(theLeve, ' ');
            // stringName = theNew + theLeve;

            //   string physicalPathPdf = HttpContext.Server.MapPath("../") + "UploadPdf";

            // string physicalPathPdf = WebConfigurationManager.AppSettings["harddisk"] + ":\\upload"; 

            // File.Delete(Server.MapPath(YourVirtualPath));

            try
            {

                //List<AdditionalChapterText> theAddChapterText = work.AdditionalChapterTextRepository.Get(a => a.ParentName.Equals(stringName)).ToList();

                //List<TextBook> theTextBook = work.TextBookRepository.Get(a => a.ParentName.Equals(stringName)).ToList();

                //List<Chapter> theChapter = work.ChapterRepository.Get(a => a.ParentName.Equals(stringName)).ToList();

                //if (theAddChapterText.Count() > 0)
                //{
                //    AdditionalChapterText theAC = theAddChapterText[0];
                //    work.AdditionalChapterTextRepository.Delete(theAC);
                //    work.Save();
                //}

                //if (theTextBook.Count() > 0)
                //{
                //    TextBook theText = theTextBook[0];
                //    work.TextBookRepository.Delete(theText);
                //    work.Save();
                //}

                //if (theChapter.Count() > 0)
                //{
                //    Chapter theChap = theChapter[0];
                //    work.ChapterRepository.Delete(theChap);
                //    work.Save();
                //}


                //delete a textbook else delete a chapter or a subchapter
                string endText = "TextBook.pdf";
                string endAdditonalChapterText = "Additional-Text.pdf";

                if (stringName.EndsWith(endAdditonalChapterText))
                {
                    //  string rootFolderPath = physicalPathPdf;
                    //  string filesToDelete = stringName;   // Only delete DOC files containing "DeleteMe" in their filenames
                    //  string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                    // foreach (string file in fileList)
                    //  {
                    //   System.IO.File.Delete(file);
                    AdditionalChapterText theAddCapText = work.AdditionalChapterTextRepository.Get(a => a.ParentName.Equals(stringName)).Single();
                    work.AdditionalChapterTextRepository.Delete(theAddCapText);
                    work.Save();
                    // }
                }
                if (stringName.EndsWith(endText))
                {
                    //  string rootFolderPath = physicalPathPdf;
                    //  string filesToDelete = stringName;   // Only delete DOC files containing "DeleteMe" in their filenames
                    // string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                    // foreach (string file in fileList)
                    // {
                    //   System.IO.File.Delete(file);
                    TextBook theCap = work.TextBookRepository.Get(a => a.ParentName.Equals(stringName)).Single();
                    work.TextBookRepository.Delete(theCap);
                    work.Save();
                    // }
                    //if (fileList.Count() == 0)
                    //{
                    //    TextBook theCap = work.TextBookRepository.Get(a => a.ParentName.Equals(stringName)).Single();
                    //    if (theCap != null)
                    //    work.TextBookRepository.Delete(theCap);
                    //    work.Save();
                    //}
                }
                else
                // if (stringName.EndsWith(endText))
                {
                    //string rootFolderPath = physicalPathPdf;
                    //string filesToDelete = stringName;   // Only delete DOC files containing "DeleteMe" in their filenames
                    //string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToDelete);
                    //foreach (string file in fileList)
                    //{

                    //    System.IO.File.Delete(file);
                    Chapter theCap = work.ChapterRepository.Get(a => a.ParentName.Equals(stringName)).Single();

                    //when u delete a chapter, you must delete its additional chapters under it
                    List<AdditionalChapterText> theAddChapt = work.AdditionalChapterTextRepository.Get(a => a.ChapterID == theCap.ChapterID).ToList();

                    // string rootFolderPath1 = physicalPathPdf;

                    foreach (var theCha in theAddChapt)
                    {
                        work.AdditionalChapterTextRepository.Delete(theCha);
                        // string filesToDelete2 = stringName;   // Only delete DOC files containing "DeleteMe" in their filenames
                        // string[] fileList3 = System.IO.Directory.GetFiles(rootFolderPath1, theCha.ParentName);
                        // foreach (string fil1e in fileList3)
                        //{
                        //  System.IO.File.Delete(fil1e);
                        // }
                    }
                    work.ChapterRepository.Delete(theCap);
                    work.Save();
                }
                // }

            }

            catch (IOException e)
            {

            }

            return RedirectToAction("Index", new { id = 1 });
            // return View(stringName);

        }

        public ActionResult ViewChapterPartial(string CoureLevel)
        {
            string[] theCoursLevel = CoureLevel.Split(':');
            string theLevel = theCoursLevel[0];

            //string theLevel = theCoursLevel[0];
            char theLeve = theLevel.Last();
            string theNew = theLevel.Replace(theLeve, ' ');
            theLevel = theNew + theLeve;

            string theCourse = theCoursLevel[1];
          
            List<Level> level = work.LevelRepository.Get(a => a.LevelName.Equals(theLevel)).ToList();
            int theNewLevel = level[0].LevelID;
            List<Course> course = work.CourseRepository.Get(a => a.Name.Equals(theCourse) && a.LevelID == theNewLevel).ToList();

            Course theNewCourse = course[0];

            List<Chapter> theChapters = work.ChapterRepository.Get(a =>a.CourseID == theNewCourse.CourseID).OrderBy(a=>a.Number).ToList();

            //List<Chapter> theChapters = work.ChapterRepository.Get(a => a.Course.Name.Equals(theCourse) && a.Course.Name.Equals(theLevel)).ToList();
            // List<Chapter> theChapters = work.ChapterRepository.Get(a => a.Course.Name.Equals(theCourse)).ToList();
            return PartialView("_IndexPartial", theChapters);
        }

        //
        // GET: /Chapter/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Chapter/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Chapter/Create

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
        // GET: /Chapter/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Chapter/Edit/5

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
        // GET: /Chapter/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Chapter/Delete/5

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
