// Time-stamp: <2021-11-06 16:32:01 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Kartotek.Modeller.Vyer
{
    /// <summary>
    /// to be done
    /// </summary>
    public class AktionSpecifiktkort
    {
	/// <summary>
	/// to be done
	/// </summary>
	[BindProperty]
	public int KortetsId { get; set; }
    }
}
