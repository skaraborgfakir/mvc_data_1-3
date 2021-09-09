// Time-stamp: <2021-09-08 23:01:45 stefan>
//

using System.Collections.Generic;

namespace Kartotek.Modeller
{
    public class PeopleViewModell
    {
	//
	// för sökkriterier
	//
	[DisplayFormat(ConvertEmptyStringToNull = true)]
	public Person sökkriterier;

	//
	// addera person
	//
	[DisplayFormat(ConvertEmptyStringToNull = true)]
	public Person personAttLäggaIn;

	//
	// listan av personer
	//
	public List<Person> _personkartotek;

	public PeopleViewModell( List<Person> personer)
	{
	    _personkartotek = personer;
	}
    }
}
