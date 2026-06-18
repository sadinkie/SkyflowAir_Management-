using Microsoft.Data.SqlClient;
using SkyFlow.Config;
using SkyFlow.Core.Entities;
using SkyFlow.Core.Interfaces;

namespace SkyFlow.Data
{
    public class SqlRepository : IDataRepository
    {
        private readonly string _connectionString = ConnectionString.Value;

        #region Authentication
        public User? Authenticate(string username, string password)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(
                "SELECT UserId, Username, Name, Role FROM Users WHERE Username = @u AND Password = @p", conn);

            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                int id = reader.GetInt32(0);
                string uname = reader.GetString(1);
                string name = reader.GetString(2);
                string role = reader.GetString(3);

                return role == "Admin"
                    ? new Admin(id, uname, name)
                    : new GateAgent(id, uname, name);
            }
            return null;
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT UserId, Username, Name, Role FROM Users", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string uname = reader.GetString(1);
                string name = reader.GetString(2);
                string role = reader.GetString(3);

                User user = role == "Admin"
                    ? new Admin(id, uname, name)
                    : new GateAgent(id, uname, name);
                users.Add(user);
            }
            return users;
        }
        #endregion

        #region Flights
        public List<Flight> GetAllFlights()
        {
            var flights = new List<Flight>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT * FROM Flights", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                flights.Add(new Flight(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetDateTime(4),
                    reader.GetString(5),
                    reader.GetInt32(6)
                ));
            }
            return flights;
        }

        public Flight? GetFlightById(int flightId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(
                "SELECT FlightId, FlightNumber, Origin, Destination, DepartureTime, Status, Capacity FROM Flights WHERE FlightId = @id",
                conn);
            cmd.Parameters.AddWithValue("@id", flightId);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Flight(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetDateTime(4),
                    reader.GetString(5),
                    reader.GetInt32(6)
                );
            }

            return null;
        }

        public bool AddFlight(Flight flight)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(
                @"INSERT INTO Flights (FlightNumber, Origin, Destination, DepartureTime, Status, Capacity) 
                  VALUES (@fn, @o, @d, @dt, @s, @c)", conn);

            cmd.Parameters.AddWithValue("@fn", flight.FlightNumber);
            cmd.Parameters.AddWithValue("@o", flight.Origin);
            cmd.Parameters.AddWithValue("@d", flight.Destination);
            cmd.Parameters.AddWithValue("@dt", flight.DepartureTime);
            cmd.Parameters.AddWithValue("@s", flight.Status);
            cmd.Parameters.AddWithValue("@c", flight.Capacity);

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool UpdateFlightStatus(int flightId, string status)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("UPDATE Flights SET Status = @s WHERE FlightId = @id", conn);
            cmd.Parameters.AddWithValue("@s", status);
            cmd.Parameters.AddWithValue("@id", flightId);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteFlight(int flightId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM Flights WHERE FlightId = @id", conn);
            cmd.Parameters.AddWithValue("@id", flightId);
            return cmd.ExecuteNonQuery() > 0;
        }
        #endregion

        #region Passengers & Bookings
        public List<Booking> GetBookingsForFlight(int flightId)
        {
            var bookings = new List<Booking>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(
                @"SELECT b.BookingId, b.FlightId, b.PassengerId, b.Status, p.Name 
                  FROM Bookings b 
                  JOIN Passengers p ON b.PassengerId = p.PassengerId 
                  WHERE b.FlightId = @fid", conn);

            cmd.Parameters.AddWithValue("@fid", flightId);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                bookings.Add(new Booking(
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.GetInt32(2),
                    reader.GetString(3),
                    reader.GetString(4)
                ));
            }
            return bookings;
        }

        public Passenger? GetPassengerByIdOrPassport(string searchTerm)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(
                @"SELECT PassengerId, Name, PassportNumber 
                  FROM Passengers 
                  WHERE PassengerId = @id OR PassportNumber = @pass", conn);

            if (int.TryParse(searchTerm, out int id))
                cmd.Parameters.AddWithValue("@id", id);
            else
                cmd.Parameters.AddWithValue("@id", -1);

            cmd.Parameters.AddWithValue("@pass", searchTerm);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Passenger(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2)
                );
            }
            return null;
        }

        public bool UpdateBookingStatus(int bookingId, string status)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("UPDATE Bookings SET Status = @s WHERE BookingId = @id", conn);
            cmd.Parameters.AddWithValue("@s", status);
            cmd.Parameters.AddWithValue("@id", bookingId);
            return cmd.ExecuteNonQuery() > 0;
        }

        public List<Passenger> GetAllPassengers()
        {
            var passengers = new List<Passenger>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT PassengerId, Name, PassportNumber FROM Passengers", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                passengers.Add(new Passenger(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2)
                ));
            }

            return passengers;
        }

        public bool AddPassenger(Passenger passenger)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(
                "INSERT INTO Passengers (Name, PassportNumber) VALUES (@name, @passport)", conn);
            cmd.Parameters.AddWithValue("@name", passenger.Name);
            cmd.Parameters.AddWithValue("@passport", passenger.PassportNumber);
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool BookFlight(int flightId, int passengerId)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand(
                "INSERT INTO Bookings (FlightId, PassengerId, Status) VALUES (@fid, @pid, @status)", conn);
            cmd.Parameters.AddWithValue("@fid", flightId);
            cmd.Parameters.AddWithValue("@pid", passengerId);
            cmd.Parameters.AddWithValue("@status", "Confirmed");
            return cmd.ExecuteNonQuery() > 0;
        }
        #endregion
    }
}