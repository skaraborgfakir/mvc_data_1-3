//
// - Time-stamp: <2021-11-05 12:26:48 stefan>
//


'use strict';

//
// funktioner för hämtning och visning av kort ur kartoteket
//
$(document).ready(function() {
    const url_samtliga_kort =  "https://localhost:5009/api/PeopleAjax/uppdateralistan"; // json-kodad lista
    const url_specifikt_kort = "https://localhost:5009/api/PeopleAjax/tagUppKortet";    // uppgifter om ett specifikt kort
    const url_kasera_kort =    "https://localhost:5009/api/PeopleAjax/kaseraKortet";    // kasera kortet

    /// <summary>
    /// aktiveras en gång efter inläsning av sidan för att få den första bilden
    /// kan sedan aktiveras via knapptryck i vyn (Uppdatera listan)
    /// </summary>
    function uppdateraVy() {
	//$("#enumreringajax").empty();  // töm ur listan helt och bygg upp den på nytt

	$("#tabell_kartotek_ajax").load( url_samtliga_kort, function() {
	    //
	    // iom att vyn laddas efter att document.ready är klar så kan
	    // uppsättning av händelsehanteringen för vyerna tas upp här
	    //

	    //
	    // TODO: se till att vyn är sortera att den som var innan load
	    //
	});

	// $.ajax({
	//     url: url_samtliga_kort,
	//     type: 'GET',
	//     success: function(res) {
	//	let utdraget = Object( res );
	//	$("tabell_kartotek_ajax").append(utdraget);
	//     }
	// });
    }

    /// få in en aktuell vy direkt
    uppdateraVy();

    /// <summary>
    /// aktiveras via knapptryck på 'Uppdatera listan' (ajaxbaserad_kortselektor.cshtml)
    /// </summary>
    $("#uppdateralistan").click(function( event) {
	event.preventDefault();
	uppdateraVy();
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (visa kortet) (ajaxbaserad_kortselektor.cshtml)
    /// </summary>
    $('#plockaframkortet').click(function( event) {
	event.preventDefault();
	// $.ajax({
	//     url: url_specifikt_kort,
	//     type: 'GET',
	//     datatype: 'json',
	//     success: function(res) {
	//	let utdraget = Object( res );
	//	$.each( utdraget, function( index, item) { enumeration( index, item); });
	//     }
	// });
    });

    /// <summary>
    /// aktiveras via knapptryck i vyn (kasera kortet) (ajaxbaserad_kortselektor.cshtml)
    /// </summary>
    $('#kaserakortet').click(function( event) {
	event.preventDefault();
	// hämta valtkortsid från skrollern och skicka vidare till kaseraspecifiktkort
    });
});

/// <summary>
/// sortera listan efter namn, aktiveras via onClick i aktivlistan.cshtml
///
/// se till att markeringarna i tabellhuvudet är korrekta
///
/// koden börjar alltid med att sortera i stigande ordning
/// därefter med vidare tryck i tabellhuvudet i sjunkande ordning för att vid nästa
/// tryck nolla sorteringen (då blir sorteringen den som repo:koden använder)
///
/// sorteraTabellStigande/sorteraTabellSjunkande sorterar listan  om den får id för tabell och indexet för den
/// kolumn som ska användas för sorteringen
/// </summary>
function sorteraefternamn() {
    //	event.preventDefault();

    $( "#sorteraefterbostadsort" ).removeClass( "sorting_asc");
    $( "#sorteraefterbostadsort" ).removeClass( "sorting_desc");

    if ( !$('#sorteraefternamn').hasClass( "sorting_asc") &&
	 !$('#sorteraefternamn').hasClass( "sorting_desc")) {
	//
	// ingen sortering är i kraft men sortera i stigande
	//
	$('#sorteraefternamn').addClass( "sorting_asc");

	sorteraTabellStigande( 'enumreringajax', 0);
    } else {
	// om sorteringen är inställd att vara sjunkande, tag bort den
	$('#sorteraefternamn.sorting_desc').removeClass( "sorting_desc");

	if ( $('#sorteraefternamn').hasClass( "sorting_asc")) {
	    $('#sorteraefternamn').removeClass( "sorting_asc");
	    $('#sorteraefternamn').addClass( "sorting_desc");

	    sorteraTabellSjunkande( 'enumreringajax', 0);
	}
    }
}

/// <summary>
/// sortera listan efter bostadsort, aktiveras via onClick i aktivlistan.cshtml
///
/// se till att markeringarna i tabellhuvudet är korrekta
///
/// koden börjar alltid med sortera i stigande ordning
/// därefter med vidare tryck i tabellhuvudet i sjunkande ordning för att vid nästa
/// tryck nolla sorteringen (då kan sorteringen bli den som repo:koden använder)
///
/// sorteraTabellStigande/sorteraTabellSjunkande sorterar listan om den får id för tabell och indexet för den
/// kolumn som ska användas för sorteringen
/// </summary>
function sorteraefterbostadsort () {
    //	event.preventDefault();

    $( "#sorteraefternamn.sorting_asc" ).removeClass( "sorting_asc");
    $( "#sorteraefternamn.sorting_desc" ).removeClass( "sorting_desc");

    if (! $('#sorteraefterbostadsort').hasClass( "sorting_asc") &&
	! $('#sorteraefterbostadsort').hasClass( "sorting_desc")) {
	//
	// ingen sortering är i kraft men sortera i stigande
	//
	$('#sorteraefterbostadsort').addClass( "sorting_asc");
	sorteraTabellStigande( 'enumreringajax', 1);
    } else {
	// om sorteringen är inställd att vara sjunkande, tag bort den
	$('#sorteraefterbostadsort.sorting_desc').removeClass( "sorting_desc");

	// om sorteringen är inställd att vara stigande, ersätt den
	// med sjunkande ordning och sortera om
	if ( $('#sorteraefterbostadsort').hasClass( "sorting_asc")) {
	    $('#sorteraefterbostadsort').removeClass( "sorting_asc");
	    $('#sorteraefterbostadsort').addClass( "sorting_desc");

	    sorteraTabellSjunkande( 'enumreringajax', 1);
	}
    }
}

/// <summary>
/// aktion i själva kortuppräkningen
///
/// aktiveras via knapptryck i listvyn (radering bland aktionerna)
/// </summary>
// function kaseraspecifiktkort( Id) {
//     //
//     // TODO: utifrån scrollern, radering av rätt kort
//     //

//     //	$.ajax({
//     //	    url: url_kasera_kort,
//     //	    type: 'POST',
//     //	    success: function(res) {
//     //		let utdraget = Object( res );
//     //	    }
//     //	});
// }

/// <summary>
/// aktion i själva kortuppräkningen
///
/// aktiveras via knapptryck i listvyn (modifiering)
/// </summary>
// function modifieraspecifiktkort( Id) {
//     //
//     // TODO: utifrån scrollern, modifiering av rätt kort
//     //
//     // $.ajax({
//     //     url: url_kasera_kort,
//     //     type: 'GET',
//     //     datatype: 'json',
//     //     success: function(res) {
//     //	let utdraget = Object( res );
//     //	/// $.each( utdraget, function( index, item) { enumeration( index, item); });
//     //     }
//     // });
// }

/// <summary>
/// Vilket ID är lägst ?
///
/// används för validering
/// </summary>
// function lägstId() {
//     // iterera igenom personlistan för att hitta högst kort:ID
// }

/// <summary>
/// Vilket ID är högst ?
///
/// används för validering
/// </summary>
// function högstId() {
//     // iterera igenom personlistan för att hitta högst kort:ID
// }

/// <summary>
/// Finns det något kort som har ett visst ID ?
///
/// används för validering
/// </summary>
// function finnsKortId( int it) {
//     // iterera igenom personlistan för se om något kort har ett visst ID
// }
