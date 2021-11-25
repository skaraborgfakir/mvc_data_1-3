//
// Time-stamp: <2021-11-25 13:15:53 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
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
	private readonly string sessionsuffix;

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

	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					 "\n" + "this.configurationsrc: " + this.configurationsrc["session_kakans_namn"]);

	    this.sessionsuffix=this.configurationsrc["session_kakans_namn"];
	}

	/// <summary>
	/// bilden med:
	///   sökformulär
	///   formulär för att addera en person i registret
	///   enbart en platsmarkör för (i AJAX:versionen)
	///       en sökväljare
	///       detaljer för ett visst kort
	///       listan av personer (vymodell.utdraget)
	///   Index körs direkt vid första visning- alltså kommer
	///   valdterm.{this.sessionsuffix}
	/// </summary>
	[HttpGet]
	[ActionName( "Index" )]
	public IActionResult Index ( HopslagenmodellVymodell vymodell )
	{
	    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
	    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
	    //				 "\n" + "this.configurationsrc: " + this.configurationsrc["app_run_miljö"]);

	    if ( (HttpContext.Session.GetString( $"namn.{this.sessionsuffix}" ) == null) ||
		 (HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}" ) == null)) {
		HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 0 );
		HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", "" );
		HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", "" );
	    }

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();
	    nyVymodell.Filtertermer = new PeopleViewModel();

	    switch (HttpContext.Session.GetInt32( $"valdterm.{this.sessionsuffix}" )) {
		case 1:
		    nyVymodell.Filtertermer.Namn = HttpContext.Session.GetString( $"namn.{this.sessionsuffix}" );
		    nyVymodell.Personlistan = this.serviceenheten.FindBy( nyVymodell.Filtertermer );
		    break;

		case 2:
		    nyVymodell.Filtertermer.Bostadsort = HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}" );
		    nyVymodell.Personlistan = this.serviceenheten.FindBy( nyVymodell.Filtertermer );
		    break;

		case 3:
		    nyVymodell.Filtertermer.Namn = HttpContext.Session.GetString( $"namn.{this.sessionsuffix}" );
		    nyVymodell.Filtertermer.Bostadsort = HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}" );
		    nyVymodell.Personlistan = this.serviceenheten.FindBy( nyVymodell.Filtertermer );
		    break;

		default:
		    nyVymodell.Personlistan = this.serviceenheten.All();
		    break;
	    }

	    // foreach (Person person in nyVymodell.Personlistan.Utdraget) {
	    //	this.loggdest.LogInformation(
	    //	    "\n" + "Namn : " + person.Namn + " Bostadsort : " + person.Bostadsort);
	    // }

	    return View( nyVymodell );
	}

	/// <summary>
	/// modifiera aktiv sökterm
	/// </summary>
	[HttpPost]
	[ActionName("filtrering")]
	public IActionResult Filtrera ( HopslagenmodellVymodell vymodell ) {
	    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
	    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    if (ModelState.IsValid) {
		if (vymodell != null) {
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		    //
		    // hur kommer man hit ? tanken är att man skulle ha sökning efter data på
		    // enbart ett ställe och filtrering då enbart ska ändra kriterierna
		    //
		    // sökkriterier (namn/bostadsort) i session ?
		    //
		    if (vymodell.Filtertermer != null) {
			this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
						     " namn : " + vymodell.Filtertermer.Namn +
						     " bostadsort " + vymodell.Filtertermer.Bostadsort);

			// nyVymodell.Filtertermer = new PeopleViewModel();

			if ( !String.IsNullOrEmpty( vymodell.Filtertermer.Namn ) &&
			     !String.IsNullOrEmpty( vymodell.Filtertermer.Bostadsort )) {
			    // båda termerna satta

			    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 3 );
			    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", vymodell.Filtertermer.Namn );
			    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", vymodell.Filtertermer.Bostadsort );
			}
			else if (!String.IsNullOrEmpty( vymodell.Filtertermer.Namn )) {
			    // filtrera enbart på namn

			    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 1 );
			    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", vymodell.Filtertermer.Namn );
			    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", "" );
			}
			else if (!String.IsNullOrEmpty( vymodell.Filtertermer.Bostadsort )) {
			    // filtrera enbart på bostadsort

			    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 2 );
			    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", "" );
			    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", vymodell.Filtertermer.Bostadsort );
			}

			if (vymodell.Filtertermer.Namn == null &&
			    vymodell.Filtertermer.Bostadsort == null) {
			    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 0 );
			    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", "" );
			    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", "" );
			}
		    }
		}
	    } else {
		HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 0 );
		HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", "" );
		HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", "" );
	    }

	    return RedirectToAction( "Index" );
	}

	/// <summary>
	/// återställ sökning, inställningen exporteras mot andra kontrollanter
	/// via sessionsvariablerna
	/// </summary>
	[HttpPost]
	[ActionName( "ingenfiltrering" )]
	public IActionResult IngenFiltrering ( HopslagenmodellVymodell vymodell ) {
	    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 0 );
	    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", "" );
	    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", "" );

	    return RedirectToAction( "Index" );
	}

	/// <summary>
	/// skicka en CreatePersonViewModel till serviceenheten
	/// </summary>
	[HttpPost]
	[ActionName( "nyttkort" )]
	public IActionResult SkapaNyttKort ( HopslagenmodellVymodell vymodell ) {
	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    if (ModelState.IsValid) {
		// this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
		//			     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
		//			     "\n" + " model is valid");
		if (!String.IsNullOrEmpty( vymodell.InskrivningNyttKort.Namn ) &&
		    !String.IsNullOrEmpty( vymodell.InskrivningNyttKort.Bostadsort ) &&
		    !String.IsNullOrEmpty( vymodell.InskrivningNyttKort.Telefonnummer )) {

		    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
		    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
		    //				 "\n" + "alla uppgifter finns med");

		    CreatePersonViewModel nyttKort = new CreatePersonViewModel();
		    nyttKort = vymodell.InskrivningNyttKort;

		    this.serviceenheten.Add( nyttKort );
		}
	    } else {
		this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					     "\n" + "modell NOT valid");
	    }

	    nyVymodell.Personlistan = this.serviceenheten.All();

	    return View( "Index", nyVymodell ); // använder Views/People/Index.cshtml
	    // return RedirectToAction( "Index", nyVymodell);
	    // return RedirectToAction( "Index");
	}

	/// <summary>
	/// modifiering av ett specifikt kort
	/// </summary>
	[HttpPost]
	[ActionName( "modifiering" )]
	public IActionResult ModifieraKort ( HopslagenmodellVymodell vymodell ) {
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
	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    this.serviceenheten.Remove( id );
	    nyVymodell.Personlistan = serviceenheten.All();

	    // throw new NotImplementedException( "public IActionResult TagBortKort( int id)");
	    return View( "Index", nyVymodell ); // använder Views/People/Index.cshtml
	}
    }
}
