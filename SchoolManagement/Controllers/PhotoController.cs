using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using JobHustler.DAL;
using SchoolManagement.Model;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    [Authorize]
    public class PhotoController : Controller
    {
        //
        UnitOfWork work = new UnitOfWork();
        // GET: /Photo/



        [HttpPost]
        public string Upload(int QuestionID, int OnlineExamID, HttpPostedFileBase file)
        // public string Upload(int id)
        {

            QuestionPhoto photo = new QuestionPhoto();
            photo.OnlineExamID = OnlineExamID;
            photo.QuestionID = QuestionID;
            // photo = QuestionID;
            // photo.i

            Question theQuestion = work.QuestionRepository.Get(a => a.OnlineExamID == OnlineExamID && a.QuestionID == QuestionID).First();

            theQuestion.HasImage = true;
            work.QuestionRepository.Update(theQuestion);
            work.Save();



            //   Photo thePhoto = new Photo();
            //  thePhoto.BookID = id;


            //  UploadedFile file = RetrieveFileFromRequest();
            // string savePath = string.Empty;
            // string virtualPath = SaveFile(file);



            using (var stream = new MemoryStream())
            {
                Request.InputStream.CopyTo(stream);
                photo.FileData = stream.ToArray();

            }




            List<QuestionPhoto> thePhotos = work.QuestionPhotoRepository.Get(a => a.QuestionID == QuestionID && a.OnlineExamID == OnlineExamID).ToList();
            if (thePhotos.Count() > 0)
            {
                // UnitOfWork work2 = new UnitOfWork();
                QuestionPhoto theOldPhoto = thePhotos[0];
                work.QuestionPhotoRepository.Delete(theOldPhoto);
                work.Save();

            }

            work.QuestionPhotoRepository.Insert(photo);
            work.Save();


            return null;
        }

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Photo/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: /Photo/Create

        public ActionResult Create2(int questionID, int onlineExamID)
        {

            QuestionPhoto thePhoto = new QuestionPhoto();
            thePhoto.QuestionID = questionID;
            thePhoto.OnlineExamID = onlineExamID;
            // thePhoto.BookID = id;
            // return View();
            return View("QuestionPhoto", thePhoto);
        }

        //
        // GET: /Photo/Create

        public ActionResult Create(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            Photo thePhoto = new Photo();
            thePhoto.PersonID = theId;
            // ViewBag.ID = "Staff";

            return View("Create", thePhoto);
        }

        [HttpPost]
        // public virtual ActionResult Snapshot(int id)
        public virtual ActionResult Snapshot(int id, string image)
        {
            List<Photo> thePersonphotos = work.PhotoRepository.Get(a => a.PersonID == id).ToList();



            //// TODO: I am saving the image on the hard disk but
            //// you could do whatever processing you want with it
            //System.IO.File.WriteAllBytes(Server.MapPath("~/app_data/capture.png"), buffer);
            //return Json(new { success = true });

            if (thePersonphotos.Count == 0)
            {


                Photo theUserpoto = new Photo();
                theUserpoto.PersonID = id;
                image = image.Substring("data:image/png;base64,".Length);
                var buffer = Convert.FromBase64String(image);




                Stream stream = new MemoryStream(buffer.ToArray());


                Stream theResizedStream = PhotoResize.GenerateThumbnails(0.3, stream);

                byte[] theBytePix = PhotoResize.ToByteArray(theResizedStream);

                // Stream strm = fileupload1.PostedFile.InputStream;

                theUserpoto.PhotoImage = theBytePix;


                // byte[] imageByte = buffer.ToArray();
                //  theUserpoto.PhotoImage = buffer.ToArray();


                //   work.PhotoRepository.Insert(theUserpoto);




                //
                //  theUserpoto.PhotoImage = imageByte.ToArray();
                work.PhotoRepository.Insert(theUserpoto);
                work.Save();
            }
            else
            {
                List<Photo> theUserpoto = work.PhotoRepository.Get(a => a.PersonID == id).ToList();
                Photo theRealPhoto = theUserpoto[0];
                work.PhotoRepository.Delete(theRealPhoto.PhotoID);
                work.Save();
                //// theUserpoto.PersonID = id;
                // // theStudentpoto.PrimarySchoolStudent = theStudent;

                // WebImage image = new System.Web.Helpers.WebImage(Request.InputStream);

                // byte[] imageByte = image.GetBytes();

                // //
                // theRealPhoto.PhotoImage = imageByte.ToArray();
                //// theUserpoto.PhotoImage = imageByte.ToArray();



                image = image.Substring("data:image/png;base64,".Length);
                var buffer = Convert.FromBase64String(image);
                // byte[] imageByte = buffer.ToArray(
               // theRealPhoto.PhotoImage = buffer.ToArray();
               



                Stream stream = new MemoryStream(buffer.ToArray());


                Stream theResizedStream = PhotoResize.GenerateThumbnails(0.3, stream);

                byte[] theBytePix = PhotoResize.ToByteArray(theResizedStream);

                // Stream strm = fileupload1.PostedFile.InputStream;

                //theRealPhoto.PhotoImage = theBytePix.ToArray();
                theRealPhoto.PhotoImage = theBytePix;





                work.PhotoRepository.Insert(theRealPhoto);
                work.Save();
            }

            return View("");
            // 
        }

        //
        // POST: /Photo/Create

        [HttpPost]
        public ActionResult Create(Photo model)
        {
            try
            {
                // TODO: Add insert logic here
                int UserID = model.PersonID;
                //   List<Photo> theUserPhoto = work.PhotoRepository.Get(a=>a.PhotoID)
                //  return RedirectToAction("Index");
                // return RedirectToAction("Details", "PrimarySchoolStudent", new { id = model.UserID });

                Person thePerson = work.PersonRepository.GetByID(UserID);

                if (thePerson is SecondarySchoolStudent)
                {
                    //  @Html.ActionLink("Comment/View", "Details", new { id = SchoolManagement.Models.Encript.EncryptString( model.PersonID .ToString(),true)}, null)
                    // SchoolManagement.Models.Encript.EncryptString(item.OrderID.ToString(),true)
                    return RedirectToAction("Details", "SecondarySchoolStudent", new { id = SchoolManagement.Models.Encript.EncryptString( model.PersonID .ToString(),true), tracker = 1});
                }

                if (thePerson is PrimarySchoolStudent)
                {
                    //if(User.Identity.)
                    return RedirectToAction("Details", "PrimarySchoolStudent", new { id = SchoolManagement.Models.Encript.EncryptString(model.PersonID.ToString(), true), tracker = 1 });
                }
                if (thePerson is PrimarySchoolStaff)
                {
                    return RedirectToAction("Details", "PrimarySchoolStaff", new { id = SchoolManagement.Models.Encript.EncryptString(model.PersonID.ToString(), true), tracker = 0 });
                }


                return View();
            }

            catch
            {
                return View();
            }

        }

        //
        // GET: /Photo/Edit/5

        public ActionResult Edit(string id)
        {
           //   @Html.ActionLink("Comment/View", "Details", new { id = SchoolManagement.Models.Encript.EncryptString(item.PostID .ToString(),true)}, null)
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            Photo thePhoto = new Photo();
            List<Photo> thePotos = work.PhotoRepository.Get(a => a.PersonID == theId).ToList();

            if (thePotos.Count == 0)
            {
               // return RedirectToAction("Create", new { id = id });
                return RedirectToAction("Create", new { id = SchoolManagement.Models.Encript.EncryptString(theId.ToString(), true) });
            }
            else
            {
                thePhoto = thePotos[0];
                return View("Edit", thePhoto);
            }
            // thePhoto.PersonID = id;
            // ViewBag.ID = "Staff";


            // return View();
        }

        //
        // POST: /Photo/Edit/5

        [HttpPost]
        public ActionResult Edit(Photo model)
        {
            try
            {
                // TODO: Add insert logic here
                int UserID = model.PersonID;
                //   List<Photo> theUserPhoto = work.PhotoRepository.Get(a=>a.PhotoID)
                //  return RedirectToAction("Index");
                // return RedirectToAction("Details", "PrimarySchoolStudent", new { id = model.UserID });

                Person thePerson = work.PersonRepository.GetByID(UserID);

                if (thePerson is SecondarySchoolStudent)
                {
                    //return RedirectToAction("Details", "PrimarySchoolStudent", new { id = SchoolManagement.Models.Encript.EncryptString(model.PersonID.ToString(), true) });
                    return RedirectToAction("Details", "SecondarySchoolStudent", new { id = SchoolManagement.Models.Encript.EncryptString(model.PersonID.ToString(), true) });
                }

                if (thePerson is PrimarySchoolStudent)
                {
                    return RedirectToAction("Details", "PrimarySchoolStudent", new { id = SchoolManagement.Models.Encript.EncryptString(model.PersonID.ToString(), true) });
                }
                if (thePerson is PrimarySchoolStaff)
                {
                    return RedirectToAction("Details", "PrimarySchoolStaff", new  { id = SchoolManagement.Models.Encript.EncryptString(model.PersonID.ToString(), true) });
                }
                return View();
                // return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Photo/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Photo/Delete/5

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
