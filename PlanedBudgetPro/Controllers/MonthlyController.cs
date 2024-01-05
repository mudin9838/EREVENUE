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
using System.Security.Policy;


namespace PlanedBudgetPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MonthlyController : Controller
    {
        // GET: Orders
        OnlineShopEntities db = new OnlineShopEntities();
    
        [HttpGet]
        public JsonResult LoadFisicalYears()
        {
            var fisicalYears = db.FisicalYears
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

        public JsonResult LoadParents()
        {
            var parents = db.Parents
                .Select(m => new { value = m.ParentId, text = m.ParentName }).ToList();
            return new JsonResult { Data = parents, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
            return new JsonResult { Data = children, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpGet]
        public ActionResult Index(int? page)
        {
            // var user = await UserManager.FindByIdAsync(User.Identity.GetUserId())

            //List<Customer> OrderAndCustomerList = db.Customers.ToList();
            IPagedList<Customerm> OrderAndCustomerList = db.Customerms.Where(a => a.FisicalYearId != "1").OrderBy(c => c.ParentId).ThenByDescending(c=>c.OrderDate).ToPagedList(page ?? 1, 10);

            return View(OrderAndCustomerList);
        }

        [HttpPost] // Add New And Edit 
        public ActionResult SaveOrder(DateTime orderdate, string id, string parentid, string childid, string fisicalyearid, string monthid, Orderm[] orders)
        {
            string result = "መረጃው ተመዝግቧል!!";
            // New Entry
            if (string.IsNullOrEmpty(id))
            {
                if (orderdate != null && !string.IsNullOrEmpty(parentid.Trim()) && !string.IsNullOrEmpty(childid.Trim()) && !string.IsNullOrEmpty(fisicalyearid.Trim()) && !string.IsNullOrEmpty(monthid.Trim()) && orders != null)
                {
                    var customerId = Guid.NewGuid();
                    Customerm customer = new Customerm
                    {
                        ParentId = parentid,
                        ChildId = childid,
                        FisicalYearId = fisicalyearid,
                        MonthId = monthid,
                        OrderDate = orderdate,
                        CustomerId = customerId
                    };
                    db.Customerms.Add(customer);

                    foreach (var o in orders)
                    {
                        var orderId = Guid.NewGuid();
                        Orderm order = new Orderm();
                        order.CustomerId = customerId;
                        order.Amountyplan = o.Amountyplan;
                        order.Amountyperformance = o.Amountyperformance;
                        //order.Amount = o.Amount;
                        order.ChartOfAccountId = o.ChartOfAccountId;
                        order.RevenueCategoryId = o.RevenueCategoryId;
                        order.OrderId = Guid.NewGuid();
                        db.Orderms.Add(order);

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
                var customerInDb = db.Customerms.FirstOrDefault(c => c.CustomerId == customerGuid);
                customerInDb.ParentId = parentid;
                customerInDb.ChildId = childid;
                customerInDb.FisicalYearId = fisicalyearid;
                customerInDb.MonthId = monthid;
                customerInDb.OrderDate = orderdate;
                //db.Customers.Add(customerInDb);

                foreach (var o in orders)
                {
                    var dbOrder = db.Orderms.FirstOrDefault(odr => odr.OrderId == o.OrderId);
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
                        Orderm order = new Orderm();
                        order.OrderId = Guid.NewGuid();
                        order.Amountyperformance = o.Amountyperformance;
                        order.Amountyplan = o.Amountyplan;
                        order.RevenueCategoryId = o.RevenueCategoryId;
                        order.ChartOfAccountId = o.ChartOfAccountId;
                        //  order.Quantity = o.Quantity;
                        order.CustomerId = customerGuid;
                        db.Orderms.Add(order);
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
            var customerDetails = db.Customerms.Include("Orderms").Single(c => c.CustomerId == customerGuid);

            vmCustomer Customer = new vmCustomer();
            Customer.CustomerId = customerDetails.CustomerId;
            Customer.ParentId = customerDetails.ParentId;
            Customer.ChildId = customerDetails.ChildId;
            Customer.FisicalYearId = customerDetails.FisicalYearId;
            Customer.MonthId = customerDetails.MonthId;
            Customer.OrderDate = customerDetails.OrderDate;

            List<vmOrder> orderslList = new List<vmOrder>();

            foreach (var o in customerDetails.Orderms)
            {
                vmOrder order = new vmOrder();
                order.OrderId = o.OrderId;
                order.ChartOfAccountId = o.ChartOfAccountId;
                order.Amountyplan = o.Amountyplan;
                order.Amountyperformance = o.Amountyperformance;
                // order.Amount = o.Amount;
                order.ChartOfAccountId = o.ChartOfAccountId;
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
            var order = db.Orderms.Find(orderGuid);
            db.Orderms.Remove(order);
            db.SaveChanges();
            return Json("መረጃው ተሰርዟል..!!", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(string customerId)
        {
            string result = "";
            var customerGuid = Guid.Parse(customerId);
            var customer = db.Customerms.FirstOrDefault(c => c.CustomerId == customerGuid);
            if (customer != null)
            {
                var orders = db.Orderms.Where(o => o.CustomerId == customerGuid).ToList();
                foreach (Orderm o in orders)
                {
                    db.Orderms.Remove(o);
                }
                db.Customerms.Remove(customer);
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