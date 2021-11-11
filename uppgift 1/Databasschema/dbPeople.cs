//
// Time-stamp: <2021-11-11 14:47:36 stefan>
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
    public class DBPeople : DbContext {
	/// <summary>
	/// </summary>
	public IHostEnvironment Environment { get; }

	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	/// </summary>
	public IConfiguration Configurationsrc { get; }

	/// <summary>
	/// loggning från DBPeople
	/// </summary>
	public ILogger<DBPeople> Loggdest { get; }

	/// <summary>
	/// databasens relation som objekt-relations mappning
	/// </summary>
	public DbSet<Person> Person { get; set; }


	/// <summary>
	/// Kreator
	/// </summary>
	/// <param name="options"></param>
	/// <param name="env"></param>
	/// <param name="loggdest"></param>
	/// <param name="configurationsrc"></param>
	public DBPeople( DbContextOptions<DBPeople> options,
			 IHostEnvironment env,
			 ILogger<DBPeople> loggdest,
			 IConfiguration configurationsrc ) : base(options)
	{
	    Environment = env;
	    Loggdest = loggdest;
	    Configurationsrc = configurationsrc;

	    Loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
				     "\n" + "Configurationsrc: " + Configurationsrc["DBConnectionStrings:People"] +
				     "\n" + "Configurationsrc: " + Configurationsrc["DBConnectionStrings:PeopleIdentity"]);

	    if ( Environment.IsDevelopment()) {
		Loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) + "\n"
					 + "MS SQL - Environment: Development");
	    }
	    if ( Environment.IsProduction()) {
		Loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) + "\n"
					 + "MS SQL - Environment: Production");
	    }
	    if ( Environment.IsEnvironment( "postgres.Development")) {
		Loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) + "\n"
					 + "Postgres - Environment: Development");
	    }
	    if ( Environment.IsEnvironment( "postgres")) {
		Loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) + "\n"
					 + "Postgres - Environment: Production");
	    }
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
	    Loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) + "\n"
				     + "Configurationsrc: " + Configurationsrc["session_kakans_namn"]);

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
	    //	    Namn= "Ulf Smedbo",
	    //	    Bostadsort= "Göteborg",
	    //	    Telefonnummer= "031"
	    //	},
	    //	new Person
	    //	{
	    //	    Namn= "Bengt Ulfsson",
	    //	    Bostadsort= "Växjö",
	    //	    Telefonnummer= "044"
	    //	}
	    //	);
	}
    }
}
