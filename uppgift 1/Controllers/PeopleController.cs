//
// Time-stamp: <2021-11-28 15:02:29 stefan>
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
    /// <remarks>
    /// kontroller-klassen knyter ihop affärs-/process-logik (serviceenhten) med UI.
    /// En kontroller implementeras utgående från serviceenhetens termer
    /// </remarks>
    public class PeopleController : Controller {
	private readonly ILogger<PeopleController> loggdest;
	private readonly IConfiguration configuration;
	private readonly IWebHostEnvironment webHostEnvironment;
	private readonly IPeopleService serviceenheten;
	private readonly string sessionsuffix;

	/// <summary>
	/// kreator för PeopleController
	/// </summary>
	/// <param name="loggdest">Ympning med loggfunktionen</param>
	/// <param name="configuration">Ympning av IConfiguration</param>
	/// <param name="webHostEnvironment">Ympning av IWebHostEnvironment</param>
	/// <param name="serviceenheten">Ympning av (I)PeopleService</param>
	public PeopleController ( ILogger<PeopleController> loggdest,
				  IConfiguration configuration,
				  IWebHostEnvironment webHostEnvironment,
				  IPeopleService serviceenheten ) {
	    this.loggdest = loggdest;
	    this.configuration   = configuration;
	    this.webHostEnvironment = webHostEnvironment;
	    this.serviceenheten     = serviceenheten;

	    this.loggdest.LogInformation( "metod : " + (new System.Diagnostics.StackFrame(0, true).GetMethod()) +
					  " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) + "\n" +
					  "this.configuration: " + this.configuration["session_kakans_namn"]);

	    this.sessionsuffix=this.configuration["session_kakans_namn"];
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
	///
	/// används inte i AJAX:versionen iom att ajax:versionen av kontrollanten ersätter denna
	/// </summary>
	/// <remarks>
	/// Funktionen är en aktör i .Net därför att den är :
	///      en metod
	///      allmänt tillgänglig (public)
	/// <seealso href="https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1#action">
	/// Routing to controller actions in ASP.Net Core
	/// </seealso>
	/// </remarks>
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

	    return View( nyVymodell );
	}

	/// <summary>
	/// modifiera aktiv sökterm
	/// aktiveras av submit (filtrering) i Views/People/filtrering/dialog.cshtml
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
		    // enbart ett ställe och att filtrering då enbart ska ändra kriterierna
		    //
		    // sökkriterier (namn/bostadsort) i session ?
		    //
		    if ( vymodell.Filtertermer != null) {
			this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
						     " namn : " + vymodell.Filtertermer.Namn +
						     " bostadsort " + vymodell.Filtertermer.Bostadsort);

			if ( !String.IsNullOrEmpty( vymodell.Filtertermer.Namn ) &&
			     !String.IsNullOrEmpty( vymodell.Filtertermer.Bostadsort ))
			{
			    // båda sökvillkoren satta
			    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
			    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
			    //				 " namn : " + vymodell.Filtertermer.Namn +
			    //				 " bostadsort : " + vymodell.Filtertermer.Bostadsort );

			    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 3 );
			    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}",       vymodell.Filtertermer.Namn );
			    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", vymodell.Filtertermer.Bostadsort );
			}
			else if ( !String.IsNullOrEmpty( vymodell.Filtertermer.Namn ))
			{
			    // filtrera enbart på namn
			    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
			    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
			    //				 " namn : " + vymodell.Filtertermer.Namn );

			    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 1 );
			    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", vymodell.Filtertermer.Namn );
			    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", "" );
			}
			else if (!String.IsNullOrEmpty( vymodell.Filtertermer.Bostadsort ))
			{
			    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
			    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
			    //				 " Bostadsort : " + vymodell.Filtertermer.Bostadsort );

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
	/// aktiveras av submit i (ingenfiltrering) Views/People/filtrering/dialog.cshtml
	/// </summary>
	[HttpPost]
	[ActionName( "ingenfiltrering" )]
	public IActionResult IngenFiltrering ( HopslagenmodellVymodell vymodell ) {
	    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
	    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    HttpContext.Session.SetInt32( $"valdterm.{this.sessionsuffix}", 0 );
	    HttpContext.Session.SetString( $"namn.{this.sessionsuffix}", "" );
	    HttpContext.Session.SetString( $"bostadsort.{this.sessionsuffix}", "" );

	    return RedirectToAction( "Index" );
	}

	/// <summary>
	/// skicka en CreatePersonViewModel till serviceenheten
	///
	/// aktiveras av submit i Views/People/inskrivning_av_nytt_kort/dialog.cshtml
	/// </summary>
	[HttpPost]
	[ActionName( "nyttkort" )]
	public IActionResult SkapaNyttKort ( HopslagenmodellVymodell vymodell ) {
	    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
	    //				    (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    // if ( ( vymodell.InskrivningNyttKort != null) &&
	    //	 ( vymodell.InskrivningNyttKort.Namn != null))
	    //	this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
	    //				     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
	    //				     "\n" + " namn " + vymodell.InskrivningNyttKort.Namn +
	    //				     "\n" + " Bostadsort " + vymodell.InskrivningNyttKort.Bostadsort  +
	    //				     "\n" + " Telefonnummer " + vymodell.InskrivningNyttKort.Telefonnummer);

	    if (ModelState.IsValid)
	    {
		// this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
		//			     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
		//			     "\n" + " model is valid");
		if (vymodell.InskrivningNyttKort != null)
		{
		    if ( !String.IsNullOrEmpty( vymodell.InskrivningNyttKort.Namn ) &&
			 !String.IsNullOrEmpty( vymodell.InskrivningNyttKort.Bostadsort ) &&
			 !String.IsNullOrEmpty( vymodell.InskrivningNyttKort.Telefonnummer ))
		    {
			// this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
			//			     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
			//			     "\n" + "alla uppgifter finns med");

			this.serviceenheten.Add( vymodell.InskrivningNyttKort );
		    }
		    else
		    {
			this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
						     "\n" + "Upppgifter sakns");
		    }
		}
		else
		{
		    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
						 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
						 "\n" + "vymodell.InskrivningNyttKort == null");
		}
	    }
	    else
	    {
		this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					     "\n" + "modell NOT valid");
	    }

	    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
	    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));
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
	///
	/// använder modellbaserad modifiering via icke-ajax baserad modifiering av korten
	///
	/// används inte
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
	    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
	    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

	    this.serviceenheten.Remove( id );

	    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
	    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));
	    nyVymodell.Personlistan = serviceenheten.All();
	    // this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
	    //				 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    return View( "Index", nyVymodell ); // använder Views/People/Index.cshtml
	}
    }
}
