# GNB_TransRates
> API - Gets all rates, all transactions or specific transaction by sku from Herokuapp.

## Technologies

- [.NET 6](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6)
- [AutoMapper 11.0.0](http://automapper.org/)
- [Newtonsoft.Json 13.0.1](https://www.newtonsoft.com/json)
- [Microsoft.EntityFrameworkCore.Design 6.0.1](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/)
- [Swashbuckle.AspNetCore 6.2.3](https://www.nuget.org/packages/Swashbuckle.AspNetCore)
- [Microsoft.Extensions.Logging 6.0.0](https://www.nuget.org/packages/Microsoft.Extensions.Logging)
- [Moq 4.16.1](https://github.com/Moq)
- [FluentAssertions 6.4.0](https://fluentassertions.com/)


## API

Resource | GET
------ | ------ 
api/transactions/list | Retrieve transactions list



### Response

````
[
  {
    "Sku": "Sku",
    "amount": XX.X,
    "Currency": "YYY"
  },
  ...
]
````

````Sku```` Sku transaction

````amount```` Transaction amount

````Currency```` Transaction currency



Resource | GET
------ | ------ 
api/transactions/listbysku?sku= | Retrieve transactions list from specific sku



### Response

````
{
  "Total": XXXX.XX,
  "Transactions": [
    {
      "Sku": "sku",
      "amount": YY.YY,
      "Currency": "EUR"
    },
    ...
    ]
}
````

````Total```` Sum total of all transactions with specific sku

````Sku```` Sku transaction

````amount```` Transaction amount in EUR

````Currency```` Always EUR



Resource | GET
------ | ------ 
api/rates/list | Retrieve rates list



### Response

````
[
  {
    "From": "XXX",
    "To": "YYY",
    "Rate": n.nn
  },
  ...
]
````

````From```` Origin currency

````To```` Destination currency

````Rate```` Rate amount



### Status codes

Status code	| Description
------ | ------
200 |	OK
400 |	BAD REQUEST
404 |	NOT FOUND
