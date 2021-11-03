// Time-stamp: <2021-11-03 13:13:27 stefan>
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
	/// </summary>
	/// <params>
	/// Personens namn
	/// Hennes bostadsort
	/// och telefonnummer.
	/// INTE id
	/// </params>
	Person Create ( string namn,
			string bostadsort,
			string telefonnummer );

	/// <summary>
	/// ett totalutdrag från registret
	/// </summary>
	List<Person> Read ();

	/// <summary>
	/// utdrag av ett specifikt kort
	/// </summary>
	/// <params>
	/// id - löpnummer för det kort som ska hämtas
	/// </params>
	Person Read ( int id );

	/// <summary>
	/// modifiering
	/// </summary>
	/// <params>
	/// Person person - det kort som ska modifieras
	/// </params>
	Person Update ( Person person );

	/// <summary>
	/// kasering av ett kort
	/// </summary>
	/// <params>
	/// Person person - det kort som ska kaseras
	/// </params>
	bool Delete ( Person person );
    }
}
