using System.Web.Mvc;
using QuestRoom.Models;
using QuestRoom.Types;

namespace QuestRoom.Controllers
{
    public class InfoController : QuestRoomController
    {
        // GET: Info
        public ActionResult Index()
        {
            var quests = Provider.GetQuests();
            return View(new InfoViewModel { Quests = quests });
        }

        
    }
}