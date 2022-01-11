# GNB_TransRates
GNB_TransRates_API

***
Proyecto GNB_TransRates API con 3 métodos públicos: /api/Rates/list, api/transactions/list, api/transactions/listbysku?sku=XXXXX

TestUnitarios en proyecto: GNB_TransRates.Test

Ejemplos llamadas y resultados:
http://localhost:34778/api/rates/list
[
  {
    "from": "EUR",
    "to": "AUD",
    "rate": 0.72
  },
  {
    "from": "AUD",
    "to": "EUR",
    "rate": 1.39
  },
  ...
]
http://localhost:34778/api/transactions/list
[
  {
    "sku": "C3950",
    "amount": 34.6,
    "currency": "USD"
  },
  {
    "sku": "P7578",
    "amount": 30.8,
    "currency": "EUR"
  },
  ...
]
http://localhost:34778/api/transactions/listbysku?sku=Y5889
{
  "Total": 6664.71,
  "Transactions": [
    {
      "sku": "Y5889",
      "amount": 25.60,
      "currency": "EUR"
    },
    {
      "sku": "Y5889",
      "amount": 24.90,
      "currency": "EUR"
    },
    ...
  ]
}
