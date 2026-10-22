namespace Smart_Car_Rental_System
{
    internal class Manager : User
    {
        public Manager(string id, string name, string phone, DateOnly hireDate, decimal salary)
            : base(id, name, phone)
        {
            if (hireDate > DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException("Hire date cannot be in the future.", nameof(hireDate));
            }

            if (salary < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(salary), "Salary cannot be negative.");
            }

            HireDate = hireDate;
            Salary = salary;
        }

        public DateOnly HireDate { get; }

        public decimal Salary { get; }

        public void DisplayManagerInfo()
        {
            Console.WriteLine("======= Manager Information =======");
            Console.WriteLine($"Manager ID: {Id} | Manager Name: {Name} | Phone: {Phone}");
            Console.WriteLine($"Salary: {Salary:C} | Hire Date: {HireDate:dd/MM/yyyy}\n");
        }
    }
}
