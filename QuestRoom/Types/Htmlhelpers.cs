using System;
using System.Collections.Generic;
using System.Linq;
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
                                            string controllerName
                                            )
        {

            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            var li = new TagBuilder("li") { InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToHtmlString() };

            if (actionName.ToLower() == currentAction.ToLower() && controllerName.ToLower() == currentController.ToLower())
                li.AddCssClass("active");

            return new MvcHtmlString(li.ToString());


        }
    } 
}