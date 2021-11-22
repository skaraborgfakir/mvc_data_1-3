//
// Time-stamp: <2021-11-22 10:01:30 stefan>
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
using System.Text.Json;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// EF6
using Microsoft.EntityFrameworkCore;

// Identity här

// egen kod
using Kartotek.Modeller;
using Kartotek.Modeller.Data;
using Kartotek.Modeller.Interfaces;
using Kartotek.Databas;

namespace Kartotek
{
    /// <summary>
    /// används av webbuilder som definition av appen
    /// </summary>
    /// <remarks>
    /// kan egentligen heta vad som helst exv REVELJ !
    /// </remarks>
    public class REVELJ
    {
	/// <summary>
	/// </summary>
	public IHostEnvironment Environment { get; }

	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	/// </summary>
	public IConfiguration Configurationsrc { get; }

	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	/// </summary>
	/// <param name="configurationsrc">DI av en instans av IConfiguration</param>
	/// <param name="env">DI av en instans av IHostEnvironment</param>
	public REVELJ( IConfiguration configurationsrc,
		       IHostEnvironment env)
	{
	    Configurationsrc = configurationsrc;
	    Environment = env;
	}

	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	///
	/// DI:ympning av loggning fungerar inte i ConfigureServices
	/// </summary>
	/// <param name="services">DI av en instans av IServiceCollection - ger tillgång till exv info om programmets miljö</param>
	/// <see href="https://stackoverflow.com/questions/41287648/how-do-i-write-logs-from-within-startup-cs">how-do-i-write-logs-from-within-startup-cs</see>
	public void ConfigureServices( IServiceCollection services)
	{
	    // Console.WriteLine( "Startup.cs: REVELJ: ConfigureServices");

	    // behövs för UseCors i Configure
	    services.AddCors( opt => { opt
			.AddPolicy( "CorsPolicy", policy => { policy
				    // .AllowAnyHeader()
				    // .AllowAnyMethod()
				    // .WithExposedHeaders( "WWW-Authenticate")
				    .WithOrigins( "http://localhost:5008/People",
						  "https://localhost:5009/People",
						  "https://localhost:5009/api/PeopleAjax" )
				    .AllowCredentials();
				// .WithOrigins( "https://localhost:5009")
				// .WithOrigins( "http://localhost:5009")
				// .WithOrigins( "https://localhost:5009/lib/jquery/jquery.js")
			}
			);
	    }
	    );

	    services.AddDistributedMemoryCache();

	    //
	    // GDPR:anpassningar i .net innebar att det inte längre automatiskt
	    // i laissez-faire anda går att som programmerare förvänta sig att användaren
	    // kan tvingas att acceptera cookies. Därför är som standard session-kakan (och alla andra
	    // kakor) blockerade
	    //
	    // options.Cookie.IsEssential tar bort blockeringen för session-kakan och enbart den !
	    //
	    // https://andrewlock.net/session-state-gdpr-and-non-essential-cookies/
	    //
	    services.AddSession( options => {
		options.Cookie.Name = Configurationsrc["session_kakans_namn"];
		options.IdleTimeout = TimeSpan.FromSeconds( 40 );
		options.Cookie.HttpOnly = true;
		options.Cookie.IsEssential = true;
	    }
	    );

	    //
	    // registrering för DI av dbcontext mot DatabasePeopleRepo
	    if( Environment.IsEnvironment( "postgres.Development") ||
		Environment.IsEnvironment( "postgres"))
	    {
		// Console.WriteLine( "services.AddDbContext<DBPeople: PostgreSQL:version");

		services.AddDbContext<DBPeople>( options =>
						 options.UseNpgsql( Configurationsrc["DBConnectionStrings:People"]));
	    }
	    else
	    {
		// Console.WriteLine( "services.AddDbContext<DBPeople: MS SQL:version");

		services.AddDbContext<DBPeople>( options =>
						 options.UseSqlServer( Configurationsrc["DBConnectionStrings:People"]));
	    }

	    services.AddControllers().AddJsonOptions( options => {                               // Convert JSON from Camel Case to Pascal Case
		options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Use the default property( Pascal) casing.
	    } );

	    services.AddScoped< IPeopleService, PeopleService>();   // används av kontrollanterna så länge de finns en igång (de avslutas efter return)
	    services.AddScoped< IPeopleRepo, DatabasePeopleRepo>(); // används av PeopleService - scoped, så den kastas efter att PeopleService avslutas

	    services.AddControllersWithViews();
	    services.AddHttpContextAccessor();
	}

