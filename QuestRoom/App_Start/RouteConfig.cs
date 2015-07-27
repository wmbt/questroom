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
                name: "Login",
                url: "login",
                defaults: new { controller = "Account", action = "Login" }
            );
            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "Account", action = "Logout" }
            );

            /*routes.MapRoute(
                name: "Backend",
                url: "backend/{action}",
                defaults: new { controller = "Home", action = "Bookings" },
                namespaces: new[] { "QuestRoom.Areas.Backend.Controllers" }
            );*/

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
                name: "Quests",
                url: "quests/{id}",
                constraints: new RouteValueDictionary
                {
                    { "id", @"\d{1,2}" }
                },
                defaults: new { controller = "Quests", action = "Index" }
            );

            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Booking", action = "Index" }
            );
        }
    }
}
