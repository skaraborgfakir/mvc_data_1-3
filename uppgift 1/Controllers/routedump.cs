//
// Time-stamp: <2021-11-26 13:45:28 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Kartotek.Controllers {
    /// <summary>
    /// anrop (GET) med curl eller postman till https://l....:5005/routes
    ///
    /// varför saknas exv:
    ///   People/Index
    /// Ingen av aktörerna i People syns i listan
    /// </summary>
    public class EnvironmentController : Controller {
	private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

	/// <summary>
	/// to be done
	/// </summary>
	public EnvironmentController ( IActionDescriptorCollectionProvider actionDescriptorCollectionProvider ) {
	    _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
	}

	/// <summary>
	/// https://l....:5005/routes
	/// </summary>
	[HttpGet( "routes", Name = "ApiEnvironmentGetAllRoutes" )]
	[Produces( typeof( ListResult<RouteModel> ) )]
	public IActionResult GetAllRoutes () {

	    var result = new ListResult<RouteModel>();
	    var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Where(
		ad => ad.AttributeRouteInfo != null ).Select( ad => new RouteModel {
			Name = ad.AttributeRouteInfo.Name,
			Template = ad.AttributeRouteInfo.Template
		    } ).ToList();
	    if (routes != null && routes.Any()) {
		result.Items = routes;
		result.Success = true;
	    }
	    return Ok( result );
	}
    }

    /// <summary>
    /// to be done
    /// </summary>
    internal class RouteModel {
	public string Name { get; set; }
	public string Template { get; set; }
    }

    /// <summary>
    /// to be done
    /// </summary>
    internal class ListResult<T> {
	public ListResult () {
	}

	public List<RouteModel> Items { get; internal set; }
	public bool Success { get; internal set; }
    }
}
