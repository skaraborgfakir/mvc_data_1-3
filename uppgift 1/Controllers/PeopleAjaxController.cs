//
// Time-stamp: <2021-11-08 16:32:54 stefan>
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
    /// <remark>
    /// kontroller-klassen knyter ihop affärs-/process-logik (serviceenhten) med UI.
    /// En kontroller implementeras utgående från serviceenhetens termer
    /// </remark>
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
	/// </summary>
	[ActionName( "uppdateralistan" )]
	public IActionResult uppdateralistanurdatabasen () {
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    this.loggdest.LogInformation(
		(new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
		(new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
		"\n" + "vald term : "  + HttpContext.Session.GetInt32( $"valdterm.{this.sessionsuffix}").ToString() +
		"\n" + "namn : "       + HttpContext.Session.GetString( $"namn.{this.sessionsuffix}") +
		"\n" + "bostadsort : " + HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}")
	    );

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
		    this.loggdest.LogInformation(
			(new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
			(new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

		    söktermer.Bostadsort = HttpContext.Session.GetString( $"bostadsort.{this.sessionsuffix}");
		    PeopleViewModel vy_efter_bostadsort = this.serviceenheten.FindBy( söktermer);

		    return PartialView( "aktivlistan", vy_efter_bostadsort);

		    // return View() {
		    //	ViewName = "aktivlistan",
		    //	    ViewData = new ViewDataDictionary( vy_efter_bostadsort)
		    //	    };

		    // return new PartialViewResult( "aktivlistan", vyn);

		default:
		    PeopleViewModel vyn = this.serviceenheten.All();

		    return PartialView( "aktivlistan", vyn);
	    }
	}

	/// <summary>
	/// ett utdrag av vissa kort från kartoteket
	/// </summary>
	[ActionName( "uppdateralistanvissakort" )]  // API mot JS !!
	public ActionResult uppdateralistanvissakort () {
	    this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
					  (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    PeopleViewModel söktermer = new PeopleViewModel(){
	    };

	    return Ok( this.serviceenheten.FindBy(söktermer));
	}

	/// <summary>
	/// tag fram ett specifikt kort
	/// </summary>
	[HttpGet( "{id=1}" )]
	[ActionName( "taguppkortet" )]  // API mot JS !!
	[Produces( "application/json" )]
	public ActionResult tagframkortet ( int id ) {
	    this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
					  (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					  "\n tag fram kortet med id : " + id.ToString());

	    // return Ok( this.serviceenheten.FindBy( id));
	    return PartialView( "aktiva_div", this.serviceenheten.FindBy( id));
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
	    this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " programrad : " +
					  (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()) +
					  "\n kasera kortet med id : " + id.ToString());

	    if ( this.serviceenheten.FindBy( id) != null) {
		return Ok( this.serviceenheten.Remove( id));
	    }
	    else
	    {
		return NotFound();
	    }
	}
    }
}
