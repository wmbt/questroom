using System.Linq;
using System.Web.Mvc;
using QuestRoom.Models;
using QuestRoom.Types;

namespace QuestRoom.Controllers
{
    public class FeedbackController : QuestRoomController
    {
        // GET: Feedback
        [HttpGet]
        public ActionResult Index()
        {
            var messages = Provider.GetFeedbackMessages();
            return View(messages);
        }

        [HttpGet]
        public ActionResult AddMessage()
        {
            var quests = Provider.GetQuests();
            return View("AddMessage", new FeedbackMessageViewModel()
            {
                Quests = quests.Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            });
        }

        [HttpPost]
        public ActionResult AddMessage(FeedbackMessageViewModel model)
        {
            var email = model.Email.Trim();
            var msg = model.Text.Trim();

            if (!Provider.AddFeedbackMessage(model.QuestId, email, msg))
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