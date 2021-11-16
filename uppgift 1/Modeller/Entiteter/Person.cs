//
// Time-stamp: <2021-11-15 16:30:02 stefan>
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
    public class Person {
	/// <summary>
	/// to be done
	/// </summary>
	public Person( int    id,
		       string namn,
		       string bostadsort,
		       string telefonnummer) {
	    Id = id;
	    Namn = namn;
	    Bostadsort = bostadsort;
	    Telefonnummer = telefonnummer;
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
	[DisplayName("Personens namn")]
	public string Namn { get; set; }

	/// <summary>
	/// to be done
	/// </summary>
	[Required]
	[DisplayName("hennes hemort")]
	public string Bostadsort { get; set; }

	/// <summary>
	/// to be done
	/// </summary>
	[Required]
	[DisplayName("telefonnummer")]
	public string Telefonnummer { get; set;}
    }
}
