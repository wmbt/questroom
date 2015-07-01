using System;
using System.Data;
using QuestRoom.Storage;

namespace QuestRoom.Models
{
    public class Period
    {
        public int Id { get; set; }
        public int QuestId { get; set; }
        public TimeSpan BeginTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public Period(IDataRecord dr)
        {
            Id = dr.GetValueOrDefault<int>("Id");
            QuestId = dr.GetValueOrDefault<int>("QuestId");
            BeginTime = dr.GetValueOrDefault<TimeSpan>("BeginTime");
            EndTime = dr.GetValueOrDefault<TimeSpan>("EndTime");
        }
    }
}