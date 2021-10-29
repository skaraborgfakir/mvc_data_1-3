//
// Time-stamp: <2021-10-27 13:56:42 stefan>
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

using Kartotek.Modeller;
using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;

namespace Kartotek.Controllers
{
    public class HomeController : Controller
    {
	private readonly ILogger<HomeController> _logger;
	private readonly IConfiguration  _configurationsrc;

	public HomeController(ILogger<HomeController> logger,
			      IConfiguration  configurationsrc)
	{
	    _configurationsrc = configurationsrc;
	    _logger = logger;
	}

	public IActionResult Index()
	{
	    return View();
	}

	public IActionResult Privacy()
	{
	    return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
	    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}
    }
}
