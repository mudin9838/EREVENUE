using PlanedBudgetPro.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
   
    public class HomeController : Controller
    {
        OnlineShopEntities db = new OnlineShopEntities();
        [HttpGet]
        public ActionResult Index()
        {
            ////Total number of parents
            //int totalParents = 0; //initialize
            //int parent = db.Parents.OrderBy(c => c.ParentId).Count();
            //totalParents += parent;
            //ViewBag.totalParents = totalParents;

            ////Total children
            //int totalChildrens = 0; //initialize
            //int children = db.Children.OrderBy(c => c.ChildId).Count();
            //totalChildrens += children;
            //ViewBag.totalChildrens = totalChildrens;

            ////Total category
            //int totalCategories = 0; //initialize
            //int categories = db.RevenueCategories.OrderBy(c => c.RevenueCategoryId).Count();
            //totalCategories += categories;
            //ViewBag.totalCategories = totalCategories;

            ////Total chartofaccount
            //int totalChartofaccounts = 0; //initialize
            //int chartofaccounts = db.ChartOfAccounts.OrderBy(c => c.ChartOfAccountId).Count();
            //totalChartofaccounts += chartofaccounts;
            //ViewBag.totalChartofaccounts = totalChartofaccounts;
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}