// Time-stamp: <2021-09-10 10:09:17 stefan>
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

	public People( ILogger<People> loggdest,
		       IConfiguration  configurationsrc) {
	    _configurationsrc=configurationsrc;
	    _loggdest=loggdest;
	}

	//
	// kontroller-klassen knyter ihop affärs-/process-logik (serviceenhten) med UI
	//
	// kontroller implementeras utgående från serviceenhetens termer
	//
	private PeopleService serviceenheten = new PeopleService();

	//
	// bilden med:
	//   sökformulär
	//   formulär för att addera en person i registret
	//   lista av personer enligt sökkriterier
	//
	[HttpGet]
	public IActionResult Index() {
	    // Console.WriteLine( "count "+ serviceEnhet.Read().Count.ToString() );

	    return View( serviceenheten.All());
	}

	//
	// skicka söktermer till serviceenheten i form av en PeopleViewModell
	// och vidarebefordra till formuläret som en sådan
	//
	[HttpPost]
	public IActionResult Filtrera( PeopleViewModell filtertermer) {
	    Console.WriteLine( "public IActionResult Filtrera( PeopleViewModell");

	    return RedirectToAction("Index", serviceenheten.FindBy(filtertermer));
	}

	//
	// skicka en CreatePersonViewModel till serviceenheten
	//
	[HttpPost]
	public IActionResult Skapa_kort( CreatePersonViewModel nyttKort) {
	    Console.WriteLine( "public IActionResult Skapa_kort( PeopleViewModell");

	    serviceenheten.Add( nyttKort);

	    return RedirectToAction("Index");
	}
    }
}
