//
// Time-stamp: <2021-09-20 09:44:25 stefan>
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using Kartotek.Modeller.Entiteter;

namespace Kartotek.Modeller.Vyer {
    public class PeopleViewModell {
	//
	// listan av personer
	//
	// public List<Person> utdraget;
	// public List<Tuple<int,Person>> utdrag;
	public List<Person> Utdraget {
	    get;
	    set;
	}

	//
	// sökkriterier
	//
	// private string namn;
	[BindProperty]
	[DisplayName("personens namn")]
	[DataType(DataType.Text)]
	public string Namn {
	    get;
	    set;
	}
	// private string bostadsort;
	[BindProperty]
	[DisplayName("bostadsort")]
	[DataType(DataType.Text)]
	public string Bostadsort {
	    get;
	    set;
	}
    }
}
