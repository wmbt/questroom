using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestRoom.Types;

namespace QuestRoom.Models
{
    public class QuestViewModel
    {
        public Quest Quest { get; set; }
        public FeedbackMessage[] Comments { get; set; }
    }
}