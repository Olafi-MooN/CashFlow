using Bogus;
using CashFlow.Communication;
using CashFlow.Domain;

namespace CommonTestUtilities.Responses;

public static class ResponseUserJsonBuilder
{
    public static User Build()
    {
        return new Faker<User>()
            .RuleFor(user => user.Name, faker => faker.Name.FirstName())
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"))
            .RuleFor(user => user.Role, faker => faker.PickRandom<ERolesType>().ToString());
    }
}