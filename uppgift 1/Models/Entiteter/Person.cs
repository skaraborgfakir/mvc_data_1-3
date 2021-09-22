using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kartotek.Modeller.Entiteter {
    public class Person {
	public Person( int    id,
		       string namn,
		       string bostadsort,
		       string telefonnummer) {
	    Id = id;
	    Namn = namn;
	    Bostadsort = bostadsort;
	    Telefonnummer = telefonnummer;
	}

	public int Id { get; }

	[DisplayName("Personens namn")]
	public string Namn { get; set; }

	[DisplayName("hennes hemort")]
	public string Bostadsort { get; set; }

	[DisplayName("telefonnummer")]
	public string Telefonnummer { get; set;}
    }
}
