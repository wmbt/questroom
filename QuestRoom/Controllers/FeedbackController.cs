using System.Linq;
using System.Web.Mvc;
using QuestRoom.Models;
using QuestRoom.Types;

namespace QuestRoom.Controllers
{
    public class FeedbackController : QuestRoomController
    {
        // GET: Feedback
        /*[HttpGet]
        public ActionResult Index()
        {
            var messages = Provider.GetConfirmedFeedbackMessages();
            return View(messages);
        }*/

        public ActionResult AddMessage(int questId)
        {
            var quest = Provider.GetQuest(questId);
            return View("AddMessage", new FeedbackMessageViewModel()
            {
                Quest = quest
                /*Quests = quests.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })*/
            });
        }

        [HttpPost]
        public ActionResult AddMessage(FeedbackMessageViewModel model)
        {
            var questId = int.Parse(Request.QueryString["questId"]);
            var email = model.Email.Trim();
            var msg = model.Text.Trim();

            if (!Provider.AddFeedbackMessage(questId, email, msg))
            {
                return View("AddMessageResult", new FeedbackMessageResultViewModel
                {
                    Title = Resources.Strings.FeedbackMessageFailureTitle,
                    Message = Resources.Strings.FeedbackMessageFailureMessage
                });
            }

            return View("AddMessageResult", new FeedbackMessageResultViewModel
            {
                Title = Resources.Strings.FeedbackMessageSuccessTitle,
                Message = Resources.Strings.FeedbackMessagSuccessMessage
            });
        }
    }
}