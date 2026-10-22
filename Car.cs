namespace Smart_Car_Rental_System
{
    internal class Car
    {
        public Car(string id, string model, int year, string condition)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Car ID cannot be empty.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(model))
            {
                throw new ArgumentException("Car model cannot be empty.", nameof(model));
            }

            int latestPlausibleModelYear = DateTime.Today.Year + 1;
            if (year < 1886 || year > latestPlausibleModelYear)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(year),
                    $"Car year must be between 1886 and {latestPlausibleModelYear}.");
            }

            if (string.IsNullOrWhiteSpace(condition))
            {
                throw new ArgumentException("Car condition cannot be empty.", nameof(condition));
            }

            Id = id.Trim();
            Model = model.Trim();
            Year = year;
            Condition = condition.Trim();
            Status = Smart_Car_Rental_System.Status.Available;
        }

        public string Id { get; }

        public string Model { get; }

        public int Year { get; }

        public string Condition { get; }

        public Status Status { get; internal set; }

        public void CarInfo()
        {
            Console.WriteLine(
                $"Car ID: {Id} | Model: {Model} {Year} | Condition: {Condition} | Status: {Status}");
        }
    }
}
