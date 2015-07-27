using QuestRoom.Types;

namespace QuestRoom.Models
{
    public class FeedbackMessageViewModel
    {
        public Quest Quest { get; set; }
        //public IEnumerable<SelectListItem> Quests { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
    }
}