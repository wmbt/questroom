using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestRoom.Models
{
    public class FeedbackMessageViewModel
    {
        public int QuestId { get; set; }
        public IEnumerable<SelectListItem> Quests { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
    }
}