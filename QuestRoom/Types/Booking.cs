using System;
using System.Data;
using QuestRoom.Storage;

namespace QuestRoom.Types
{
    public class Booking
    {
        public int Id { get; set; }
        public int QuestId { get; set; }
        public string QuestName { get; set; }
        public DateTime Date { get; set; }
        public string PlayerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public BookingStatus Status { get; set; }
        public DateTime Created { get; set; }
        public int? OperatorId { get; set; }
        public string OperatorName { get; set; }
        public DateTime? Processed { get; set; }
        public int Cost { get; set; }
        public string Persons { get; set; }

        public Booking(IDataRecord dr)
        {
            Id = dr.GetValueOrDefault<int>("Id");
            QuestId = dr.GetValueOrDefault<int>("QuestId");
            QuestName = dr.GetValueOrDefault<string>("QuestName");
            Date = dr.GetValueOrDefault<DateTime>("Date");
            PlayerName = dr.GetValueOrDefault<string>("PlayerName");
            Email = dr.GetValueOrDefault<string>("Email");
            Phone = dr.GetValueOrDefault<string>("Phone");
            Comment = dr.GetValueOrDefault<string>("Comment");
            Status = (BookingStatus)dr.GetValueOrDefault<byte>("Status");
            Created = dr.GetValueOrDefault<DateTime>("Created");
            OperatorId = dr.GetValueOrDefault<int?>("OperatorId");
            OperatorName = dr.GetValueOrDefault<string>("OperatorName");
            Processed = dr.GetValueOrDefault<DateTime?>("Processed");
            Cost = dr.GetValueOrDefault<int>("Cost");
            Persons = dr.GetValueOrDefault<string>("Persons");
        }
    }
}