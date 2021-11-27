//
// Time-stamp: <2021-11-27 17:39:41 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//
// olika metoder som kan anropas från ett formulär i javascript
//   1 uppräkning av korten med personernas namn, telefonnummer och hemort
//   2 visning av ett specifikt kort enbart, med möjlighet att kasera det eller begära att få ändra i det
//       olika vyer:
//         visningsvy
//         modifieringsvy
//         vy för radering ?
//     visningsvyn kommer man åt igenom att välja kortets nummer i dialog ovanför
//     eller igenom att klicka 'visa kort' i uppräkningen
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

using Newtonsoft.Json.Serialization;

using Kartotek.Modeller;
using Kartotek.Modeller.Interfaces;
using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;

namespace Kartotek.Controllers {
    /// <summary>
    /// ett personkartotek
    ///
    /// kontrollant för ett MVC-program som förutom vyer, använder AJAX:teknik inklusive att
    /// sidan/sidorna delvis byggs upp helt via JavaScript/jQuery
    ///
    /// Data transporteras som JSON/XML mellan programmet i webläsaren
    /// och bakliggande system/program
    ///
    /// En AJAX:anpassad kontrollklass <c>PeopleAjaxController</c> för medlemskartoteket.
    /// </summary>
    /// <remarks>
    /// kontroller-klassen knyter ihop affärs-/process-logik (serviceenhten) med UI.
    /// En kontroller implementeras utgående från serviceenhetens termer
    /// </remarks>
    /// <remarks>
    /// Route:attributet på klassen innebär att all interaktion mellan front och metoder i klassen
    /// i klassen istället för route-reglerna i Startup, istället kommer gå på uppmärkningen i koden (attributen)
    /// </remarks>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PeopleAjaxController : Controller {
	private readonly ILogger<PeopleAjaxController> loggdest;
	private readonly IConfiguration configurationsrc;
	private readonly IWebHostEnvironment webHostEnvironment;
	private readonly IPeopleService serviceenheten;
	private readonly string sessionsuffix;

	/// <summary>
	/// kreator för PeopleAjaxController
	/// </summary>
	public PeopleAjaxController ( ILogger<PeopleAjaxController> loggdest,
				      IConfiguration configurationsrc,
				      IWebHostEnvironment webHostEnvironment,
				      IPeopleService serviceenheten ) {
	    this.loggdest           = loggdest;
	    this.configurationsrc   = configurationsrc;
	    this.webHostEnvironment = webHostEnvironment;
	    this.serviceenheten     = serviceenheten;

	    this.sessionsuffix=this.configurationsrc["session_kakans_namn"];
	}

	/// <summary>
	/// komplett kartotek, utan någon filtrering
	/// API mot JS !!
	/// använder de sessionsvariabler som kontrolleras av PeopleController
	/// anropas från js/person/ajaxkortfunktioner.js för visning av listan
	/// </summary>
	[ActionName( "uppdateralistan" )]
	public IActionResult uppdateralistanurdatabasen () {
	    // this.loggdest.LogInformation(
	    //	(new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
	    //	(new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
	    //	"\n" + "vald term : "  + HttpContext.Session.GetInt32( $"valdterm.{this.sessionsuffix}").ToString() +
	    //	"\n" + "namn : "       + HttpContext.Session.GetString( $"namn.{this.sessionsuffix}") +
	    //	"\n" + "bostadsort : " + HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}")
	    // );

	    PeopleViewModel söktermer = new PeopleViewModel();

	    switch (HttpContext.Session.GetInt32( $"valdterm.{this.sessionsuffix}")) {
		case 1 :
		    söktermer.Namn = HttpContext.Session.GetString( $"namn.{this.sessionsuffix}");
		    PeopleViewModel vy_efter_namn = this.serviceenheten.FindBy( söktermer);

		    return PartialView( "aktivlistan", vy_efter_namn);

		    // return  View() {
		    //	ViewName = "aktivlistan",
		    //	    ViewData = new ViewDataDictionary( vy_efter_namn),
		    //	    };

		    // return new PartialViewResult() {
		    //	ViewName = "aktivlistan",
		    //	    ViewData = vyn
		    //	    };

		case 2:
		    söktermer.Bostadsort = HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}");
		    PeopleViewModel vy_efter_bostadsort = this.serviceenheten.FindBy( söktermer);

		    return PartialView( "aktivlistan", vy_efter_bostadsort);

		case 3:
		    söktermer.Namn = HttpContext.Session.GetString( $"namn.{this.sessionsuffix}");
		    söktermer.Bostadsort = HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}");
		    PeopleViewModel vy_flera_termer = this.serviceenheten.FindBy( söktermer);