	/// <summary>
	/// Konfiguration av hur http-trafik ska hanteras
	/// överföring mellan de olika stegen via Routing, Session, Autentisering till mottagande kontrollant
	///
	/// klistra in olika funktioner i ramverkets avveckling av jobb (inkommande trafik via http och returnerade svar)
	///
	/// DI av loggning från rot
	/// </summary>
	public void Configure( IApplicationBuilder app,
			       ILogger<REVELJ> loggdest)
	{
	    //
	    // gaffling i flödet beroende på programmets startmiljö
	    // och vilken databas som ska användas
	    //
	    if( Environment.IsEnvironment( "postgres.Development") ||
		Environment.IsEnvironment( "postgres"))
		loggdest.LogInformation( "Startup.cs: PostgreSQL:version");
	    else
		loggdest.LogInformation( "Startup.cs: MS SQL:version");

	    if( Environment.IsEnvironment( "Development") ||
		Environment.IsEnvironment( "postgres.Development"))
		loggdest.LogInformation( "Startup.cs: Utvecklingsmiljö");
	    else
		loggdest.LogInformation( "Startup.cs: Production");

	    if( Environment.IsDevelopment() ||
		Environment.IsEnvironment( "Development_postgres"))
	    {
		app.UseDeveloperExceptionPage();  // plockar upp händelser( exceptions) i aktiverade moduler( exv en kontrollant) för att ge meddelandestatus till användaren
	    }
	    else
	    {
		app.UseExceptionHandler( "/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	    }

	    app.UseHttpsRedirection(); // blockera automatisk uppgradering till https
	    app.UseStatusCodePages();     // mer förklarande beskrivning av fel( http status 400-599) som saknar en beskrivning
	    app.UseStaticFiles();         // get av statisk filer exv script/css etc

	    //
	    // aktivera vidarebefordran av frågor till olika kontrollanter
	    // MapControllerRoute är beroende
	    app.UseRouting();

	    app.UseCors(); // blockera CORS-hantering

	    //
	    // debug-utskrift - vad är adressen ???
	    if ( Environment.IsDevelopment())
		app.Use( next => context =>
		{
		    Console.WriteLine( $"Found: {context.GetEndpoint()?.DisplayName}");
		    return next( context);
		});

	    //
	    // sessions-kakan - det här anropet måste vara placerad innan de moduler( exv
	    // aktion-metoder) som ska ha tillgång till kakan.
	    //
	    app.UseSession();

	    app.UseAuthorization();

	    //
	    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.1
	    //
	    // avgrening till olika styrprogram( Controller) - ingen egentlig återuppsamling efter
	    // dessa så inte en egentlig gaffling
	    //
	    // beroende av UseRouting() !
	    //
	    // mönsterigenkänning:
	    //   en inkommande URL ska ha ett nominerat styrprogram( Controller)
	    //   och som option en aktion som eventuellt kan använda ett Id
	    //
	    // för varje fördelning ska det finnas ett unikt namn( id)
	    //
	    // mer specifika  rutter ska vara före mer generella( more specific patterns before less  specific ones.)
	    //
	    // UseEndpoints är en utökning av samma typ som de tidigare UseStaticFiles( middleware component)
	    // men den är speciell i att den är final dvs slutdestination
	    //
	    app.UseEndpoints( endpoints => {
// endpoints.MapControllerRoute( name:     "people-radering",
//			      pattern:  "People/"});
// endpoints.MapControllerRoute( name: "ajax",
//			      pattern: "Ajax/{action=uppdateralistan}",
//			      defaults: new { controller = "PeopleAjax" });
// endpoints.MapControllerRoute( name: "kaserakort",
//			      pattern: "PeopleAjax/kaserakortet",
//			      defaults: new { controller = "PeopleAjax", action = "kaserakortet" } );
		endpoints.MapControllerRoute( name: "default",
					      pattern: "{controller=Home}/{action=Index}/{id?}" );
	    } );
	}
    }
}
