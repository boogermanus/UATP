# UATP Coding Challange

## Running the Project
* Clone the repository
* Run the project by using `dotnet watch run --project UATP.API`. Note: If you use the `dotnet run` command you will need to manually navigate to the Swagger page `http://localhost:5202/swagger/index.html`.
* There is no need to generate the database, it is included in the repository along with some sample data.
## Running the tests
Run tests using the `dotnet test` command from the repository directory.
## Sample Payloads for the `ingest` endpoint
The following JSON is an example input for the `ingest` endpoint.
```
{
  "amount": 10.0,
  "currency": "usd",
  "status": 0,
  "payerEmail": "user@example.com",
  "paymentMethod": "creditcard"
}
```
* Valid `ingest/{paymentProvider}` endpoint are: `paypal`, `trustly`. They are case insensitive, so `paypal` and `PayPal` will both work.
* The amount field can be positive or negative, but it cannot be zero. (See my assumptions for more information)
* Valid values for the status field are: 0, 1, and 2. Any other value should throw an error on execute.
* Valid values for the currency field are: `usd`, `jpy`, and `eur`.
* The payerEmail field must be a valid email address.
* Valid values for the paymentMethod are: `creditcard`, `ach`, and `wallet`. The are case insensitive, so `CreditCard` and `creditcard` will both work.
The following JSON is an example invalid input for the `ingest` endpoint.
```
{
  "amount": 0,
  "currency": "rub",
  "status": 3,
  "payerEmail": "user@",
  "paymentMethod": "wallett"
}
```
Submitting any field with an invalid value will result in a validation error from the endpoint.
## Sample Payloads for the `transactions` endpoint.
Note: the transaction endpoint accepts a query string, each value is optional, and if no query string is provided the endpoint will return all transactions. Swagger will let you specify the values.
The date format can be any .NET valid DateTime format. From date cannot be after the To date, and To date cannot be before the From date.
### Example URLs
* http://localhost:5202/api/PaymentTransactions/transactions
* http://localhost:5202/api/PaymentTransactions/transactions?ProviderName=trustly
* http://localhost:5202/api/PaymentTransactions/transactions?ProviderName=trustly&Status=0
* http://localhost:5202/api/PaymentTransactions/transactions?ProviderName=paypal&Status=0&From=2025-05-06&To=2025-05-07
## Notes on design and assumptions
* My original design for the database was simply the PaymentTransactions table. While this worked, I realized that there were a couple of issues with it.
  - The post method would accept any paymentProvider, not an ideal situation. The same goes with the Currency, and PaymentMethod columns. Unless I wanted to spend a lot
    of time sanatizing input; I needed a new solution.
  - I created the PaymentProviders table and seeded it with some data. This allowed my business service to unit test that the paymentProvider input was valid and something it should accept.
  - The Currency column through me for a loop; however, after thinking about it I thought it would be a good idea to store currency codes, names, and symbols.
    I created the Currencies table for just this purpose.
  - I added foreign keys for the payment provider Id, and the currency Id, to the PaymentTransactions table, so that I could join on those tables to get the data.
  - I did not do this with the PaymentMethod field, which is not a good pattern. I got lazy and just validated the payment method against a list of valid options.
    If I were to make another pass at this, I would add a new table and relationship between PaymentTransactions and the new PaymentMethod table.
  - The final design allows me to easily add new payment providers and currencies to the database without having to worrying about input validation.
* In a larger application I would have created a base repository system in order to avoid having to constantly write CRUD operations for **every repository**. I do this with most of my personal
  projects because I came from an organization that had no concept of this, and amount of time spend writing boiler plate code was unnecessary. Since this project is relatively small I and the
  repositories were very complex I opted for a simple approach. I would be happy to discuss this further.
* I faked all my repositories in my unit tests because this is just the simple approach and the one which is recommened by Microsoft.
* I did not unit test my controller for the main reason being that business logic should never live in a controller. While its easy to unit test controllers, I believe that integration testing is better
  for this sort of system.

  
