using System.Web;
using System.Web.Optimization;

namespace AdminApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Assets/Styles").Include(
                      "~/Assets/Styles/Stylesheet.css"));
        }
    }
}
