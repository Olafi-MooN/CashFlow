﻿using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CommonTestUtilities.Requests;

public class RequestRegisterExpensiveJsonBuilder
{
    public RequestRegisterExpensiveJson Build()
    {

        return new Faker<RequestRegisterExpensiveJson>()
            .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<EPaymentTypeEnum>())
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000));
    }
}
