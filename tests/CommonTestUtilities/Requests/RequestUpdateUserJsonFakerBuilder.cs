using Bogus;
using CashFlow.Communication;

namespace CommonTestUtilities.Requests;
public static class RequestUpdateUserJsonFakerBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));
    }
}
