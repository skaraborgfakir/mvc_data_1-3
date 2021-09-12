// Time-stamp: <2021-09-11 16:50:25 stefan>
//

//
// garantera att:
//   att användarna inte försöker skapa flera kort med samma innehåll
//   kontroll av uppgifterna
//      att följande är satta dvs ej NULL
//        namn
//        bostadsort
//        telefonnummer
//

namespace Kartotek.Modeller {
    public class HopslagenmodellVymodell {
	public CreatePersonViewModell createPersonViewModell;
	public PeopleViewModell	PeopleViewModell;

	public HopslagenmodellVymodell () {
	}
    }
}
