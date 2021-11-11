//
// Time-stamp: <2021-11-11 23:15:23 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

using System;
using System.Collections.Generic;
using System.Linq;

// https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http?view=aspnetcore-3.1
using Microsoft.Extensions.Logging;

using Kartotek.Modeller.Interfaces;
using Kartotek.Modeller.Data;
using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;
using Kartotek.Databas;

namespace Kartotek.Modeller {
    public class DatabasePeopleRepo : IPeopleRepo {
	private dbPeople databas;

	/// <summary>
	/// loggdestination
	/// </summary>
	private readonly ILogger<DatabasePeopleRepo> loggdest;

	/// <summary>
	/// Kreator - använd ympning (DI) för att få med ett databaslager
	/// databaslagret är registrerat i Startup.cs: ConfigureServices
	/// </summary>
	public DatabasePeopleRepo ( ILogger<DatabasePeopleRepo> loggdest,
				    dbPeople databaslager )
	{
	    databas = dbPeople;
	}

	/// <summary>
	/// testdata för övriga delar. I EF:versionen ersätts den här med en SQL:databas
	/// </summary>
    }
}
