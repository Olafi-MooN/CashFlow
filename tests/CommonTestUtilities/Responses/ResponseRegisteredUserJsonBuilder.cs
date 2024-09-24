using Bogus;
using CashFlow.Communication;

namespace UseCases.Tests.Responses;

public static class ResponseRegisteredUserJsonBuilder
{
    public static ResponseRegisteredUserJson Build()
    {
        return new Faker<ResponseRegisteredUserJson>()
            .RuleFor(user => user.Name, faker => faker.Name.FirstName())
            .RuleFor(user => user.Token, (faker) => "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c");
    }
}
