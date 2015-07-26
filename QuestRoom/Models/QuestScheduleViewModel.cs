using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestRoom.Types;

namespace QuestRoom.Models
{
    public class QuestScheduleViewModel
    {
        public QuestSchedule QuestSchedule { get; set; }
        public DateTime SelectedDate { get; set; }
    }
}