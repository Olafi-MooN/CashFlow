﻿namespace CashFlow.Domain;

public class User
{
    public Guid Id { get; set; } = Guid.Empty;
    public Guid UserIdentifier { get; set; } = Guid.Empty;
    public string Role { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public ICollection<Expense> Expenses { get; set; } = [];

}
