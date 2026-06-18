using SkyFlow.Core.Interfaces;
using SkyFlow.Core.Helpers;

namespace SkyFlow.Core.Entities
{
    public class GateAgent : User
    {
        public GateAgent(int userId, string username, string name)
            : base(userId, username, name, "GateAgent") { }

        public override void DisplayDashboard(IDataRepository repo)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== GATE AGENT DASHBOARD ===");
                Console.WriteLine($"Welcome, {Name}!\n");
                Console.WriteLine("1. View Flight Manifest");
                Console.WriteLine("2. Passenger Check-in");
                Console.WriteLine("3. Boarding / Depart Flight");
                Console.WriteLine("4. Logout");
                Console.Write("\nSelect option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewManifest(repo);
                        break;
                    case "2":
                        CheckInPassenger(repo);
                        break;
                    case "3":
                        BoardFlight(repo);
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

        private void ViewManifest(IDataRepository repo)
        {
            Console.Write("Enter Flight ID: ");
            if (int.TryParse(Console.ReadLine(), out int flightId) && flightId > 0)
            {
                var bookings = repo.GetBookingsForFlight(flightId);
                Console.WriteLine($"\n--- Manifest for Flight ID {flightId} ---");
                TableRenderer.Render(bookings);
            }
            else
            {
                Console.WriteLine("❌ Invalid Flight ID.");
            }
        }

        private void CheckInPassenger(IDataRepository repo)
        {
            Console.Write("Search Passenger (ID or Passport Number): ");
            string search = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(search))
            {
                Console.WriteLine("❌ Search term cannot be empty.");
                return;
            }

            var passenger = repo.GetPassengerByIdOrPassport(search);
            if (passenger != null)
            {
                Console.WriteLine($"Found: {passenger.Name}");
                Console.Write("Enter Booking ID to Check-in: ");
                if (int.TryParse(Console.ReadLine(), out int bookingId) && bookingId > 0)
                {
                    if (repo.UpdateBookingStatus(bookingId, "CheckedIn"))
                        Console.WriteLine("✅ Passenger Checked In Successfully!");
                    else
                        Console.WriteLine("❌ Failed to check-in passenger.");
                }
                else
                {
                    Console.WriteLine("❌ Invalid Booking ID.");
                }
            }
            else
            {
                Console.WriteLine("❌ Passenger not found.");
            }
        }

        private void BoardFlight(IDataRepository repo)
        {
            Console.Write("Enter Flight ID to Depart: ");
            if (int.TryParse(Console.ReadLine(), out int flightId) && flightId > 0)
            {
                if (repo.UpdateFlightStatus(flightId, "Departed"))
                    Console.WriteLine("✅ Flight status updated to Departed.");
                else
                    Console.WriteLine("❌ Failed to update flight status.");
            }
            else
            {
                Console.WriteLine("❌ Invalid Flight ID.");
            }
        }
    }
}