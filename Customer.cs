using System.Net.Mail;

namespace Smart_Car_Rental_System
{
    internal class Customer : User
    {
        private static int _nextId = 1;
        private string _email = string.Empty;

        public Customer(string name, string phone, string email, DateOnly joinDate)
            : base(GenerateCustomerId(name, phone, email, joinDate), name, phone)
        {
            Email = email;
            JoinDate = joinDate;
            ActiveRentals = 0;
        }

        public string Email
        {
            get => _email;
            private set
            {
                string email = value?.Trim() ?? string.Empty;

                if (!IsValidEmailAddress(email))
                {
                    throw new ArgumentException(
                        "Please enter a valid email address, such as mina@example.com.",
                        nameof(value));
                }

                _email = email;
            }
        }

        public int ActiveRentals { get; private set; }

        public DateOnly JoinDate { get; }

        private static string GenerateCustomerId(string name, string phone, string email, DateOnly joinDate)
        {
            // Validate every constructor input before incrementing the ID counter.
            // A rejected customer therefore does not consume an ID.
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(name));
            }

            if (!User.IsValidPhoneNumber(phone))
            {
                throw new ArgumentException(
                    "Phone number must contain 7 to 15 digits and may use only digits, spaces, +, hyphens and parentheses.",
                    nameof(phone));
            }

            if (!IsValidEmailAddress(email))
            {
                throw new ArgumentException(
                    "Please enter a valid email address, such as mina@example.com.",
                    nameof(email));
            }

            if (joinDate > DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException("Join date cannot be in the future.", nameof(joinDate));
            }

            return $"CUST-{_nextId++:D3}";
        }

        internal static bool IsValidEmailAddress(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string email = value.Trim();

            // Avoid accepting display-name input such as "Mina <mina@example.com>".
            if (email.Any(char.IsWhiteSpace) ||
                !MailAddress.TryCreate(email, out MailAddress? parsedAddress) ||
                parsedAddress is null ||
                !string.Equals(parsedAddress.Address, email, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            int atIndex = email.IndexOf('@');
            if (atIndex <= 0 || atIndex != email.LastIndexOf('@') || atIndex == email.Length - 1)
            {
                return false;
            }

            string domain = email[(atIndex + 1)..];
            string[] domainLabels = domain.Split('.');

            // Require a conventional domain, with no empty labels or malformed hyphens.
            if (domainLabels.Length < 2 || domainLabels.Any(label =>
                    label.Length is 0 or > 63 ||
                    label.StartsWith('-') ||
                    label.EndsWith('-') ||
                    label.Any(character => !char.IsLetterOrDigit(character) && character != '-')))
            {
                return false;
            }

            string topLevelDomain = domainLabels[^1];
            return topLevelDomain.Length >= 2 && topLevelDomain.Any(char.IsLetter);
        }

        internal void RegisterRental()
        {
            ActiveRentals++;
        }

        internal void CompleteRental()
        {
            if (ActiveRentals <= 0)
            {
                throw new InvalidOperationException(
                    $"The active rental count for customer {Id} is inconsistent.");
            }

            ActiveRentals--;
        }

        public void CustomerInformation()
        {
            Console.WriteLine("======== Customer Information ========");
            Console.WriteLine($"ID: {Id} | Name: {Name} | Joined: {JoinDate:dd/MM/yyyy}");
            Console.WriteLine($"Phone: {Phone} | Email: {Email} | Active Rentals: {ActiveRentals}\n");
        }
    }
}
