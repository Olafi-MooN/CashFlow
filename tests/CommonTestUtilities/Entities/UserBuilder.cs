using Bogus;
using CashFlow.Domain;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities;

public static class UserBuilder
{
    public static User Build()
    {
        var encryptPassword = EncryptPasswordBuilder.Build();

        return new Faker<User>()
            .RuleFor(User => User.Id, _ => Guid.NewGuid())
            .RuleFor(user => user.Name, faker => faker.Name.FirstName())
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, (_, user) => encryptPassword.Encrypt(user.Password));
    }
}
