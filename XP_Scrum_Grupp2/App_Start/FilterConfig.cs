using System.Web;
using System.Web.Mvc;

namespace XP_Scrum_Grupp2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
