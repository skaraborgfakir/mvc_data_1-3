//
// Time-stamp: <2021-09-20 16:13:18 stefan>
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
using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;

namespace Kartotek.Controllers {
    public class PeopleController : Controller {
	private readonly ILogger<PeopleController> _loggdest;
	private readonly IConfiguration  _configurationsrc;

	public PeopleController( ILogger<PeopleController> loggdest,
				 IConfiguration  configurationsrc) {
	    _configurationsrc=configurationsrc;
	    _loggdest=loggdest;
	}

	//
	// sökning efter enbart vissa poster ?
	//
	// påverkar Index-funktionen
	//
	enum Sökläge { Ingen, Namn, Bostadsort};
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
	[ActionName("Index")]
	public IActionResult Index( HopslagenmodellVymodell vymodell) {
	    Console.WriteLine( "public IActionResult Index");

	    //_loggdest.Test();
	    // Console.WriteLine( "public IActionResult Index - sökterm: " + sökterm);
	    // Console.WriteLine( "public IActionResult Index - sökstatus: " + sökstatus.ToString() );

	    Sökläge sökstatus = Sökläge.Ingen;
	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    //
	    // eventuell sökning ?
	    //
	    if ( vymodell == null ) {
		//
		// eventuellt första anrop
		//
		Console.WriteLine( "public IActionResult Index - if ( vymodell == null");
	    } else {
		if ( vymodell.Filtertermer == null) {
		    //
		    // okänd filtrering
		    //
		    Console.WriteLine( "if ( vymodell.Filtertermer == null)");
		} else {
		    nyVymodell.Filtertermer = new PeopleViewModell();

		    if ( ! String.IsNullOrEmpty( vymodell.Filtertermer.Namn)) {
			Console.WriteLine( "public IActionResult Index - sökning på namn");
			Console.WriteLine( "public IActionResult Index - if ( " + vymodell.Filtertermer.Namn);
			sökstatus = Sökläge.Namn;
			nyVymodell.Filtertermer.Namn = vymodell.Filtertermer.Namn;
			nyVymodell.Filtertermer.Bostadsort= "";
		    } else if ( ! String.IsNullOrEmpty( vymodell.Filtertermer.Bostadsort)) {
			Console.WriteLine( "public IActionResult Index - bostadsort");
			Console.WriteLine( "public IActionResult Index - if ( " + vymodell.Filtertermer.Bostadsort);
			sökstatus = Sökläge.Bostadsort;
			nyVymodell.Filtertermer.Namn = "";
			nyVymodell.Filtertermer.Bostadsort = vymodell.Filtertermer.Bostadsort;
		    }
		}
	    }

	    switch (sökstatus) {
		case Sökläge.Namn:
		    Console.WriteLine( "public IActionResult Index sålla på namn");
		    nyVymodell.Personlistan = serviceenheten.FindBy( nyVymodell.Filtertermer);
		    break;

		case Sökläge.Bostadsort:
		    Console.WriteLine( "public IActionResult Index sålla på bostadsort");
		    nyVymodell.Personlistan = serviceenheten.FindBy( nyVymodell.Filtertermer);
		    break;

		default:
		    Console.WriteLine( "public IActionResult Index ingen sållning");
		    nyVymodell.Personlistan = serviceenheten.All();
		    break;
	    }

	    return View( nyVymodell);
	}

	//
	// modifiera aktiv sökterm
	//
	[HttpPost]
	[ActionName("filtrering")]
	public IActionResult Filtrera( HopslagenmodellVymodell vymodell) {
	    Console.WriteLine( "public IActionResult Filtrera( PeopleViewModell");

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();
	    nyVymodell.NyttKort = new CreatePersonViewModell();

	    if ( ModelState.IsValid ) {
		if ( vymodell == null ) {
		    Console.WriteLine( "public IActionResult Filtrera vymodell == null");
		} else {
		    //
		    // hur kommer man hit ? tanken är att man skulle ha sökning efter data på
		    // enbart ett ställe och filtrering då enbart ska ändra kriterierna
		    //
		    // sökkriterier (namn/bostadsort) i session ?
		    //
		    if ( vymodell.Filtertermer != null ) {
			nyVymodell.Filtertermer = new PeopleViewModell();

			if ( ! String.IsNullOrEmpty( vymodell.Filtertermer.Namn)) {
			    Console.WriteLine( "public IActionResult Filtrera på namn -- " + vymodell.Filtertermer.Namn + " --");
			    nyVymodell.Filtertermer.Namn = vymodell.Filtertermer.Namn;
			    nyVymodell.Filtertermer.Bostadsort = "";
			} else if ( ! String.IsNullOrEmpty( vymodell.Filtertermer.Bostadsort)) {
			    Console.WriteLine( "public IActionResult Filtrera på bostadsort -- " + vymodell.Filtertermer.Bostadsort + " --");
			    nyVymodell.Filtertermer.Namn = "";
			    nyVymodell.Filtertermer.Bostadsort = vymodell.Filtertermer.Bostadsort;
			}

			nyVymodell.Personlistan = serviceenheten.FindBy( nyVymodell.Filtertermer);
		    } else {
			nyVymodell.Personlistan = serviceenheten.All();
		    }
		}
	    }

	    return View( "Index", nyVymodell); // använder Views/People/Index.cshtml
	}

