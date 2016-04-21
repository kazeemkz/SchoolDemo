using JobHustler.DAL;
using SchoolManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SchoolManagement.Models;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;

namespace SchoolManagement.Controllers
{
      [Authorize]
    public class SalaryPaymentHistoryController : Controller
    {
        //
        // GET: /SalaryPaymentHistory/

        UnitOfWork work = new UnitOfWork();
        // public ActionResult Index()
        public ActionResult Index(int? page, string datefrom, string dateto, string staffid)
        {
            List<SalaryPaymentHistory> theSalaryPaymentHistory = work.SalaryPaymentHistoryRepository.Get().OrderByDescending(a => a.DatePaid).ToList();


            // List<AttendanceStaff> theAtten = work.AttendanceStaffRepository.Get().OrderByDescending(a => a.DateTaken).ToList();

            if (!String.IsNullOrEmpty(datefrom) && !(String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(datefrom);
                DateTime to = Convert.ToDateTime(dateto);
                theSalaryPaymentHistory = theSalaryPaymentHistory.Where(a => a.DatePaid.Date >= from.Date && a.DatePaid.Date <= to.Date).ToList();
            }

            if (!String.IsNullOrEmpty(datefrom) && (String.IsNullOrEmpty(dateto)))
            {
                DateTime from = Convert.ToDateTime(datefrom);
                // DateTime to = Convert.ToDateTime(dateto);
                theSalaryPaymentHistory = theSalaryPaymentHistory.Where(a => a.DatePaid.Date == from.Date).ToList();
            }

            if (!String.IsNullOrEmpty(staffid))
            {
                int theStaffID = Convert.ToInt16(staffid);
                theSalaryPaymentHistory = theSalaryPaymentHistory.Where(a => a.StaffID == theStaffID).ToList();
            }





            int pageSize = 100;
            int pageNumber = (page ?? 1);

            ViewBag.Count = theSalaryPaymentHistory.Count();
            return View(theSalaryPaymentHistory.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /SalaryPaymentHistory/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /SalaryPaymentHistory/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /SalaryPaymentHistory/Create

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
        // GET: /SalaryPaymentHistory/Edit/5

        public ActionResult Edit()
        {
            List<PrimarySchoolStaff> theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID != 0).ToList();
            List<SalaryPaymentHistory> theSalary = new List<SalaryPaymentHistory>();
            SalaryPaymentHistoryViewModel salaryPayment = new SalaryPaymentHistoryViewModel();

            foreach (var s in theStaff)
            {
                if (s.SalaryID != null)
                {
                    Salary salary = work.SalaryRepository.GetByID(s.SalaryID);
                    // s.SalaryID
                    theSalary.Add(new SalaryPaymentHistory { StaffID = s.UserID, ActualSalary = salary.Amount, AmountPaid = 0, FirstName = s.FirstName, LastName = s.LastName, PaySalary = true });
                }
            }
            salaryPayment.TheSalaryPaymentHistory = theSalary;
            //  SalaryPaymentHistoryViewModel
            return View(salaryPayment);
        }

        //
        // POST: /SalaryPaymentHistory/Edit/5

        [HttpPost]
        public ActionResult Edit(SalaryPaymentHistoryViewModel model)
        {
            try
            {

                // TODO: Add update logic here
                List<SalaryPaymentHistory> thePaymentNow = new List<SalaryPaymentHistory>();
                if (model != null)
                {
                    List<SalaryPaymentHistory> theSalaryHistory = new List<SalaryPaymentHistory>();
                    theSalaryHistory = work.SalaryPaymentHistoryRepository.Get(a => a.DatePaid.Month == DateTime.Now.Month && a.DatePaid.Year == DateTime.Now.Year).ToList();

                  
                        foreach (var staffPayment in model.TheSalaryPaymentHistory)
                        {
                            //check if there are payments logged aready

                           // staffPayment.
                            if (staffPayment.PaySalary == true)
                            //
                            {
                                SalaryPaymentHistory individualPayment = new SalaryPaymentHistory();

                                individualPayment = new SalaryHelper().Lateness(staffPayment);
                                individualPayment = new SalaryHelper().Abscent(staffPayment);
                                individualPayment = new SalaryHelper().Loan(staffPayment);
                                individualPayment = new SalaryHelper().ContributionsOrDeductions(staffPayment);

                                thePaymentNow.Add(individualPayment);
                            }

                        }

                        StringWriter oStringWriter1 = new StringWriter();
                        Document itextDoc = new Document(PageSize.LETTER);
                        itextDoc.Open();                   
                        Response.ContentType = "application/pdf";
                        PrintResult print = new PrintResult();
                        // Set up the document and the MS to write it to and create the PDF writer instance
                        MemoryStream ms = new MemoryStream();
                        //Document document = new Document(PageSize.A3.Rotate());
                        Document document = new Document(PageSize.A4);
                        PdfWriter writer = PdfWriter.GetInstance(document, ms);

                        // Open the PDF document
                        document.Open();
                    
                        Document thedoc = new SalaryPrinting().PrintPaySlip(thePaymentNow, ref oStringWriter1, ref document);
                        iTextSharp.text.pdf.draw.VerticalPositionMark seperator = new iTextSharp.text.pdf.draw.LineSeparator();
                        seperator.Offset = -6f;

                        document.Close();

                        // Hat tip to David for his code on stackoverflow for this bit
                        // http://stackoverflow.com/questions/779430/asp-net-mvc-how-to-get-view-to-generate-pdf
                        byte[] file = ms.ToArray();
                        MemoryStream output = new MemoryStream();
                        output.Write(file, 0, file.Length);
                        output.Position = 0;

                        if (theSalaryHistory.Count == 0)
                        {
                            //log payment
                            decimal totalDeduction = 0;
                            // decimal totalSalarytobeDeducted = 0;
                            foreach (var m in thePaymentNow)
                            {
                                // m.ActualSalary
                                //  work.SalaryPaymentHistoryRepository.Insert(m);
                                if (m.TotalLatenessDeduction > 0)
                                {
                                    DeductionHistory theLateness = new DeductionHistory();
                                    theLateness.AmountDeducted = Convert.ToDecimal(m.TotalLatenessDeduction);
                                    theLateness.DatePaid = DateTime.Now;
                                    theLateness.Description = "Lasteness Deduction for the Month " + string.Format("{0:MMMM-yyyy}", DateTime.Now);
                                    theLateness.StaffID = m.StaffID.ToString();
                                    totalDeduction = totalDeduction + theLateness.AmountDeducted;
                                    work.DeductionHistoryRepository.Insert(theLateness);
                                    // theLateness.
                                }

                                if (m.TotalAbscentDeduction > 0)
                                {
                                    DeductionHistory theLateness = new DeductionHistory();
                                    theLateness.AmountDeducted = Convert.ToDecimal(m.TotalAbscentDeduction);
                                    theLateness.DatePaid = DateTime.Now;
                                    theLateness.Description = "Abscent Deduction for the Month " + string.Format("{0:MMMM-yyyy}", DateTime.Now);
                                    theLateness.StaffID = m.StaffID.ToString();
                                    totalDeduction = totalDeduction + theLateness.AmountDeducted;
                                    work.DeductionHistoryRepository.Insert(theLateness);
                                    // theLateness.
                                }

                                if (m.TotalLoan > 0)
                                {
                                    DeductionHistory theLateness = new DeductionHistory();
                                    theLateness.AmountDeducted = Convert.ToDecimal(m.TotalLoan);
                                    theLateness.DatePaid = DateTime.Now;
                                    theLateness.Description = "Loan Deduction for the Month " + string.Format("{0:MMMM-yyyy}", DateTime.Now);
                                    theLateness.StaffID = m.StaffID.ToString();
                                    totalDeduction = totalDeduction + theLateness.AmountDeducted;
                                    work.DeductionHistoryRepository.Insert(theLateness);
                                    // theLateness.
                                }

                                // other subscibed deductions
                                foreach (var subscibed in m.TheDeduction)
                                {
                                    if (subscibed != null)
                                    {
                                        DeductionHistory theLateness = new DeductionHistory();
                                        theLateness.AmountDeducted = Convert.ToDecimal(subscibed.Amount);
                                        theLateness.DatePaid = DateTime.Now;
                                        theLateness.Description = subscibed.DeductionDescription + " Deduction for the Month " + string.Format("{0:MMMM-yyyy}", DateTime.Now);
                                        theLateness.StaffID = m.StaffID.ToString();
                                        totalDeduction = totalDeduction + theLateness.AmountDeducted;
                                        work.DeductionHistoryRepository.Insert(theLateness);
                                        // work.Save();
                                    }
                                }
                                //save the actual salary payment
                                PrimarySchoolStaff theStaf = work.PrimarySchoolStaffRepository.Get(a => a.UserID == m.StaffID).First();
                                Salary theStaffSalary = work.SalaryRepository.GetByID(theStaf.SalaryID);
                                m.ActualSalary = theStaffSalary.Amount;
                                m.Description = "Salary Payment for the Month of " + string.Format("{0:MMMM-yyyy}", DateTime.Now);
                                m.AmountPaid = theStaffSalary.Amount - totalDeduction;
                                m.DatePaid = DateTime.Now;
                                work.SalaryPaymentHistoryRepository.Insert(m);
                                work.Save();
                                totalDeduction = 0;
                                // SalaryPaymentHistory theHistory = new SalaryPaymentHistory{ AmountPaid = m.AmountPaid, ActualSalary = m.ActualSalary, DatePaid = DateTime.Now, Description = "Salary Payment for "+ DateTime.Now.Month, FirstName = m.FirstName}
                            }

                        }
                        // work.DeductionHistoryRepository
                        HttpContext.Response.AddHeader("content-disposition", "attachment; filename=form.pdf");
                        return new FileStreamResult(output, "application/pdf"); //new FileStreamResult(output, "application/pdf");

                        //return RedirectToAction("Index", new FileStreamResult(output, "application/pdf"));

                        //  return new ActionAsPdf
                   // }

                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /SalaryPaymentHistory/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /SalaryPaymentHistory/Delete/5

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
