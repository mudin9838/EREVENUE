﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using PagedList;
using PagedList.Mvc;
using PlanedBudgetPro.Models;
using PlanedBudgetPro.ViewModels;

namespace PlanBudgetPro.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderfdsController : Controller
    {
        // GET: Orders
        OnlineShopEntities db = new OnlineShopEntities();

        [HttpGet]

        public JsonResult LoadChartOfAccounts()
        {
            var chartOfAccount = db.ChartOfAccountfs
                .Select(m => new { value = m.ChartOfAccountId, text = m.ChartOfAccountName }).ToList();
            return new JsonResult { Data = chartOfAccount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        public JsonResult GetChartOfAccountList(string RevenueCategoryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var StateList = db.ChartOfAccountfs.Where(x => x.RevenueCategoryId == RevenueCategoryId).ToList();
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
            var parents = db.Parentfs
                .Select(m => new { value = m.ParentId, text = m.ParentName }).ToList();
            return new JsonResult { Data = parents, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetChildList(string ParentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var StateList = db.Childfs.Where(x => x.ParentId == ParentId).ToList();
            return Json(StateList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadChildren()
        {
            var children = db.Childfs
                .Select(m => new { value = m.ChildId, text = m.ChildName }).ToList();
            return new JsonResult { Data = children, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult LoadFisicalYears()
        {
            var fisicalYears = db.FisicalYears.Where(a => a.FisicalYearId != "1")
                .Select(m => new { value = m.FisicalYearId, text = m.FisicalYearName }).ToList();
            return new JsonResult { Data = fisicalYears, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

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
            IPagedList<Customerdf> OrderAndCustomerList = db.Customerdfs.ToList().OrderBy(c => c.ChildId).ThenByDescending(c => c.OrderDate).ToPagedList(page ?? 1, 20);
            return View(OrderAndCustomerList);
        }

        [HttpPost] // Add New And Edit 
        public ActionResult SaveOrder(DateTime orderdate, string id, string parentid, string childid, string fisicalyearid, string monthid, Orderfd[] orders)
        {
            string result = "መረጃው ተመዝግቧል!!";
            // New Entry
            if (string.IsNullOrEmpty(id))
            {
                if (orderdate != null && !string.IsNullOrEmpty(parentid.Trim()) && !string.IsNullOrEmpty(childid.Trim()) && !string.IsNullOrEmpty(fisicalyearid.Trim()) && !string.IsNullOrEmpty(monthid.Trim()) && orders != null)
                {
                    var customerId = Guid.NewGuid();
                    Customerdf customer = new Customerdf
                    {
                        ParentId = parentid,
                        ChildId = childid,
                        FisicalYearId = fisicalyearid,
                        MonthId = monthid,
                        OrderDate = orderdate,
                        CustomerId = customerId
                    };
                    db.Customerdfs.Add(customer);

                    foreach (var o in orders)
                    {
                        var orderId = Guid.NewGuid();
                        Orderfd order = new Orderfd();
                        order.CustomerId = customerId;
                        order.Amountyplan = o.Amountyplan;
                        order.Amountyperformance = o.Amountyperformance;
                        order.RevenueCategoryId = o.RevenueCategoryId;
                        order.ChartOfAccountId = o.ChartOfAccountId;
                        order.OrderId = Guid.NewGuid();
                        db.Orderfds.Add(order);

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
                var customerInDb = db.Customerdfs.FirstOrDefault(c => c.CustomerId == customerGuid);
                customerInDb.ParentId = parentid;
                customerInDb.ChildId = childid;
                customerInDb.FisicalYearId = fisicalyearid;
                customerInDb.MonthId = monthid;
                customerInDb.OrderDate = orderdate;

                foreach (var o in orders)
                {
                    var dbOrder = db.Orderfds.FirstOrDefault(odr => odr.OrderId == o.OrderId);
                    if (dbOrder != null)
                    {
                        dbOrder.Amountyperformance = o.Amountyperformance;
                        dbOrder.Amountyplan = o.Amountyplan;
                        dbOrder.RevenueCategoryId = o.RevenueCategoryId;
                        dbOrder.ChartOfAccountId = o.ChartOfAccountId;
                    }
                    else
                    {
                        Orderfd order = new Orderfd();
                        order.OrderId = Guid.NewGuid();
                        order.Amountyperformance = o.Amountyperformance;
                        order.Amountyplan = o.Amountyplan;
                        order.RevenueCategoryId = o.RevenueCategoryId;
                        order.ChartOfAccountId = o.ChartOfAccountId;
                        order.CustomerId = customerGuid;
                        db.Orderfds.Add(order);
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
            var customerDetails = db.Customerdfs.Include("Orderfds").Single(c => c.CustomerId == customerGuid);

            vmCustomer Customer = new vmCustomer();
            Customer.CustomerId = customerDetails.CustomerId;
            Customer.ParentId = customerDetails.ParentId;
            Customer.ChildId = customerDetails.ChildId;
            Customer.FisicalYearId = customerDetails.FisicalYearId;
            Customer.MonthId = customerDetails.MonthId;
            Customer.OrderDate = customerDetails.OrderDate;

            List<vmOrder> orderslList = new List<vmOrder>();

            foreach (var o in customerDetails.Orderfds)
            {
                vmOrder order = new vmOrder();
                order.OrderId = o.OrderId;
                order.Amountyplan = o.Amountyplan;
                order.Amountyperformance = o.Amountyperformance;
                order.RevenueCategoryId = o.RevenueCategoryId;
                order.ChartOfAccountId = o.ChartOfAccountId;

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
            var order = db.Orderfds.Find(orderGuid);
            db.Orderfds.Remove(order);
            db.SaveChanges();
            return Json("መረጃው ተሰርዟል..!!", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(string customerId)
        {
            string result = "";
            var customerGuid = Guid.Parse(customerId);
            var customer = db.Customerdfs.FirstOrDefault(c => c.CustomerId == customerGuid);
            if (customer != null)
            {
                var orders = db.Orderfds.Where(o => o.CustomerId == customerGuid).ToList();
                foreach (Orderfd o in orders)
                {
                    db.Orderfds.Remove(o);
                }
                db.Customerdfs.Remove(customer);
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