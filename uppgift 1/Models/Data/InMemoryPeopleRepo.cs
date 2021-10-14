//
// Time-stamp: <2021-09-20 16:16:56 stefan>
//

using System;
using System.Collections.Generic;
using System.Linq;

using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Interfaces;

namespace Kartotek.Modeller.Data {
   public class InMemoryPeopleRepo : IPeopleRepo {
      // private class Item {
      //     int    id;
      //     Person personkortet;

      //     public Item( int id, Person person) {
      //	Id = id;
      //	Personkortet = person;
      //     }
      //     public int Id {
      //	get { return id; }
      //	set { id = value; }
      //     }
      //     public Person Personkortet {
      //	get { return personkortet; }
      //	set { personkortet = value; }
      //     }
      // }

      private static List<Person> kartoteket = new List<Person> ();
      private static int idCounter; // nästa tillgänglig id. Används som nyckel in i
                                    // personregistret

      public InMemoryPeopleRepo () {
         this.Create( namn: "Ulf Smedbo", bostadsort: "Göteborg", telefonnummer: "031" );
         this.Create( namn: "Bengt Ulfsson", bostadsort: "Växjö", telefonnummer: "044" );
         this.Create( namn: "Micke Carlsson", bostadsort: "Solberga", telefonnummer: "0321" );
         this.Create( namn: "Ulf Bengtsson", bostadsort: "Växjö", telefonnummer: "044" );
         this.Create( namn: "Simon Heinonen", bostadsort: "Skövde", telefonnummer: "0500" );
         this.Create( namn: "Wei C", bostadsort: "Göteborg", telefonnummer: "031" );
         this.Create( namn: "Jonathan Krall", bostadsort: "Stenstorp", telefonnummer: "0500" );
      }

      public Person Create ( string namn,
                  string bostadsort,
                  string telefonnummer ) {
         Person person = new Person( id: idCounter++,
                 namn: namn,
                 bostadsort: bostadsort,
                 telefonnummer: telefonnummer );

         kartoteket.Add( person );

         return person;
      }

      //
      // används från vyn indirekt via PeopleService
      //
      // alla personer i listan
      //
      public List<Person> Read () {
         List<Person> utdraget = new List<Person>();

         foreach (Person item in kartoteket) {
            utdraget.Add( item: item );
         }

         return utdraget;
      }

      //
      // används från PeopleService
      //
      public Person Read ( int id ) {
		  return kartoteket.FirstOrDefault( predicate => predicate.Id == id);
         /* foreach (Person item in kartoteket) {
            if (item.Id == id) {
               return item;
            }
         } */

         // borde vara omöjligt att komma hit
         throw new ArgumentException( "felaktigt id i InMemoryPeopleRepo:Read" );
      }

      //
      // hur ??
      //
      public Person Update ( Person person ) {
         // Not developed yet.
         throw new NotImplementedException( "InMemoryPeopleRepo.cs: Update" );
      }

      //
      // aktiveras från delete i vyn via PeopleService
      //
      public bool Delete ( Person personkortet ) {
         bool status = false;

         //
         // använd en iterator som räknas ner
         //
         // fördelen är att remove kommer enbart påverka tupler
         // som vi redan har passerat, annars blir det en höna-och-ägget:situation
         //
         foreach (Person item in kartoteket.Reverse<Person>()) {
            if (item == personkortet) {
               kartoteket.Remove( item );

               status = true;
            }
         }

         return status;
      }
   }
}
