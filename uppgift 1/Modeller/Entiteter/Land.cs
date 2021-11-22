//
// Time-stamp: <2021-11-22 18:01:48 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kartotek.Modeller.Entiteter {
    /// <summary>
    /// to be done
    /// </summary>
    public class Land {
	/// <summary>
	/// to be done
	/// </summary>
	public Land( int    id,
		     string namn) {
	    Id = id;
	    Namn = namn;
	}

	/// <summary>
	/// to be done
	/// </summary>
	[Required]
	public int Id { get; set; }

	/// <summary>
	/// to be done
	/// </summary>
	[Required]
	[StringLength(60,MinimumLength=4)]
	[DisplayName("Landets namn")]
	public string Namn { get; set; }

	/// <summary>
	/// städer i landet i fråga
	/// </summary>
	[Required]
	[DisplayName("Städer i landet")]
	public List<Hemort> StäderILandet { get; set; }
    }
}
