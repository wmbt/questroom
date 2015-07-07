using System;
using System.Data;

namespace QuestRoom.Types
{
    public class Quest
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Complexity { get; private set; }
        public string Persons { get; private set; }
        public TimeSpan Duration { get; private set; }
        public bool Active { get; private set; }
        public bool Visible { get; private set; }
        public string Note { get; private set; }

        public Quest(IDataRecord dr)
        {
            Id = dr.GetValueOrDefault<int>("Id");
            Name = dr.GetValueOrDefault<string>("Name");
            Description = dr.GetValueOrDefault<string>("Description");
            Complexity = dr.GetValueOrDefault<int>("Complexity");
            Persons = dr.GetValueOrDefault<string>("Persons");
            Duration = dr.GetValueOrDefault<TimeSpan>("Duration");
            Active = dr.GetValueOrDefault<bool>("Active");
            Visible = dr.GetValueOrDefault<bool>("Visible");
            Note = dr.GetValueOrDefault<string>("Note");
        }
    }
}