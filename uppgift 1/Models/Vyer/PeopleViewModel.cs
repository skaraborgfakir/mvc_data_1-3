//
// Time-stamp: <2021-11-26 14:44:02 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using Kartotek.Modeller.Entiteter;

namespace Kartotek.Modeller.Vyer {
    /// <summary>
    /// to be done
    /// </summary>
    public class PeopleViewModel {
	// <summary>
	// PeopleController.cs hanterar filtrering på så vis
	// den kontroller filtreringen via session-variablerna.
	// Nackdel/fördel: PeopleController och PeopleAjaxController har olika URL,
	// alltså får man slåss med CORS
	// </summary>

	/// <summary>
	/// sökkriterier i sidhuvudet (används av PeopleController)
	/// private string namn;
	///
	/// används av Shared/Filtrering/Filtreringstermer.cshtml
	/// </summary>
	[BindProperty]
	[StringLength(60,MinimumLength=4)]
	[DisplayName("Personens namn")]
	[DataType(DataType.Text)]
	public string Namn { get; set;
	}

	/// <summary>
	/// sökkriterier i sidhuvudet (används av PeopleController)
	/// string bostadsort;
	/// </summary>
	[BindProperty]
	[StringLength(60,MinimumLength=2)]
	[DisplayName("Hennes hemort")]
	[DataType(DataType.Text)]
	public string Bostadsort { get; set; }

	/// <summary>
	/// innehåller listan på de kort som ska synas i vyn (från PeopleController)
	/// används inte i den AJAX-baserade versionen iom den är ersatt av en partial view
	/// som utgår från en lista av personer, för varje person skapas en specfik partial view
	/// </summary>
	public List<Person> Utdraget { get; set; }
    }
}
