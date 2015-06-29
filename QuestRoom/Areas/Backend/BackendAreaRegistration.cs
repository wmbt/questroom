﻿using System.Web.Mvc;

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
                "Backend_default",
                "Backend/{controller}/{action}",
                new { controler = "Home", action = "Index" }
            );
        }
    }
}