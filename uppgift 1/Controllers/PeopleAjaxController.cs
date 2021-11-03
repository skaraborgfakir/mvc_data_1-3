//
// Time-stamp: <2021-11-03 13:29:18 stefan>
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
    public class PeopleAjaxController : ControllerBase {
	private readonly ILogger<PeopleAjaxController> loggdest;
	private readonly IConfiguration configurationsrc;
	private readonly IWebHostEnvironment webHostEnvironment;
	private readonly IPeopleService serviceenheten;

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
	}

	/// <summary>
	/// komplett kartotek, utan någon filtrering
	/// </summary>
	[ActionName( "uppdateralistan" )]  // API mot JS !!
	public ActionResult uppdateraListanUrDatabasen () {
	    this.loggdest.LogInformation((new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " +
					 (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    return Ok( this.serviceenheten.All());
	}

	/// <summary>
	/// ett utdrag av vissa kort från kartoteket
	/// </summary>
	[ActionName( "uppdateralistanvissakort" )]  // API mot JS !!
	public ActionResult uppdateraListanVissaKort () {
	    this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) +
					  " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    PeopleViewModel söktermer = new PeopleViewModel(){
	    };
	    return Ok( this.serviceenheten.FindBy(söktermer));
	}

	/// <summary>
	/// tag fram ett specifikt kort
	/// </summary>
	[HttpGet( "{id=1}" )]
	[ActionName( "tagUppKortet" )]  // API mot JS !!
	[Produces( "application/json" )]
	public ActionResult tagFramKortet ( int id ) {
	    this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) +
					  " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    return Ok( this.serviceenheten.FindBy( id));
	}

	/// <summary>
	/// kassera ett specifikt kort
	/// </summary>
	[HttpDelete( "{id}" )]
	[ActionName( "kaseraKortet" )]  // API mot JS !!
	public ActionResult rensaUrKortet ( int id ) {
	    this.loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) +
					  " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    throw new NotImplementedException( "public IActionResult rensaUrKortet())" );
	}
    }
}
