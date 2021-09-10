// Time-stamp: <2021-09-10 15:00:57 stefan>
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace Kartotek.Modeller {
    public class PeopleService : IPeopleService
    {
	private static InMemoryPeopleRepo repo = new InMemoryPeopleRepo();

	public Person Add( CreatePersonViewModell nyttKort)
	{
	    return repo.Add( nyttKort.personAttLäggaIn);
	}

	public PeopleViewModell All()
	{
	    PeopleViewModell vyn = new PeopleViewModell( personer: repo.Read() );

	    return vyn;
	}

	//
	// används för filtrering av poster
	//    bostadsort
	//
	public PeopleViewModell FindBy( PeopleViewModell sökterm)
	{
	    List<Person> poster = repo.Read();

	    if ( ! IsNullOrEmpty( sökterm.sökkriterier.namn)) {
		//
		// sökning på namn
		//
		// iterering över listan från slutet
		//   rensa bort poster som inte passar
		//
	    } else if  ( ! IsNullOrEmpty( sökterm.sökkriterier.bostadsort)) {
		//
		// sökning på bostadsort
		//
		// iterering över listan från slutet
		//   rensa bort poster som inte passar
		//
	    }
	}

	public Person FindBy( int id)
	{
	    return repo.Read( id: id);
	}

	public Person Edit( int id, Person person)
	{
	    throw NotImplementedException();
	}

	public bool Remove( int id)
	{
	    throw NotImplementedException();
	}
    }
}
