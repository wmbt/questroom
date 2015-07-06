using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestRoom.Models
{
    public class ConfirmResultViewModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool ShowLinkToSchedule { get; set; }
    }
}