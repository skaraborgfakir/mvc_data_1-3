// Time-stamp: <2021-09-08 20:49:24 stefan>
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace Kartotek.Modeller {
    public class InMemoryPeopleRepo : IPeopleRepo
    {
	private class Item
	{
	    Person personen;
	    int    id;

	    public Item( int id,
			 Person person)
	    {
		Personen = person;
		Id = id;
	    }
	    public int Id
	    {
		get { return id; }
		set { id = value; }
	    }
	    public Person Personen
	    {
		get { return personen; }
		set { personen = value; }
	    }
	}

	private static List<Item> personregister = new List<Item>() ;
	private static int idCounter; // nästa tillgänglig id. Används som nyckel in i
				      // personregistret

	public InMemoryPeopleRepo() {
	    this.Create( namn: "Ulf Smedbo",
			 bostadsort: "Göteborg",
			 telefon: "031");
	    this.Create( namn: "Bengt Ulfsson",
			 bostadsort: "Växjö",
			 telefon: "044");
	    this.Create( namn: "Micke Carlsson",
			 bostadsort: "Högsby",
			 telefon: "0321");
	    this.Create( namn: "Ulf Bengtsson",
			 bostadsort: "Växjo",
			 telefon: "044");
	    this.Create( namn: "Simon Josefsson",
			 bostadsort: "Skövde",
			 telefon: "0500");
	    this.Create( namn: "Jonathan Krall",
			 bostadsort: "Stenstorp",
			 telefon: "0500");
	}

	public Person Create( string namn,
			      string bostadsort,
			      string telefon)
	{
	    personregister.Add( new Item( id: idCounter++,
					  person: new Person( namn: namn,
							      bostadsort: bostadsort,
							      telefon: telefon)));
	    return Read( idCounter-1);
	}

	//
	// används från vyn
	//
	// alla personer i listan
	//
	public List<Person> Read()
	{
	    List<Person> sökresultat = new List<Person>();

	    foreach (Item item in personregister)
	    {
		sökresultat.Add( item.Personen);
	    }

	    return sökresultat;
	}

	//
	// används från vyn
	//
	public Person Read( int id)
	{
	    foreach (Item item in personregister)
	    {
		if ( item.Id == id)
		{
		    return item.Personen;
		}
	    }

	    // Not developed yet.
	    throw new NotImplementedException();
	}

	//
	// hur ??
	//
	public Person Update( Person person) {
	    // Not developed yet.
	    throw new NotImplementedException();
	}

	//
	// aktiveras från delete i vyn
	//
	public bool Delete( Person personen)
	{
	    bool status = false;

	    // använd en iterator som räknas ner
	    //
	    // fördelen är att remove kommer enbart påverka tupler
	    // som vi redan har passerat, annars blir det en höna-och-ägget:situation
	    //
	    foreach (Item item in personregister.Reverse<Item>() ) {
		if ( item.Personen == personen) {
		    personregister.Remove( item);

		    status=true;
		}
	    }

	    return status;
	}
    }
}
