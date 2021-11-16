//
// Time-stamp: <2021-11-16 00:27:16 stefan>
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
	/// <summary>
	/// PeopleController.cs hanterar filtrering på så vis
	/// den kontroller filtreringen via session-variablerna.
	/// Nackdel/fördel: PeopleController och PeopleAjaxController har olika URL,
	/// alltså får man slåss med CORS
	/// </summary>
	public Filtreringstermer Termer { get; set; }

	/// <summary>
	/// innehåller listan på de kort som ska synas i vyn (från PeopleController)
	/// används inte i den AJAX-baserade versionen iom den är ersatt av en partial view
	/// som utgår från en lista av personer, för varje person skapas en specfik partial view
	/// </summary>
	public List<Person> Utdraget { get; set; }
    }
}
