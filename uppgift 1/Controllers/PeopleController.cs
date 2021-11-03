//
// Time-stamp: <2021-11-03 15:48:10 stefan>
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http?view=aspnetcore-3.1
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using Kartotek.Modeller;
using Kartotek.Modeller.Interfaces;
using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;

namespace Kartotek.Controllers {
    /// <summary>
    /// ett personkartotek
    ///
    /// kontrollant för ett MVC-baserat program.
    ///
    /// Den kontrollklass som enbart är skriven enligt MVC:konceptet och
    /// där sidan/sidorna i huvudsak formas som partial view - komponentbaserade vyer ?
    /// </summary>
    /// <remark>
    /// kontroller-klassen knyter ihop affärs-/process-logik (serviceenhten) med UI.
    /// En kontroller implementeras utgående från serviceenhetens termer
    /// </remark>
    public class PeopleController : Controller {
	private readonly ILogger<PeopleController> loggdest;
	private readonly IConfiguration configurationsrc;
	private readonly IWebHostEnvironment webHostEnvironment;
	private readonly IPeopleService serviceenheten;

	/// <summary>
	/// kreator för PeopleController
	/// </summary>
	public PeopleController ( ILogger<PeopleController> loggdest,
				  IConfiguration configurationsrc,
				  IWebHostEnvironment webHostEnvironment,
				  IPeopleService serviceenheten ) {
	    this.configurationsrc   = configurationsrc;
	    this.loggdest           = loggdest;
	    this.webHostEnvironment = webHostEnvironment;
	    this.serviceenheten     = serviceenheten;
	}

	/// <summary>
	/// bilden med:
	///   sökformulär
	///   formulär för att addera en person i registret
	///   lista av personer enligt sökkriterier
	/// </summary>
	[HttpGet]
	[ActionName( "Index" )]
	public IActionResult Index ( HopslagenmodellVymodell vymodell ) {
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					 "this.configurationsrc: " + this.configurationsrc["Logging:LogLevel:Default"]);

	    if (HttpContext.Session.GetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st" ) == null) {
		HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 0 );
		HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", "" );
		HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", "" );
	    }

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();
	    nyVymodell.Filtertermer = new PeopleViewModel();

	    switch (HttpContext.Session.GetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st" )) {
		case 1:
		    nyVymodell.Filtertermer.Namn = HttpContext.Session.GetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st" );
		    nyVymodell.Personlistan = this.serviceenheten.FindBy( nyVymodell.Filtertermer );
		    break;
		case 2:
		    nyVymodell.Filtertermer.Bostadsort = HttpContext.Session.GetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st" );
		    nyVymodell.Personlistan = this.serviceenheten.FindBy( nyVymodell.Filtertermer );
		    break;
		default:
		    nyVymodell.Personlistan = this.serviceenheten.All();
		    break;
	    }

	    return View( nyVymodell );
	}

	/// <summary>
	/// modifiera aktiv sökterm
	/// </summary>
	[HttpPost]
	[ActionName("filtrering")]
	public IActionResult Filtrera ( HopslagenmodellVymodell vymodell ) {
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    // HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();
	    // nyVymodell.NyttKort = new CreatePersonViewModel();

	    if (ModelState.IsValid) {
		if (vymodell != null) {
		    this.loggdest.LogInformation( "public IActionResult Filtrera - if ( vymodell != null " );
		    //
		    // hur kommer man hit ? tanken är att man skulle ha sökning efter data på
		    // enbart ett ställe och filtrering då enbart ska ändra kriterierna
		    //
		    // sökkriterier (namn/bostadsort) i session ?
		    //
		    if (vymodell.Filtertermer != null) {
			// nyVymodell.Filtertermer = new PeopleViewModel();

			if (!String.IsNullOrEmpty( vymodell.Filtertermer.Namn )) {
			    HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 1 );
			    HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", vymodell.Filtertermer.Namn );
			    HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", "" );
			} else if (!String.IsNullOrEmpty( vymodell.Filtertermer.Bostadsort )) {
			    HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 2 );
			    HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", "" );
			    HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", vymodell.Filtertermer.Bostadsort );
			}

			if (vymodell.Filtertermer.Namn == null &&
			    vymodell.Filtertermer.Bostadsort == null) {
			    HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 0 );
			    HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", "" );
			    HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", "" );
			}
		    }
		}
	    } else {
		HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 0 );
		HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", "" );
		HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", "" );
	    }

	    return RedirectToAction( "Index" );
	}

	/// <summary>
	/// återställ sökning, måste ange mot serviceenheten förändringen
	/// </summary>
	[HttpPost]
	[ActionName( "ingenfiltrering" )]
	public IActionResult IngenFiltrering ( HopslagenmodellVymodell vymodell ) {
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 0 );
	    HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", "" );
	    HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", "" );

	    return RedirectToAction( "Index" );
	}

	/// <summary>
	/// skicka en CreatePersonViewModel till serviceenheten
	/// </summary>
	[HttpPost]
	[ActionName( "nyttkort" )]
	public IActionResult SkapaNyttKort ( HopslagenmodellVymodell vymodell ) {
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    if (ModelState.IsValid) {
		if (!String.IsNullOrEmpty( vymodell.NyttKort.Namn ) &&
		    !String.IsNullOrEmpty( vymodell.NyttKort.Bostadsort ) &&
		    !String.IsNullOrEmpty( vymodell.NyttKort.Telefonnummer )) {

		    CreatePersonViewModel nyttKort = new CreatePersonViewModel();
		    nyttKort = vymodell.NyttKort;

		    this.serviceenheten.Add( nyttKort );
		}
	    } else {
		this.loggdest.LogInformation( "public IActionResult Skapa_kort( PeopleViewModel not if ( ModelState.IsValid ) {" );
	    }

	    nyVymodell.Personlistan = this.serviceenheten.All();

	    return View( "Index", nyVymodell ); // använder Views/People/Index.cshtml
	    // return RedirectToAction( "Index", nyVymodell);
	}

	/// <summary>
	/// modifiering av ett specifikt kort
	/// </summary>
	[HttpPost]
	[ActionName( "modifiering" )]
	public IActionResult ModifieraKort ( HopslagenmodellVymodell vymodell ) {
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    throw new NotImplementedException( "public IActionResult ModifieraKort( HopslagenmodellVymodell vymodell)" );
	    // return RedirectToAction( "Index", nyVymodell);
	}

	/// <summary>
	/// radering av item i listan
	/// </summary>
	[HttpGet]
	[ActionName( "radering" )]
	public IActionResult TagBortKort ( int id ) {
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    this.serviceenheten.Remove( id );
	    nyVymodell.Personlistan = serviceenheten.All();

	    // throw new NotImplementedException( "public IActionResult TagBortKort( int id)");
	    return View( "Index", nyVymodell ); // använder Views/People/Index.cshtml
	}
    }
}
