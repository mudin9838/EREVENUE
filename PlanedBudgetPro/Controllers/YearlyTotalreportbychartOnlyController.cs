using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace PlanedBudgetPro.Controllers
{

    public class YearlyTotalreportbychartOnlyController : Controller
    {

        // GET: Yearlyreport
        public ActionResult Index()
        {
           
            return View();

        }


    }
}
