using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobHustler.DAL;
using JobHustler.Models;
using SchoolManagement.DAL;
using SchoolManagement.Model;
using SchoolManagement.Models;
using PagedList;

namespace SchoolManagement.Controllers
{
      //[DynamicAuthorize]
    [Authorize]
    public class StudentStoreItemController : Controller
    {
        //
        UnitOfWork work = new UnitOfWork();
        // GET: /StudentStoreItemRepository/

        //public ActionResult Index()
        //{
        //    return View();
        //}

        [DynamicAuthorize]
        public ViewResult Index(string arm, string sortOrder, string currentFilter, string ApprovedString, string searchString, string SexString, string LevelString, string StudentIDString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            ViewBag.ClassSortParm = sortOrder == "class" ? "class desc" : "class";
            ViewBag.GenderSortParm = sortOrder == "gender" ? "gender desc" : "gender";


            List<Level> theLevels = work.LevelRepository.Get().ToList();
            List<SelectListItem> theItem = new List<SelectListItem>();
            theItem.Add(new SelectListItem() { Text = "None", Value = "" });

            foreach (var level in theLevels)
            {
                theItem.Add(new SelectListItem() { Text = level.LevelName + ":" + level.Type, Value = level.LevelName + ":" + level.Type });
            }
            //theItem.Add(new SelectListItem() { Text = "Graduated", Value = "Graduated" });
            //theItem.Add(new SelectListItem() { Text = "Withdraw", Value = "Withdraw" });
            //theItem.Add(new SelectListItem() { Text = "Left", Value = "Left" });

            ViewData["arm"] = theItem;

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString;

            var students = from s in work.PrimarySchoolStudentRepository.Get()
                           select s;
            students = students.Where(s => s.LevelType != "Graduated");
            students = students.Where(s => s.LevelType != "Withdraw");
            students = students.Where(s => s.LevelType != "Left");

            if (!String.IsNullOrEmpty(SexString))
            {
                students = students.Where(s => s.Sex == (SexString));
            }

            if (!String.IsNullOrEmpty(LevelString))
            {
                students = students.Where(s => s.PresentLevel == (LevelString));
            }

            if (!String.IsNullOrEmpty(arm))
            {
                //students = students.Where(s => s.PresentLevel.Equals(LevelString) && s.PresentLevel !=null);
                students = students.Where(s => s.LevelType == arm);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstName.ToUpper().Contains(searchString.ToUpper()) || s.Middle.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!String.IsNullOrEmpty(StudentIDString))
            {
                int theID = Convert.ToInt32(StudentIDString);
                students = students.Where(s => s.UserID == theID);
            }

            //if (!String.IsNullOrEmpty(SexString))
            //{
            //    students = students.Where(s => s.Sex.Equals(SexString));
            //}
            switch (sortOrder)
            {
                case "Name desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;

                case "class":
                    students = students.OrderBy(s => s.PresentLevel);
                    break;
                case "class desc":
                    students = students.OrderByDescending(s => s.PresentLevel);
                    break;

                case "gender":
                    students = students.OrderBy(s => s.Sex);
                    break;
                case "gender desc":
                    students = students.OrderByDescending(s => s.Sex);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            int pageSize = 100;
            int pageNumber = (page ?? 1);


            if (User.IsInRole("Teacher"))
            {

                int UserName = Convert.ToInt32(User.Identity.Name);
                List<PrimarySchoolStaff> theStaff = work.PrimarySchoolStaffRepository.Get(a => a.UserID == UserName).ToList();
                PrimarySchoolStaff theStaf = theStaff[0];
                students = students.Where(a => a.LevelType == theStaf.ClassTeacherOf && a.IsApproved == true);
                return View(students.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return View(students.ToPagedList(pageNumber, pageSize));
            }
        }

        //
        // GET: /StudentStoreItemRepository/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /StudentStoreItemRepository/Create

        public ActionResult Create(int? id, string level)
        {
         PrimarySchoolStudent theStudent =   work.PrimarySchoolStudentRepository.GetByID(id);
         List<Store> theStoreItems = work.StoreRepository.Get(a => a.Level.Equals(level)).ToList();
         List<Store> theStoreItemWithoutClass = work.StoreRepository.Get(a => a.Level== null).ToList();
          List<Store> theItems = new List<Store>();
          foreach (var item in theStoreItems)
          {
              theItems.Add(item);
          }

          foreach (var item in theStoreItemWithoutClass)
          {
              theItems.Add(item);
          }


       

          StudentItemViewModel theItemViewModel = new StudentItemViewModel();
          theItemViewModel.StudentStoreItem = theItems;
          theItemViewModel.StudentID = theStudent.UserID;
          theItemViewModel.StudentName = theStudent.LastName;
          theItemViewModel.Level = theStudent.PresentLevel;
           // theItemViewModel.


          return View(theItemViewModel);
        }

        //
        // POST: /StudentStoreItemRepository/Create

        [HttpPost]
        public ActionResult Create(StudentItemViewModel model)
        {
            try
            {
                int quantity = 0;
                decimal totalAmount = 0;
                if (ModelState.IsValid)
                {
                    

                    Order theOrder = new Order();
                List<Order> theLastOrder =    work.OrderRepository.Get().OrderBy(a=>a.OrderID).ToList();
                    if (theLastOrder.Count == 0)
                    {
                      theOrder.OrderNumber =  10000001;
                    }
                    else
                    {
                        theOrder.OrderNumber = theLastOrder.Last().OrderNumber+1;
                    }
                    theOrder.StudentLastName = model.StudentName;
                    theOrder.studentID = model.StudentID;
                   // theOrder.Level = model.Level;
                    theOrder.OrderDate = DateTime.Now;
                   // theOrder.
                    int Quantity = 0;
                    decimal TotalAmount = 0;

                    foreach (var theitem in model.StudentStoreItem)
                    {
                  
                   Quantity   = theitem.QuantityNeeded + Quantity;
                   TotalAmount = (theitem.QuantityNeeded * theitem.Amount) + TotalAmount;
                    }

                    theOrder.ItemQuantity = Quantity;
                    theOrder.TotalAmount = TotalAmount;
                    theOrder.Term = model.Term;

                      quantity =Quantity ;
                      totalAmount = TotalAmount;

                    work.OrderRepository.Insert(theOrder);
                    work.Save();
                    
                    foreach (var theitem in model.StudentStoreItem)
                    {
                       // model.to
                        OrderItem theOrderItem = new OrderItem();
                        theOrderItem.Quantity = theitem.QuantityNeeded;
                        theOrderItem.SubTotal = theitem.QuantityNeeded * theitem.Amount;
                        theOrderItem.ItemName = theitem.ItemName;
                        theOrderItem.OrderID = theOrder.OrderID;
                        theOrderItem.OrderNumber = theOrder.OrderNumber;
                        work.OrderItemRepository.Insert(theOrderItem);
                        work.Save();
                       
                    }
                }
                if (User.Identity.Name != "5000001")
                {
                    //quantity = Quantity;
                    //totalAmount = TotalAmount;
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "Item Bought by  Name:-" + model.StudentID + " " + "Quantity and Total amount:- " + quantity + " " + totalAmount, UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
                    work.Save();
                }
                work.Save();
                // TODO: Add insert logic here

                return RedirectToAction("Index","Order");
            }
            catch
            {
                return View("Edit", model);
            }
        }

        //
        // GET: /StudentStoreItemRepository/Edit/5
        [DynamicAuthorize]
        public ActionResult Edit(string id, string level)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            PrimarySchoolStudent theStudent = work.PrimarySchoolStudentRepository.GetByID(theId);
            List<Store> theStoreItems = work.StoreRepository.Get(a => a.Level.Equals(level)).OrderBy(a=>a.ItemName).ToList();
            List<Store> theStoreItemWithoutClass = work.StoreRepository.Get(a => a.Level == "").OrderBy(a => a.ItemName).ToList();
            List<Store> theItems = new List<Store>();
            foreach (var item in theStoreItems)
            {
                theItems.Add(item);
            }

            foreach (var item in theStoreItemWithoutClass)
            {
                theItems.Add(item);
            }

            StudentItemViewModel theItemViewModel = new StudentItemViewModel();
            theItemViewModel.StudentStoreItem = theItems;
            theItemViewModel.StudentID = theStudent.UserID;
            theItemViewModel.StudentName = theStudent.LastName;
            theItemViewModel.Level = theStudent.PresentLevel;
            // theItemViewModel.


            return View(theItemViewModel);
        }

        //
        // POST: /StudentStoreItemRepository/Edit/5

        [HttpPost]
        [DynamicAuthorize]
        public ActionResult Edit(StudentItemViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    Order theOrder = new Order();
                    theOrder.Term = model.Term;
                    List<Order> theLastOrder = work.OrderRepository.Get().OrderBy(a => a.OrderID).ToList();
                    if (theLastOrder.Count == 0)
                    {
                        theOrder.OrderNumber = 10000001;
                    }
                    else
                    {
                        theOrder.OrderNumber = theLastOrder.Last().OrderNumber + 1;
                    }
                    theOrder.StudentLastName = model.StudentName;
                    theOrder.studentID = model.StudentID;
                    theOrder.OrderDate = DateTime.Now;
                    // theOrder.
                    int Quantity = 0;
                  //  decimal TotalAmount = 0;

                    foreach (var theitem in model.StudentStoreItem)
                    {

                        Quantity = theitem.QuantityNeeded + Quantity;
                       // TotalAmount = (theitem.QuantityNeeded * theitem.Amount) + TotalAmount;
                    }

                    theOrder.ItemQuantity = Quantity;
                    theOrder.TotalAmount = model.TotalCost;
                    theOrder.Level = model.Level;

                    work.OrderRepository.Insert(theOrder);
                   // work.Save();

                    foreach (var theitem in model.StudentStoreItem)
                    {
                        if (theitem.QuantityNeeded != 0)
                        {

                           
                            Store theStoreItem = work.StoreRepository.GetByID(theitem.StoreID);

                            if (theStoreItem.Quanity < theitem.QuantityNeeded)
                            {
                                ModelState.AddModelError("", "Quantity Requested Most be Less than Quantity in Store");
                                return View(model);
                            }

                            int availableQuanity = theStoreItem.Quanity;
                            theStoreItem.Quanity = availableQuanity - theitem.QuantityNeeded;
                            work.StoreRepository.Update(theStoreItem);
                            OrderItem theOrderItem = new OrderItem();
                            theOrderItem.Quantity = theitem.QuantityNeeded;
                            theOrderItem.SubTotal = theitem.QuantityNeeded * theitem.Amount;
                            theOrderItem.ItemName = theitem.ItemName;
                            theOrderItem.OrderID = theOrder.OrderID;
                            theOrderItem.OrderNumber = theOrder.OrderNumber;
                            work.OrderItemRepository.Insert(theOrderItem);
                            work.Save();
                        }
                       
                    }
                }
                work.Save();
                // TODO: Add insert logic here
                return RedirectToAction("Index", "Order");
               // return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }

        //
        // GET: /StudentStoreItemRepository/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /StudentStoreItemRepository/Delete/5

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
