using SkyFlow.Core.Entities;
using SkyFlow.Data;

namespace SkyFlow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "SkyFlow - Airport & Airline Management System";

            var repo = new SqlRepository();

            Console.WriteLine("=================================================");
            Console.WriteLine("       Welcome to SkyFlow Management System");
            Console.WriteLine("=================================================\n");

            while (true)
            {
                Console.Write("Username: ");
                string? username = Console.ReadLine();

                Console.Write("Password: ");
                string? password = Console.ReadLine();

                var user = repo.Authenticate(username ?? "", password ?? "");

                if (user != null)
                {
                    Console.Clear();
                    user.DisplayDashboard(repo);
                }
                else
                {
                    Console.WriteLine("❌ Invalid credentials. Please try again.\n");
                    Console.ReadKey(true);
                }
            }
        }
    }
}