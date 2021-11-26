// Time-stamp: <2021-11-26 14:33:06 stefan>
//
// dokumentationstaggning
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/
//   https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags#seealso
//

using System.Collections.Generic;

using Kartotek.Modeller.Entiteter;

namespace Kartotek.Modeller.Interfaces {
    /// <summary>
    /// ett åtagande för ett det gränssnitt som affärslogikfunktioner har mot
    /// datalagernivån
    ///
    /// Moduler som implementerar IPeopleService ska kunna använda varje modul som
    /// följer IPeopleRepo-åtagandet
    /// </summary>
    /// <remarks>
    /// Används som ett kontrakt mellan olika utvecklare hur de kommer att konstruera något
    ///
    /// Definitioner av åtagandena finns i http://skaraborgfakir.github.io/lexicon/3. MVC
    /// </remarks>
    /// <see href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%View Models.pdf">
    /// Designen måste följa dokumenten MVC Data, View Models</see>
    /// <see href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%20Partial Views.pdf">
    /// och MVC Data, Partial Views</see>
    /// <see href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%20AJAX.pdf">
    /// och MVC Data, AJAX</see>
    public interface IPeopleRepo {
	/// <summary>
	/// inläggning av ett nytt kort
	/// INTE id som en parameter
	/// </summary>
	/// <param name="namn">Medlemmens namn,</param>
	/// <param name="bostadsort">hemort</param>
	/// <param name="telefonnummer">telefonnummer hem</param>
	/// <returns>Kopia på det nu tillagda kortobjektet</returns>
	/// <seealso href="https://skaraborgfakir.github.io/lexicon/3.%20MVC/MVC%20Data,%20Assignment%203%20-%View Models.pdf">
	/// Designen måste följa dokumenten MVC Data, View Models (sidan två)</seealso>
	Person Create ( string namn,
			string bostadsort,
			string telefonnummer );

	/// <summary>
	/// ett totalutdrag från registret
	/// </summary>
	/// <returns>En samling med samtliga inlagda kort</returns>
	List<Person> Read ();

	/// <summary>
	/// utdrag av ett specifikt kort
	/// </summary>
	/// <param name="id">id - löpnummer för det kort som ska hämtas</param>
	/// <returns>Kopia på ett visst kort</returns>
	Person Read ( int id );

	/// <summary>
	/// modifiering
	/// </summary>
	/// <param name="person">det kort som ska modifieras</param>
	/// <returns>Modifiering av ett visst kort</returns>
	Person Update ( Person person );

	/// <summary>
	/// kasering av ett kort
	/// </summary>
	/// <param name="person">Person person - det kort som ska kaseras</param>
	/// <returns>Flagga som anger om kasering fungerade, fanns kortet överhuvudtaget ?</returns>
	bool Delete ( Person person );
    }
}
