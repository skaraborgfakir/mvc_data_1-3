//
// Time-stamp: <2021-11-03 15:40:52 stefan>
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
    /// <summary>
    /// to be done
    /// </summary>
    public class CreatePersonViewModel {
	/// <summary>
	/// Namnet på den person som det nya kortet upptar
	/// </summary>
	[BindProperty]
	[StringLength(60,MinimumLength=8)]
	[DisplayName("personens namn")]
	public string Namn { get; set; }

	/// <summary>
	/// Personens bostadsort
	/// </summary>
	[BindProperty]
	[StringLength(60,MinimumLength=2)]
	[DisplayName("bostadsort")]
	public string Bostadsort { get; set; }

	/// <summary>
	/// Personens telefonnummer - kontaktuppgifter
	/// </summary>
	[BindProperty]
	[DisplayName("telefonnummer")]
	[DataType(DataType.PhoneNumber)]
	public string Telefonnummer {
	    get;// { return telefonnummer; }
	    set;// { telefonnummer = value; }
	}
    }
}
