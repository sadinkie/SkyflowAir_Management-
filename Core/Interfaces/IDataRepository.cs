using SkyFlow.Core.Entities;

namespace SkyFlow.Core.Interfaces
{
    public interface IDataRepository
    {
        // Authentication
        User? Authenticate(string username, string password);
        List<User> GetAllUsers();

        // Flights
        List<Flight> GetAllFlights();
        Flight? GetFlightById(int flightId);
        bool AddFlight(Flight flight);
        bool UpdateFlightStatus(int flightId, string status);
        bool DeleteFlight(int flightId);

        // Passengers & Bookings
        List<Booking> GetBookingsForFlight(int flightId);
        List<Passenger> GetAllPassengers();
        Passenger? GetPassengerByIdOrPassport(string searchTerm);
        bool UpdateBookingStatus(int bookingId, string status);
        bool AddPassenger(Passenger passenger);
        bool BookFlight(int flightId, int passengerId);
    }
}