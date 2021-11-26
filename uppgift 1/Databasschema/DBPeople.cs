//
// Time-stamp: <2021-11-26 14:20:25 stefan>
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
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// egen kod
using Kartotek.Modeller.Entiteter;

namespace Kartotek.Databas {
    /// <summary>
    /// to be done
    /// </summary>
    public class DBPeople : DbContext {
	/// <summary>
	/// loggning från DBPeople
	/// </summary>
	private readonly ILogger<DBPeople> loggdest;

	/// <summary>
	/// </summary>
	public IHostEnvironment Environment { get; }

	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	/// </summary>
	public IConfiguration Configuration { get; }

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
	/// <param name="configuration"></param>
	public DBPeople( DbContextOptions<DBPeople> options,
			 IHostEnvironment env,
			 ILogger<DBPeople> loggdest,
			 IConfiguration configuration ) : base(options)
	{
	    this.loggdest = loggdest;
	    Environment = env;
	    Configuration = configuration;

	    if ( Environment.IsDevelopment()) {
		this.loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					      "\n" + "Configuration: " + Configuration["DBConnectionStrings:People"] +
					      "\n" + "Configuration: " + Configuration["DBConnectionStrings:PeopleIdentity"] +
					      "\n" + "MS SQL - Environment: Development");
	    }
	    else if ( Environment.IsProduction()) {
		this.loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					      "\n" + "Configuration: " + Configuration["DBConnectionStrings:People"] +
					      "\n" + "Configuration: " + Configuration["DBConnectionStrings:PeopleIdentity"] +
					      "\n" + "MS SQL - Environment: Production");
	    }
	    else if ( Environment.IsEnvironment( "postgres.Development")) {
		this.loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					      "\n" + "Configuration: " + Configuration["DBConnectionStrings:People"] +
					      "\n" + "Configuration: " + Configuration["DBConnectionStrings:PeopleIdentity"] +
					      "\n" + "Postgres - Environment: Development");
	    }
	    else if ( Environment.IsEnvironment( "postgres")) {
		this.loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					      "\n" + "Configuration: " + Configuration["DBConnectionStrings:People"] +
					      "\n" + "Configuration: " + Configuration["DBConnectionStrings:PeopleIdentity"] +
					      "\n" + "Postgres - Environment: Production");
	    }
	}

	/// <summary>
	/// Använder PostgreSQL istället för MS SQL som database
	/// Istället för att via appsettings*.json sätta anslutningssträngarna kan man göra så här istället
	/// </summary>
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
	    if( Environment.IsEnvironment( "postgres.Development") ||
		Environment.IsEnvironment( "postgres"))
	    {
		optionsBuilder.UseNpgsql(Configuration["DBConnectionStrings:People"]);
	    } else {
		optionsBuilder.UseSqlServer(Configuration["DBConnectionStrings:People"]);
	    }
	}

	/// <summary>
	/// flyttar ut funktionen specifikt för Person till separat klass/funktion
	/// </summary>
	/// <see href="https://docs.microsoft.com/en-us/ef/core/modeling/">Databasscheman i EF</see>
	/// <see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.metadata.builders.entitytypebuilder?view=efcore-5.0">EntityTypeBuilder</see>
	public class PersonEntityTypeConfiguration : IEntityTypeConfiguration<Person>
	{
	    /// <summary>
	    /// beskrivning av relationen i mot EF
	    /// </summary>
	    public void Configure(EntityTypeBuilder<Person> builder)
	    {
		builder
		    .HasKey(o => new
		    {
			o.Id
		    });

		builder
		    .Property(o => o.Namn)
		    .IsRequired();
		builder
		    .Property(o => o.Bostadsort)
		    .IsRequired();

		builder
		    .HasData(
			new
			{
			    Id = 1,
			    Namn = "Michael Carlsson",
			    Bostadsort = "Solberga",
			    Telefonnummer = "0433"
			}
		    );
		builder
		    .HasData(
			new
			{
			    Id = 2,
			    Namn = "Ulf Smedbo",
			    Bostadsort = "Göteborg",
			    Telefonnummer = "031"
			}
		    );
	    }
	}

	/// <summary>
	/// </summary>
	/// <see href="https://docs.microsoft.com/en-us/ef/core/modeling/">Databasscheman i EF</see>
	/// <see href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.metadata.builders.entitytypebuilder?view=efcore-5.0">EntityTypeBuilder</see>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
	    base.OnModelCreating(modelBuilder);

	    modelBuilder.HasDefaultSchema("people");
	    new PersonEntityTypeConfiguration().Configure(modelBuilder.Entity<Person>());
	}
    }
}
