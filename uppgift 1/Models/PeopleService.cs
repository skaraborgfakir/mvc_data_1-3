// Time-stamp: <2021-11-04 10:11:25 stefan>
//

using System;
using System.Collections.Generic;
using System.Linq;

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

	/// <summary>
	/// kreator av PeopleService
	/// Ympas med DI med ett kartotek
	/// injektion för att få tillgång till gemensam InMemoryPeopleRepo
	/// </summary>
	/// <param name="kartoteket"></param>
	public PeopleService( IPeopleRepo kartoteket) {
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
	    Person kortet = this.FindBy( id );

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
	    PeopleViewModel vyn = new PeopleViewModel() { Utdraget = kartoteket.Read() };

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
	    ///<summary>
	    /// IPeopleRepo definierar inte någon operator som kan söka på något annat än int eller
	    /// så den här funktionen behöver läsa in och iterera igenom hela listan (relationen)
	    ///</summary>
	    PeopleViewModel vyn = new PeopleViewModel();

	    List<Person> posterna = kartoteket.Read();

	    //
	    // sökning på namn eller bostadsort, eller båda ?
	    //
	    if ( !String.IsNullOrEmpty( search.Namn ) &&
		 !String.IsNullOrEmpty( search.Bostadsort ))
		vyn.Utdraget = posterna.Where( posterna => ( posterna.Namn == search.Namn && posterna.Bostadsort == search.Bostadsort)).ToList();
	    else if ( !String.IsNullOrEmpty( search.Namn ))
		vyn.Utdraget = posterna.Where( posterna => posterna.Namn == search.Namn).ToList();
	    else if ( !String.IsNullOrEmpty( search.Namn ))
		vyn.Utdraget = posterna.Where( posterna => posterna.Bostadsort == search.Bostadsort).ToList();
	    else
		vyn.Utdraget = posterna;

	    return vyn;
	}
    }
}
