using BoldReports.Web.ReportViewer;
using System.Collections.Generic;
using System.Web.Http;



namespace PlanedBudgetPro.Controllers
{
    public class ReportViewerController : ApiController, IReportController
    {
        // Post action for processing the RDL/RDLC report
        public object PostReportAction(Dictionary<string, object> jsonResult)
        {
            return ReportHelper.ProcessReport(jsonResult, this);
        }

        // Get action for getting resources from the report
        [System.Web.Http.ActionName("GetResource")]
        [AcceptVerbs("GET")]
        public object GetResource(string key, string resourcetype, bool isPrint)
        {
            return ReportHelper.GetResource(key, resourcetype, isPrint);
        }

        // Method that will be called when initialize the report options before start processing the report
        [NonAction]
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            // You can update report options here
            reportOption.ReportModel.ReportServerCredential = new System.Net.NetworkCredential("Muhdin", "123");

            reportOption.ReportModel.DataSourceCredentials.Add(new BoldReports.Web.DataSourceCredentials("ReportServer", "Muhdin", "123"));
        }

        // Method that will be called when reported is loaded
        [NonAction]
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
            //You can update report options here
            //Add SSRS Report Server and data source credentials
            //  reportOption.ReportModel.ReportServerCredential = new System.Net.NetworkCredential("sa", "Test@@1234");

            // reportOption.ReportModel.DataSourceCredentials.Add(new BoldReports.Web.DataSourceCredentials("OnlineShop", "sa", "Test@@1234"));
        }
    }
}
