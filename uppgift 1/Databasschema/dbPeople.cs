//
// Time-stamp: <2021-11-09 14:46:08 stefan>
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;

// egen kod
using Kartotek.Modeller.Entiteter;

namespace Kartotek.Databas {
    /// <summary>
    /// to be done
    /// </summary>
    public class dbPeople : DbContext {
	/// <summary>
	/// </summary>
	public IHostEnvironment Environment { get; }

	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	/// </summary>
	public IConfiguration Configurationsrc { get; }

	/// <summary>
	/// databasens relation som objekt-relations mappning
	/// </summary>
	public DbSet<Person> Person { get; set; }

	/// <summary>
	///
	/// </summary>
	/// <param name="options"></param>
	/// <param name="env"></param>
	/// <param name="loggdest"></param>
	/// <param name="configurationsrc"></param>
	public dbPeople( DbContextOptions<dbPeople> options,
			 IHostEnvironment env,
			 ILogger<dbPeople> loggdest,
			 IConfiguration configurationsrc ) : base(options)
	{
	    Console.WriteLine("public dbPeople( DbContextOptions<dbPeople> options,");

	    if ( Environment.IsDevelopment() ||
		 Environment.IsEnvironment( "Development_postgres")) {
		loggdest.LogInformation( "public dbPeople( DbContextOptions<dbPeople> options," +
					 "\n" + Configurationsrc["DBConnectionStrings:People"]);
	    }

	    Environment = env;
	    Configurationsrc = configurationsrc;
	}

	/// <summary>
	/// Använder PostgreSQL istället för MS SQL som database
	/// Istället för att via appsettings*.json sätta anslutningssträngarna kan man göra så här istället
	/// </summary>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder
	    .UseNpgsql(Configurationsrc["DBConnectionStrings:People"]);

	/// <summary>
	/// </summary>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	    Console.WriteLine("protected override void OnModelCreating(ModelBuilder modelBuilder)");

	    base.OnModelCreating(modelBuilder);
	    modelBuilder.Entity<Person>()
		.HasKey(o => new
		{
		    o.Id
		});

	    // modelBuilder
	    //	.Entity<Person>().HasData(
	    //	new Person
	    //	{
	    //	    id = 1,
	    //	    Namn= "Ulf Smedbo",
	    //	    Bostadsort= "Göteborg",
	    //	    Telefonnummer= "031"
	    //	},
	    //	new Person
	    //	{
	    //	    id = 2,
	    //	    Namn= "Bengt Ulfsson",
	    //	    Bostadsort= "Växjö",
	    //	    Telefonnummer= "044"
	    //	}
	    //	);
	}
    }
}
