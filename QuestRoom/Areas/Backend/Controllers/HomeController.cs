using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestRoom.Types;

namespace QuestRoom.Areas.Backend.Controllers
{
    [Authorize]
    public class HomeController : QuestRoomController
    {
        // GET: Backend/Home
        public ActionResult Bookings()
        {
            return View();
        }

        public ActionResult Feedback()
        {
            return View();
        }
    }
}