﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace QuestRoom.Types
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper,
                                            string linkText,
                                            string actionName,
                                            string controllerName,
                                            string route = "Default"
                                            )
        {

            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            //var li = new TagBuilder("li") { InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName, new{}, new{}).ToHtmlString() };

            var li = new TagBuilder("li") { InnerHtml = htmlHelper.RouteLink(linkText, route, new { action = actionName, controller = controllerName }).ToHtmlString() };

            if (actionName.ToLower() == currentAction.ToLower() && string.Equals(controllerName, currentController, StringComparison.CurrentCultureIgnoreCase))
                li.AddCssClass("active");

            return new MvcHtmlString(li.ToString());


        }

        public static MvcHtmlString Stars(this HtmlHelper htmlHelper, int count, int maxCount = 4)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < maxCount; i++)
            {
                sb.Append(i <= count - 1
                    ? "<span class=\"glyphicon glyphicon-star\"></span>"
                    : "<span class=\"glyphicon glyphicon-star inactive\"></span>");
            }
            return new MvcHtmlString(sb.ToString());
        }
    } 
}