using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Kartotek.Modeller;

namespace Kartotek.Controllers {
    /// <summary>
    /// grundklass från mallen för en MVC:applikation
    /// </summary>
   public class HomeController : Controller {
      private readonly ILogger<HomeController> _logger;


      /// <summary>
      ///  kreator för HomeController
      /// </summary>
      public HomeController ( ILogger<HomeController> logger ) {
	 _logger = logger;
      }

      /// <summary>
      ///  Hämta ut index.html
      /// </summary>
      public IActionResult Index () {
	 return View();
      }

      /// <summary>
      /// Uppgifter om personuppgiftskyddet
      /// </summary>
      public IActionResult Privacy () {
	 return View();
      }

      /// <summary>
      /// Något är fel, en felvy
      /// </summary>
      [ResponseCache( Duration = 0, Location = ResponseCacheLocation.None, NoStore = true )]
      public IActionResult Error () {
	 return View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
      }
   }
}
