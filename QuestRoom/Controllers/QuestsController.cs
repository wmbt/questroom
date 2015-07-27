using System.Web.Mvc;
using QuestRoom.Models;
using QuestRoom.Types;

namespace QuestRoom.Controllers
{
    public class QuestsController : QuestRoomController
    {
        // GET: Info
        public ActionResult Index(int id)
        {
            var quest = Provider.GetQuest(id);
            var comments = Provider.GetFeedbackMessages(id);
            return View(new QuestViewModel()
            {
                Quest = quest,
                Comments = comments
            });
        }

        
    }
}