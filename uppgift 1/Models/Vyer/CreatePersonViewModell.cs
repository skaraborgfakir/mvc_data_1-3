// Time-stamp: <2021-09-20 14:04:52 stefan>
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

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Kartotek.Modeller.Vyer {
    public class CreatePersonViewModell {
	// private string namn;

	[BindProperty]
	[StringLength(60,MinimumLength=8)]
	[DisplayName("personens namn")]
	public string Namn { get; set; }

	// private string bostadsort;
	[BindProperty]
	[StringLength(60,MinimumLength=2)]
	[DisplayName("bostadsort")]
	public string Bostadsort { get; set; }

	//	private string telefonnummer;
	[BindProperty]
	[DisplayName("telefonnummer")]
	[DataType(DataType.PhoneNumber)]
	public string Telefonnummer {
	    get;// { return telefonnummer; }
	    set;// { telefonnummer = value; }
	}
    }
}
