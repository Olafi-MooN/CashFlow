using Bogus;
using CashFlow.Communication;
using CashFlow.Communication.Enums;

namespace CommonTestUtilities.Requests;
public static class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateExpenseJson Build()
    {
        return new Faker<RequestUpdateExpenseJson>()
            .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<EPaymentTypeEnum>())
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000));
    }
}