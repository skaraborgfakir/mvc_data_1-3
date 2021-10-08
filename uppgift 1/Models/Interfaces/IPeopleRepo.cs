// Time-stamp: <2021-09-16 14:34:27 stefan>
//
// ett åtagande för ett gränssnitt till ett personkartotek
//

using System.Collections.Generic;

using Kartotek.Modeller.Entiteter;

namespace Kartotek.Modeller.Interfaces {
   public interface IPeopleRepo {
      Person Create ( string namn,           // inläggning av ett kort
                string bostadsort,
                string telefonnummer );
      List<Person> Read ();            // ett totalutdrag från registret
      Person Read ( int id );                 // utdrag av ett specifikt kort
      Person Update ( Person person );        // modifiering
      bool Delete ( Person person );          // borttagning
   }
}
