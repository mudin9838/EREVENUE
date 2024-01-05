using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PlanedBudgetPro.Models;
using PagedList;
using PlanedBudgetPro.ViewModels;
using Microsoft.AspNet.Identity;
using IdentitySample.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.UI.WebControls;

namespace PlanedBudgetPro.Controllers
{
    public class OrderdsController : Controller
    {
        // GET: Orders
        OnlineShopEntities db = new OnlineShopEntities();
        private ApplicationDbContext dbs;
        private UserManager<ApplicationUser> manager;
        public OrderdsController()
        {
            dbs = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbs));
        }
        [HttpGet]
        public JsonResult LoadFisicalYears()
        {
            var fisicalYears = db.FisicalYears.Where(a => a.FisicalYearId != "1")
                .Select(m => new { value = m.FisicalYearId, text = m.FisicalYearName }).ToList();
            return new JsonResult { Data = fisicalYears, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        public JsonResult LoadRevenueCategories()
        {
            var revenueCategories = db.RevenueCategories
                .Select(m => new { value = m.RevenueCategoryId, text = m.RevenueCategoryName }).ToList();
            return new JsonResult { Data = revenueCategories, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult LoadParents()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var SITEID = currentUser.ParentId;
            var parents = db.Parents
                .Select(m => new { value = m.ParentId, text = m.ParentName });
            return new JsonResult { Data = parents.Where(x => x.value == SITEID).ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetChildList(string ParentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Child> StateList = db.Children.Where(x => x.ParentId == ParentId).ToList();
            return Json(StateList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadChildren()
        {
            var children = db.Children
                .Select(m => new { value = m.ChildId, text = m.ChildName }).ToList();
            return new JsonResult { Data = children.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        public JsonResult LoadMonths()
        {
            var months = db.Months
                .Select(m => new { value = m.MonthId, text = m.MonthName }).ToList();
            return new JsonResult { Data = months, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult GetMonthList(string FisicalYearId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Month> StateList = db.Months.Where(x => x.FisicalYearId == FisicalYearId).ToList();
            return Json(StateList, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult Index(int? page)
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var SITEID = currentUser.ParentId;
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
            {
                IPagedList<Customerd> OrderAndCustomerList = db.Customerds.Where(a => a.FisicalYearId != "1").OrderBy(c => c.ChildId).ThenByDescending(c => c.OrderDate).ToPagedList(page ?? 1, 20);
                return View(OrderAndCustomerList);
            }
            if (User.IsInRole("Center"))
            {
                IPagedList<Customerd> OrderAndCustomerList = db.Customerds.Where(c => c.ParentId == SITEID).Where(a => a.FisicalYearId != "1").OrderBy(c => c.ChildId).ThenByDescending(c => c.OrderDate).ToPagedList(page ?? 1, 20);
                return View(OrderAndCustomerList);
            }
            return View();
        }

        [HttpPost] // Add New And Edit 
        public ActionResult SaveOrder(DateTime orderdate, string id, string parentid, string childid, string fisicalyearid, string monthid, Orderd[] orders)
        {
            string result = "መረጃው ተመዝግቧል!!";
            // New Entry
            if (string.IsNullOrEmpty(id))
            {
                if (orderdate != null && !string.IsNullOrEmpty(parentid.Trim()) && !string.IsNullOrEmpty(childid.Trim()) && !string.IsNullOrEmpty(fisicalyearid.Trim()) && !string.IsNullOrEmpty(monthid.Trim()) && orders != null)
                {
                    var customerId = Guid.NewGuid();
                    Customerd customer = new Customerd
                    {
                        ParentId = parentid,
                        ChildId = childid,
                        FisicalYearId = fisicalyearid,
                        MonthId = monthid,
                        OrderDate = orderdate,
                        CustomerId = customerId
                    };
                    db.Customerds.Add(customer);

                    foreach (var o in orders)
                    {
                        var orderId = Guid.NewGuid();
                        Orderd order = new Orderd();
                        order.CustomerId = customerId;
                        order.Amountyplan = o.Amountyplan;
                        order.Amountyperformance = o.Amountyperformance;
                        order.RevenueCategoryId = o.RevenueCategoryId;
                        order.OrderId = Guid.NewGuid();
                        db.Orderds.Add(order);

                        //
                    }
                    db.SaveChanges();
                }
            }
            // Edit Orders 
            else
            {
                //var customerGuid = Guid.Parse(id);
                var customerGuid = Guid.Parse(id);
                var customerInDb = db.Customerds.FirstOrDefault(c => c.CustomerId == customerGuid);
                customerInDb.ParentId = parentid;
                customerInDb.ChildId = childid;
                customerInDb.FisicalYearId = fisicalyearid;
                customerInDb.MonthId = monthid;
                customerInDb.OrderDate = orderdate;
                //db.Customers.Add(customerInDb);

                foreach (var o in orders)
                {
                    var dbOrder = db.Orderds.FirstOrDefault(odr => odr.OrderId == o.OrderId);
                    if (dbOrder != null)
                    {
                        dbOrder.Amountyperformance = o.Amountyperformance;
                        dbOrder.Amountyplan = o.Amountyplan;
                        dbOrder.RevenueCategoryId = o.RevenueCategoryId;
                    }
                    else
                    {
                        Orderd order = new Orderd();
                        order.OrderId = Guid.NewGuid();
                        order.Amountyperformance = o.Amountyperformance;
                        order.Amountyplan = o.Amountyplan;
                        order.RevenueCategoryId = o.RevenueCategoryId;
                        order.CustomerId = customerGuid;
                        db.Orderds.Add(order);
                    }
                }
                db.SaveChanges();
                result = "መረጃው ተስተካክሏል..";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetOrderDetails(string customerId)
        {
            var customerGuid = Guid.Parse(customerId);
            var customerDetails = db.Customerds.Include("Orderds").Single(c => c.CustomerId == customerGuid);

            vmCustomer Customer = new vmCustomer();
            Customer.CustomerId = customerDetails.CustomerId;
            Customer.ParentId = customerDetails.ParentId;
            Customer.ChildId = customerDetails.ChildId;
            Customer.MonthId = customerDetails.MonthId;
            Customer.FisicalYearId = customerDetails.FisicalYearId;
            Customer.OrderDate = customerDetails.OrderDate;

            List<vmOrder> orderslList = new List<vmOrder>();

            foreach (var o in customerDetails.Orderds)
            {
                vmOrder order = new vmOrder();
                order.OrderId = o.OrderId;
                // order.OrderDate = o.OrderDate;
                order.Amountyplan = o.Amountyplan;
                order.Amountyperformance = o.Amountyperformance;
                // order.Amount = o.Amount;
                order.RevenueCategoryId = o.RevenueCategoryId;

                orderslList.Add(order);
            }

            vmCustomerAndOrders CustomerAndOrders = new vmCustomerAndOrders()
            {
                Customer = Customer,
                Orders = orderslList
            };

            return Json(CustomerAndOrders, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteAnOrder(string orderId)
        {
            var orderGuid = Guid.Parse(orderId);
            var order = db.Orderds.Find(orderGuid);
            db.Orderds.Remove(order);
            db.SaveChanges();
            return Json("መረጃው ተሰርዟል..!!", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(string customerId)
        {
            string result = "";
            var customerGuid = Guid.Parse(customerId);
            var customer = db.Customerds.FirstOrDefault(c => c.CustomerId == customerGuid);
            if (customer != null)
            {
                var orders = db.Orderds.Where(o => o.CustomerId == customerGuid).ToList();
                foreach (Orderd o in orders)
                {
                    db.Orderds.Remove(o);
                }
                db.Customerds.Remove(customer);
                db.SaveChanges();
                result = "መረጃው ተሰርዟል";
            }
            else
            {
                result = "እቅዱ አልተገኘም..!!";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}