// Time-stamp: <2021-10-30 22:14:30 stefan>
//

using System;
using System.Collections.Generic;
using System.Linq;

using Kartotek.Modeller.Interfaces;
using Kartotek.Modeller.Data;
using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;

namespace Kartotek.Modeller {
    public class PeopleService : IPeopleService {
	private static InMemoryPeopleRepo kartoteket = new InMemoryPeopleRepo();

	/// <summary>
	/// gör ett komplett utdrag ur registret
	/// enbart i form av entiteter, ej vymodell
	/// </summary>
	/// <returns>
	/// lista av personer
	/// </returns>
	/// <see cref="PeopleAjaxController"/>
	public List<Person> Utdraget() {
	    return kartoteket.Read();
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
	public Person Add ( CreatePersonViewModell nyttKort ) {
	    return kartoteket.Create( namn: nyttKort.Namn,
				      bostadsort: nyttKort.Bostadsort,
				      telefonnummer: nyttKort.Telefonnummer );
	}

	/// <summary>
	/// sökning efter poster - utgår från en vymodell som sökterm
	/// kriterier : inga, dvs alla kort ska hämtas och levereras
	/// </summary>
	public PeopleViewModell All () {
	    PeopleViewModell vyn = new PeopleViewModell() { Utdraget = kartoteket.Read() };

	    return vyn;
	}

	/// <summary>
	/// sökning efter poster - utgår från en vymodell som sökterm
	/// kriterier : namn
	/// </summary>
	private PeopleViewModell FindByNamn ( String namn,
					      List<Person> poster,
					      PeopleViewModell vyn ) {
	    foreach (Person item in poster) {
		if (namn.Equals( item.Namn, StringComparison.OrdinalIgnoreCase )) {
		    vyn.Utdraget.Add( item );
		}
	    }

	    return vyn;
	}

	/// <summary>
	/// sökning efter poster - utgår från en vymodell som sökterm
	/// kriterier : bostadsort
	/// </summary>
	private PeopleViewModell FindByBostadsort ( String bostadsort,
						    List<Person> poster,
						    PeopleViewModell vyn ) {
	    foreach (Person item in poster) {
		if (bostadsort.Equals( item.Bostadsort, StringComparison.OrdinalIgnoreCase )) {
		    vyn.Utdraget.Add( item );
		}
	    }

	    return vyn;
	}

	/// <summary>
	/// sökning efter poster - utgår från en vymodell som sökterm
	/// kriterier :
	///    namn
	///  eller
	///    bostadsort
	/// </summary>
	public PeopleViewModell FindBy ( PeopleViewModell sökterm ) {
	    List<Person> poster = kartoteket.Read();
	    PeopleViewModell vyn = new PeopleViewModell();
	    vyn.Utdraget = new List<Person>();

	    if (!String.IsNullOrEmpty( sökterm.Namn )) {
		vyn = FindByNamn( namn: sökterm.Namn,
				  poster: poster,
				  vyn: vyn );
	    } else if (!String.IsNullOrEmpty( sökterm.Bostadsort )) {
		vyn = FindByBostadsort( bostadsort: sökterm.Bostadsort,
					poster: poster,
					vyn: vyn );
	    }

	    return vyn;
	}

    }
}
