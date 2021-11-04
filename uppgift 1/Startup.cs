//
// Time-stamp: <2021-11-03 22:16:22 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	/// </summary>
	public REVELJ(IConfiguration configuration)
	{
	    Configuration = configuration;
	}

	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	/// </summary>
	public IConfiguration Configuration { get; }

	/// <summary>
	/// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	/// </summary>
	public void ConfigureServices(IServiceCollection services)
	{
	    // behövs för UseCors i Configure
	    services.AddCors(opt => {
		opt.AddPolicy("CorsPolicy", policy => { policy
			    .AllowAnyHeader()
			    .AllowAnyMethod()
			    .WithExposedHeaders("WWW-Authenticate")
			    .WithOrigins("https://localhost:44345")
			    .WithOrigins("https://localhost:5009")
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
	    services.AddSingleton<IPeopleRepo, InMemoryPeopleRepo>(); // används av PeopleService

	    services.AddControllersWithViews();
	    services.AddHttpContextAccessor();
	}

	/// <summary>
	/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	/// </summary>
	public void Configure ( IApplicationBuilder app,
				IWebHostEnvironment env ) {
	    //
	    // klistra in olika funktioner i ramverkets avveckling av jobb (inkommande trafik via http och returnerade svar)
	    //
	    // gaffling i flödet beroende på programmets startmiljö
	    if (env.IsDevelopment())
	    {
		app.UseDeveloperExceptionPage();  // plockar upp händelser (exceptions) i aktiverade moduler (exv en kontrollant) för att ge meddelandestatus till användaren
	    }
	    else
	    {
		app.UseExceptionHandler("/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	    }
	    app.UseHttpsRedirection();
	    app.UseStatusCodePages();  // mer förklarande beskrivning av fel (http status 400-599) som saknar en beskrivning
	    app.UseStaticFiles();              // get av statisk filer exv script/css etc

	    //
	    // aktivera vidarebefordran av frågor till olika kontrollanter
	    // MapControllerRoute är beroende
	    app.UseRouting();

	    if (env.IsDevelopment())
		app.Use(next => context =>
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
// endpoints.MapControllerRoute( name: "people",
//			      pattern: "People",
//			      defaults: new { controller = "People", action = "Index" } );
		endpoints.MapControllerRoute( name: "default",
					      pattern: "{controller=Home}/{action=Index}/{id?}" );
	    } );
	}
    }
}
