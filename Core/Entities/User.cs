using SkyFlow.Core.Interfaces;

namespace SkyFlow.Core.Entities
{
    public abstract class User
    {
        public int UserId { get; private set; }
        public string Username { get; private set; }
        public string Name { get; private set; }
        public string Role { get; private set; }

        protected User(int userId, string username, string name, string role)
        {
            UserId = userId;
            Username = username;
            Name = name;
            Role = role;
        }

        public abstract void DisplayDashboard(IDataRepository repo);

        public virtual void Logout()
        {
            Console.WriteLine("\n✅ Logged out successfully.");
        }
    }
}