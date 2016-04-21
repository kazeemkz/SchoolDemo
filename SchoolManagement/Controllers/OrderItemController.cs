using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobHustler.DAL;
using JobHustler.Models;
using SchoolManagement.Model;

namespace SchoolManagement.Controllers
{
    // [DynamicAuthorize]
    [Authorize]
    public class OrderItemController : Controller
    {
        UnitOfWork work = new UnitOfWork();
        //
        // GET: /OrderItem/
        // [DynamicAuthorize]
        public ActionResult Index(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            List<OrderItem> theOrderItems = work.OrderItemRepository.Get(a => a.OrderID == theId).ToList();

            return View(theOrderItems);
        }
        [Authorize(Roles = "Student,Parent")]
        public ActionResult Index2(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            List<OrderItem> theOrderItems = work.OrderItemRepository.Get(a => a.OrderID == theId).ToList();

            return View(theOrderItems);
        }

        //
        // GET: /OrderItem/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpPost]
        // [Authorize(Roles = "SuperAdmin,Admin")]
        [DynamicAuthorize]
        public ActionResult Edit(string id)
        {
            Int32 theId = Convert.ToInt32(Models.Encript.DecryptString(id, true));
            OrderItem theOrderItem = work.OrderItemRepository.GetByID(theId);
            string theItemName = theOrderItem.ItemName;
            int theItemQuantity = theOrderItem.Quantity;
            List<Store> theStore = work.StoreRepository.Get(a => a.ItemName.Equals(theItemName)).ToList();

            if (theStore.Count > 0)
            {
                Store theStoreItem = theStore[0];

                theStoreItem.Quanity = theItemQuantity + theStoreItem.Quanity;

                work.StoreRepository.Update(theStoreItem);
                work.Save();

                Order theOrder = work.OrderRepository.GetByID(theOrderItem.OrderID);
                theOrder.ItemQuantity = theOrder.ItemQuantity - theItemQuantity;
                theOrder.TotalAmount = theOrder.TotalAmount - theOrderItem.SubTotal;
                //  theOrder.OrderDate = theOrder.OrderDate;

                if (theOrder.ItemQuantity == 0)
                {
                    work.OrderRepository.Delete(theOrder);
                }
                else
                {
                    work.OrderRepository.Update(theOrder);
                }

                work.OrderItemRepository.Delete(theOrderItem);

                string orderNumberString = theOrderItem.OrderNumber.ToString();
                List<SchoolAccount> theAccount = new List<SchoolAccount>();
                theAccount = work.SchoolAccountRepository.Get(a => a.TransactionMethod == orderNumberString).ToList();

                if (theAccount.Count() > 0)
                {
                    decimal theAmount = theOrderItem.SubTotal;
                    SchoolAccount thePaymenttoBeDeleted = new SchoolAccount();
                    thePaymenttoBeDeleted = theAccount[0];
                    thePaymenttoBeDeleted.Amount = thePaymenttoBeDeleted.Amount - theAmount;
                    if (thePaymenttoBeDeleted.Amount == 0)
                    {
                        work.SchoolAccountRepository.Delete(thePaymenttoBeDeleted);
                    }
                    else
                    {
                        work.SchoolAccountRepository.Update(thePaymenttoBeDeleted);
                    }
                }
                work.Save();
                if (User.Identity.Name != "5000001")
                {
                    AuditTrail audit = new AuditTrail { Date = DateTime.Now, Action = "An Order was reversed, item name:-" + " " + theItemName + " " + "Quntities :- " + theItemQuantity, UserID = User.Identity.Name };
                    work.AuditTrailRepository.Insert(audit);
                    work.Save();
                }
            }
            return RedirectToAction("Index", "Order");
            //  return View("Order");
        }

        //
        // GET: /OrderItem/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /OrderItem/Create

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
        // GET: /OrderItem/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //
        // POST: /OrderItem/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /OrderItem/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /OrderItem/Delete/5

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
