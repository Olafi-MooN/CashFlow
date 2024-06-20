using CashFlow.Domain;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure;

public class CashFlowDbContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<Expense> Expenses { get; set; }
}
