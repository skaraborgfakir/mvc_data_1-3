//
// Time-stamp: <2021-11-28 19:55:33 stefan>
//

// EF6
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// egen kod
using Kartotek.Modeller.Entiteter;

namespace Kartotek.Databas
{
    /// <summary>
    /// flyttar ut funktionen specifikt för Person till separat klass/metod
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
}
