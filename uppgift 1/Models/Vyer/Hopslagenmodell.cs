//
// Time-stamp: <2021-11-03 15:40:53 stefan>
//
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
	/// de personer som ska synas i den vybaserade sidans lista
	/// </summary>
	public PeopleViewModel Personlistan { get; set; }

	/// <summary>
	/// söktermer från filterdialogen
	/// kan innehålla namn eller bostadsort
	/// </summary>
	/// <see cref="PeopleService">PeopleService</see>
	public PeopleViewModel Filtertermer { get; set; }

	/// <summary>
	/// Skriv ut ett nytt kort - nytt-kort delen i fönstret
	/// </summary>
	public CreatePersonViewModel NyttKort { get; set; }

	/// <summary>
	/// skrollistan i den ajaxbaserade kort-väljaren
	/// </summary>
	// public  AktionSpecifiktkort specifiktKort
	// {
	//     get; set;
	// }
    }
}
