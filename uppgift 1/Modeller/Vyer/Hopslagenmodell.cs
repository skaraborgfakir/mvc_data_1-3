//
// Time-stamp: <2021-11-16 13:27:59 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;

using Kartotek.Modeller.Vyer;

namespace Kartotek.Modeller.Vyer
{
    /// <summary>
    /// används i PeopleController:s egen index.html som datakälla - blir model
    ///
    /// Hopslagning av de klasser som är del av definitionerna i IPeopleService
    /// iom att razor-filerna enbart vill hantera en modell, alltså måste klasserna
    /// slås ihop till en gemensam modell i kontroller
    ///
    /// garantera att:
    ///   att användarna inte försöker skapa flera kort med samma innehåll
    ///   kontroll av uppgifterna
    ///      att följande är satta dvs ej NULL
    ///        namn
    ///        bostadsort
    ///        telefonnummer
    ///
    /// slås isär i kontroller innan delarna lämnas över till Peopleservice
    /// </summary>
    public class HopslagenmodellVymodell
    {
	/// <summary>
	/// söktermer i filtreringsdialogen
	/// kan innehålla namn eller bostadsort
	/// </summary>
	/// <see cref="PeopleService">PeopleService</see>
	public PeopleViewModel Listfiltrering { get; set; }

	/// <summary>
	/// Fyll i ett nytt kort - nytt-kort delen i fönstret
	/// </summary>
	public CreatePersonViewModel InskrivningNyttKort; // { get; set; }

	/// <summary>
	/// de personer som ska synas i den vybaserade sidans lista
	/// </summary>
	public PeopleViewModel Personlistan { get; set; }
    }
}
