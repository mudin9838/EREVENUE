using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlanedBudgetPro.Models;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;
using PlanedBudgetPro.ViewModels;
using Microsoft.AspNet.Identity;
using IdentitySample.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;

namespace AspNetIdentity2ExtendingUsersAndRoles.Controllers
{
    [Authorize]
    public class OrdersController : Controller 
    {
        // GET: Orders
        OnlineShopEntities db = new OnlineShopEntities();
        private ApplicationDbContext dbs;
        private UserManager<ApplicationUser> manager;
        public OrdersController()
        {
            dbs = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbs));
        }
   


        // GET: /ToDo/
        // GET ToDo for the logged in user
       

        
        [HttpGet]
        public JsonResult LoadFisicalYears()
        {
            var fisicalYears = db.FisicalYears.Where(a => a.FisicalYearId != "1")
                .Select(m => new { value = m.FisicalYearId, text = m.FisicalYearName }).ToList();
            return new JsonResult { Data = fisicalYears, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
        public JsonResult LoadChartOfAccounts()
        {
            var chartOfAccount = db.ChartOfAccounts
                .Select(m => new { value = m.ChartOfAccountId, text = m.ChartOfAccountName }).ToList();
            return new JsonResult { Data = chartOfAccount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        public JsonResult GetChartOfAccountList(string RevenueCategoryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<ChartOfAccount> StateList = db.ChartOfAccounts.Where(x => x.RevenueCategoryId == RevenueCategoryId).ToList();
            return Json(StateList, JsonRequestBehavior.AllowGet);

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
     

        [HttpGet]
        public ActionResult Index(int? page)
        {
            // var user = await UserManager.FindByIdAsync(User.Identity.GetUserId())
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var SITEID = currentUser.ParentId;
            //List<Customer> OrderAndCustomerList = db.Customers.ToList();
            IPagedList<Customer> OrderAndCustomerList = db.Customers.Where(c => c.ParentId == SITEID).Where(a => a.FisicalYearId != "1").OrderBy(c => c.ChildId).ThenBy(c=>c.FisicalYearId).ThenByDescending(c => c.OrderDate).ToPagedList(page ?? 1, 20);
            return View(OrderAndCustomerList);
        }
        // GET: //AllPlans
      

        [HttpPost] // Add New And Edit 
        public ActionResult SaveOrder(DateTime orderdate, string id, string parentid, string childid, string fisicalyearid, Order[] orders)
        {
            string result = "መረጃው ተመዝግቧል!!";
            // New Entry
            if (string.IsNullOrEmpty(id))
            {
                if (orderdate != null && !string.IsNullOrEmpty(parentid.Trim()) && !string.IsNullOrEmpty(childid.Trim()) && !string.IsNullOrEmpty(fisicalyearid.Trim()) && orders != null)
                {
                    //var currentUser = manager.FindById(User.Identity.GetUserId());
                    //var SITEID = currentUser.ChildId;
                    //var PARENTID = currentUser.ParentId;
                    var customerId = Guid.NewGuid();
                    Customer customer = new Customer
                    {
                        ParentId = parentid,
                        ChildId = childid,
                        FisicalYearId = fisicalyearid,
                        OrderDate = orderdate,
                        CustomerId = customerId
                    };
                    db.Customers.Add(customer);

                    foreach (var o in orders)
                    {
                        var orderId = Guid.NewGuid();
                        Order order = new Order();
                        order.CustomerId = customerId;
                        order.Amountyplan = o.Amountyplan;
                        order.Amountyperformance = o.Amountyperformance;
                        //order.Amount = o.Amount;
                        order.ChartOfAccountId = o.ChartOfAccountId;
                        order.RevenueCategoryId = o.RevenueCategoryId;
                        order.OrderId = Guid.NewGuid();
                        db.Orders.Add(order);

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
                var customerInDb = db.Customers.FirstOrDefault(c => c.CustomerId == customerGuid);
                customerInDb.ParentId = parentid;
                customerInDb.ChildId = childid;
                customerInDb.FisicalYearId = fisicalyearid;
                customerInDb.OrderDate = orderdate;
                //db.Customers.Add(customerInDb);

                foreach (var o in orders)
                {
                    var dbOrder = db.Orders.FirstOrDefault(odr => odr.OrderId == o.OrderId);
                    if (dbOrder != null)
                    {
                        dbOrder.Amountyperformance = o.Amountyperformance;
                        dbOrder.Amountyplan = o.Amountyplan;
                        dbOrder.ChartOfAccountId = o.ChartOfAccountId;
                        // dbOrder.Amount = o.Amount;
                        dbOrder.RevenueCategoryId = o.RevenueCategoryId;
                    }
                    else
                    {
                        Order order = new Order();
                        order.OrderId = Guid.NewGuid();
                        order.Amountyperformance = o.Amountyperformance;
                        order.Amountyplan = o.Amountyplan;
                        order.RevenueCategoryId = o.RevenueCategoryId;
                        order.ChartOfAccountId = o.ChartOfAccountId;
                        //  order.Quantity = o.Quantity;
                        order.CustomerId = customerGuid;
                        db.Orders.Add(order);
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
            var customerDetails = db.Customers.Include("Orders").Single(c => c.CustomerId == customerGuid);

            vmCustomer Customer = new vmCustomer();
            Customer.CustomerId = customerDetails.CustomerId;
            Customer.ParentId = customerDetails.ParentId;
            Customer.ChildId = customerDetails.ChildId;
            Customer.FisicalYearId = customerDetails.FisicalYearId;
            Customer.OrderDate = customerDetails.OrderDate;

            List<vmOrder> orderslList = new List<vmOrder>();

            foreach (var o in customerDetails.Orders)
            {
                vmOrder order = new vmOrder();
                order.OrderId = o.OrderId;
                order.ChartOfAccountId = o.ChartOfAccountId;
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
            var order = db.Orders.Find(orderGuid);
            db.Orders.Remove(order);
            db.SaveChanges();
            return Json("መረጃው ተሰርዟል..!!", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(string customerId)
        {
            string result = "";
            var customerGuid = Guid.Parse(customerId);
            var customer = db.Customers.FirstOrDefault(c => c.CustomerId == customerGuid);
            if (customer != null)
            {
                var orders = db.Orders.Where(o => o.CustomerId == customerGuid).ToList();
                foreach (Order o in orders)
                {
                    db.Orders.Remove(o);
                }
                db.Customers.Remove(customer);
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