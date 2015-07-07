using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestRoom.Types;

namespace QuestRoom.Areas.Backend.Models
{
    public class BookingsViewModel
    {
        public DateTime CurrentDate { get; set; }
        public Booking[] Bookings { get; set; }
    }
}