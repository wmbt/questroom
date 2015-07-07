using System.Data;
using QuestRoom.Storage;

namespace QuestRoom.Types
{
    public class Cost
    {
        public int Id { get; set; }
        public string Persons { get; set; }
        public int Workdays { get; set; }
        public int Weekends { get; set; }

        public Cost(IDataRecord dr)
        {
            Id = dr.GetValueOrDefault<int>("Id");
            Persons = dr.GetValueOrDefault<string>("Persons");
            Workdays = dr.GetValueOrDefault<int>("Workdays");
            Weekends = dr.GetValueOrDefault<int>("Weekends");
        }
    }
}