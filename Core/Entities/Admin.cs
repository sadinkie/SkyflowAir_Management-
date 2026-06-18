using SkyFlow.Core.Interfaces;
using SkyFlow.Config;
using SkyFlow.Core.Helpers;
using Microsoft.Data.SqlClient;

namespace SkyFlow.Core.Entities
{
    public class Admin : User
    {
        public Admin(int userId, string username, string name)
            : base(userId, username, name, "Admin") { }

        public override void DisplayDashboard(IDataRepository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ADMIN DASHBOARD ===");
                Console.WriteLine($"Welcome, {Name}!\n");
                Console.WriteLine("1. Manage Flights");
                Console.WriteLine("2. System Oversight (All Flights + Occupancy)");
                Console.WriteLine("3. Staff Management");
                Console.WriteLine("4. Logout");
                Console.Write("\nSelect option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageFlights(repo);
                        break;
                    case "2":
                        SystemOversight(repo);
                        break;
                    case "3":
                        ManageStaff(repo);
                        break;
                    case "4":
                        Logout();
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey(true);
            }
        }

        private void ManageFlights(IDataRepository repo)
        {
            Console.WriteLine("\n--- Manage Flights ---");
            Console.WriteLine("1. Add New Flight");
            Console.WriteLine("2. View All Flights");
            Console.Write("Choose: ");
            string? sub = Console.ReadLine();

            if (sub == "1")
            {
                Console.Write("Flight Number: ");
                string fnum = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrEmpty(fnum))
                {
                    Console.WriteLine("❌ Flight Number cannot be empty!");
                    return;
                }

                Console.Write("Origin: ");
                string origin = Console.ReadLine()?.Trim() ?? "";
                Console.Write("Destination: ");
                string dest = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Departure Time (yyyy-MM-dd HH:mm): ");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime depTime))
                {
                    Console.WriteLine("❌ Invalid date format!");
                    return;
                }

                Console.Write("Capacity: ");
                if (!int.TryParse(Console.ReadLine(), out int capacity) || capacity <= 0)
                {
                    Console.WriteLine("❌ Capacity must be a positive number!");
                    return;
                }

                var flight = new Flight(0, fnum, origin, dest, depTime, "Scheduled", capacity);
                if (repo.AddFlight(flight))
                    Console.WriteLine("✅ Flight added successfully!");
                else
                    Console.WriteLine("❌ Failed to add flight.");
            }
            else if (sub == "2")
            {
                TableRenderer.Render(repo.GetAllFlights());
            }
            else
            {
                Console.WriteLine("❌ Invalid option.");
            }
        }

        private void SystemOversight(IDataRepository repo)
        {
            Console.WriteLine("\n--- System Oversight ---");
            var flights = repo.GetAllFlights();
            foreach (var f in flights)
            {
                var bookings = repo.GetBookingsForFlight(f.FlightId);
                int occupied = bookings.Count(b => b.Status != "Cancelled");
                Console.WriteLine($"{f.FlightNumber,-10} | {f.Origin}-{f.Destination} | {f.DepartureTime:g} | {f.Status} | {occupied}/{f.Capacity}");
            }
        }

        private void ManageStaff(IDataRepository repo)
        {
            Console.WriteLine("\n--- Staff Management ---");
            Console.WriteLine("1. View All Staff");
            Console.WriteLine("2. Add New Gate Agent");
            Console.Write("Choose: ");
            string? sub = Console.ReadLine();

            if (sub == "1")
            {
                TableRenderer.Render(repo.GetAllUsers());
            }
            else if (sub == "2")
            {
                Console.Write("Username: ");
                string uname = Console.ReadLine() ?? "";
                Console.Write("Password: ");
                string pass = Console.ReadLine() ?? "";
                Console.Write("Full Name: ");
                string sname = Console.ReadLine() ?? "";

                using var conn = new SqlConnection(ConnectionString.Value);
                conn.Open();
                using var cmd = new SqlCommand(
                    "INSERT INTO Users (Username, Password, Role, Name) VALUES (@u, @p, 'GateAgent', @n)", conn);

                cmd.Parameters.AddWithValue("@u", uname);
                cmd.Parameters.AddWithValue("@p", pass);
                cmd.Parameters.AddWithValue("@n", sname);
                cmd.ExecuteNonQuery();
                Console.WriteLine("✅ Gate Agent added successfully!");
            }
        }
    }
}