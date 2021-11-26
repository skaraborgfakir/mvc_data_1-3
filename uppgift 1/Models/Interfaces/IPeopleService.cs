//
// Time-stamp: <2021-11-26 14:35:01 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
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
    /// Används som ett kontrakt mellan olika utvecklare hur de kommer att konstruera något
    ///
    /// Moduler som implementerar IPeopleService ska ansvara för affärs-logik/-funktioner
    ///
    /// En MVC-kontrollant ska exv inte användas för att söka eller få fram exv ordersummor
    /// Sådant ska vara i exv OrderService (som ansvarar för åtagandet IOrderService)
    ///
    /// Definitioner av åtagandena finns i http://skaraborgfakir.github.io/lexicon/3. MVC
    /// </remarks>
    /// <seealso href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%View Models.pdf">
    /// Designen måste följa dokumenten MVC Data, View Models</seealso>
    /// <seealso href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%20Partial Views.pdf">
    /// och MVC Data, Partial Views</seealso>
    /// <seealso href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%20AJAX.pdf">
    /// och MVC Data, AJAX</seealso>
    public interface IPeopleService {
	/// <summary>
	/// inläggning av ett kort, använder den Create som definieras av IPeopleRepo
	/// </summary>
	/// <param name="person">uppgifter till det nya kortet</param>
	/// <returns>Inläggning av ett nytt kort med uppgifter om en medlem</returns>
	/// <see cref="IPeopleRepo">Create i IPeopleRepo</see>
	/// <see cref="CreatePersonViewModel">Vymodellen CreatePersonViewModel</see>
	/// <seealso href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%View Models.pdf">
	/// Designen måste följa dokumenten MVC Data, View Models (sidan två)</seealso>
	Person Add ( CreatePersonViewModel person );

	/// <summary>
	/// totalutdrag från registret, som vymodell
	/// </summary>
	/// <returns>En lista med alla kort i kartoteket</returns>
	/// <see cref="IPeopleRepo">Read i IPeopleRepo</see>
	PeopleViewModel All ();

	/// <summary>
	/// specifik(a) kort, som vymodell. PeopleViewModel har sökvillkoren
	/// </summary>
	/// <param name="search">PeopleViewModel där sökvillkoret anges</param>
	/// <returns>PeopleViewModel där funna kort finns i Utdraget</returns>
	/// <see cref="IPeopleRepo">Read i IPeopleRepo</see>
	PeopleViewModel FindBy ( PeopleViewModel search );

	/// <summary>
	/// ett specifikt kort
	/// </summary>
	/// <param name="id">Nummer för det kort som efterfrågas</param>
	/// <returns>Hittat kort</returns>
	/// <see cref="IPeopleRepo">Read i IPeopleRepo</see>
	Person FindBy ( int id );

	/// <summary>
	/// modifiering av ett kort med ett visst nummer
	/// </summary>
	/// <param name="id">Kortnummer för det kort som ska modifieras</param>
	/// <param name="person">Hur alla uppgifterna ska se ut</param>
	/// <returns>Kopia på det modifierade kortet</returns>
	/// <see cref="IPeopleRepo">Update i IPeopleRepo</see>
	Person Edit ( int id, Person person );

	/// <summary>
	/// borttagning av ett visst kort
	/// </summary>
	/// <param name="id">id</param>
	/// <returns>flagga som anger om kaseringen fungerade (fanns kortet egentligen ?)
	/// true = kasering fungerade, false = något fel, fanns det alls ?
	/// </returns>
	/// <see cref="IPeopleRepo">Delete i IPeopleRepo</see>
	bool Remove ( int id );
    }
}
