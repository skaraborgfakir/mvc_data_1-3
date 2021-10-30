//
// Time-stamp: <2021-10-30 21:37:44 stefan>
//
// ett åtagande för ett gränssnitt till ett personkartotek
//

using System.Collections.Generic;

using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;

namespace Kartotek.Modeller.Interfaces {
    public interface IPeopleService {
	//
	// använder enbart entiteter direkt
	//
	List<Person> Utdraget ();              // totalutdrag från registret, en ren lista
	Person FindBy ( int id );              // ett specifikt kort
	Person Edit ( int id, Person person ); // modifiering av ett kort med ett visst nummer
	bool Remove ( int id );                // borttagning av ett visst kort

	//
	// använder vymodellerna
	//
	PeopleViewModell All ();                             // totalutdrag från registret, som vymodell
	PeopleViewModell FindBy ( PeopleViewModell search ); // specifik(a) kort, som vymodell
	Person Add ( CreatePersonViewModell person );        // inläggning av ett kort, använder Create i PeopleRep
    }
}
