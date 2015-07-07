using System;
using System.Data;
using QuestRoom.Storage;

namespace QuestRoom.Types
{
    public class FeedbackMessage
    {
        public int Id { get; set; }
        public int QuestId { get; set; }
        public string QuestName { get; set; }
        public DateTime Created { get; set; }
        public FeedbackMessageStatus Status { get; set; }
        public string PlayerName { get; set; }
        public string Text { get; set; }

        public FeedbackMessage(IDataRecord dr)
        {
            Id = dr.GetValueOrDefault<int>("Id");
            QuestId = dr.GetValueOrDefault<int>("QuestId");
            QuestName = dr.GetValueOrDefault<string>("QuestName");
            Created = dr.GetValueOrDefault<DateTime>("Created");
            PlayerName = dr.GetValueOrDefault<string>("PlayerName");
            Text = dr.GetValueOrDefault<string>("Text");
            Status = (FeedbackMessageStatus)dr.GetValueOrDefault<byte>("Status");
        }
    }
}