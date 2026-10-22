using System.Globalization;

namespace Smart_Car_Rental_System
{
    internal class Program
    {
        private const string AppVersion = "1.1";

        private static void Main(string[] args)
        {
            Branch branch = CreateSampleBranch();
            while (true)
            {
                Console.WriteLine($"------ Smart Car Rental System v{AppVersion} ------\n");
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1 - Branch Information");
                Console.WriteLine("2 - Show All Users");
                Console.WriteLine("3 - Show Available Cars");
                Console.WriteLine("4 - Show All Cars");
                Console.WriteLine("5 - Rent a Car");
                Console.WriteLine("6 - Return a Car");
                Console.WriteLine("7 - Customer Rental History");
                Console.WriteLine("8 - Register New Customer");
                Console.WriteLine("0 - Exit");
                Console.Write("Choose an option: ");

                string? optionText = Console.ReadLine();
                if (optionText is null)
                {
                    Console.WriteLine("Input stream closed. Exiting the application.");
                    break;
                }

                if (!int.TryParse(optionText, out int option))
                {
                    Console.WriteLine("Invalid option. Please enter a number from the menu.\n");
                    continue;
                }

                if (option == 0)
                {
                    Console.WriteLine("Exiting Smart Car Rental System. Goodbye!");
                    break;
                }

                try
                {
                    switch (option)
                    {
                        case 1:
                            branch.DisplayBranchInfo();
                            break;

                        case 2:
                            branch.DisplayAllCustomer();
                            break;

                        case 3:
                            branch.AvailableCars();
                            break;

                        case 4:
                            branch.AllCars();
                            break;

                        case 5:
                            Console.WriteLine("======= Rent a Car =======");
                            string customerId = ReadRequiredInput("Enter Customer ID (e.g., CUST-001): ");
                            branch.AvailableCars();
                            string carId = ReadRequiredInput("Enter Car ID to rent (e.g., CAR-01): ");
                            branch.RentCar(customerId, carId);
                            break;

                        case 6:
                            Console.WriteLine("======= Return a Car =======");
                            string returnCustomerId = ReadRequiredInput("Enter Customer ID: ");
                            string returnCarId = ReadRequiredInput("Enter Car ID to return: ");
                            branch.ReturnCar(returnCustomerId, returnCarId);
                            break;

                        case 7:
                            string historyCustomerId = ReadRequiredInput("Enter Customer ID: ");
                            branch.TransactionInfo(historyCustomerId);
                            break;

                        case 8:
                            RegisterCustomer(branch);
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please choose an option from the menu.\n");
                            break;
                    }
                }
                catch (EndOfStreamException)
                {
                    Console.WriteLine("\nInput stream closed. Exiting the application.");
                    return;
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"\nOperation error: {ex.Message}\n");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"\nInput error: {ex.Message}\n");
                }

                Console.WriteLine("Press Enter to return to the menu...");
                if (Console.ReadLine() is null)
                {
                    Console.WriteLine("Input stream closed. Exiting the application.");
                    break;
                }

                ClearConsoleSafely();
            }
        }

        private static void RegisterCustomer(Branch branch)
        {
            Console.WriteLine("======= Register New Customer =======");
            string name = ReadRequiredInput("Enter Customer Name: ");
            string phone = ReadPhone("Enter Customer Phone: ");
            string email = ReadEmail();
            DateOnly joinDate = ReadJoinDate();

            Customer newCustomer = new Customer(name, phone, email, joinDate);
            branch.AddCustomer(newCustomer);

            Console.WriteLine("\nCustomer registered successfully.");
            Console.WriteLine($"Customer ID: {newCustomer.Id}\n");
        }

        private static Branch CreateSampleBranch()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            Branch branch = new Branch(
                "BR-01",
                "Downtown Branch",
                "123 Main St, Cityville",
                "555-1234",
                "9 AM - 6 PM",
                new Manager("MGR-01", "John Doe", "555-5678", today, 50000m));

            branch.AddCar(new Car("CAR-01", "Toyota Corolla", 2022, "Excellent"));
            branch.AddCar(new Car("CAR-02", "BMW X5", 2023, "New"));
            branch.AddCar(new Car("CAR-03", "Hyundai Elantra", 2021, "Good"));
            branch.AddCar(new Car("CAR-04", "Mercedes C200", 2024, "New"));

            branch.AddCustomer(new Customer("Mina Shenouda", "0123456789", "mina@example.com", today));
            branch.AddCustomer(new Customer("Michale Mamdoh", "0126576789", "michale@example.com", today));
            branch.AddCustomer(new Customer("Kirollos Nabil", "0112233445", "kirollos@example.com", today));

            // No rental transactions are seeded, so all sample customers start with zero active rentals.
            return branch;
        }

        private static string ReadRequiredInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (input is null)
                {
                    throw new EndOfStreamException("No input was received.");
                }

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                Console.WriteLine("This field cannot be empty. Please try again.");
            }
        }

        private static string ReadPhone(string prompt)
        {
            while (true)
            {
                string phone = ReadRequiredInput(prompt);

                if (User.IsValidPhoneNumber(phone))
                {
                    return phone;
                }

                Console.WriteLine("Invalid phone number. Use 7 to 15 digits; spaces, +, hyphens and parentheses are allowed.");
            }
        }

        private static string ReadEmail()
        {
            while (true)
            {
                string email = ReadRequiredInput("Enter Customer Email: ");

                if (Customer.IsValidEmailAddress(email))
                {
                    return email;
                }

                Console.WriteLine("Invalid email. Enter an address such as mina@example.com, then try again.");
            }
        }

        private static DateOnly ReadJoinDate()
        {
            while (true)
            {
                string input = ReadRequiredInput("Enter Join Date (yyyy-MM-dd): ");

                bool validDate = DateOnly.TryParseExact(
                    input,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateOnly joinDate);

                if (validDate && joinDate <= DateOnly.FromDateTime(DateTime.Today))
                {
                    return joinDate;
                }

                Console.WriteLine("Invalid date. Enter a real date in yyyy-MM-dd format that is not in the future.");
            }
        }

        private static void ClearConsoleSafely()
        {
            if (Console.IsOutputRedirected)
            {
                return;
            }

            try
            {
                Console.Clear();
            }
            catch (IOException)
            {
                // Some terminals do not support clearing the screen. Continuing is safe.
            }
            catch (PlatformNotSupportedException)
            {
                // Some environments do not support clearing the screen. Continuing is safe.
            }
        }
    }
}
