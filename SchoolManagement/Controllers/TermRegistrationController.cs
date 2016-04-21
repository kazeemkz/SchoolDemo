using JobHustler.DAL;
using SchoolManagement.Model;
using SchoolManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Controllers
{
    public class TermRegistrationController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        UnitOfWork work2 = new UnitOfWork();
        //
        // GET: /TermRegistration/

        public ActionResult Index()
        {
            return RedirectToAction("Edit");
            // return View();
        }

        //
        // GET: /TermRegistration/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /TermRegistration/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TermRegistration/Create

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
        // GET: /TermRegistration/Edit/5

        public ActionResult Edit(string level, string Term, string Session)
        {


            //ViewData["level"] = theItem;
            if (!string.IsNullOrEmpty(level) && !string.IsNullOrEmpty(Term) && !string.IsNullOrEmpty(Session))
            {
                List<PrimarySchoolStudent> theStudents = new List<PrimarySchoolStudent>();
                List<TermRegistration> theTermRegistration = new List<TermRegistration>();
                TermRegistrationViewModel theStudentsTermRegistration = new TermRegistrationViewModel();
                List<TermRegistration> normalizedRegisteredStudents = new List<TermRegistration>();

                //   var selectedCoursesHS = new HashSet<string>(selectedCourses);
                //  var instructorCourses = new HashSet<int>(instructorToUpdate.Subjects.Select(c => c.SubjectID));

                List<TermRegistration> termRegistrations = work.TermRegistrationRepository.Get(a => a.Term == Term && a.Level == level && a.Session == Session).ToList();

                HashSet<TermRegistration> reg = new HashSet<TermRegistration>(termRegistrations);
                //    var instructorCourses = new HashSet<int>(instructor.Subjects.Select(c => c.SubjectID));

                if (!(string.IsNullOrEmpty(level)))
                {
                    theStudents = work.PrimarySchoolStudentRepository.Get(a => a.PresentLevel == level).OrderByDescending(a => a.LastName).ToList();
                }
                if (theStudents.Count() > 0)
                {
                    foreach (PrimarySchoolStudent p in theStudents)
                    {
                        bool foundValue = false;
                        foreach (TermRegistration t in reg)
                        {
                            if (t.StudentID == p.UserID)
                            {
                                foundValue = true;
                                break;
                            }
                        }
                        //check if the user is in the termregistration database
                        if (foundValue == false)
                        {


                            reg.Add(new TermRegistration { StudentID = p.UserID, FirstName = p.FirstName, Level = p.PresentLevel, LastName = p.LastName, Sex = p.Sex, Owing = 0 });

                        }
                    }


                    foreach (TermRegistration t in reg)
                    {
                        List<SchoolFeePayment> theStudentFeesPayment = new List<SchoolFeePayment>();
                        theStudentFeesPayment = work.SchoolFeePaymentRepository.Get(a => a.StudentID == t.StudentID).OrderByDescending(a => a.DatePaid).ToList();
                        decimal totalOwing = 0;
                        if (theStudentFeesPayment.Count > 0)
                        {
                            SchoolFeePayment thePayment = theStudentFeesPayment[0];
                            //check if its this current term we only wana disable for future registration
                            // string s =  thePayment.Term + thePayment.Session;
                            // if (thePayment.Term != Term && thePayment.Session != Session)

                          double paymentRepository =  SessionHelper.FindLager(thePayment.Session, thePayment.Term);

                          double thisTermPayment = SessionHelper.FindLager(Session, Term);
                          if (thePayment.Term != Term && paymentRepository < thisTermPayment)
                            {
                                totalOwing = 0;
                                // foreach (SchoolFeePayment s in theStudentFeesPayment)
                                // if (theStudentFeesPayment.Count > 0)
                                // {
                                // SchoolFeePayment sf = theStudentFeesPayment.OrderByDescending(a => a.DatePaid).First();
                                // totalOwing = sf.Owing;
                                totalOwing = thePayment.Owing;
                                //}
                            }

                        }

                        //get the last registration to find out if there where payments
                        string lastTerm = null;
                        string lastSession = null;
                        string lastLevel = null;
                        TermRegistration theLastRegisteredTerm = new TermRegistration();
                        List<TermRegistration> pastRegistrationForCurrentGuy = new List<TermRegistration>();
                        // decimal lastTermCost = 0;

                        // that means he has registered for this current term if CurrentRegistrationForCurrentGuy > 0
                        List<TermRegistration> currentRegistrationForCurrentGuy = work.TermRegistrationRepository.Get(a => a.Term == Term && a.Level == level && a.Session == Session && a.StudentID == t.StudentID && t.Register == true).OrderByDescending(a => a.DateRegistered).ToList();
                        if (currentRegistrationForCurrentGuy.Count > 0)
                        {
                            TermRegistration theCurrentReg = currentRegistrationForCurrentGuy[0];
                            pastRegistrationForCurrentGuy = work.TermRegistrationRepository.Get(a => a.StudentID == t.StudentID && a.DateRegistered < theCurrentReg.DateRegistered).OrderByDescending(a => a.DateRegistered).ToList();
                        }
                        else
                        {
                            pastRegistrationForCurrentGuy = work.TermRegistrationRepository.Get(a => a.StudentID == t.StudentID).OrderByDescending(a => a.DateRegistered).ToList();

                        }

                       
                        if (pastRegistrationForCurrentGuy.Count() > 0)
                        {
                            theLastRegisteredTerm = pastRegistrationForCurrentGuy[0];
                            lastTerm = theLastRegisteredTerm.Term;
                            lastSession = theLastRegisteredTerm.Session;
                            lastLevel = theLastRegisteredTerm.Level;
                            // lastTermCost = theLastRegisteredTerm.Cost;


                            //get if there were payments for last term
                            List<SchoolFeePayment> theLastPayments = new List<SchoolFeePayment>();
                            theLastPayments = work.SchoolFeePaymentRepository.Get(a => a.Term == lastTerm && a.StudentID == t.StudentID && a.Session == lastSession && a.Level == lastLevel).ToList();

                            if (theLastPayments.Count() > 0)
                            {

                            }
                            else
                            {
                                //no payment for last term
                                totalOwing = theLastRegisteredTerm.Cost;
                            }
                        }
                        //(string session, string term)
                        //  int sessionToQuery = Models.SessionHelper.FindLager(lastTerm, lastTerm);

                        //(string level, string Term, string Session)

                        // int currentSession = Models.SessionHelper.FindLager(Term, Session);

                        normalizedRegisteredStudents.Add(new TermRegistration { StudentID = t.StudentID, FirstName = t.FirstName, Level = t.Level, LastName = t.LastName, Sex = t.Sex, Owing = totalOwing, SchoolFeesKind = t.SchoolFeesKind, Cost = t.Cost, Session = t.Session, Term = t.Term, DateRegistered = t.DateRegistered, Register = t.Register });
                    }

                    // theTermRegistration.Add(new TermRegistration { StudentID = p.UserID, FirstName = p.FirstName, Level = p.PresentLevel, LastName = p.LastName, Sex = p.Sex });
                    // }
                }

                theStudentsTermRegistration.TheTermRegistration = normalizedRegisteredStudents;// = theTermRegistration;


                // return View();
                return View("Edit", theStudentsTermRegistration);
            }
            else
            {
                TermRegistrationViewModel theStudentsTermRegistration = new TermRegistrationViewModel();
                return View("Edit", theStudentsTermRegistration);
            }
        }

        //
        // POST: /TermRegistration/Edit/5

        [HttpPost]
        public ActionResult Edit(TermRegistrationViewModel model, string level1, string Term2, string Session3)
        //public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                foreach (TermRegistration m in model.TheTermRegistration)
                {
                    if (m.Register == true && !string.IsNullOrEmpty(m.SchoolFeesKind))
                    {
                        string theSchoolFeeKind = m.SchoolFeesKind;
                        SchoolFeesType theFeeCost = work.SchoolFeesTypeRepository.Get(a => a.SchoolFeesKind == theSchoolFeeKind).First();
                        m.Cost = theFeeCost.Amount;
                        m.Session = Session3;
                        m.Term = Term2;
                        List<TermRegistration> theReg = work2.TermRegistrationRepository.Get(a => a.StudentID == m.StudentID && a.Session == m.Session && a.Term == m.Term).ToList();

                        if (theReg.Count() > 0)
                        {
                            TermRegistration theRegi = theReg[0];
                            theRegi.Cost = m.Cost;
                            theRegi.Register = m.Register;
                            theRegi.SchoolFeesKind = m.SchoolFeesKind;
                            //  theRegi.
                            work2.TermRegistrationRepository.Update(theRegi);
                            // work2.TermRegistrationRepository.Delete(theRegi);
                        }
                        else
                        {
                            m.DateRegistered = DateTime.Now;
                            work.TermRegistrationRepository.Insert(m);
                        }

                    }
                }
                work2.Save();
                work.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /TermRegistration/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /TermRegistration/Delete/5

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
