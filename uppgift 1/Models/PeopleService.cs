// Time-stamp: <2021-09-20 16:16:40 stefan>
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

	public Person Add( CreatePersonViewModell nyttKort) {
	    return kartoteket.Create( namn:          nyttKort.Namn,
				      bostadsort:    nyttKort.Bostadsort,
				      telefonnummer: nyttKort.Telefonnummer);
	}

	//
	// sökning efter poster
	//
	// kriterier : inga
	//
	public PeopleViewModell All() {
	    PeopleViewModell vyn = new PeopleViewModell();
	    vyn.Utdraget = kartoteket.Read();

	    return vyn;
	}

	//
	// kriterier : namn
	//
	private PeopleViewModell FindByNamn( String namn,
					     List<Person> poster,
					     PeopleViewModell vyn ) {
	    Console.WriteLine( "public PeopleViewModell FindByNamn( String namn,");
	    foreach (Person item in poster) {
		if (item.Namn == namn) {
		    vyn.Utdraget.Add( item);
		}
	    }

	    return vyn;
	}
	//
	// kriterier : bostadsort
	//
	private PeopleViewModell FindByBostadsort( String bostadsort,
						   List<Person> poster,
						   PeopleViewModell vyn) {
	    Console.WriteLine( "public PeopleViewModell FindByBostadsort( String bostadsort");

	    foreach (Person item in poster) {
		if (item.Bostadsort == bostadsort ) {
		    vyn.Utdraget.Add( item);
		}
	    }

	    return vyn;
	}

	//
	// sökning efter poster
	//
	// kriterier :
	//    namn
	//  eller
	//    bostadsort
	//
	public PeopleViewModell FindBy( PeopleViewModell sökterm) {
	    List<Person> poster = kartoteket.Read();
	    PeopleViewModell vyn = new PeopleViewModell();
	    vyn.Utdraget = new List<Person>();

	    if ( ! String.IsNullOrEmpty( sökterm.Namn)) {
		Console.WriteLine( "public PeopleViewModell FindBy( PeopleViewModell namn");
		vyn = FindByNamn( namn: sökterm.Namn,
				  poster: poster,
				  vyn: vyn);
	    } else if  ( ! String.IsNullOrEmpty( sökterm.Bostadsort)) {
		Console.WriteLine( "public PeopleViewModell FindBy( PeopleViewModell bostadsort");
		vyn = FindByBostadsort( bostadsort: sökterm.Bostadsort,
					poster: poster,
					vyn: vyn);
	    }

	    return vyn;
	}

	public Person FindBy( int id) {
	    return kartoteket.Read( id: id);
	}

	public Person Edit( int id, Person person) {
	    // Not developed yet.
	    throw new NotImplementedException();
	}

	public bool Remove( int id) {
	    Person kortet = this.FindBy( id);

	    if ( kortet == null) {
		return false;
	    } else {
		return kartoteket.Delete( kortet);
	    }
	    // Not developed yet.
	    //throw new NotImplementedException();
	}
    }
}
