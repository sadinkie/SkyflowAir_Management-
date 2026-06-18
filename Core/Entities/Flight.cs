namespace SkyFlow.Core.Entities
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public string Status { get; private set; }
        public int Capacity { get; set; }

        public Flight(int flightId, string flightNumber, string origin,
                     string destination, DateTime departureTime, string status, int capacity)
        {
            FlightId = flightId;
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            DepartureTime = departureTime;
            Status = status;
            Capacity = capacity;
        }

        // Encapsulation: Status can only be changed via method
        public void UpdateStatus(string newStatus)
        {
            if (!string.IsNullOrEmpty(newStatus))
                Status = newStatus;
        }
    }
}