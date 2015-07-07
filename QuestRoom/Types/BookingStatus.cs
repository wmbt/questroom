using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace QuestRoom.Types
{
    public enum BookingStatus
    {
        [Description("Новая")]
        New,
        [Description("Подтверждена")]
        Confirmed,
        [Description("Отменена")]
        Canceled
    }
}