	//
	// skicka en CreatePersonViewModel till serviceenheten
	//
	[HttpPost]
	[ActionName("nyttkort")]
	public IActionResult SkapaNyttKort( HopslagenmodellVymodell vymodell) {
	    Console.WriteLine( "public IActionResult Skapa_kort( PeopleViewModell");

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    if ( ModelState.IsValid ) {
		Console.WriteLine( "public IActionResult Skapa_kort( PeopleViewModell if ( ModelState.IsValid ) {");
		if ( ! String.IsNullOrEmpty(vymodell.NyttKort.Namn) &&
		     ! String.IsNullOrEmpty(vymodell.NyttKort.Bostadsort) &&
		     ! String.IsNullOrEmpty(vymodell.NyttKort.Telefonnummer)) {
		    Console.WriteLine( "  " + vymodell.NyttKort.Namn);
		    Console.WriteLine( "  " + vymodell.NyttKort.Bostadsort);
		    Console.WriteLine( "  " + vymodell.NyttKort.Telefonnummer);

		    CreatePersonViewModell nyttKort = new CreatePersonViewModell();
		    nyttKort = vymodell.NyttKort;
		    // nyttKort.Namn = vymodell.NyttKortNyttKortNamn;
		    // nyttKort.Bostadsort = vymodell.NyttKortBostadsort;
		    // nyttKort.Telefonnummer = vymodell.NyttKortTelefonnummer;

		    serviceenheten.Add( nyttKort);
		}
	    } else {
		Console.WriteLine( "public IActionResult Skapa_kort( PeopleViewModell not if ( ModelState.IsValid ) {");
	    }

	    nyVymodell.Personlistan = serviceenheten.All();

	    return View( "Index", nyVymodell); // använder Views/People/Index.cshtml
	    // return RedirectToAction( "Index", nyVymodell);
	}

	//
	// modifiering
	//
	[HttpPost]
	[ActionName("modifiering")]
	public IActionResult ModifieraKort( HopslagenmodellVymodell vymodell) {
	    Console.WriteLine( "public IActionResult ModifieraKort( HopslagenmodellVymodell");

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    throw new NotImplementedException( "public IActionResult ModifieraKort( HopslagenmodellVymodell vymodell)");
	    // return RedirectToAction( "Index", nyVymodell);
	}

	//
	// radering av item i listan
	//
	// [HttpGet]
	// [ActionName("radering")]
	// public IActionResult TagBortKort( HopslagenmodellVymodell vymodell) {
	//     Console.WriteLine( "public IActionResult TagBortKort( HopslagenmodellVymodell");

	//     HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();


	//     throw new NotImplementedException( "public IActionResult TagBortKort( HopslagenmodellVymodell vymodell)");
	//     // return RedirectToAction( "Index", nyVymodell);

	//     // nyVymodell.Personlistan = serviceenheten.All();

	//     // return View( "Index", nyVymodell); // använder Views/People/Index.cshtml
	// }

	//
	// radering av item i listan
	//
	[HttpGet]
	[ActionName("radering")]
	public IActionResult TagBortKort( int id) {
	    Console.WriteLine( "public IActionResult TagBortKort( int id");
	    Console.WriteLine( "public IActionResult TagBortKort id = " + id.ToString());

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    serviceenheten.Remove( id);
	    nyVymodell.Personlistan = serviceenheten.All();

	    // throw new NotImplementedException( "public IActionResult TagBortKort( int id)");
	    return View( "Index", nyVymodell); // använder Views/People/Index.cshtml
	}
    }
}
