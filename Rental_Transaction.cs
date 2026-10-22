namespace Smart_Car_Rental_System
{
    internal class Rental_Transaction
    {
        private static int _nextTransactionId = 1;
        private const decimal LateFeePerDay = 150m;

        public Rental_Transaction(Customer customer, Car car, DateOnly rentDate, DateOnly dueDate)
        {
            ArgumentNullException.ThrowIfNull(customer);
            ArgumentNullException.ThrowIfNull(car);

            if (dueDate < rentDate)
            {
                throw new ArgumentException("Due date cannot be earlier than the rent date.", nameof(dueDate));
            }

            TransactionID = $"TXN-{_nextTransactionId++:D3}";
            Customer = customer;
            Car = car;
            RentDate = rentDate;
            DueDate = dueDate;
            Status = Status_Tran.Active;
            Fee = 0m;
        }

        public string TransactionID { get; }

        public Customer Customer { get; }

        public Car Car { get; }

        public DateOnly RentDate { get; }

        public DateOnly DueDate { get; }

        public DateOnly? ReturnDate { get; private set; }

        public Status_Tran Status { get; private set; }

        public decimal Fee { get; private set; }

        public decimal CalculateFee(DateOnly returnDate)
        {
            if (returnDate < RentDate)
            {
                throw new ArgumentException("Return date cannot be earlier than the rent date.", nameof(returnDate));
            }

            if (returnDate <= DueDate)
            {
                return 0m;
            }

            int lateDays = returnDate.DayNumber - DueDate.DayNumber;
            return lateDays * LateFeePerDay;
        }

        public void CompleteReturn(DateOnly returnDate)
        {
            if (Status != Status_Tran.Active)
            {
                throw new InvalidOperationException("This rental transaction has already been returned.");
            }

            if (returnDate < RentDate)
            {
                throw new ArgumentException("Return date cannot be earlier than the rent date.", nameof(returnDate));
            }

            ReturnDate = returnDate;
            Fee = CalculateFee(returnDate);
            Status = Status_Tran.Returned;
        }

        public void TransactionInfo()
        {
            Console.WriteLine("====== Transaction Information ======");
            Console.WriteLine($"Transaction ID: {TransactionID}");
            Console.WriteLine($"Customer ID   : {Customer.Id}, Name: {Customer.Name}");
            Console.WriteLine($"Car ID        : {Car.Id}, Model: {Car.Model}");
            Console.WriteLine($"Rent Date     : {RentDate:dd/MM/yyyy}");
            Console.WriteLine($"Due Date      : {DueDate:dd/MM/yyyy}");
            Console.WriteLine($"Return Date   : {(ReturnDate.HasValue ? ReturnDate.Value.ToString("dd/MM/yyyy") : "Not returned yet")}");
            Console.WriteLine($"Status        : {Status}");
            Console.WriteLine($"Fee           : {Fee:C}\n");
        }
    }
}
