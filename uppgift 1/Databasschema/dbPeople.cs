//
// Time-stamp: <2021-11-08 20:51:59 stefan>
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Kartotek.Modeller.Entiteter;

namespace Kartotek.Databas {
    /// <summary>
    /// to be done
    /// </summary>
    public class dbPeople : DbContext {
	private readonly IConfiguration configurationsrc;
	public DbSet<Person> Person { get; set; }

	public dbPeople( DbContextOptions<dbPeople> options,
			 IConfiguration configurationsrc ) : base(options)
	{
	    this.configurationsrc = configurationsrc;
	}

	/// <summary>
	/// Använder PostgreSQL istället för MS SQL som database
	/// Istället för att via appsettings*.json sätta anslutningssträngarna kan man göra så här istället
	/// </summary>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
	    .UseNpgsql(configurationsrc["DBConnectionStrings:People"]);

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	    base.OnModelCreating(modelBuilder);
	    modelBuilder.Entity<Person>()
		.HasKey(o => new
		{
		    o.Id
		});
	}
    }
}
