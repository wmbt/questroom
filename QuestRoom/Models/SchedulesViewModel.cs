using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestRoom.Types;

namespace QuestRoom.Models
{
    public class SchedulesViewModel
    {
        public QuestSchedule[] QuestSchedules { get; set; }
        public Cost[] Costs { get; set; }
        public DateTime SelectedDate { get; set; }
        public DateTime CurrentMinDate { get; set; }
        public DateTime CurrentMaxDate { get; set; }
        public DateTime JumpNextDate { get; set; }
        public DateTime JumpPrevDate { get; set; }
    }
}
