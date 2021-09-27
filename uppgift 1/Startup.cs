// Time-stamp: <2021-09-24 12:00:08 stefan>

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
    public class REVELJ
    {
	public REVELJ(IConfiguration configuration)
	{
	    Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
	    services.AddControllersWithViews();
	    // services.AddScoped<IPeopleService, PeopleService>();
	    // services.AddScoped<IPeopleRepo,InMemoryPeopleRepo>();
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
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
	    app.UseStaticFiles();

	    app.UseRouting();

	    app.UseAuthorization();

	    app.UseEndpoints(endpoints =>
	    {
		endpoints.MapControllerRoute( name:     "people-radering",
					      pattern:  "People/"});
		endpoints.MapControllerRoute( name:     "people",
					      pattern:  "People",
					      defaults: new { controller = "PeopleController", action = "Index"});
		endpoints.MapControllerRoute( name:     "default",
					      pattern:  "{controller=Home}/{action=Index}/{id?}");
	    });
	}
    }
}
