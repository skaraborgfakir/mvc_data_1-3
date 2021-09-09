// Time-stamp: <2021-09-08 22:56:13 stefan>
//

//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

//
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using Kartotek.Modeller;

namespace Kartotek.Controllers {
    public class People : Controller {
	private readonly ILogger<People> _loggdest;
	private readonly IConfiguration  _configurationsrc;
	private static InMemoryPeopleRepo repo = new InMemoryPeopleRepo();

	public People( ILogger<People> loggdest,
		       IConfiguration  configurationsrc) {
	    _configurationsrc=configurationsrc;
	    _loggdest=loggdest;
	}

	//
	// bilden med:
	//   sökformulär
	//   formulär för att addera en person i registret
	//   lista av personer enligt sökkriterier
	//
	[HttpGet]
	public IActionResult Index() {
	    Console.WriteLine( "count "+ repo.Read().Count.ToString() );

	    PeopleViewModell vyn = new PeopleViewModell( repo.Read());

	    return View( vyn);
	}

	[HttpPost]
	public Filtrera( PeopleViewModell filtertermer) {
	}

	[HttpPost]
	public Skapa_kort( PeopleViewModell filtertermer) {
	}
    }
}
