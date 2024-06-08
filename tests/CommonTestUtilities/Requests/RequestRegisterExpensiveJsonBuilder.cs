using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CommonTestUtilities.Requests;

public class RequestRegisterExpensiveJsonBuilder
{
    public RequestRegisterExpensiveJson Build()
    {

        var faker = new Faker("en");

        return new RequestRegisterExpensiveJson
        {
            Amount = faker.Finance.Amount(),
            Date = faker.Date.Past(),
            Description = faker.Commerce.Price(),
            PaymentType = faker.PickRandom<EPaymentTypeEnum>(),
            Title = faker.Commerce.ProductName(),
        };
    }
}
