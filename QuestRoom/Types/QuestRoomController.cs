using System;
using System.Web.Mvc;
using QuestRoom.Storage;

namespace QuestRoom.Types
{
    public abstract class QuestRoomController : Controller
    {
        protected Provider Provider = new Provider();
    }
}