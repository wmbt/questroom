using System.ComponentModel;

namespace QuestRoom.Types
{
    public enum FeedbackMessageStatus
    {
        [Description("Новый")]
        New,
        [Description("Опубликован")]
        Published,
        [Description("Заблокирован")]
        Banned
    }
}