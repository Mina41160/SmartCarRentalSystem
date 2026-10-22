namespace Smart_Car_Rental_System
{
    internal class Branch
    {
        public Branch(string id, string name, string address, string phone, string openingHours, Manager manager)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Branch ID cannot be empty.", nameof(id));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Branch name cannot be empty.", nameof(name));
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ArgumentException("Branch address cannot be empty.", nameof(address));
            }

            if (!User.IsValidPhoneNumber(phone))
            {
                throw new ArgumentException("Branch phone number must contain 7 to 15 digits.", nameof(phone));
            }

            if (string.IsNullOrWhiteSpace(openingHours))
            {
                throw new ArgumentException("Opening hours cannot be empty.", nameof(openingHours));
            }

            ArgumentNullException.ThrowIfNull(manager);

            Id = id.Trim();
            Name = name.Trim();
            Address = address.Trim();
            Phone = phone.Trim();
            OpeningHours = openingHours.Trim();
            Manager = manager;
        }

        public string Id { get; }

        public string Name { get; }

        public string Address { get; }

        public string Phone { get; }

        public string OpeningHours { get; }

        public Manager Manager { get; }

        public List<Customer> CustomerList { get; } = new();

        public List<Car> CarList { get; } = new();

        public List<Rental_Transaction> TransactionsList { get; } = new();

        public void DisplayBranchInfo()
        {
            Console.WriteLine("======= Branch Information =======");
            Console.WriteLine($"Branch ID       : {Id}");
            Console.WriteLine($"Branch Name     : {Name}");
            Console.WriteLine($"Address         : {Address}");
            Console.WriteLine($"Phone           : {Phone}");
            Console.WriteLine($"Hours           : {OpeningHours}");
            Console.WriteLine($"Manager         : {Manager.Name}");
            Console.WriteLine($"Total Customers : {CustomerList.Count}");
            Console.WriteLine($"Total Cars      : {CarList.Count}\n");
        }

        public void DisplayAllCustomer()
        {
            Console.WriteLine("====== All Registered Users ======\n");
            Manager.DisplayManagerInfo();

            if (CustomerList.Count == 0)
            {
                Console.WriteLine("No customers registered yet.");
                return;
            }

            foreach (Customer customer in CustomerList)
            {
                customer.CustomerInformation();
            }
        }

        public void AvailableCars()
        {
            Console.WriteLine("====== All Available Cars ======\n");
            bool found = false;

            foreach (Car car in CarList)
            {
                if (car.Status == Status.Available)
                {
                    car.CarInfo();
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No available cars found.\n");
            }
        }

        public void AllCars()
        {
            Console.WriteLine("====== All Cars in Branch ======\n");

            if (CarList.Count == 0)
            {
                Console.WriteLine("No cars found.\n");
                return;
            }

            foreach (Car car in CarList)
            {
                car.CarInfo();
            }
        }

        public Customer FindCustomerById(string customerId)
        {
            if (string.IsNullOrWhiteSpace(customerId))
            {
                throw new InvalidOperationException("Customer ID cannot be empty.");
            }

            Customer? customer = CustomerList.Find(
                item => string.Equals(item.Id, customerId.Trim(), StringComparison.OrdinalIgnoreCase));

            return customer ?? throw new InvalidOperationException(
                $"Customer with ID '{customerId}' does not exist.");
        }

        public Car FindCarById(string carId)
        {
            if (string.IsNullOrWhiteSpace(carId))
            {
                throw new InvalidOperationException("Car ID cannot be empty.");
            }

            Car? car = CarList.Find(
                item => string.Equals(item.Id, carId.Trim(), StringComparison.OrdinalIgnoreCase));

            return car ?? throw new InvalidOperationException($"Car with ID '{carId}' does not exist.");
        }

        public void RentCar(string customerId, string carId)
        {
            Customer customer = FindCustomerById(customerId);
            Car car = FindCarById(carId);

            if (car.Status != Status.Available)
            {
                throw new InvalidOperationException($"Car {car.Id} ({car.Model}) is not available for rent.");
            }

            bool hasActiveTransaction = TransactionsList.Any(transaction =>
                string.Equals(transaction.Car.Id, car.Id, StringComparison.OrdinalIgnoreCase) &&
                transaction.Status == Status_Tran.Active);

            if (hasActiveTransaction)
            {
                throw new InvalidOperationException(
                    $"Car {car.Id} already has an active rental transaction. Check the car status.");
            }

            DateOnly rentDate = DateOnly.FromDateTime(DateTime.Today);
            DateOnly dueDate = rentDate.AddDays(7);
            Rental_Transaction transaction = new Rental_Transaction(customer, car, rentDate, dueDate);

            TransactionsList.Add(transaction);
            car.Status = Status.Rented;
            customer.RegisterRental();

            Console.WriteLine($"Car [{car.Id}] \"{car.Model}\" rented by {customer.Name}.");
            Console.WriteLine($"Transaction ID: {transaction.TransactionID}");
            Console.WriteLine($"Rent date: {rentDate:dd/MM/yyyy}");
            Console.WriteLine($"Due date: {dueDate:dd/MM/yyyy}\n");
        }

        public void ReturnCar(string customerId, string carId)
        {
            Customer customer = FindCustomerById(customerId);
            Car car = FindCarById(carId);

            Rental_Transaction? transaction = TransactionsList.Find(item =>
                string.Equals(item.Customer.Id, customer.Id, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(item.Car.Id, car.Id, StringComparison.OrdinalIgnoreCase) &&
                item.Status == Status_Tran.Active);

            if (transaction is null)
            {
                throw new InvalidOperationException(
                    $"No active rental was found for customer {customer.Id} and car {car.Id}.");
            }

            if (car.Status != Status.Rented)
            {
                throw new InvalidOperationException(
                    $"The transaction is active, but car {car.Id} is not marked as rented. Check the data.");
            }

            if (customer.ActiveRentals <= 0)
            {
                throw new InvalidOperationException(
                    $"The active rental count for customer {customer.Id} is inconsistent.");
            }

            DateOnly returnDate = DateOnly.FromDateTime(DateTime.Today);
            transaction.CompleteReturn(returnDate);
            customer.CompleteRental();
            car.Status = Status.Available;

            Console.WriteLine($"Car {car.Id} ({car.Model}) has been returned by {customer.Name}.");
            Console.WriteLine($"Return date: {returnDate:dd/MM/yyyy}");

            if (transaction.Fee > 0m)
            {
                Console.WriteLine($"Late fee: {transaction.Fee:C}");
            }
            else
            {
                Console.WriteLine("No late fee.");
            }

            Console.WriteLine();
        }

        public void TransactionInfo(string customerId)
        {
            Customer customer = FindCustomerById(customerId);
            bool found = false;

            Console.WriteLine($"====== Rental History for {customer.Name} ({customer.Id}) ======\n");

            foreach (Rental_Transaction transaction in TransactionsList)
            {
                if (string.Equals(transaction.Customer.Id, customer.Id, StringComparison.OrdinalIgnoreCase))
                {
                    transaction.TransactionInfo();
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No rental history found.");
            }
        }

        public void AddCustomer(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);

            bool alreadyExists = CustomerList.Any(item =>
                string.Equals(item.Id, customer.Id, StringComparison.OrdinalIgnoreCase));

            if (alreadyExists)
            {
                throw new InvalidOperationException($"Customer with ID '{customer.Id}' already exists.");
            }

            CustomerList.Add(customer);
        }

        public void AddCar(Car car)
        {
            ArgumentNullException.ThrowIfNull(car);

            bool alreadyExists = CarList.Any(item =>
                string.Equals(item.Id, car.Id, StringComparison.OrdinalIgnoreCase));

            if (alreadyExists)
            {
                throw new InvalidOperationException($"Car with ID '{car.Id}' already exists.");
            }

            CarList.Add(car);
        }
    }
}
