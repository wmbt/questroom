using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestRoom.Types;

namespace QuestRoom.Areas.Backend.Controllers
{
    public class PromoController : QuestRoomController
    {
        // GET: Backend/Promo
        public ActionResult Index()
        {
            return View();
        }
    }
}