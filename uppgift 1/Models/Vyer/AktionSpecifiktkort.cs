// Time-stamp: <2021-09-20 14:04:52 stefan>
//

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

namespace Kartotek.Modeller.Vyer
{
    public class AktionSpecifiktkort
    {
        [BindProperty]
        public int KortetsId { get; set; }                
    }
}
