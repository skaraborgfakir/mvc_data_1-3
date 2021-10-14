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
using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;

namespace Kartotek.Controllers {
   // [Route( "api/[controller]" )]
   //[ApiController]
   public class PeopleAjaxController : Controller {
      private readonly ILogger<PeopleController> _loggdest;
      private readonly IConfiguration _configurationsrc;
      private readonly IWebHostEnvironment _webHostEnvironment;

      public PeopleAjaxController ( ILogger<PeopleController> loggdest,
                            IConfiguration configurationsrc,
                           IWebHostEnvironment webHostEnvironment ) {
         _loggdest = loggdest;
         _configurationsrc = configurationsrc;
         _webHostEnvironment = webHostEnvironment;
      }

      //
      // funktioner/aktioner i labb 3 - AJAX
      //
      [HttpGet( "listan" )]
      [ActionName( "uppdateralistan" )]
      [Produces( "application/json" )]
      public JsonResult uppdateraListanUrDatabasen () {
         _loggdest.LogInformation( string.Concat( "public JsonResult uppdateraListanUrDatabasen " ) );

         string filePath = _webHostEnvironment.ContentRootPath + "/App_Data/test.json";

         throw new NotImplementedException( "public JsonResult tagFramKortet())" );
         // return System.IO.File.ReadAllText( filePath );
      }

      [HttpGet( "{id}" )]
      [ActionName( "tagUppKortet" )]
      [Produces( "application/json" )]
      public JsonResult tagFramKortet ( int id ) {
         _loggdest.LogInformation( string.Concat( "public JsonResult tagFramKortet " ) );

         throw new NotImplementedException( "public JsonResult tagFramKortet())" );
      }

      [HttpDelete( "{id}" )]
      [ActionName( "rensaBortKortet" )]
      public IActionResult rensaUrKortet ( int id ) {
         _loggdest.LogInformation( string.Concat( "public IActionResult rensaUrKortet " ) );

         throw new NotImplementedException( "public IActionResult rensaUrKortet())" );
      }

   }
}
