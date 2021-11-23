//
// Time-stamp: <2021-11-23 10:05:43 stefan>
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
    public class Språk {
	/// <summary>
	/// to be done
	/// </summary>
	public Språk( int    id,
		      string namn) {
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
	[StringLength(60,MinimumLength=4)]
	[DisplayName("Personens namn")]
	public string Namn { get; set; }

	/// <summary>
	/// to be done
	/// </summary>
	[Required]
	[DisplayName("Hennes hemort")]
	public Hemort Bostadsort { get; set; }

	/// <summary>
	/// to be done
	/// </summary>
	[Required]
	[DisplayName("Telefonnummer")]
	public string Telefonnummer { get; set;}

	/// <summary>
	/// vilka har ett visst språk som aktivt dvs
	/// har vissa kunskaper i det ?
	/// </summary>
	public List<Person> användare;
    }
}
