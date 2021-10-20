//
// Time-stamp: <2021-10-20 23:54:28 stefan>
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
    public class Program
    {
	public static void Main ( string[] args )
	{
	    CreateHostBuilder( args ).Build().Run();
	}

	public static IHostBuilder CreateHostBuilder ( string[] args ) => Host
	    .CreateDefaultBuilder( args: args )
	    .ConfigureLogging( logging => {
		logging.ClearProviders();
		logging.AddConsole();
	    } )
	    // här skulle man kunna lägga till en modifierad konfiguration-module
	    .ConfigureWebHostDefaults( configure: webBuilder => {
		webBuilder.UseStartup<REVELJ>(); } );
    }
}
