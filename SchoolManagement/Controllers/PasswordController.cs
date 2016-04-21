using JobHustler.DAL;
using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Controllers
{
    public class PasswordController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        //
        // GET: /Password/

        [Authorize]
        public ActionResult Index()
        {
            //CodeGenerator theCode = new CodeGenerator();
            //theCode.Code = "23i9hs";
            //theCode.DateCreated = DateTime.Now;
            //theCode.GivenOut = false;
            //work.CodeGeneratorRepository.Insert(theCode);
            //work.Save();

            List<Password> theCodes = work.PasswordRepository.Get().ToList();
            //List<CodeGenerator> theCodes = work.CodeGeneratorRepository.Get().OrderBy(a => a.DateCreated).ToList();
            return View(theCodes);
        }

        //
        // GET: /CodeGenerator/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /CodeGenerator/Create

        public ActionResult Create()
        {
            var chars = "1uzbcvfgh6i7qazcbxmk5k67eg8g91d2ia3dcjk9v8ecvb1n1z1x484n5dsbs9z82h3u4ey6w8i7h9p5d6nwpbdinkskw3485p3e9v3n44m3t8cb7n9c4j3nxp459iasj39vi49cm1nx8y74na8z9173k1q2a3z4xd4r45tfgcbvhy376emij2j1j3lvmxiizqp1i59x8czk14mc9a81m7yuxi6n4ma1ichja85mea9mc9uymib4kazihm38i9dlvc2m56ty79d1man5v7";
            //   var chars = "1BC2DEFGH2JK4LM5N6P7Q8RS9TU1VWXYZ9865432";
            var random = new Random();

            for (int k = 0; k <= 20; k++)
            {
                string randomPassword = new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
                Password theCode = new Password();
                theCode.DateCreated = DateTime.Now;
                theCode.Code = randomPassword;
                theCode.GivenOut = false;

                work.PasswordRepository.Insert(theCode);

            }

            work.Save();
           
            return View("Index");
        }

        //
        // POST: /CodeGenerator/Create

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
        // GET: /CodeGenerator/Edit/5

        public ActionResult Edit(int id)
        {
            return View("Index");
        }

        //
        // POST: /CodeGenerator/Edit/5

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
        // GET: /CodeGenerator/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /CodeGenerator/Delete/5

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
