//
// Time-stamp: <2021-11-22 01:23:03 stefan>
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
	    this.loggdest = loggdest;
	    this.configurationsrc   = configurationsrc;
	    this.webHostEnvironment = webHostEnvironment;
	    this.serviceenheten     = serviceenheten;

	    this.loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) +
					  " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) + "\n" +
					  "this.configurationsrc: " + this.configurationsrc["session_kakans_namn"]);

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
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    if ( (HttpContext.Session.GetString( $"namn.{this.sessionsuffix}" ) == null) ||
		 (HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}" ) == null)) {
		HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 0 );
		HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", "" );
		HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", "" );
	    }

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();
	    nyVymodell.InskrivningNyttKort = new CreatePersonViewModel();
	    PeopleViewModel filter = new PeopleViewModel();

	    switch (HttpContext.Session.GetInt32( $"valdterm.{this.sessionsuffix}" )) {
		case 3:
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		    filter.Namn =       HttpContext.Session.GetString( $"namn.{this.sessionsuffix}" );
		    filter.Bostadsort = HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}" );
		    nyVymodell.Listfiltrering = filter;
		    nyVymodell.Personlistan = this.serviceenheten.FindBy( filter );
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		    break;

		case 2:
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		    filter.Bostadsort = HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}" );
		    nyVymodell.Listfiltrering = filter;
		    nyVymodell.Personlistan = this.serviceenheten.FindBy( filter );
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));
		    break;

		case 1:
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		    filter.Namn = HttpContext.Session.GetString( $"namn.{this.sessionsuffix}" );
		    nyVymodell.Listfiltrering = filter;
		    nyVymodell.Personlistan = this.serviceenheten.FindBy( filter );
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));
		    break;

		default:
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		    nyVymodell.Listfiltrering = filter;
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));
		    nyVymodell.Personlistan = this.serviceenheten.All();
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

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
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		    //
		    // hur kommer man hit ? tanken är att man skulle ha sökning efter data på
		    // enbart ett ställe och att filtrering då enbart ska ändra kriterierna
		    //
		    // sökkriterier (namn/bostadsort) i session ?
		    //
		    if ( vymodell.Listfiltrering != null) {
			this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
						     " namn : " + vymodell.Listfiltrering.Namn +
						     " bostadsort " + vymodell.Listfiltrering.Bostadsort);

			// båda sökvillkoren satta
			if ( !String.IsNullOrEmpty( vymodell.Listfiltrering.Namn ) &&
			     !String.IsNullOrEmpty( vymodell.Listfiltrering.Bostadsort ))
			{
			    //
			    // båda sökvillkoren satta
			    //
			    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
							 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
							 " namn : " + vymodell.Listfiltrering.Namn +
							 " bostadsort : " + vymodell.Listfiltrering.Bostadsort );

			    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 3 );
			    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}",       vymodell.Listfiltrering.Namn );
			    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", vymodell.Listfiltrering.Bostadsort );
			}
			else if ( !String.IsNullOrEmpty( vymodell.Listfiltrering.Namn ))
			{
			    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
							 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
							 " namn : " + vymodell.Listfiltrering.Namn );

			    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 1 );
			    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", vymodell.Listfiltrering.Namn );
			    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", "" );
			}
			else if (!String.IsNullOrEmpty( vymodell.Listfiltrering.Bostadsort ))
			{
			    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
							 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
							 " Bostadsort : " + vymodell.Listfiltrering.Bostadsort );

			    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 2 );
			    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", "" );
			    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", vymodell.Listfiltrering.Bostadsort );
			}

			if (vymodell.Listfiltrering.Namn == null &&
			    vymodell.Listfiltrering.Bostadsort == null) {
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
	/// återställ sökning, måste ange mot serviceenheten förändringen
	/// </summary>
	[HttpPost]
	[ActionName( "ingenfiltrering" )]
	public IActionResult IngenFiltrering ( HopslagenmodellVymodell vymodell ) {
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

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
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    if ( ( vymodell.InskrivningNyttKort != null) &&
		 ( vymodell.InskrivningNyttKort.Namn != null))
		this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					     "\n" + " namn " + vymodell.InskrivningNyttKort.Namn +
					     "\n" + " Bostadsort " + vymodell.InskrivningNyttKort.Bostadsort  +
					     "\n" + " Telefonnummer " + vymodell.InskrivningNyttKort.Telefonnummer);

	    if (ModelState.IsValid) {
		this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					     "\n" + " model is valid");

		if (vymodell.InskrivningNyttKort != null) {
		    if (!String.IsNullOrEmpty( vymodell.InskrivningNyttKort.Namn ) &&
			!String.IsNullOrEmpty( vymodell.InskrivningNyttKort.Bostadsort ) &&
			!String.IsNullOrEmpty( vymodell.InskrivningNyttKort.Telefonnummer )) {

			this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
						     "\n" + "alla uppgifter finns med");

			// CreatePersonViewModel nyttKort = new CreatePersonViewModel();
			// nyttKort = vymodell.InskrivningNyttKort;

			this.serviceenheten.Add( vymodell.InskrivningNyttKort );
		    }
		}
		else
		{
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
						 "\n" + "Upppgifter sakns");
		}
	    } else {
		this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					     "\n" + "modell NOT valid");
	    }

	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));
	    nyVymodell.Personlistan = this.serviceenheten.All();
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    // return View( "Index", nyVymodell ); // använder Views/People/Index.cshtml
	    // return RedirectToAction( "Index", nyVymodell);
	    return RedirectToAction( "Index");
	}

	/// <summary>
	/// modifiering av ett specifikt kort
	/// använder modifieringsfunktionen i kortlistan
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
	/// kasering
	/// använder kaseringsfunktionen i kortlistan
	/// </summary>
	[HttpGet]
	[ActionName( "radering" )]
	public IActionResult TagBortKort ( int id ) {
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    this.serviceenheten.Remove( id );

	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));
	    nyVymodell.Personlistan = serviceenheten.All();
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    // throw new NotImplementedException( "public IActionResult TagBortKort( int id)");
	    return View( "Index", nyVymodell ); // använder Views/People/Index.cshtml
	}
    }
}
