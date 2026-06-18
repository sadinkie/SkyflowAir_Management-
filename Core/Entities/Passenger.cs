namespace SkyFlow.Core.Entities
{
    public class Passenger
    {
        public int PassengerId { get; set; }
        public string Name { get; set; }
        public string PassportNumber { get; set; }

        public Passenger(int passengerId, string name, string passportNumber)
        {
            PassengerId = passengerId;
            Name = name;
            PassportNumber = passportNumber;
        }
    }
}