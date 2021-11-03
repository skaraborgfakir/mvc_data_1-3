//
// Time-stamp: <2021-11-02 16:44:11 stefan>
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
	/// </summary>
	public static void Main ( string[] args )
	{
	    CreateHostBuilder( args: args ).Build().Run();
	}

	/// <summary>
	/// startrutin för .Net core
	///
	/// inkluderar logging mot konsoll och får webBuilder att aktivera klassen REVELJ
	/// </summary>
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
	    // här skulle man kunna lägga till en modifierad konfiguration-module
	    //
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
