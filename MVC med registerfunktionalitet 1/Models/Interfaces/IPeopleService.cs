// Time-stamp: <2021-09-08 15:49:28 stefan>
//
// ett åtagande för ett gränssnitt till ett personkartotek
//

namespace Kartotek.Modeller
{
    interface IPeopleService
    {
	Person Add( CreatePersonViewModell person);
	PeopleViewModell All();
	PeopleViewModell FindBy( PeopleViewModell sökterm);
	Person FindBy( int id);
	Person Edit( int id, Person person);
	bool Remove( int id);
    }
}
