//
// Time-stamp: <2021-11-06 16:31:59 stefan>
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
	/// innehåller listan på de kort som ska synas i vyn
	/// </summary>
	public List<Person> Utdraget {
	    get;
	    set;
	}

	/// <summary>
	/// sökkriterier
	/// private string namn;
	/// </summary>
	[BindProperty]
	[DisplayName("personens namn")]
	[DataType(DataType.Text)]
	public string Namn {
	    get;
	    set;
	}
	/// <summary>
	/// sökkriterier
	/// string bostadsort;
	/// </summary>
	[BindProperty]
	[DisplayName("bostadsort")]
	[DataType(DataType.Text)]
	public string Bostadsort {
	    get;
	    set;
	}
    }
}
