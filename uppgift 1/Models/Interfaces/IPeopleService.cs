//
// Time-stamp: <2021-11-03 15:44:58 stefan>
//

using System.Collections.Generic;

using Kartotek.Modeller.Entiteter;
using Kartotek.Modeller.Vyer;

namespace Kartotek.Modeller.Interfaces {
    /// <summary>
    /// En design och ett åtagande för affärslogikmoduler mot kontrollanterna i ett
    /// medlemskartotek
    /// </summary>
    /// <remarks>
    /// Används som ett kontrakt mellan olika utvecklare hur de kommer att konstruera
    /// något
    ///
    /// Moduler som implementerar IPeopleService ska ansvara för
    /// affärs-logik/-funktioner
    ///
    /// En MVC-kontrollant ska exv inte användas för att söka eller få fram exv
    /// ordersummor
    /// Sådant ska vara i exv OrderService (som ansvarar för åtagandet IOrderService)
    /// </remarks>
    /// <seealso href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%View Models.pdf">Designen måste följa dokumenten MVC Data, View Models</seealso>
    /// <seealso href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%20Partial Views.pdf">och MVC Data, Partial Views</seealso>
    /// <seealso href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%20AJAX.pdf">och MVC Data, AJAX</seealso>
    public interface IPeopleService {
	/// <summary>
	/// inläggning av ett kort, använder den Create som definieras av IPeopleRepo
	/// </summary>
	/// <see cref="IPeopleRepo">Create i IPeopleRepo</see>
	Person Add ( CreatePersonViewModel person );

	/// <summary>
	/// totalutdrag från registret, som vymodell
	/// </summary>
	/// <see cref="IPeopleRepo">Read i IPeopleRepo</see>
	PeopleViewModel All ();

	/// <summary>
	/// specifik(a) kort, som vymodell. PeopleViewModel har sökvillkoren
	/// </summary>
	/// <see cref="IPeopleRepo">Read i IPeopleRepo</see>
	PeopleViewModel FindBy ( PeopleViewModel search );

	/// <summary>
	/// ett specifikt kort
	/// </summary>
	/// <see cref="IPeopleRepo">Read i IPeopleRepo</see>
	Person FindBy ( int id );

	/// <summary>
	/// modifiering av ett kort med ett visst nummer
	/// </summary>
	/// <see cref="IPeopleRepo">Update i IPeopleRepo</see>
	Person Edit ( int id, Person person );

	/// <summary>
	/// borttagning av ett visst kort
	/// </summary>
	/// <params>int id
	/// </params>
	/// <see cref="IPeopleRepo">Delete i IPeopleRepo</see>
	bool Remove ( int id );
    }
}
