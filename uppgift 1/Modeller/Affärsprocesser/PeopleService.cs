//
// Time-stamp: <2021-11-28 17:51:02 stefan>
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

using Kartotek.Modeller.Interfaces;
using Kartotek.Modeller.Data;
using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;

namespace Kartotek.Modeller {
    /// <summary>
    /// to be done
    /// </summary>
    public class PeopleService : IPeopleService {
	private readonly IPeopleRepo kartoteket;
	private readonly ILogger<PeopleService> loggdest;

	/// <summary>
	/// kreator av PeopleService
	/// Ympas med DI med ett kartotek
	/// injektion för att få tillgång till gemensam InMemoryPeopleRepo
	/// </summary>
	/// <param name="loggdest">Ympning med loggfunktionen</param>
	/// <param name="kartoteket">Ympning av IPeopleRepo</param>
	public PeopleService( ILogger<PeopleService> loggdest,
			      IPeopleRepo kartoteket) {
	    this.loggdest = loggdest;
	    this.kartoteket = kartoteket;
	}

	/// <summary>
	/// plocka fram ett visst kort
	/// enbart i form av entiteter, ej vymodell
	/// </summary>
	public Person FindBy ( int id ) {
	    return kartoteket.Read( id: id );
	}

	/// <summary>
	/// modifiering av ett specifikt kort
	/// enbart i form av entiteter, ej vymodell
	/// </summary>
	public Person Edit ( int id, Person person ) {
	    // Not developed yet.
	    throw new NotImplementedException();
	}

	/// <summary>
	/// kasering av ett specifikt kort
	/// enbart i form av entiteter, ej vymodell
	/// </summary>
	public bool Remove ( int id ) {
	    this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
					  (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					  "\n kasera kortet med id : " + id.ToString());

	    Person kortet = this.FindBy( id );

	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
				    (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    if (kortet != null)
		return kartoteket.Delete( kortet );

	    return false;
	}

	/// <summary>
	/// tillägg av ett kort
	/// utgår från en vymodell
	/// </summary>
	public Person Add ( CreatePersonViewModel nyttKort ) {
	    return kartoteket.Create( namn: nyttKort.Namn,
				      bostadsort: nyttKort.Bostadsort,
				      telefonnummer: nyttKort.Telefonnummer );
	}

	/// <summary>
	/// sökning efter poster - utgår från en vymodell som sökterm
	/// kriterier : inga, dvs alla kort ska hämtas och levereras
	/// </summary>
	public PeopleViewModel All () {
	    List<Person> utdrag = kartoteket.Read();

	    foreach (Person kort in utdrag) {
		this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) + "\n" +
					      "id: " + kort.Id.ToString() + "\n" +
					      "namn: " + kort.Namn + "\n" +
					      "namn: " + kort.Bostadsort);
	    }

	    PeopleViewModel vyn = new PeopleViewModel() { Utdraget = utdrag };

	    return vyn;
	}

	/// <summary>
	/// sökning efter poster - utgår från en vymodell som sökterm
	/// kriterier :
	///    namn
	///  eller
	///    bostadsort
	///  eller
	///    båda ?
	/// </summary>
	public PeopleViewModel FindBy ( PeopleViewModel search ) {
	    this.loggdest.LogInformation(
		(new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
		(new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    //<summary>
	    // IPeopleRepo definierar inte någon operator som kan söka på något annat än int eller
	    // så den här funktionen behöver läsa in och iterera igenom hela listan (relationen)
	    //</summary>
	    PeopleViewModel vyn = new PeopleViewModel();

	    List<Person> posterna = kartoteket.Read();

	    //
	    // sökning på namn eller bostadsort, eller båda ?
	    //
	    if ( !String.IsNullOrEmpty( search.Namn ) &&
		 !String.IsNullOrEmpty( search.Bostadsort ))
	    {
		this.loggdest.LogInformation(
		    (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
		    (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString())+
		    "\n" + "Namn : " + search.Namn +
		    "\n" + "Bostadsort : " + search.Bostadsort);

		vyn.Utdraget = posterna
		    .Where( posterna => ( posterna.Namn.ToLower() == search.Namn.ToLower() &&
					  posterna.Bostadsort.ToLower() == search.Bostadsort.ToLower()))
		    .ToList();

		this.loggdest.LogInformation(
		    (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
		    (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));
	    }
	    //
	    // båda var inte satta, kan det finnas något i Namn ?
	    //
	    else if ( !String.IsNullOrEmpty( search.Namn ))
	    {
		this.loggdest.LogInformation(
		    (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
		    (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		vyn.Utdraget = posterna
		    .Where( posterna => posterna.Namn.ToLower() == search.Namn.ToLower())
		    .ToList();
	    }
	    //
	    // kan det finnas något i Bostadsort ?
	    //
	    else if ( !String.IsNullOrEmpty( search.Bostadsort ))
	    {
		this.loggdest.LogInformation(
		    (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
		    (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		vyn.Utdraget = posterna
		    .Where( posterna => posterna.Bostadsort.ToLower() == search.Bostadsort.ToLower())
		    .ToList();
	    }
	    //
	    // Nähä ingen sökterm var satt.
	    //
	    else
	    {
		this.loggdest.LogInformation(
		    (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
		    (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		vyn.Utdraget = posterna;
	    }

	    return vyn;
	}
    }
}
