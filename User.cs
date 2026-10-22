namespace Smart_Car_Rental_System
{
    internal abstract class User
    {
        private string _name = string.Empty;
        private string _phone = string.Empty;

        protected User(string id, string name, string phone)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(id));
            }

            Id = id.Trim();
            Name = name;
            Phone = phone;
        }

        public string Id { get; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Name cannot be empty.", nameof(value));
                }

                _name = value.Trim();
            }
        }

        public string Phone
        {
            get => _phone;
            set
            {
                string phone = value?.Trim() ?? string.Empty;

                if (!IsValidPhoneNumber(phone))
                {
                    throw new ArgumentException(
                        "Phone number must contain 7 to 15 digits and may use only digits, spaces, +, hyphens and parentheses.",
                        nameof(value));
                }

                _phone = phone;
            }
        }

        internal static bool IsValidPhoneNumber(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            string phone = value.Trim();
            int digitCount = 0;
            bool plusSeen = false;

            for (int i = 0; i < phone.Length; i++)
            {
                char character = phone[i];

                if (char.IsDigit(character))
                {
                    digitCount++;
                    continue;
                }

                if (character == '+')
                {
                    // A plus sign is allowed only once, at the beginning of the number.
                    if (i != 0 || plusSeen)
                    {
                        return false;
                    }

                    plusSeen = true;
                    continue;
                }

                if (character == '-' || char.IsWhiteSpace(character) || character == '(' || character == ')')
                {
                    continue;
                }

                return false;
            }

            return digitCount is >= 7 and <= 15;
        }

        public void UserInformation()
        {
            Console.WriteLine("======= User Information =======");
            Console.WriteLine($"ID: {Id} | Name: {Name} | Phone: {Phone}\n");
        }
    }
}
