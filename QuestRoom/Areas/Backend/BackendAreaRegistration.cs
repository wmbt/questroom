using System.Web.Mvc;
using System.Web.Routing;

namespace QuestRoom.Areas.Backend
{
    public class BackendAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Backend";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "Backend",
                url: "backend/{action}/{param}",
                defaults: new { controller = "Home", action = "Bookings", param = UrlParameter.Optional },
                constraints: new RouteValueDictionary { { "date", @"\d{6}|^$" } },
                namespaces: new[] { "QuestRoom.Areas.Backend.Controllers" }
            );
        }
    }
}