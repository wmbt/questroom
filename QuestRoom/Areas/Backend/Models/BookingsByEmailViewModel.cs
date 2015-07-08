using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QuestRoom.Types;

namespace QuestRoom.Areas.Backend.Models
{
    public class BookingsByEmailViewModel
    {
        public string Email { get; set; }
        public Booking[] Bookings { get; set; }
    }
}