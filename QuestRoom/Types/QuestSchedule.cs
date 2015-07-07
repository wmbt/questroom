namespace QuestRoom.Types
{
    public class QuestSchedule
    {
        public Quest Quest { get; set; }
        public Period[] Schedule { get; set; }
    }
}