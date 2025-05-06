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
### Example URLS
* http://localhost:5202/api/PaymentTransactions/transactions
* http://localhost:5202/api/PaymentTransactions/transactions?ProviderName=trustly
* http://localhost:5202/api/PaymentTransactions/transactions?ProviderName=trustly&Status=0
* http://localhost:5202/api/PaymentTransactions/transactions?ProviderName=paypal&Status=0&From=2025-05-06&To=2025-05-07

