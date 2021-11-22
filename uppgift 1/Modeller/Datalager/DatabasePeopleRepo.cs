//
// Time-stamp: <2021-11-22 12:46:25 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

using System;
using System.Collections.Generic;
using System.Linq;

// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http?view=aspnetcore-3.1
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore;

using Kartotek.Modeller.Interfaces;
using Kartotek.Modeller.Data;
using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;
using Kartotek.Databas;

namespace Kartotek.Modeller {
    /// <summary>
    /// datalagerklass
    ///   databasen hanterar en entitet (än så länge)
    ///
    ///   två relationer : stad och land ska in.
    /// </summary>
    public class DatabasePeopleRepo : IPeopleRepo {
	/// <summary>
	/// referens till EF:s databaskontext
	/// </summary>
	private DBPeople Kartoteket { get; set; }

	/// <summary>
	/// loggdestination
	/// </summary>
	private readonly ILogger<DatabasePeopleRepo> loggdest;

	/// <summary>
	/// Kreator - använd ympning (DI) för att få med ett databaslager
	/// databaslagret är registrerat i Startup.cs: ConfigureServices
	/// </summary>
	/// <param name="loggdest">Loggning (DI) från modulen</param>
	/// <param name="kartoteket">Ympning (DI) med en referens till databaskontextet</param>
	public DatabasePeopleRepo ( ILogger<DatabasePeopleRepo> loggdest,
				    DBPeople kartoteket )
	{
	    this.loggdest = loggdest;
	    Kartoteket = kartoteket;

	    this.loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));
	}

	/// <summary>
	/// </summary>
	public Person Create( string namn,
			      string bostadsort,
			      string telefonnummer) {

	    Person ny = new Person( id: 0,
				    namn: namn,
				    bostadsort: bostadsort,
				    telefonnummer: telefonnummer);

	    Kartoteket.Person.Add( ny);
	    Kartoteket.SaveChanges();

	    return ny;
	}

	/// <summary>
	/// returnerar en lista (iterator) för läsning från databasen
	/// </summary>
	public List<Person> Read() {
	    this.loggdest.LogInformation(
		(new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
		(new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    return Kartoteket.Person
		.Where ( p => p.Id != 0)
		.ToList();
	}

	/// <summary>
	/// inläsning av ett visst kort
	/// </summary>
	public Person Read ( int id ) {
	    this.loggdest.LogInformation(
		(new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
		(new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
		"\n sök efter kortet med id : " + id.ToString() );

	    // LINQ metod syntax
	    // return Kartoteket.Person
	    //	.Where (p => p.Id == id)
	    //	.Single();
	    return Kartoteket.Person
		.SingleOrDefault (p => p.Id == id);
	}

	/// <summary>
	/// modfiering av ett visst kort
	/// </summary>
	public Person Update ( Person person) {
	    throw new NotImplementedException( "funktionalitet ej klar");
	}

	/// <summary>
	/// kasering av uppgifter
	/// </summary>
	public bool Delete ( Person person )
	{
	    bool result = true;
	    try
	    {
		Kartoteket.Person.Remove( person);
		Kartoteket.SaveChanges();
		result = true;
	    }
	    catch (DbUpdateException /* ex */)
	    {
		result = false;
	    }

	    return result;
	}
    }
}
