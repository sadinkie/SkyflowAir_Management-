namespace SkyFlow.Core.Entities
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public string Status { get; set; }
        public string PassengerName { get; set; }   // For display in tables

        public Booking(int bookingId, int flightId, int passengerId,
                      string status, string passengerName)
        {
            BookingId = bookingId;
            FlightId = flightId;
            PassengerId = passengerId;
            Status = status;
            PassengerName = passengerName;
        }
    }
}