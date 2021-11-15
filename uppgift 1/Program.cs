//
// Time-stamp: <2021-11-14 16:42:35 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

// från mvc-mallen:
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// egna tillägg
using Microsoft.Extensions.DependencyInjection;

namespace Kartotek
{
    /// <summary>
    /// Huvud-rutin/-klass i ett medlemskartotek
    ///
    /// Programmet körs som en webserver
    /// </summary>
    public class Program
    {
	/// <summary>
	/// Huvud-rutin/-klass i ett medlemskartotek
	///
	/// Inte något annat än ett omslag runt CreateHostBuilder och därefter build och run
	///
	/// EF6:verktygen är beroende av att det finns en metod i klassen Program med namneet CreateHostBuilder
	/// </summary>
	/// <param name="args">argumentvektor ekvivalent med argc/argv i C</param>
	/// <see href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host?view=aspnetcore-3.1">.Net Generic Host</see>
	/// <see href="https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/web-host?view=aspnetcore-3.1">.Net Web Host</see>
	/// <see href="https://stackoverflow.com/questions/41287648/how-do-i-write-logs-from-within-startup-cs">how-do-i-write-logs-from-within-startup-cs</see>
	public static void Main ( string[] args )
	{
	    // netcore 3: mallen för en web host använder CreateHostBuilder
	    // vilket skiljer sig mot den äldre CreateWebHostBuilder (som finns kvar
	    // av kompatibilitetsskäl)
	    // skillnader:
	    //   loggning i Startup är mer begränsad med CreateHostBuilder
	    //   jämfört med CreateWebHostBuilder
	    //   CreateHostBuilder stödjer fler applikationstyper än jämfört med CreateWebHostBuilder
	    //   som i huvudsak lämpar sig för web:applikationer
	    //   därför kan inte loggning ympas helt i REVELJ:klassen (Startup) utan enbart via
	    //   DI i Configure:metoden
	    // CreateHostBuilder( args: args ).Build().Run();
	    var host = CreateHostBuilder( args: args ).Build();

	    // dump av vad som kan hittas via Configuration
	    // var config = host.Services.GetRequiredService<IConfiguration>();
	    // Console.WriteLine("c in config.AsEnumerable");
	    // foreach (var c in config.AsEnumerable())
	    // {
	    //	Console.WriteLine(c.Key + " = " + c.Value);
	    // }

	    host.Run();
	}

	/// <summary>
	/// startrutin för .Net core
	///
	/// inkluderar logging mot konsoll och får webBuilder att aktivera klassen REVELJ
	///
	/// Den här metoden är speciell iom att EF6 (och Identity Server) förväntar sig att just den här
	/// metoden finns med just det här namnet
	/// </summary>
	/// <param name="args">
	/// argumentvektor ekvivalent med argc/argv i C. CreateHostBuilder exv
	/// kan plocka bort de argument i listan som är i dess fögderi
	/// </param>
	/// <see href="https://stackoverflow.com/questions/41287648/how-do-i-write-logs-from-within-startup-cs">how-do-i-write-logs-from-within-startup-cs</see>
	public static IHostBuilder CreateHostBuilder ( string[] args ) => Host
	    // kestrel som httpd
	    // aktivera IConfiguration och läs in från appsettings.json
	    // loggning (iloggerfactory) aktiveras
	    // Development-miljön är speciell iom att scope i DI valideras
	    .CreateDefaultBuilder( args: args )
	    // CreateDefaultBuilder kommer att ympa in loggning men för att få kontroll över den
	    // och var det skickas, används detta
	    .ConfigureLogging( logging => {
		logging.ClearProviders();
		logging.AddConsole();
	    } )
	    //
	    // konfigurering av en web-app
	    //   Kestrel som web-server används
	    //   se till att www-static dvs statiska filer tillgängliga i wwwroot även är tillgänglig när allt körs
	    //   hostfiltering
	    //   forwardedheaders om man så vill
	    //   IIS:integration
	    // och
	    //   REVELJ:klassen med sina metoder garantera konfiguration
	    //
	    .ConfigureWebHostDefaults( configure: webBuilder => {
		webBuilder.UseStartup<REVELJ>(); } );
    }
}
