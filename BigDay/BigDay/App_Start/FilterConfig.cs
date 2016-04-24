using System.Web;
using System.Web.Mvc;

namespace BigDay
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //Uncomment below to enable authentication
            //filters.Add(new AuthorizeAttribute());
        }
    }
}
