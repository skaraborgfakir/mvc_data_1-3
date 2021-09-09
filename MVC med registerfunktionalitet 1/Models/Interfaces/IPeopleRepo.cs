// Time-stamp: <2021-09-08 15:07:10 stefan>
//
// ett åtagande för ett gränssnitt till ett personkartotek
//

using System.Collections.Generic;

namespace Kartotek.Modeller
{
    interface IPeopleRepo
    {
	Person Create( string namn,
		       string bostadsort,
		       string telefon);
	List<Person> Read();
	Person Read(int id);
	Person Update(Person person);
	bool Delete(Person person);
    }
}
