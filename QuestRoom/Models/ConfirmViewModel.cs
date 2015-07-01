using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestRoom.Models
{
    public class ConfirmViewModel
    {
        public DateTime SelectedDate { get; set; }
        public TimeSpan SelectedTime { get; set; }
        public Cost[] Costs { get; set; }
        public Quest Quest { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
    }
}