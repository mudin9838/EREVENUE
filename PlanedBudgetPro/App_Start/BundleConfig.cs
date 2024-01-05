using System.Web.Optimization;

namespace IdentitySample
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                     // "~/Content/bootstrap.css",
                     "~/Content/Site.css"
                      ));
            bundles.Add(new StyleBundle("~/Adminlte/css").Include(
                    //  "~/Scripts/jquery-ui-1.13.1.min.js",
                      "~/Scripts/AdminLTE/plugins/fontawesome-free/css/all.min.css",                                           "~/node_modules/bootstrap-icons/font/bootstrap-icons.css",
                       "~/node_modules/bootstrap-icons/font/bootstrap-icons.css",
                      "~/Content/ionicons.min.css",
                      "~/Content/AdminLTE/adminlte.min.css",
                      "~/css/adminlte.min.css",
                      "~/Content/css.css",
                      "~/Scripts/AdminLTE/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
                      "~/Scripts/AdminLTE/plugins/summernote/summernote-bs4.css",
                      "~/Scripts/AdminLTE/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css",
                      "~/Scripts/AdminLTE/plugins/datatables-responsive/css/responsive.bootstrap4.min.css",
                      "~/Scripts/AdminLTE/plugins/datatables-buttons/css/buttons.bootstrap4.min.css",
                      "~/Scripts/AdminLTE/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                      "~/Scripts/AdminLTE/plugins/jqvmap/jqvmap.min.css",
                      "~/Scripts/AdminLTE/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                      "~/Scripts/AdminLTE/plugins/daterangepicker/daterangepicker.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/customscript").Include(
          "~/Scripts/AdminLTE/plugins/bootstrap/js/bootstrap.bundle.min.js",
          "~/js/adminlte.min.js",
          "~/js/demo.js",
          "~/Scripts/AdminLTE/plugins/summernote/summernote-bs4.min.js",
          "~/Scripts/AdminLTE/plugins/datatables/jquery.dataTables.min.js",
         "~/Scripts/AdminLTE/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js",
         "~/Scripts/AdminLTE/plugins/datatables-responsive/js/dataTables.responsive.min.js",
         "~/Scripts/AdminLTE/plugins/datatables-responsive/js/responsive.bootstrap4.min.js",
         "~/Scripts/AdminLTE/plugins/datatables-buttons/js/dataTables.buttons.min.js",
         "~/Scripts/AdminLTE/plugins/datatables-buttons/js/buttons.bootstrap4.min.js",
         "~/Scripts/AdminLTE/plugins/jszip/jszip.min.js",
         "~/Scripts/AdminLTE/plugins/pdfmake/pdfmake.min.js",
         "~/Scripts/AdminLTE/plugins/pdfmake/vfs_fonts.js",
         "~/Scripts/AdminLTE/plugins/datatables-buttons/js/buttons.html5.min.js",
         "~/Scripts/AdminLTE/plugins/datatables-buttons/js/buttons.print.min.js",
         "~/Scripts/AdminLTE/plugins/datatables-buttons/js/buttons.colVis.min.js",
         "~/Scripts/AdminLTE/plugins/chart.js/Chart.min.js",
         "~/js/site.js"
         ));
        }
    }
}
