using System.Data;
using QuestRoom.Storage;

namespace QuestRoom.Types
{
    public class Cost
    {
        public int Id { get; set; }
        public string Persons { get; set; }
        public int WorkdaysEvening { get; set; }
        public int WorkdaysDay { get; set; }
        public int Weekends { get; set; }

        public Cost(IDataRecord dr)
        {
            Id = dr.GetValueOrDefault<int>("Id");
            Persons = dr.GetValueOrDefault<string>("Persons");
            WorkdaysDay = dr.GetValueOrDefault<int>("WorkdaysDay");
            WorkdaysEvening = dr.GetValueOrDefault<int>("WorkdaysEvening");
            Weekends = dr.GetValueOrDefault<int>("Weekends");
        }
    }
}