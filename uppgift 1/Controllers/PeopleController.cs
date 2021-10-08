//
// Time-stamp: <2021-10-06 12:12:03 stefan>
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
   public class PeopleController : Controller {
      private readonly ILogger<PeopleController> _loggdest;
      private readonly IConfiguration _configurationsrc;
      private readonly IWebHostEnvironment _webHostEnvironment;

      public PeopleController ( ILogger<PeopleController> loggdest,
                        IConfiguration configurationsrc,
                        IWebHostEnvironment webHostEnvironment,
                        IPeopleService serviceenheten ) {
         _configurationsrc = configurationsrc;
         _loggdest = loggdest;
         _webHostEnvironment = webHostEnvironment;
         _serviceenheten = serviceenheten;
         
      }

      //
      // sökning efter enbart vissa poster ?
      //
      // påverkar Index-funktionen
      //
      private enum Sökläge { Ingen, Namn, Bostadsort };
      //
      // kontroller-klassen knyter ihop affärs-/process-logik (serviceenhten) med UI
      //
      // kontroller implementeras utgående från serviceenhetens termer
      //
      //   private  PeopleService serviceenheten = new PeopleService();
      private readonly IPeopleService _serviceenheten;

      //
      // bilden med:
      //   sökformulär
      //   formulär för att addera en person i registret
      //   lista av personer enligt sökkriterier
      //
      [HttpGet]
      [ActionName( "Index" )]
      public IActionResult Index ( HopslagenmodellVymodell vymodell ) {
         Console.WriteLine( "public IActionResult Index" );

         //_loggdest.Test();

         if (HttpContext.Session.GetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st" ) == null) {
            HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 0 );
            HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", "" );
            HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", "" );
         }

         Sökläge sökstatus = Sökläge.Ingen;

         HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();
         nyVymodell.Filtertermer = new PeopleViewModell();

         switch (HttpContext.Session.GetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st" )) {
            case 1:
               // Console.WriteLine( "public IActionResult Index - Sökläge sökstatus = Sökläge.Namn");
               sökstatus = Sökläge.Namn;
               break;
            case 2:
               sökstatus = Sökläge.Bostadsort;
               // Console.WriteLine( "public IActionResult Index - Sökläge sökstatus = Sökläge.Bostadsort");
               break;
            default:
               sökstatus = Sökläge.Ingen;
               // Console.WriteLine( "public IActionResult Index - Sökläge sökstatus = inget speciellt");
               break;
         }

         switch (sökstatus) {
            case Sökläge.Namn:
               // Console.WriteLine( "public IActionResult Index sålla på namn");
               nyVymodell.Filtertermer.Namn = HttpContext.Session.GetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st" );
               nyVymodell.Personlistan = _serviceenheten.FindBy( nyVymodell.Filtertermer );
               break;

            case Sökläge.Bostadsort:
               // Console.WriteLine( "public IActionResult Index sålla på bostadsort");
               nyVymodell.Filtertermer.Bostadsort = HttpContext.Session.GetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st" );
               nyVymodell.Personlistan = _serviceenheten.FindBy( nyVymodell.Filtertermer );
               break;

            default:
               // Console.WriteLine( "public IActionResult Index ingen sållning");
               nyVymodell.Personlistan = _serviceenheten.All();
               break;
         }

         return View( nyVymodell );
      }

      //
      // modifiera aktiv sökterm
      //
      [HttpPost]
      [ActionName( "filtrering" )]
      public IActionResult Filtrera ( HopslagenmodellVymodell vymodell ) {
         Console.WriteLine( "public IActionResult Filtrera( PeopleViewModell" );

         // HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();
         // nyVymodell.NyttKort = new CreatePersonViewModell();

         if (ModelState.IsValid) {
            if (vymodell == null) {
               Console.WriteLine( "public IActionResult Filtrera vymodell == null" );
            } else {
               Console.WriteLine( "public IActionResult Filtrera - if ( vymodell != null " );
               //
               // hur kommer man hit ? tanken är att man skulle ha sökning efter data på
               // enbart ett ställe och filtrering då enbart ska ändra kriterierna
               //
               // sökkriterier (namn/bostadsort) i session ?
               //
               if (vymodell.Filtertermer != null) {
                  // nyVymodell.Filtertermer = new PeopleViewModell();

                  if (!String.IsNullOrEmpty( vymodell.Filtertermer.Namn )) {
                     // Console.WriteLine( "public IActionResult Filtrera på namn -- " + vymodell.Filtertermer.Namn + " --");

                     HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 1 );
                     HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", vymodell.Filtertermer.Namn );
                     HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", "" );
                  } else if (!String.IsNullOrEmpty( vymodell.Filtertermer.Bostadsort )) {
                     // Console.WriteLine( "public IActionResult Filtrera på bostadsort -- " + vymodell.Filtertermer.Bostadsort + " --");

                     HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 2 );
                     HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", "" );
                     HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", vymodell.Filtertermer.Bostadsort );
                  }

                  if (vymodell.Filtertermer.Namn == null &&
                        vymodell.Filtertermer.Bostadsort == null) {// &&
                                                                   // Console.WriteLine( "public IActionResult Filtrera -- vymodell.Filtertermer.Namn != null &&");

                     HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 0 );
                     HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", "" );
                     HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", "" );
                  }

               } else {
                  Console.WriteLine( "public IActionResult - if ( vymodell.Filtertermer == null )  " );
               }
            }
         } else {
            // Console.WriteLine( "public IActionResult - if ( ! ModelState.IsValid ) {");

            HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 0 );
            HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", "" );
            HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", "" );
         }

         return RedirectToAction( "Index" );
      }

      //
      // återställ sökning
      //
      [HttpPost]
      [ActionName( "ingenfiltrering" )]
      public IActionResult IngenFiltrering ( HopslagenmodellVymodell vymodell ) {
         // Console.WriteLine( "public IActionResult Filtrera( PeopleViewModell");

         HttpContext.Session.SetInt32( "valdterm.kartotek.netcore3.1.fakirenstenstorp.st", 0 );
         HttpContext.Session.SetString( "namn.kartotek.netcore3.1.fakirenstenstorp.st", "" );
         HttpContext.Session.SetString( "bostadsort.kartotek.netcore3.1.fakirenstenstorp.st", "" );

         return RedirectToAction( "Index" );
      }

      //
      // skicka en CreatePersonViewModel till serviceenheten
      //
      [HttpPost]
      [ActionName( "nyttkort" )]
      public IActionResult SkapaNyttKort ( HopslagenmodellVymodell vymodell ) {
         // Console.WriteLine( "public IActionResult Skapa_kort( PeopleViewModell" );

         HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

         if (ModelState.IsValid) {
            // Console.WriteLine( "public IActionResult Skapa_kort( PeopleViewModell if ( ModelState.IsValid ) {" );
            if (!String.IsNullOrEmpty( vymodell.NyttKort.Namn ) &&
                 !String.IsNullOrEmpty( vymodell.NyttKort.Bostadsort ) &&
                 !String.IsNullOrEmpty( vymodell.NyttKort.Telefonnummer )) {

               CreatePersonViewModell nyttKort = new CreatePersonViewModell();
               nyttKort = vymodell.NyttKort;

               _serviceenheten.Add( nyttKort );
            }
         } else {
            Console.WriteLine( "public IActionResult Skapa_kort( PeopleViewModell not if ( ModelState.IsValid ) {" );
         }

         nyVymodell.Personlistan = _serviceenheten.All();

         return View( "Index", nyVymodell ); // använder Views/People/Index.cshtml
                                             // return RedirectToAction( "Index", nyVymodell);
      }

      //
      // modifiering
      //
      [HttpPost]
      [ActionName( "modifiering" )]
      public IActionResult ModifieraKort ( HopslagenmodellVymodell vymodell ) {
         Console.WriteLine( "public IActionResult ModifieraKort( HopslagenmodellVymodell" );

         HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

         throw new NotImplementedException( "public IActionResult ModifieraKort( HopslagenmodellVymodell vymodell)" );
         // return RedirectToAction( "Index", nyVymodell);
      }

      //
      // radering av item i listan
      //
      [HttpGet]
      [ActionName( "radering" )]
      public IActionResult TagBortKort ( int id ) {
         // Console.WriteLine( "public IActionResult TagBortKort( int id");
         // Console.WriteLine( "public IActionResult TagBortKort id = " + id.ToString());

         HopslagenmodellVymodell nyVymodell = new HopslagenmodellVymodell();

         _serviceenheten.Remove( id );
         nyVymodell.Personlistan = _serviceenheten.All();

         // throw new NotImplementedException( "public IActionResult TagBortKort( int id)");
         return View( "Index", nyVymodell ); // använder Views/People/Index.cshtml
      }


   }
}
