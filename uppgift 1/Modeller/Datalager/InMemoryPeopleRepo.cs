//
// Time-stamp: <2021-11-26 14:29:35 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Extensions.Logging;

using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Interfaces;

namespace Kartotek.Modeller.Data {
    /// <summary>
    /// datalager - en abstraktion som lämnar garantier för data:uppgifters oföränderlighet (persistence)
    /// användande lager ska ha möjlighet att ställa olika frågor mot modulen.
    /// specifikt i det här fallet :
    ///   vilka personer heter så-och-så ? Ge mig dem!
    ///   vilka personer bor här-eller-där ? Ge mig dem!
    /// uppgifterna lämnas som en referense till en behållare. Behållaren garanteras kunna bli komplett
    /// dvs modulen kommer att sätta upp den så uppgifterna kan hämtas ut av användande modulen
    /// gradvis.
    /// </summary>
    public class InMemoryPeopleRepo : IPeopleRepo {
	/// <summary>
	/// testdata för övriga delar. I EF:versionen ersätts den här med en SQL:databas
	/// </summary>
	private static List<Person> kartoteket = new List<Person> ();
	private static int idCounter; // nästa tillgänglig id. Används och exponeras som nyckel in i personregistret
	private readonly ILogger<IPeopleRepo> loggdest;

	/// <summary>
	/// Kreator - fyller i seed-data
	/// </summary>
	public InMemoryPeopleRepo ( ILogger<IPeopleRepo> loggdest) {
	    this.loggdest = loggdest;

	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    this.Create( namn: "Ulf Smedbo", bostadsort: "Göteborg", telefonnummer: "031" );
	    this.Create( namn: "Ulf Smedbo", bostadsort: "Växjö", telefonnummer: "0444" );
	    this.Create( namn: "Bengt Ulfsson", bostadsort: "Växjö", telefonnummer: "044" );
	    this.Create( namn: "Micke Carlsson", bostadsort: "Solberga", telefonnummer: "0321" );
	    this.Create( namn: "Ulf Bengtsson", bostadsort: "Växjö", telefonnummer: "044" );
	    this.Create( namn: "Simon Heinonen", bostadsort: "Skövde", telefonnummer: "0500" );
	    this.Create( namn: "Wei C", bostadsort: "Göteborg", telefonnummer: "031" );
	    this.Create( namn: "Jonathan Krall", bostadsort: "Stenstorp", telefonnummer: "0500" );
	}

	/// <summary>
	/// Lägg upp ett nytt personkort
	/// </summary>
	public Person Create ( string namn,
			       string bostadsort,
			       string telefonnummer ) {
	    Person person = new Person( id : idCounter++,
					namn : namn,
					bostadsort : bostadsort,
					telefonnummer : telefonnummer);
	    kartoteket.Add( person );

	    return person;
	}

	//
	// används från vyn indirekt via PeopleService
	//
	// alla personer i listan
	//
	/// <summary>
	/// to be done
	/// </summary>
	public List<Person> Read () {
	    List<Person> utdraget = new List<Person>();

	    foreach (Person item in kartoteket) {
		utdraget.Add( item: item );
	    }

	    return utdraget;
	}

	//
	// används från PeopleService
	//
	/// <summary>
	/// to be done
	/// </summary>
	public Person Read ( int id ) {
	    return kartoteket.FirstOrDefault(person => person.Id == id);

	    // borde vara omöjligt att komma hit
	    throw new ArgumentException( "felaktigt id i InMemoryPeopleRepo:Read" );
	}

	//
	// hur ??
	//
	/// <summary>
	/// to be done
	/// </summary>
	public Person Update ( Person person ) {
	    // Not developed yet.
	    throw new NotImplementedException( "InMemoryPeopleRepo.cs: Update" );
	}

	//
	// aktiveras från delete i vyn via PeopleService
	//
	/// <summary>
	/// to be done
	/// </summary>
	public bool Delete ( Person personkortet ) {
	    bool status = false;

	    //
	    // använd en iterator som räknas ner
	    //
	    // fördelen är att remove kommer enbart påverka tupler
	    // som vi redan har passerat, annars blir det en höna-och-ägget:situation
	    //
	    foreach (Person item in kartoteket.Reverse<Person>()) {
		if (item == personkortet) {
		    kartoteket.Remove( item );

		    status = true;
		}
	    }

	    return status;
	}
    }
}
