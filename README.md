# Smart Car Rental System

A C# .NET Console Application demonstrating object-oriented programming and core car-rental workflows.

## Features

- Display branch information, customers and vehicles.
- Display available cars separately from the full fleet.
- Register customers with an automatically generated customer ID.
- Validate branch, car, manager and customer data, including phone numbers, email addresses, dates and plausible car model years.
- Re-prompt the user when an email, phone number or date is invalid instead of abandoning customer registration.
- Rent an available car and create a rental transaction with a seven-day due date.
- Prevent a second active rental for the same car.
- Return a car only when the selected customer has an active rental for that specific car.
- Record the return date, close the rental transaction and update the customer's active-rental count.
- Calculate a late fee of 150 currency units per day after the due date.
- Display a customer's complete rental history.
- Handle invalid menu selections and end-of-input without an endless loop.

## Run

1. Extract the ZIP to a new folder.
2. Open **the `SmartCarRentalSystem.csproj` inside the extracted `SmartCarRentalSystem_Fixed` folder** in Visual Studio.
3. Build with **Build > Build Solution** or `Ctrl+Shift+B`.
4. Run the project.
5. Confirm that the menu heading says **Smart Car Rental System v1.1**. This helps verify that the fixed source version is the one being run.

If Visual Studio still runs an older build, close the running console, use **Build > Clean Solution**, then **Build > Rebuild Solution**. Make sure the project opened is the one from this extracted folder, not the original project folder.

## Sample IDs

- Customers: `CUST-001`, `CUST-002`, `CUST-003`
- Cars: `CAR-01`, `CAR-02`, `CAR-03`, `CAR-04`

## Quick manual test: customer registration

1. Select option `8`.
2. Enter a name and phone number, for example `Mina` and `0123456789`.
3. Enter `Mina` as the email. The application should reject it and ask for the email again.
4. Enter `mina@example.com`; the application should then ask for the join date.
5. Enter `2011-01-22`; registration should succeed and display the generated customer ID.

Invalid examples such as `2011/22/1` and `2014-30-02` should be rejected because they do not represent a real date in the required `yyyy-MM-dd` format.

## Important limitations

- Data is stored in memory. Customers, cars and transactions added during a run are reset when the application closes.
- This version does not use SQL Server, Entity Framework Core or a web frontend.
- The due date is seven days after rental, and the late-fee rate is 150 currency units per day. Adjust these business rules if your project requirements differ.

## Manual regression checklist

- [ ] Register a customer with valid details.
- [ ] Confirm invalid phone, email and join date inputs each re-prompt for that field.
- [ ] Rent an available car.
- [ ] Confirm another customer cannot rent the same car while it is rented.
- [ ] Confirm a customer cannot return another customer's rental.
- [ ] Return the correct rental and verify the status becomes `Returned` and the car becomes available.
- [ ] View rental history and verify it includes the return date and fee.
- [ ] Enter a non-number at the main menu and confirm the app continues normally.
