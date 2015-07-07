using System.Data;

namespace QuestRoom.Types
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public string Email { get; set; }

        public User(IDataRecord dr)
        {
            Id = dr.GetValueOrDefault<int>("Id");
            Login = dr.GetValueOrDefault<string>("Login");
            Name = dr.GetValueOrDefault<string>("Name");
            Active = dr.GetValueOrDefault<bool>("Active");
            Email = dr.GetValueOrDefault<string>("Email");
        }
    }
}
