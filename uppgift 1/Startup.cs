//
// Time-stamp: <2021-10-14 16:02:50 stefan>
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Kartotek.Modeller;
using Kartotek.Modeller.Data;
using Kartotek.Modeller.Interfaces;

namespace Kartotek
{
    // kan egentligen heta vad som helst exv REVELJ !
    public class REVELJ
    {
	public REVELJ(IConfiguration configuration)
	{
	    Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	//
	// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
	//
	public void ConfigureServices(IServiceCollection services)
	{
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
		options.IdleTimeout = TimeSpan.FromSeconds(40);
		options.Cookie.HttpOnly = true;
		options.Cookie.IsEssential = true;
	    }
	    );

	    // services.AddScoped<IPeopleService, PeopleService>();
	    // services.AddScoped<IPeopleRepo,InMemoryPeopleRepo>();

	    services.AddControllersWithViews();
	    services.AddMvc();
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure( IApplicationBuilder app,
			       IWebHostEnvironment env)
	{
	    //
	    // klistra in olika funktioner i ramverkets avveckling av jobb (inkommande trafik via http och returnerade svar)
	    //
	    // gaffling i flödet beroende på programmets startmiljö
	    if (env.IsDevelopment())
	    {
		app.UseDeveloperExceptionPage();
	    }
	    else
	    {
		app.UseExceptionHandler("/Home/Error");
		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseHsts();
	    }
	    app.UseHttpsRedirection();
	    app.UseStatusCodePages();
	    app.UseStaticFiles();

	    //
	    // aktivera vidarebefordran av frågor till olika kontrollanter
	    // MapControllerRoute är beroende
	    app.UseRouting();

	    //
	    // sessions-kakan - det här anropet måste vara placerad för de moduler (exv aktion-metoder)
	    // som ska ha tillgång till kakan.
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
	    // UseEndpoints är en utökning av samma typ som de tidigare UseStaticFiles (middleware component)
	    // men den är speciell i att den är final dvs slutdestination
	    //
	    app.UseEndpoints(endpoints =>
	    {
		// endpoints.MapControllerRoute( name:     "people-radering",
		//			      pattern:  "People/"});
		endpoints.MapControllerRoute( name:     "people",
					      pattern:  "People",
					      defaults: new { controller = "PeopleController", action = "Index"});
		endpoints.MapControllerRoute( name:     "default",
					      pattern:  "{controller=Home}/{action=Index}/{id?}");
	    });
	}
    }
}
