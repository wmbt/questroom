using System;
using System.Data;

namespace QuestRoom.Types
{
    public class Period
    {
        public int Id { get; set; }
        public int QuestId { get; set; }
        public TimeSpan BeginTime { get; set; }
        public bool Booked { get; set; }
        public TimeSpan EndTime { get; set; }

        public Period(IDataRecord dr)
        {
            Id = dr.GetValueOrDefault<int>("Id");
            QuestId = dr.GetValueOrDefault<int>("QuestId");
            BeginTime = dr.GetValueOrDefault<TimeSpan>("BeginTime");
            EndTime = dr.GetValueOrDefault<TimeSpan>("EndTime");
            Booked = dr.GetValueOrDefault<bool>("Booked");
        }
    }
}