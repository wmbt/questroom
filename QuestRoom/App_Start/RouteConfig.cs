using System.Web.Mvc;
using System.Web.Routing;

namespace QuestRoom
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Booking",
                url: "{date}",
                constraints: new RouteValueDictionary { { "date", @"\d{6}|^$" } },
                defaults: new { controller = "Booking", action = "Index", date = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Confirm",
                url: "confirm/{questid}/{date}/{time}",
                constraints: new RouteValueDictionary
                {
                    { "questid", @"\d{1,2}" },    
                    { "date", @"\d{6}" },
                    { "time", @"\d{4}" }
                },
                defaults: new { controller = "Booking", action = "Confirm" }
            );

            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Booking", action = "Index" }
            );
        }
    }
}
