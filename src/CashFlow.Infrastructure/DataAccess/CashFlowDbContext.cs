using CashFlow.Domain;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure;

public class CashFlowDbContext : DbContext
{
	public DbSet<Expense> Expenses { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		var connectionString = "server=localhost;port=3306;database=CashFlowDb;Uid=root;Pwd=password;";
		var serverVersion = new MySqlServerVersion(new Version(5, 7, 44));
		optionsBuilder.UseMySql(connectionString, serverVersion);
	}
}