		    return PartialView( "aktivlistan", vy_flera_termer);

		default:
		    PeopleViewModel vyn = this.serviceenheten.All();

		    return PartialView( "aktivlistan", vyn);
	    }
	}

	/// <summary>
	/// ett utdrag av vissa kort från kartoteket
	/// används inte, istället används sessionvariabler i uppdateralistanurdatabasen
	/// för att begränsa vad som listas
	/// </summary>
	[ActionName( "uppdateralistanvissakort" )]  // API mot JS !!
	public ActionResult uppdateralistanvissakort () {
	    PeopleViewModel söktermer = new PeopleViewModel(){
	    };

	    return Ok( this.serviceenheten.FindBy(söktermer));
	}

	/// <summary>
	/// tag fram ett specifikt kort för visning enbart
	/// det kan finnas knappar i bilden för att modifiera eller ta bort det
	/// </summary>
	[HttpGet( "id={id=1}" )]
	[ActionName( "tagframvisstkort" )]  // API mot JS !!
	public ActionResult tagframkortet ( int id ) {
	    // this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
	    //				     (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
	    //				     "\n tag fram kortet med id : " + id.ToString());

	    Person personkort = this.serviceenheten.FindBy( id);

	    if ( personkort != null )
		return PartialView( "personkortvy_objektbaserad/separat_kortvisning", personkort);
	    else
		return NotFound();
	}

	/// <summary>
	/// modifieringsvy för ett specifikt kort
	/// </summary>
	[HttpPost]
	[ActionName( "modifieravisstkort")]
	public ActionResult modifieravisstkort( int id) {
	    Person personkort = this.serviceenheten.FindBy( id);

	    if ( personkort != null )
		return PartialView( "modifiering_av_kort/dialog", personkort);
	    else
		return NotFound();
	}

	/// <summary>
	/// modifieringsvy för ett specifikt kort
	/// </summary>
	[HttpPost]
	[ActionName( "modifiera")]
	public ActionResult modifiera( Person person) {
	    this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
					  (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					  "\n tag fram kortet med id : " + person.Id);

	    // Person personkort = this.serviceenheten.FindBy( id);

	    // if ( personkort != null )
	    //	return PartialView( "modifiering_av_kort/dialog", personkort);
	    // else
	    return NotFound();
	}

	/// <summary>
	/// kassera ett specifikt kort
	/// DELETE som metod !
	///
	/// Id kan inte vara med i body, den måste skickas av jQuery som del av URI:n, så bort med Id i HttpDelete:attributet
	/// </summary>
	/// <see href="https://stackoverflow.com/questions/15088955/how-to-pass-data-in-the-ajax-delete-request-other-than-headers">JQuery bug</see>
	/// <see href="http://bugs.jquery.com/ticket/11586">bug i jQuery (?): använder man DELETE så klipps data-klumpen bort</see>
	[HttpDelete]
	[ActionName( "kaserakortet" )]  // API mot JS !!
	public ActionResult kaserakortet ( int id ) {
	    // this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
	    //				  (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
	    //				  "\n kasera kortet med id : " + id.ToString());

	    if ( this.serviceenheten.FindBy( id) != null)
		return Ok( this.serviceenheten.Remove( id));
	    else
		//
		// kan bli exv om valideringen av valtkortsid i ajaxbaserad_kortselektor.cshtml
		// inte fungerar, dvs att valt kort id i scroller är ogiltigt
		//
		return NotFound();
	}
    }
}
