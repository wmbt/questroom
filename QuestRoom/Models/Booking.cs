using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuestRoom.Models
{
    public class Booking
    {
        public Quest Quest { get; set; }
        public Period[] Schedule { get; set; }
    }
}