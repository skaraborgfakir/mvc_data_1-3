//
// Time-stamp: <2021-11-24 15:11:20 stefan>
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

// egen kod
using Kartotek.Modeller;
using Kartotek.Modeller.Data;
using Kartotek.Modeller.Interfaces;

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
	public IConfiguration Configuration { get; }

	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	/// </summary>
	/// <param name="configuration">DI av en instans av IConfiguration</param>
	/// <param name="env">DI av en instans av IWebHostEnvironment</param>
	public REVELJ(IConfiguration configuration,
		      IWebHostEnvironment env)
	{
	    Configuration = configuration;
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
	    services.AddCors(options => {
		options.AddPolicy( "CorsPolicy", policy => { policy
			    .AllowAnyHeader()
			    .AllowAnyMethod()
			    // .WithExposedHeaders( "WWW-Authenticate")
			    .WithOrigins( "http://localhost:5002/People",
					  "https://localhost:5003/People",
					  "https://localhost:5003/api/PeopleAjax"
			    )
			    .AllowCredentials();
		});
	    });

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
		options.Cookie.Name = Configuration["session_kakans_namn"];
		options.IdleTimeout = TimeSpan.FromSeconds( 40 );
		options.Cookie.HttpOnly = true;
		options.Cookie.IsEssential = true;
	    }
	    );

	    services.AddControllers().AddJsonOptions( options => {                               // Convert JSON from Camel Case to Pascal Case
		options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; // Use the default property (Pascal) casing.
	    } );

	    // services.AddSingleton
	    // services.AddTransient
	    services.AddScoped<IPeopleService, PeopleService>();
	    services.AddSingleton<IPeopleRepo, InMemoryPeopleRepo>(); // används av PeopleService - singleton to rot ansvarar för dess levnad

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
	public void Configure ( IApplicationBuilder app,
				ILogger<REVELJ> loggdest)
	{
	    //
	    // gaffling i flödet beroende på programmets startmiljö
	    //
	    if (Environment.IsDevelopment() ||
		Environment.IsEnvironment( "Development"))
	    {
		app.UseDeveloperExceptionPage();  // plockar upp händelser (exceptions) i aktiverade moduler (exv en kontrollant) för att ge meddelandestatus till användaren
	    }
	    else
	    {
		app.UseExceptionHandler("/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	    }

	    // app.UseHttpsRedirection();  // blockera automatisk uppgradering till https
	    app.UseStatusCodePages();  // mer förklarande beskrivning av fel (http status 400-599) som saknar en beskrivning
	    app.UseStaticFiles();              // get av statisk filer exv script/css etc

	    //
	    // aktivera vidarebefordran av frågor till olika kontrollanter
	    // MapControllerRoute är beroende
	    app.UseRouting();

	    app.UseCors();

	    if (Environment.IsDevelopment())
		app.Use( next => context =>
		{
		    Console.WriteLine($"Found: {context.GetEndpoint()?.DisplayName}");
		    return next(context);
		});

	    //
	    // sessions-kakan - det här anropet måste vara placerad innan de moduler (exv
	    // aktion-metoder) som ska ha tillgång till kakan.
	    //
	    app.UseSession();

	    app.UseAuthorization();

	    //
	    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.1
	    //
	    // avgrening till olika styrprogram (Controller) - ingen egentlig återuppsamling efter
	    // dessa så inte en egentlig gaffling
	    //
	    // beroende av UseRouting() !
	    //
	    // mönsterigenkänning:
	    //   en inkommande URL ska ha ett nominerat styrprogram (Controller)
	    //   och som option en aktion som eventuellt kan använda ett Id
	    //
	    // för varje fördelning ska det finnas ett unikt namn (id)
	    //
	    // mer specifika  rutter ska vara före mer generella ( more specific patterns before less  specific ones.)
	    //
	    // UseEndpoints är en utökning av samma typ som de tidigare UseStaticFiles (middleware component)
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
