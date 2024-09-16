using System;
using CashFlow.Domain;
using CashFlow.Domain.Services.LoggedUser;
using CommonTestUtilities.Entities;
using Moq;

namespace UseCases.Tests.Repositories;

public class LoggedUserBuilder
{
    private readonly Mock<ILoggedUser> _mock;

    public LoggedUserBuilder()
    {
        _mock = new Mock<ILoggedUser>();
        _mock.Setup(m => m.Get()).ReturnsAsync(UserBuilder.Build());
    }

    public ILoggedUser Build() => _mock.Object;
    public LoggedUserBuilder Build(User user)
    {
        _mock.Setup(m => m.Get()).ReturnsAsync(user);
        return this;
    }
}
