//
// Time-stamp: <2021-10-30 19:44:38 stefan>
//
// ett personkartotek
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PeopleAjaxController : ControllerBase {
	private readonly ILogger<PeopleAjaxController> _loggdest;
	private readonly IConfiguration _configurationsrc;
	private readonly IWebHostEnvironment _webHostEnvironment;

	//
	// kontroller-klassen knyter ihop affärs-/process-logik (serviceenhten) med UI
	// kontroller implementeras utgående från serviceenhetens termer
	//
	private readonly IPeopleService _serviceenheten;

	public PeopleAjaxController ( ILogger<PeopleAjaxController> loggdest,
				      IConfiguration configurationsrc,
				      IWebHostEnvironment webHostEnvironment,
				      IPeopleService serviceenheten ) {
	    _loggdest = loggdest;
	    _configurationsrc = configurationsrc;
	    _webHostEnvironment = webHostEnvironment;
	    _serviceenheten = serviceenheten;
	}

	//
	// funktioner/aktioner i labb 3 - AJAX
	//
	[ActionName( "uppdateralistan" )]  // API mot JS !!
	public ActionResult uppdateraListanUrDatabasen () {
	    _loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    return Ok( _serviceenheten.Utdraget());
	}

	[HttpGet( "{id=1}" )]
	[ActionName( "tagUppKortet" )]  // API mot JS !!
	[Produces( "application/json" )]
	public ActionResult tagFramKortet ( int id ) {
	    _loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    return Ok( _serviceenheten.FindBy( id));
	}

	[HttpDelete( "{id}" )]
	[ActionName( "kaseraKortet" )]  // API mot JS !!
	public ActionResult rensaUrKortet ( int id ) {
	    _loggdest.LogInformation( (new System.Diagnostics.StackFrame(0, true).GetMethod()) + " rad : " + (new System.Diagnostics.StackFrame(0, true).GetFileLineNumber().ToString()));

	    throw new NotImplementedException( "public IActionResult rensaUrKortet())" );
	}
    }
}
