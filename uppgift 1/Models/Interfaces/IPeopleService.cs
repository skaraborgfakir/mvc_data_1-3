//
// Time-stamp: <2021-09-16 14:33:15 stefan>
//
// ett åtagande för ett gränssnitt till ett personkartotek
//

using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;

namespace Kartotek.Modeller.Interfaces {
   public interface IPeopleService {
      Person Add ( CreatePersonViewModell person );        // inläggning av ett kort
                                                           // använder Create i PeopleRepo
      PeopleViewModell All ();                            // totalutdrag från registret
      PeopleViewModell FindBy ( PeopleViewModell search ); // specifik(a) kort
      Person FindBy ( int id );                            // ett specifikt kort
      Person Edit ( int id, Person person );               // modifiering av ett kort med ett visst nummer
      bool Remove ( int id );                              // borttagning av ett visst kort
   }
}
