using Bogus;
using CashFlow.Communication;

namespace CommonTestUtilities.Requests;
public static class RequestChangePasswordJsonFakerBuilder
{
    public static RequestChangePasswordJson Build()
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(user => user.OldPassword, faker => faker.Internet.Password(prefix: "!Aa1"))
            .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa2"));
    }
}
