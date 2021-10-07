//
// Time-stamp: <2021-09-20 09:44:24 stefan>
//
// Hopslagning av de klasser som är del av definitionerna i IPeopleService
// iom att razor-filerna enbart vill hantera en modell, alltså måste klasserna
// slås ihop till en gemensam modell i kontroller
//
// garantera att:
//   att användarna inte försöker skapa flera kort med samma innehåll
//   kontroll av uppgifterna
//      att följande är satta dvs ej NULL
//        namn
//        bostadsort
//        telefonnummer
//
// slås isär i kontroller innan delarna lämnas över till Peopleservice
//

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using Kartotek.Modeller.Vyer;

namespace Kartotek.Modeller.Vyer
{
    public class HopslagenmodellVymodell
    {
        //
        // medlemmar som motsvarar PeopleService API
        //
        // public PeopleViewModell personlistan; // från PeopleService - en eventuellt filtrerad lista av kort
        public PeopleViewModell Personlistan
        {
            get;
            set;
        }

        public PeopleViewModell Filtertermer
        { // till PeopleService - // innehåller sökkriterier
            get; set;
        }

        public CreatePersonViewModell NyttKort
        {
            get; set;
        }

        public AktionSpecifiktkort specifiktKort
        {
            get; set;
        }

    }
}
