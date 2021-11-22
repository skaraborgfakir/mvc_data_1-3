//
// Time-stamp: <2021-11-22 18:03:02 stefan>
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
    public class Hemort {
	/// <summary>
	/// to be done
	/// </summary>
	public Hemort( int    id,
		       string namn,
		       Land   land) {
	    Id = id;
	    Namn = namn;
	    Land = land;
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
	[DisplayName("Stadens namn")]
	public string Namn { get; set; }

	/// <summary>
	/// to be done
	/// </summary>
	[Required]
	public Land VilketLand { get; set; }

	/// <summary>
	/// to be done
	/// </summary>
	public List<Person> Boende { get; set; }
    }
}
