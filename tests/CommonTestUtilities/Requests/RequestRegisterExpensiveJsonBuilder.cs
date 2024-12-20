﻿using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

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
            .RuleFor(r => r.Tags, faker => faker.Make(1, () => faker.PickRandom<ETagTypeEnum>()))
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000));
    }
}
