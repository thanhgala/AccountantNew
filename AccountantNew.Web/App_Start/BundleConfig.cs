using System.Web;
using System.Web.Optimization;

namespace AccountantNew.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundle/jsMVC").Include(
                "~/Assets/client/js/jquery.min.js",
                "~/Assets/client/js/bootstrap.min.js",
                "~/Assets/client/js/smoothscroll.js",
                "~/Assets/client/js/jquery.simplyscroll.js",
                "~/Assets/client/js/classie.js",
                "~/Assets/client/js/selectFx.js",
                "~/Assets/client/js/jquery.nanoscroller.js",
                "~/Assets/client/js/owl.carousel.min.js",
                "~/Assets/client/js/jquery.flexslider-min.js",
                "~/Assets/client/js/jquery.sticky.js",
                "~/Assets/client/js/main.js",
                "~/Assets/client/js/switcher.js",
                "~/Assets/admin/libs/jstree/dist/jstree.js",
                "~/Assets/admin/libs/tinymce/tinymce.min.js",
                "~/Assets/admin/libs/ckeditor/ckeditor.js",
                "~/Assets/admin/libs/toastr/toastr.min.js"
            ));

            var bundleCss =  new StyleBundle("~/bundle/css").Include(
                      "~/Assets/client/css/bootstrap.min.css",
                      "~/Assets/client/css/cs-select.css",
                      "~/Assets/client/css/animate.css",
                      "~/Assets/client/css/nanoscroller.css",
                      "~/Assets/client/css/owl.carousel.css",
                      "~/Assets/client/css/flexslider.css",
                      "~/Assets/client/css/style.css",
                      "~/Assets/client/css/presets/preset1.css",
                      "~/Assets/client/css/responsive.css",
                      "~/Assets/client/css/presets/preset1.css",
                      "~/Assets/client/css/articleImg.css",
                      "~/Assets/client/css/weather.css",
                      "~/Assets/client/css/forum.css",
                      "~/Assets/client/css/jquery.simplyscroll.css",
                      "~/Assets/admin/libs/jstree/dist/themes/proton/style.css",
                      "~/Assets/client/css/treefile.css",
                      "~/app/components/news/img.css",
                      "~/Assets/admin/libs/toastr/toastr.min.css"
                      ).Include("~/Assets/client/css/font-awesome.min.css", new CssRewriteUrlTransform());
            bundles.Add(bundleCss);
            BundleTable.EnableOptimizations = false;
        }
    }
}
