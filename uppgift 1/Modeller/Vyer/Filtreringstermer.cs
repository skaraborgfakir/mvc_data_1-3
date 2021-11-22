//
// Time-stamp: <2021-11-22 09:50:44 stefan>
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

namespace Kartotek.Modeller.Vyer {
    /// <summary>
    /// Vymodell för sökningstermerna, programmet tillåter sökning på
    /// en valfri term (namn eller hemort)
    ///
    /// TODO:
    /// Något är fel i sökningen så sökning på både namn och hemort ger oväntat resultat
    /// </summary>
    public class Filtreringstermer {
	/// <summary>
	/// sökkriterier i sidhuvudet (används av PeopleController)
	/// private string namn;
	///
	/// används av Shared/Filtrering/Filtreringstermer.cshtml
	/// </summary>
	[BindProperty]
	[DisplayName("personens namn")]
	[DataType(DataType.Text)]
	public string Namn { get; set;
	}
	/// <summary>
	/// sökkriterier i sidhuvudet (används av PeopleController)
	/// string bostadsort;
	/// </summary>
	[BindProperty]
	[DisplayName("bostadsort")]
	[DataType(DataType.Text)]
	public string Bostadsort { get; set; }
    }
}
