// Time-stamp: <2021-11-02 14:43:07 stefan>
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
